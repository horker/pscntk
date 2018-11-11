using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;
using CNTK;

namespace Horker.PSCNTK
{
    public class MsgPackSampler : ISampler, IDisposable
    {
        public int _minibatchSize;

        private ParallelQueue<DataSourceSet> _dataSourceQueue;
        private ParallelSampler _parallelSampler;
        private Dictionary<string, SlidingDataSource<float>> _dataSourceBuffer;

        private Stream _stream;
        private Task _loadingTask;
        private Task _slicingTask;
        private bool _canceled;

        private bool _randomize;

        private Exception _lastException;

        public int CountInQueue => _parallelSampler.CountInQueue;

        public bool ReuseSamples => _parallelSampler.ReuseSamples;

        public int MinibatchSize => _minibatchSize;
        public int SampleCountPerEpoch => _parallelSampler.SampleCountPerEpoch;
        public long TotalSampleCount => _parallelSampler.TotalSampleCount;
        public int Epoch => _parallelSampler.Epoch;

        public long ReadCount => _parallelSampler.ReadCount;
        public long WriteCount => _parallelSampler.WriteCount;

        public int TimeoutForAdd => _parallelSampler.TimeoutForAdd;
        public int TimeoutForTake => _parallelSampler.TimeoutForTake;

        public Exception LastException => _lastException;

        public MsgPackSampler(int minibatchSize, bool randomize, int sampleCountPerEpoch, int queueSize, bool reuseSamples, int bufferSize = 1000, int timeoutForAdd = 60 * 60 * 1000, int timeoutForTake = 60 * 60 * 1000)
        {
            _minibatchSize = minibatchSize;
            _randomize = randomize;

            _dataSourceQueue = new ParallelQueue<DataSourceSet>(queueSize, reuseSamples, bufferSize, timeoutForAdd, timeoutForTake);
            _parallelSampler = new ParallelSampler(sampleCountPerEpoch, queueSize, reuseSamples, bufferSize, timeoutForAdd, timeoutForTake);
            _dataSourceBuffer = new Dictionary<string, SlidingDataSource<float>>();

            _stream = null;
            _loadingTask = null;
            _slicingTask = null;
            _canceled = false;
        }

        public void StartLoading(string path)
        {
            _stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            StartLoading(_stream);
        }

        public void StartLoading(Stream stream)
        {
            if (_loadingTask != null || _canceled)
                throw new InvalidOperationException("Loading already stared");

            _canceled = false;
            _lastException = null;

            _loadingTask = Task.Run(() => {
                try
                {
                    while (!_canceled)
                    {
                        while (stream.Position < stream.Length)
                        {
                            var dss = MsgPackSerializer.Deserialize(stream);
                            _dataSourceQueue.Add(dss);
                        }

                        if (!_canceled)
                            stream.Position = 0;
                    }
                }
                catch (Exception ex)
                {
                    _lastException = ex;
                    _canceled = true;
                    _dataSourceQueue.CancelAdding();
                    _dataSourceQueue.CancelTaking();
                    _parallelSampler.CancelAdding();
                    _parallelSampler.CancelTaking();
                }
            });

            _slicingTask = Task.Run(() => {
                try
                {
                    while (!_canceled)
                    {
                        var dss = _dataSourceQueue.Take();
                        foreach (var entry in dss.Features)
                        {
                            SlidingDataSource<float> ds = null;
                            if (_dataSourceBuffer.TryGetValue(entry.Key, out ds))
                            {
                                if (!ds.Shape.EqualsExceptAxis(entry.Value.Shape, -1))
                                    throw new InvalidOperationException("Shape of the loaded data source doesn't match those of the existing ones");
                                ds.AddSamples(entry.Value.Data);
                            }
                            else
                            {
                                ds = new SlidingDataSource<float>(new IList<float>[] { entry.Value.Data }, entry.Value.Shape.Dimensions);
                                _dataSourceBuffer.Add(entry.Key, ds);
                            }
                        }

                        var sampleCount = _dataSourceBuffer.First().Value.Shape[-1];
                        if (sampleCount >= _minibatchSize)
                        {
                            var batchCount = sampleCount / _minibatchSize;
                            var order = RandomizedList<float>.GetRandomizedIndexes(sampleCount);
                            var batches = new DataSourceSet[batchCount];
                            for (var i = 0; i < batchCount; ++i)
                                batches[i] = new DataSourceSet();

                            foreach (var entry in _dataSourceBuffer)
                            {
                                IDataSource<float> ds = entry.Value;
                                if (_randomize)
                                {
                                    var stride = ds.Shape.GetSize(-2);
                                    var randomized = new RandomizedList<float>(ds.Data, order, stride);
                                    ds = new DataSourceBase<float, RandomizedList<float>>(randomized, ds.Shape.Dimensions);
                                }

                                for (var i = 0; i < batchCount; ++i)
                                {
                                    var slice = ds.Subset(i * _minibatchSize, _minibatchSize);
                                    // Data in SlidingDataSource must be copied because they become invalid with a data window sliding.
                                    var copy = DataSourceFactory.Copy<float, IList<float>>(slice.Data, slice.Shape.Dimensions);
                                    batches[i].Add(entry.Key, copy);
                                }

                                entry.Value.SkipSamples(batchCount * _minibatchSize);
                            }

                            for (var i = 0; i < batchCount; ++i)
                                _parallelSampler.AddMinibatch(batches[i]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _lastException = ex;
                    _canceled = true;
                    _dataSourceQueue.CancelAdding();
                    _dataSourceQueue.CancelTaking();
                    _parallelSampler.CancelAdding();
                    _parallelSampler.CancelTaking();
                }
            });
        }

        public void StopLoading()
        {
            _canceled = true;
            _dataSourceQueue.CancelAdding();
            _dataSourceQueue.CancelTaking();
            _parallelSampler.CancelAdding();
            _parallelSampler.CancelTaking();

            if (_loadingTask != null)
            {
                _loadingTask.Wait();
                _loadingTask = null;
            }

            if (_slicingTask != null)
            {
                _slicingTask.Wait();
                _slicingTask = null;
            }

            if (_stream != null)
            {
                _stream.Dispose();
                _stream = null;
            }
        }

        public Minibatch GetNextMinibatch(DeviceDescriptor device = null)
        {
            return _parallelSampler.GetNextMinibatch(device);
        }

        public void Dispose()
        {
            StopLoading();
            if (_loadingTask != null)
                ((IDisposable)_loadingTask).Dispose();
            if (_slicingTask != null)
                ((IDisposable)_slicingTask).Dispose();
        }
    }
}
