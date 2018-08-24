using System;
using System.Linq;
using System.Reflection;
using CNTK;

namespace Horker.PSCNTK
{
    public partial class Composite
    {
        // Assume input shape is such as (x [, y [, z]], channels)
        public static Function ConvolutionTranspose(Variable input, int[] filterShape, int numFilters, string activation, CNTKDictionary initializer, bool[] padding, int[] strides, bool useBias, CNTKDictionary biasInitializer, int[] outputShape, int[] dilation, int reductionRank, int maxTempMemSizeInSamples, string name)
        {
            try
            {
                NodeGroup.EnterNewGroup(name);

                // Initializers

                if (initializer == null)
                    initializer = CNTKLib.GlorotUniformInitializer();

                if (useBias && biasInitializer == null)
                    biasInitializer = CNTKLib.ConstantInitializer(0);

                // Convolution map
                // (kernelWidth, kernelHeight, featureMapCount, kernelChannel)

                var convDims = new int[filterShape.Length + 2];
                filterShape.CopyTo(convDims, 0);
                convDims[convDims.Length - 2] = numFilters;
                convDims[convDims.Length - 1] = input.Shape.Dimensions[filterShape.Length]; // input channel

                var convolutionMap = new Parameter(convDims, DataType.Float, initializer, DeviceDescriptor.UseDefaultDevice(), name + "_w");
                Register(convolutionMap);

                var conv = CNTKLib.ConvolutionTranspose(
                    convolutionMap,                      // CNTK.Variable convolutionMap
                    input,                               // CNTK.Variable operand
                    strides,                             // CNTK.NDShape strides
                    new BoolVector(new bool[] { true }), // CNTK.BoolVector sharing
                    new BoolVector(padding),             // CNTK.BoolVector autoPadding
                    outputShape,                         // CNTK.NDShape outputShape
                    dilation,                            // CNTK.NDShape dilation
                    (uint)reductionRank,                 // uint reductionRank
                    (uint)maxTempMemSizeInSamples,       // uint maxTempMemSizeInSamples
                    ""                                   // string name
                );
                Register(conv);

                if (useBias)
                {
                    var bias = new Parameter(conv.Output.Shape, DataType.Float, biasInitializer, DeviceDescriptor.UseDefaultDevice(), name + "_b");
                    Register(bias);
                    conv = CNTKLib.Plus(conv, bias);
                    Register(conv);
                }

                conv = ApplyActivation(conv, activation);

                conv.RootFunction.SetName(name);

                return conv;
            }
            finally
            {
                NodeGroup.LeaveGroup();
            }
        }

        public static Function ConvolutionTransposexD(int numDimensions, bool channelFirst, Variable input, int[] filterShape, int numFilters, string activation, CNTKDictionary initializer, bool[] padding, int[] strides, bool useBias, CNTKDictionary biasInitializer, int[] outputShape, int[] dilation, int reductionRank, int maxTempMemSizeInSamples, string name)
        {
            if (filterShape.Length > numDimensions)
                throw new ArgumentException("Dimensions of filterShape should be <= " + numDimensions);

            if (strides.Length > numDimensions)
                throw new ArgumentException("Dimensions of strides should be <= " + numDimensions);

            var st = FillShapeArray(strides, numDimensions, input, channelFirst);

            return ConvolutionTranspose(input, filterShape, numFilters, activation, initializer, padding, st, useBias, biasInitializer, outputShape, dilation, reductionRank, maxTempMemSizeInSamples, name);
        }
    }
}
