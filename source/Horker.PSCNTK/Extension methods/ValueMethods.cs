using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNTK;

namespace Horker.PSCNTK
{
    public class ValueMethods
    {
        public static Value SafeCreate(NDShape shape, float[] data, DeviceDescriptor device)
        {
            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();

            return new Value(new NDArrayView(shape, data, device, false)).DeepClone();
        }
    }
}
