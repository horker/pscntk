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
            // NOTE 1)
            // The readOnly flag is meaningless because the data of NDArrayView are inaccessible from .NET anyway.
            // We always set it to false.
            // NOTE 2)
            // An NDArrayView object created by NDArrayView(NDShape, float[], DeviceDescriptor, readOnly) constructor
            // DOES NOT hold reference to its data. This will cause destructive memory corruption and make the whole process crash.
            // To make the object keep track of its data by itself, make a copy of it by calling DeepClone().

            // Enclose with unsafe to avoid garbage collection.
            unsafe
            {
                var a = new NDArrayView(shape, data, DeviceDescriptor.CPUDevice, false);
                return a.DeepClone(device, false);
            }
        }
    }
}
