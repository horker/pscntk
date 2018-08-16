using CNTK;

namespace Horker.PSCNTK
{
    public interface IMinibatchDefinition
    {
        Minibatch GetNextBatch(DeviceDescriptor device = null);
        Minibatch GetValidationBatch(DeviceDescriptor device = null);
    }
}
