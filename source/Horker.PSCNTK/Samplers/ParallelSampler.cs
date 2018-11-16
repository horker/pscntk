using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Management.Automation;
using System.Threading;
using CNTK;

namespace Horker.PSCNTK
{
    public class ParallelSampler : SamplerBase
    {
        private ParallelQueue<DataSourceSet> _dataQueue;
        private int _sampleCountPerEpoch;
        private int _totalSampleCount;
        private int _epoch;

        public int CountInQueue => _dataQueue.Count;

        public bool ReuseSamples => _dataQueue.ReuseSamples;

        public int SampleCountPerEpoch => _sampleCountPerEpoch;
        public long TotalSampleCount => _totalSampleCount;
        public int Epoch => _epoch;

        public long ReadCount => _dataQueue.ReadCount;
        public long WriteCount => _dataQueue.WriteCount;

        public int TimeoutForAdd => _dataQueue.TimeoutForAdd;
        public int TimeoutForTake => _dataQueue.TimeoutForTake;

        public ParallelSampler(int sampleCountPerEpoch, int queueSize, bool reuseSamples, int bufferSize = 1000, int timeoutForAdd = 10 * 1000, int timeoutForTake = 60 * 60 * 10000)
        {
            _sampleCountPerEpoch = sampleCountPerEpoch;
            _totalSampleCount = 0;
            _epoch = 0;

            _dataQueue = new ParallelQueue<DataSourceSet>(queueSize, reuseSamples, bufferSize, timeoutForAdd, timeoutForTake);
        }

        protected override void Dispose(bool disposing)
        {
            // Nothing to do.
        }

        public void AddMinibatch(DataSourceSet dataSourceSet)
        {
            _dataQueue.Add(dataSourceSet);
        }

        public override Minibatch GetNextMinibatch(DeviceDescriptor device = null)
        {
            var dataSourceSet = _dataQueue.Take();
            _totalSampleCount += dataSourceSet.SampleCount;

            var sweepEnd = false;
            if ((int)Math.Floor((double)_totalSampleCount / _sampleCountPerEpoch) > _epoch)
            {
                ++_epoch;
                sweepEnd = true;
            }

            return new Minibatch(dataSourceSet.Features, sweepEnd, device);
        }

        public DataSourceSet Deque()
        {
            return _dataQueue.Take();
        }

        public void CancelAdding()
        {
            _dataQueue.CancelAdding();
        }

        public void CancelTaking()
        {
            _dataQueue.CancelTaking();
        }
    }
}
