using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNTK;

namespace Horker.PSCNTK
{
    public abstract class SamplerBase : ISampler
    {
        public abstract Minibatch GetNextMinibatch(DeviceDescriptor device = null);

        #region IDisposable Support

        public bool Disposed => _disposed;

        private bool _disposed = false;

        protected abstract void Dispose(bool disposing);

        ~SamplerBase() {
            Dispose(false);
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            Dispose(true);
            GC.SuppressFinalize(this);
            _disposed = true;
        }

        #endregion
    }
}
