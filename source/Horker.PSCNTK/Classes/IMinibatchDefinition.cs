using System.Collections.Generic;
using CNTK;

namespace Horker.PSCNTK
{
    public class Minibatch
    {
        public Dictionary<string, MinibatchData> Features;
        public bool SweepEnd;

        public Minibatch()
        {
            Features = new Dictionary<string, MinibatchData>();
        }
    }

    public interface IMinibatchDefinition
    {
        Minibatch GetNextBatch(DeviceDescriptor device = null);
        Minibatch GetValidationBatch(DeviceDescriptor device = null);
    }
}
