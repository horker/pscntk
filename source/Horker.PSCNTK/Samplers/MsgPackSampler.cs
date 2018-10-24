using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;
using CNTK;

namespace Horker.PSCNTK
{
    public class MsgPackSampler : ISampler, IDisposable
    {
        private ParallelSampler _parallelSampler;

        private Stream _stream;
        private Task _loadingTask;
        private bool _canceled;

        private Exception _lastException;

        public int CountInQueue => _parallelSampler.CountInQueue;

        public bool ReuseSamples => _parallelSampler.ReuseSamples;

        public int SampleCountPerEpoch => _parallelSampler.SampleCountPerEpoch;
        public long TotalSampleCount => _parallelSampler.TotalSampleCount;
        public int Epoch => _parallelSampler.Epoch;

        public long ReadCount => _parallelSampler.ReadCount;
        public long WriteCount => _parallelSampler.WriteCount;

        public int TimeoutForAdd => _parallelSampler.TimeoutForAdd;
        public int TimeoutForTake => _parallelSampler.TimeoutForTake;

        public Exception LastException => _lastException;

        public MsgPackSampler(int sampleCountPerEpoch, int queueSize, bool reuseSamples, int bufferSize = 1000, int timeoutForAdd = 10 * 1000, int timeoutForTake = 60 * 60 * 1000)
        {
            _parallelSampler = new ParallelSampler(sampleCountPerEpoch, queueSize, reuseSamples, bufferSize, timeoutForAdd, timeoutForTake);

            _stream = null;
            _loadingTask = null;
            _canceled = false;
        }

        public void StartLoading(string path)
        {
            _stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            StartLoading(_stream);
        }

        public void StartLoading(Stream stream)
        {
            if (_loadingTask != null)
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
                            if (!_parallelSampler.AddMinibatch(dss))
                            {
                                _canceled = true;
                                break;
                            }
                        }

                        if (!_canceled)
                            stream.Position = 0;
                    }
                }
                catch (Exception ex)
                {
                    _lastException = ex;
                }
            });
        }

        public void StopLoading()
        {
            _canceled = true;
            _parallelSampler.CancelAdding();

            if (_loadingTask != null)
            {
                _loadingTask.Wait();
                _loadingTask = null;
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
        }
    }
}
