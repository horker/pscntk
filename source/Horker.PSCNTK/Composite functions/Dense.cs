using System;
using System.Linq;
using System.Reflection;
using CNTK;

namespace Horker.PSCNTK
{
    public partial class Composite
    {
        public static Function Dense(Variable input, int[] outputDimensions, CNTKDictionary initializer, bool useBias, CNTKDictionary biasInitializer, bool stabilize, double steepness, string activation, DeviceDescriptor device, string name)
        {
            try
            {
                NodeGroup.EnterNewGroup(name);

                if (outputDimensions == null)
                    outputDimensions = new Shape(input.Shape.Dimensions.ToArray());

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

                var inputDimensions = input.Shape.Dimensions[0];

                int hiddenSize = outputDimensions.Aggregate((d1, d2) => d1 * d2);

                var weight = new Parameter(new int[] { hiddenSize, inputDimensions }, DataType.Float, initializer, device, name + "/weight");
                Register(weight);

                Parameter bias = null;
                if (useBias)
                {
                    bias = new Parameter(new int[] { hiddenSize }, DataType.Float, biasInitializer, device, name + "/bias");
                    Register(bias);
                }

                if (stabilize)
                    input = Stabilize(input, steepness, device, name + "/stabilizer");

                var output = GetAffine(input, weight, bias);

                if (outputDimensions.Length > 1)
                {
                    output = CNTKLib.Reshape(output, outputDimensions);
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
