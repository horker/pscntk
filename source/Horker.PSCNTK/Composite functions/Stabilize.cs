using System;
using CNTK;

namespace Horker.PSCNTK
{
    public partial class Composite
    {
        public static Function Stabilize(Variable input, double steepness, DeviceDescriptor device, string name = "")
        {
            var f = Constant.Scalar(DataType.Float, steepness, device);
            var fInv = Constant.Scalar(DataType.Float, 1.0 / steepness, device);

            var initial = Math.Log(Math.Exp(steepness) - 1) / steepness;

            var param = new Parameter(new NDShape(), DataType.Float, initial, device, name + "/weight");
            var beta = CNTKLib.ElementTimes(fInv, CNTKLib.Softplus(CNTKLib.ElementTimes(f, param)));
            return CNTKLib.ElementTimes(beta, input, name);
        }
    }
}