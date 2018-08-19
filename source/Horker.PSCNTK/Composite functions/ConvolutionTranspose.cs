using System;
using System.Linq;
using System.Reflection;
using CNTK;

namespace Horker.PSCNTK
{
    public partial class Composite
    {
        public static Function ConvolutionTranspose(Variable input, int[] filterShape, int numFilters, string activation, CNTKDictionary initializer, bool[] padding, int[] strides, bool useBias, CNTKDictionary biasInitializer, int[] outputShape, int[] dilation, int reductionRank, int maxTempMemSizeInSamples, string name)
        {
            // Initializers

            if (initializer == null)
                initializer = CNTKLib.GlorotUniformInitializer();

            if (useBias && biasInitializer == null)
                biasInitializer = CNTKLib.ConstantInitializer(0);

            // Convolution map
            // (kernelWidth, kernelHeight, kernelInputChannels, numChannels, featureMapCount)

            var convDims = new int[filterShape.Length + 1];
            filterShape.CopyTo(convDims, 0);
            convDims[filterShape.Length] = numFilters; // feature map count

            var convolutionMap = new Parameter(convDims, DataType.Float, initializer);

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
                name                                 // string name
            );

            if (useBias)
            {
                var bias = new Parameter(conv.Output.Shape, DataType.Float, biasInitializer);
                conv = CNTKLib.Plus(conv, bias);
            }

            conv = Helpers.ApplyActivation(conv, activation);

            return conv;
        }

        public static Function ConvolutionTransposexD(int numDimensions, bool channelFirst, Variable input, int[] filterShape, int numFilters, string activation, CNTKDictionary initializer, bool[] padding, int[] strides, bool useBias, CNTKDictionary biasInitializer, int[] outputShape, int[] dilation, int reductionRank, int maxTempMemSizeInSamples, string name)
        {
            if (filterShape.Length > numDimensions)
                throw new ArgumentException("Dimensions of filterShape should be <= " + numDimensions);

            if (strides.Length > numDimensions)
                throw new ArgumentException("Dimensions of strides should be <= " + numDimensions);

            var fil = FillShapeArray(filterShape, numDimensions, input, channelFirst);
            var st = FillShapeArray(strides, numDimensions, input, channelFirst);

            return ConvolutionTranspose(input, fil, numFilters, activation, initializer, padding, st, useBias, biasInitializer, outputShape, dilation, reductionRank, maxTempMemSizeInSamples, name);
        }
    }
}
