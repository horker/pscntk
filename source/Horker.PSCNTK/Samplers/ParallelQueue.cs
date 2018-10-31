using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Management.Automation;
using System.Threading;
using CNTK;

namespace Horker.PSCNTK
{
    public class ParallelQueue<T>
    {
        private BlockingCollection<T> _dataQueue;

        private bool _reuseSamples;
        private RingBuffer<T> _ringBuffer;

        private long _readCount;
        private long _writeCount;

        private CancellationTokenSource _cancelTokenSourceForAdd;
        private CancellationTokenSource _cancelTokenSourceForTake;

        private int _timeoutForAdd;
        private int _timeoutForTake;

        public int Count => _dataQueue.Count;

        public bool ReuseSamples => _reuseSamples;

        public long ReadCount => _readCount;
        public long WriteCount => _writeCount;

        public int TimeoutForAdd => _timeoutForAdd;
        public int TimeoutForTake => _timeoutForTake;

        public ParallelQueue(int queueSize, bool reuseSamples, int bufferSize = 1000, int timeoutForAdd = 10 * 1000, int timeoutForTake = 60 * 60 * 10000)
        {
            _readCount = _writeCount = 0;

            _dataQueue = new BlockingCollection<T>(queueSize);

            _cancelTokenSourceForAdd = new CancellationTokenSource();
            _cancelTokenSourceForTake = new CancellationTokenSource();

            _timeoutForAdd = timeoutForAdd;
            _timeoutForTake = timeoutForTake;

            _reuseSamples = reuseSamples;
            if (reuseSamples)
                _ringBuffer = new RingBuffer<T>(bufferSize);
        }

        public bool Add(T data)
        {
            try
            {
                _cancelTokenSourceForAdd.CancelAfter(_timeoutForAdd);
                _dataQueue.Add(data, _cancelTokenSourceForAdd.Token);
                ++_writeCount;
                return true;
            }
            catch (OperationCanceledException ex)
            {
                throw new TimeoutException("Posting data into queue timed out", ex);
            }
        }

        public T Take()
        {
            T data = default(T);

            if (_reuseSamples)
            {
                if (_dataQueue.TryTake(out data))
                    _ringBuffer.WriteNext(data);
                else
                {
                    if (_ringBuffer.Peek() != null)
                        data = _ringBuffer.ReadNext();
                }
            }

            if (data == null)
            {
                try
                {
                    _cancelTokenSourceForTake.CancelAfter(_timeoutForTake);
                    data = _dataQueue.Take(_cancelTokenSourceForTake.Token);
                }
                catch (OperationCanceledException ex)
                {
                    throw new TimeoutException("Taking data from queue timed out", ex);
                }
            }

            ++_readCount;

            return data;
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
