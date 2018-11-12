using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNTK;

namespace Horker.PSCNTK
{
    [Serializable]
    public class CompositeSampler : SamplerBase
    {
        private ISampler[] _samplers;

        public IEnumerable<ISampler> Samplers;

        public CompositeSampler(params ISampler[] samplers)
        {
            _samplers = samplers;
        }

        public override Minibatch GetNextMinibatch(DeviceDescriptor device = null)
        {
            return new Minibatch(_samplers.Select(x => x.GetNextMinibatch(device)));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var s in _samplers)
                    s.Dispose();
            }
        }
    }
}
