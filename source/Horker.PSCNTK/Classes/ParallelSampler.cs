using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Management.Automation;
using System.Threading;
using CNTK;

namespace Horker.PSCNTK
{
    public class ParallelSampler : ISampler
    {
        private BlockingCollection<DataSourceSet> _dataQueue;

        private int _sampleCountPerEpoch;
        private int _totalSampleCount;
        private int _epoch;

        private DataSourceSet _validationData;
        private Minibatch _validationMinibatch;

        private DataSourceSet _lastMinibatch;

        private CancellationTokenSource _cancelTokenSourceForAdd;
        private CancellationTokenSource _cancelTokenSourceForTake;

        private int _timeoutForAdd;
        private int _timeoutForTake;

        public int CountInQueue { get => _dataQueue.Count; }

        public int TimeoutForAdd { get => _timeoutForAdd; }
        public int TimeoutForTake { get => _timeoutForTake; }

        public ParallelSampler(int sampleCountPerEpoch, int queueSize, int timeoutForAdd = 60 * 1000, int timeoutForTake = 30 * 1000)
        {
            _sampleCountPerEpoch = sampleCountPerEpoch;
            _totalSampleCount = 0;
            _epoch = 0;

            _dataQueue = new BlockingCollection<DataSourceSet>(queueSize);

            _validationData = null;
            _validationMinibatch = null;
            _lastMinibatch = null;

            _cancelTokenSourceForAdd = new CancellationTokenSource();
            _cancelTokenSourceForTake = new CancellationTokenSource();

            _timeoutForAdd = timeoutForAdd;
            _timeoutForTake = timeoutForTake;
        }

        public bool AddMinibatch(DataSourceSet dataSourceSet)
        {
            try
            {
                _cancelTokenSourceForAdd.CancelAfter(_timeoutForAdd);
                _dataQueue.Add(dataSourceSet, _cancelTokenSourceForAdd.Token);
                return true;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
        }

        public bool AddMinibatch(Hashtable dataSet)
        {
            var set = new DataSourceSet();
            foreach (DictionaryEntry entry in dataSet)
            {
                var value = entry.Value;
                if (value is PSObject)
                    value = (value as PSObject).BaseObject;
                set.Add((string)entry.Key, (DataSource<float>)value);
            }

            return AddMinibatch(set);
        }

        public Minibatch GetNextBatch(DeviceDescriptor device = null)
        {
            DataSourceSet dataSourceSet;
            try
            {
                _cancelTokenSourceForTake.CancelAfter(_timeoutForTake);
                dataSourceSet = _dataQueue.Take(_cancelTokenSourceForTake.Token);
            }
            catch (OperationCanceledException)
            {
                return null;
            }

            var minibatch = new Minibatch(dataSourceSet.Features, false, device);

            _totalSampleCount += minibatch.SampleCount;

            if ((int)Math.Floor((double)_totalSampleCount / _sampleCountPerEpoch) > _epoch)
            {
                ++_epoch;
                minibatch.SweepEnd = true;
            }

            // Preserve the last data source until the next call to avoid it being garbage-collected.
            _lastMinibatch = dataSourceSet;

            return minibatch;
        }

        public void CancelAdding()
        {
            _cancelTokenSourceForAdd.Cancel();
        }

        public void CancelTaking()
        {
            _cancelTokenSourceForTake.Cancel();
        }

        public Minibatch GetValidationBatch(DeviceDescriptor device = null)
        {
            return _validationMinibatch;
        }

        public void SetValidationData(DataSourceSet validationData)
        {
            // Keep the original data to avoid data being garbage-collected.
            _validationData = validationData;

            _validationMinibatch = new Minibatch(validationData.Features, false, null);
        }

        public void SetValidationData(Hashtable dataSet)
        {
            var set = new DataSourceSet();
            foreach (DictionaryEntry entry in dataSet)
            {
                var value = entry.Value;
                if (value is PSObject)
                    value = (value as PSObject).BaseObject;
                set.Add((string)entry.Key, (DataSource<float>)value);
            }

            SetValidationData(set);
        }
    }
}
