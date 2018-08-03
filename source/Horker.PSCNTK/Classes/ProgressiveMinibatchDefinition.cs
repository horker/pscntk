using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using CNTK;

namespace Horker.PSCNTK
{
    internal class DataSourceBuffer
    {
        private int _minibatchSize;
        private int _sizePerSample;
        private List<DataSource<float>> _dataSources;
        private int _dataSourceIndex;

        private DataSource<float> _current;
        private int _currentIndex;

        public DataSourceBuffer(int minibatchSize, DataSource<float> prototype)
        {
            _minibatchSize = minibatchSize;
            _sizePerSample = prototype.Shape.GetSize(-2);

            _dataSources = new List<DataSource<float>>();
            _dataSourceIndex = 0;

            var shape = prototype.Shape.Clone();
            shape[-1] = minibatchSize;

            _current = new DataSource<float>(new float[_sizePerSample * minibatchSize], shape);
            _currentIndex = 0;
        }

        public void AddDataSource(DataSource<float> dataSource)
        {
            _dataSources.Add(dataSource);
        }

        public MinibatchData GetMinibatchData(DeviceDescriptor device, bool sweepEnd)
        {
            if (_currentIndex < _minibatchSize)
            {
                while (true)
                {
                    if (_dataSources.Count == 0)
                        throw new ArgumentException("data empty");

                    var ds = _dataSources[0];
                    var size = Math.Min(ds.Shape[-1] - _dataSourceIndex, _minibatchSize - _currentIndex);

                    Array.Copy(ds.Data, _sizePerSample * _dataSourceIndex, _current.Data, _sizePerSample * _currentIndex, _sizePerSample * size);

                    _currentIndex += size;
                    _dataSourceIndex += size;

                    if (_dataSourceIndex >= ds.Shape[-1])
                    {
                        _dataSources.RemoveAt(0);
                        _dataSourceIndex = 0;
                    }

                    if (_currentIndex >= _minibatchSize)
                        break;
                }
            }

            var shape = _current.Shape.Clone();
            shape[-1] = _minibatchSize;

            var buffer = new float[_current.Data.Length];
            _current.Data.CopyTo(buffer, 0);
            _currentIndex = 0;

            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();

            var value = new Value(new NDArrayView(shape.Dimensions, buffer, device, true));
            var batch = new MinibatchData(value, (uint)_minibatchSize, (uint)(_minibatchSize * shape[-2]), sweepEnd);

            return batch;
        }

        public int TransferInternalData(DataSourceBuffer other)
        {
            if (_sizePerSample != other._sizePerSample)
                throw new ArgumentException("Shape does not match with destination");

            int sampleCount = 0;

            if (_currentIndex > 0)
            {
                var ds = _current.Slice(0, _currentIndex);
                other._dataSources.Add(ds);
                sampleCount += _currentIndex;
            }

            if (_dataSources.Count >= 1)
            {
                var ds = _dataSources[0].Slice(_dataSourceIndex, _dataSources[0].Shape[-1]);
                other._dataSources.Add(ds);
                sampleCount += ds.Shape[-1];
            }

            for (var i = 1; i < _dataSources.Count; ++i)
            {
                other._dataSources.Add(_dataSources[i]);
                sampleCount += _dataSources[i].Shape[-1];
            }

            _currentIndex = 0;
            _dataSourceIndex = 0;
            _dataSources.Clear();

            return sampleCount;
        }
    }

    internal class DataSourceBufferSet
    {
        private int _minibatchSize;
        private Dictionary<string, DataSourceBuffer> _buffers;

        public DataSourceBuffer this[string name]
        {
            get => _buffers[name];
        }

        public DataSourceBufferSet(int minibatchSize)
        {
            _minibatchSize = minibatchSize;
            _buffers = null;
        }

