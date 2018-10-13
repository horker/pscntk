using CNTK;

namespace Horker.PSCNTK
{
    public interface ISampler
    {
        Minibatch GetNextMinibatch(DeviceDescriptor device = null);
    }
}
