using CNTK;

namespace Horker.PSCNTK
{
    public partial class Composite
    {
        // ref. https://github.com/Microsoft/CNTK/blob/master/Examples/TrainingCSharp/Common/LSTMSequenceClassifier.cs

        public static Function Stabilize(Variable input, DeviceDescriptor device, string name)
        {
            var f = Constant.Scalar(4.0f, device);
            var fInv = Constant.Scalar(f.DataType, 1.0 / 4.0f);

            var weight = new Parameter(new NDShape(), f.DataType, 0.99537863 /* 1/f*ln (e^f-1) */, device);

            var beta = CNTKLib.ElementTimes(
                fInv,
                CNTKLib.Log(
                    Constant.Scalar(DataType.Float, 1.0) +
                    CNTKLib.Exp(CNTKLib.ElementTimes(f, weight))));

            return CNTKLib.ElementTimes(beta, input, name);
        }
    }
}