        public int UpdateDataSourceBuffer(DataSourceSet dataSet)
        {
            if (_buffers == null)
            {
                _buffers = new Dictionary<string, DataSourceBuffer>();
                foreach (var entry in dataSet)
                {
                    var buffer = new DataSourceBuffer(_minibatchSize, entry.Value);
                    _buffers.Add(entry.Key, buffer);
                }
            }

            int sampleCount = -1;

            foreach (var entry in dataSet)
            {
                if (!_buffers.ContainsKey(entry.Key))
                    throw new ArgumentException(string.Format("Unknown data name: {0}", entry.Key));

                _buffers[entry.Key].AddDataSource(entry.Value);

                var count = entry.Value.Shape[-1];
                if (sampleCount != -1 && count != sampleCount)
                    throw new ArgumentException(string.Format("Sample count is differenct from the other data sources: {0}", entry.Key));

                sampleCount = count;
            }

            return sampleCount;
        }

        public Minibatch GetMinibatch(DeviceDescriptor device, bool sweepEnd)
        {
            var batch = new Minibatch();
            batch.SweepEnd = sweepEnd;

            foreach (var entry in _buffers)
            {
                var b = entry.Value.GetMinibatchData(device, sweepEnd);
                batch.Features.Add(entry.Key, b);
            }

            return batch;
        }

        public int TransferInternalData(DataSourceBufferSet other)
        {
            int sampleCount = -1;
            foreach (var entry in _buffers)
            {
                int count = entry.Value.TransferInternalData(other[entry.Key]);
                if (sampleCount != -1 && sampleCount != count)
                    throw new ApplicationException("bug: data inconsistent in TransferInternalData");
                sampleCount = count;
            }

            return sampleCount;
        }
    }

    public class ProgressiveMinibatchDefinition : IMinibatchDefinition
    {
        private int _minibatchSize;
        private int _sampleCountPerEpoch;

        private BlockingCollection<DataSourceSet> _dataSetQueue;
        private DataSourceBufferSet _buffers;

        private int _availableSampleCount;
        private int _totalSampleCount;
        private int _epoch;

        private int _validationDataSize;
        private Minibatch _validationData;

        public ProgressiveMinibatchDefinition(int minibatchSize, int sampleCountPerEpoch, int validationDataSize, int queueSize)
        {
            _minibatchSize = minibatchSize;
            _sampleCountPerEpoch = sampleCountPerEpoch;

            _dataSetQueue = new BlockingCollection<DataSourceSet>(queueSize);
            _buffers = new DataSourceBufferSet(minibatchSize);

            _availableSampleCount = 0;
            _totalSampleCount = 0;
            _epoch = 0;

            _validationDataSize = validationDataSize;
            _validationData = null;
        }

        public void AddDataSourceSet(DataSourceSet dataSet)
        {
            _dataSetQueue.Add(dataSet);
        }

        public Minibatch GetNextBatch(DeviceDescriptor device = null)
        {
            while (_availableSampleCount < _minibatchSize)
            {
                var ds = _dataSetQueue.Take();
                _availableSampleCount += _buffers.UpdateDataSourceBuffer(ds);
            }

            _availableSampleCount -= _minibatchSize;
            _totalSampleCount += _minibatchSize;

            var sweepEnd = false;
            if ((int)Math.Floor((double)_totalSampleCount / _sampleCountPerEpoch) > _epoch)
            {
                ++_epoch;
                sweepEnd = true;
            }

            var batch = _buffers.GetMinibatch(device, sweepEnd);

            return batch;
        }

        public Minibatch GetValidationBatch(DeviceDescriptor device = null)
        {
            if (_validationData != null)
                return _validationData;

            var bufferSet = new DataSourceBufferSet(_validationDataSize);

            for (int count = 0; count < _validationDataSize;)
            {
                var ds = _dataSetQueue.Take();
                count += bufferSet.UpdateDataSourceBuffer(ds);
            }

            _validationData = bufferSet.GetMinibatch(device, false);

            _availableSampleCount += bufferSet.TransferInternalData(_buffers);

            return _validationData;
        }

        public void SetValidationData(DataSourceSet dataSet, DeviceDescriptor device = null)
        {
            var bufferSet = new DataSourceBufferSet(dataSet.SampleCount);

            bufferSet.UpdateDataSourceBuffer(dataSet);

            _validationData = bufferSet.GetMinibatch(device, false);
        }
    }
}
