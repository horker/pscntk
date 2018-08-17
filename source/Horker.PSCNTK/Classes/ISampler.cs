using CNTK;

namespace Horker.PSCNTK
{
    public interface ISampler
    {
        Minibatch GetNextBatch(DeviceDescriptor device = null);
        Minibatch GetValidationBatch(DeviceDescriptor device = null);
    }
}
