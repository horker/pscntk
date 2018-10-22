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

        private bool _reuseSamples;
        private RingBuffer<DataSourceSet> _ringBuffer;

        private int _sampleCountPerEpoch;
        private long _totalSampleCount;
        private int _epoch;

        private long _readCount;
        private long _writeCount;

        private DataSourceSet _lastMinibatch;

        private CancellationTokenSource _cancelTokenSourceForAdd;
        private CancellationTokenSource _cancelTokenSourceForTake;

        private int _timeoutForAdd;
        private int _timeoutForTake;

        public int CountInQueue => _dataQueue.Count;

        public bool ReuseSamples => _reuseSamples;

        public int SampleCountPerEpoch => _sampleCountPerEpoch;
        public long TotalSampleCount => _totalSampleCount;
        public int Epoch => _epoch;

        public long ReadCount => _readCount;
        public long WriteCount => _writeCount;

        public int TimeoutForAdd => _timeoutForAdd;
        public int TimeoutForTake => _timeoutForTake;

        public ParallelSampler(int sampleCountPerEpoch, int queueSize, bool reuseSamples, int bufferSize = 1000, int timeoutForAdd = 10 * 1000, int timeoutForTake = 10 * 1000)
        {
            _sampleCountPerEpoch = sampleCountPerEpoch;
            _totalSampleCount = 0;
            _epoch = 0;

            _readCount = _writeCount = 0;

            _dataQueue = new BlockingCollection<DataSourceSet>(queueSize);

            _lastMinibatch = null;

            _cancelTokenSourceForAdd = new CancellationTokenSource();
            _cancelTokenSourceForTake = new CancellationTokenSource();

            _timeoutForAdd = timeoutForAdd;
            _timeoutForTake = timeoutForTake;

            _reuseSamples = reuseSamples;
            if (reuseSamples)
                _ringBuffer = new RingBuffer<DataSourceSet>(bufferSize);
        }

        public bool AddMinibatch(DataSourceSet dataSourceSet)
        {
            try
            {
                _cancelTokenSourceForAdd.CancelAfter(_timeoutForAdd);
                _dataQueue.Add(dataSourceSet, _cancelTokenSourceForAdd.Token);
                ++_writeCount;
                return true;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
        }

        public Minibatch GetNextMinibatch(DeviceDescriptor device = null)
        {
            DataSourceSet dataSourceSet = null;

            if (_reuseSamples)
            {
                if (_dataQueue.TryTake(out dataSourceSet))
                    _ringBuffer.WriteNext(dataSourceSet);
                else
                {
                    if (_ringBuffer.Peek() != null)
                        dataSourceSet = _ringBuffer.ReadNext();
                }
            }

            if (dataSourceSet == null)
            {
                try
                {
                    _cancelTokenSourceForTake.CancelAfter(_timeoutForTake);
                    dataSourceSet = _dataQueue.Take(_cancelTokenSourceForTake.Token);
                }
                catch (OperationCanceledException)
                {
                    return null;
                }
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

            ++_readCount;

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
    }
}
