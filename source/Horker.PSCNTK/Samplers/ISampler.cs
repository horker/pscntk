using System;
using CNTK;

namespace Horker.PSCNTK
{
    public interface ISampler : IDisposable
    {
        Minibatch GetNextMinibatch(DeviceDescriptor device = null);
    }
}
