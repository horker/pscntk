using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNTK;

namespace Horker.PSCNTK
{
    [Serializable]
    public class CompositeSampler : ISampler
    {
        private ISampler[] _samplers;

        public IEnumerable<ISampler> Samplers;

        public CompositeSampler(params ISampler[] samplers)
        {
            _samplers = samplers;
        }

        public Minibatch GetNextMinibatch(DeviceDescriptor device = null)
        {
            return new Minibatch(_samplers.Select(x => x.GetNextMinibatch(device)));
        }
    }
}
