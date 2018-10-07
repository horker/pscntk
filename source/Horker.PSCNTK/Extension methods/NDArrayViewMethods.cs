using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNTK;

namespace Horker.PSCNTK
{
    public class NDArrayViewMethods
    {
        public static NDArrayView SafeCreate(NDShape shape, float[] data, DeviceDescriptor device)
        {
            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();

            if (device == DeviceDescriptor.CPUDevice)
            {
                unsafe
                {
                    fixed (float* f = data)
                    {
                        using (var a = new NDArrayView(shape, data, DeviceDescriptor.CPUDevice, false))
                        {
                            // The NDArrayview constructor with float[] data does not copy nor hold the reference to the source data.
                            // To make the object keep track of its data by itself, make a copy of it by calling DeepClone().
                            return a.DeepClone(device, false);
                        }
                    }
                }
            }

            // Allocating in GPU memory inevitably causes copying.
            return new NDArrayView(shape, data, device, false);
        }
    }
}
