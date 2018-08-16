using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using CNTK;

namespace Horker.PSCNTK
{
    public class ProgressiveMinibatchDefinition : IMinibatchDefinition
    {
        private BlockingCollection<Minibatch> _minibatchQueue;

        private int _sampleCountPerEpoch;
        private int _totalSampleCount;
        private int _epoch;

        private Minibatch _validationData;

        private CancellationTokenSource _cancelTokenSourceForAdd;
        private CancellationTokenSource _cancelTokenSourceForTake;

        private int _timeoutForAdd;
        private int _timeoutForTake;

        public int CountInQueue { get => _minibatchQueue.Count; }

        public int TimeoutForAdd { get => _timeoutForAdd; }
        public int TimeoutForTake { get => _timeoutForTake; }

        public ProgressiveMinibatchDefinition(int sampleCountPerEpoch, int queueSize, int timeoutForAdd = 60 * 1000, int timeoutForTake = 30 * 1000)
        {
            _sampleCountPerEpoch = sampleCountPerEpoch;
            _totalSampleCount = 0;
            _epoch = 0;

            _minibatchQueue = new BlockingCollection<Minibatch>(queueSize);

            _validationData = null;

            _cancelTokenSourceForAdd = new CancellationTokenSource();
            _cancelTokenSourceForTake = new CancellationTokenSource();

            _timeoutForAdd = timeoutForAdd;
            _timeoutForTake = timeoutForTake;
        }

        public bool AddMinibatch(Minibatch minibatch)
        {
            try
            {
                _cancelTokenSourceForAdd.CancelAfter(_timeoutForAdd);
                _minibatchQueue.Add(minibatch, _cancelTokenSourceForAdd.Token);
                return true;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
        }

        public Minibatch GetNextBatch(DeviceDescriptor device = null)
        {
            Minibatch minibatch;
            try
            {
                _cancelTokenSourceForTake.CancelAfter(_timeoutForTake);
                minibatch = _minibatchQueue.Take(_cancelTokenSourceForTake.Token);
            }
            catch (OperationCanceledException)
            {
                return null;
            }

            _totalSampleCount += minibatch.SampleCount;

            if ((int)Math.Floor((double)_totalSampleCount / _sampleCountPerEpoch) > _epoch)
            {
                ++_epoch;
                minibatch.SweepEnd = true;
            }

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
            return _validationData;
        }

        public void SetValidationData(Minibatch validationData)
        {
            _validationData = validationData;
        }
    }
}
