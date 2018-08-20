using System;
using System.Linq;
using System.Reflection;
using CNTK;

namespace Horker.PSCNTK
{
    public partial class Composite
    {
        public static Function Dense(Variable input, Shape outputShape, CNTKDictionary initializer, bool useBias, CNTKDictionary biasInitializer, string activation, string name)
        {
            if (initializer == null)
                initializer = CNTKLib.GlorotUniformInitializer();

            if (useBias && biasInitializer == null)
                biasInitializer = CNTKLib.ConstantInitializer(0);

            if (input.Shape.Rank > 1)
            {
                int newDim = input.Shape.Dimensions.Aggregate((d1, d2) => d1 * d2);
                input = CNTKLib.Reshape(input, new int[] { newDim });
            }

            var inDim = input.Shape.Dimensions[0];

            int hiddenSize = outputShape.Dimensions.Aggregate((d1, d2) => d1 * d2);

            var weight = new Parameter(new int[] { hiddenSize, inDim }, DataType.Float, initializer, DeviceDescriptor.UseDefaultDevice(), name + "_w");

            Function output;

            if (useBias)
            {
                var bias = new Parameter(new int[] { hiddenSize }, DataType.Float, biasInitializer, DeviceDescriptor.UseDefaultDevice(), name + "_b");
                output = CNTKLib.Plus(CNTKLib.Times(weight, input), bias);
            }
            else
                output = CNTKLib.Times(weight, input);

            if (outputShape.Rank > 1)
                output = CNTKLib.Reshape(output, outputShape.Dimensions);

            output = Helpers.ApplyActivation(output, activation);

            return output;
        }
    }
}
