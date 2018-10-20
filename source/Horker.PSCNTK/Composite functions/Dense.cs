using System;
using System.Linq;
using System.Reflection;
using CNTK;

namespace Horker.PSCNTK
{
    public partial class Composite
    {
        public static Function Dense(Variable input, Shape outputShape, CNTKDictionary initializer, bool useBias, CNTKDictionary biasInitializer, bool stabilize, string activation, DeviceDescriptor device, string name)
        {
            try
            {
                NodeGroup.EnterNewGroup(name);

                if (initializer == null)
                    initializer = CNTKLib.GlorotUniformInitializer();

                if (useBias && biasInitializer == null)
                    biasInitializer = CNTKLib.ConstantInitializer(0);

                if (input.Shape.Rank > 1)
                {
                    int newDim = input.Shape.Dimensions.Aggregate((d1, d2) => d1 * d2);
                    input = CNTKLib.Reshape(input, new int[] { newDim });
                    Register(input);
                }

                var inDim = input.Shape.Dimensions[0];

                int hiddenSize = outputShape.Dimensions.Aggregate((d1, d2) => d1 * d2);

                var weight = new Parameter(new int[] { hiddenSize, inDim }, DataType.Float, initializer, device, name + "_w");
                Register(weight);

                Parameter bias = null;
                if (useBias)
                {
                    bias = new Parameter(new int[] { hiddenSize }, DataType.Float, biasInitializer, device, name + "_b");
                    Register(bias);
                }

                if (stabilize)
                    input = Stabilize(input, device, name + "_st");

                var output = GetAffine(input, weight, bias);

                if (outputShape.Rank > 1)
                {
                    output = CNTKLib.Reshape(output, outputShape.Dimensions);
                    Register(output);
                }

                output = ApplyActivation(output, activation);

                output.RootFunction.SetName(name);

                return output;
            }
            finally
            {
                NodeGroup.LeaveGroup();
            }
        }
    }
}
