using System;
using System.Linq;
using System.Reflection;
using CNTK;

namespace Horker.PSCNTK
{
    public partial class Composite
    {
        public static Function ConvolutionTranspose(Variable input, int[] filterShape, int numFilters, string activation, CNTKDictionary initializer, bool[] padding, int[] strides, bool[] sharing, bool useBias, CNTKDictionary biasInitializer, int[] outputShape, int[] dilation, int reductionRank, int maxTempMemSizeInSamples, string name)
        {
            // Assume input variable's dimensions = (x, [y, [z,]] channels)

            if (filterShape.Length != input.Shape.Rank - 1)
                throw new ArgumentException("Length of filterShape should match input's dimension minus one");

            if (strides.Length != 1 && strides.Length != input.Shape.Rank - 1)
                throw new ArgumentException("Length of strides should be one, or match input's dimension minus one");

            // Initializers

            if (initializer == null)
                initializer = CNTKLib.GlorotUniformInitializer();

            if (useBias && biasInitializer == null)
                biasInitializer = CNTKLib.ConstantInitializer(0);

            // Convolution map
            // convolutionMap = CNTK.Parameter (I, O, kernelWidth, kernelHeight, numInputChannels, featureMapCount)

            var convDims = new int[filterShape.Length + 2];
            filterShape.CopyTo(convDims, 2);
            convDims[0] = input.Shape.Dimensions.Aggregate((a, b) => a * b);
            convDims[1] = numFilters; // feature map count

            var convolutionMap = new Parameter(convDims, DataType.Float, initializer);

            // Strides

            var st = new int[input.Shape.Rank]; // plus channel
            if (strides.Length == 1)
                for (var i = 0; i < input.Shape.Rank - 1; ++i)
                    st[i] = strides[0];
            else
                strides.CopyTo(st, 0);

            st[input.Shape.Rank - 1] = numFilters;

            var conv = CNTKLib.ConvolutionTranspose(
                convolutionMap,                // CNTK.Variable convolutionMap
                input,                         // CNTK.Variable operand
                st,                            // CNTK.NDShape strides
                new BoolVector(sharing),       // CNTK.BoolVector sharing
                new BoolVector(padding),       // CNTK.BoolVector autoPadding
                outputShape,                   // CNTK.NDShape outputShape
                dilation,                      // CNTK.NDShape dilation
                (uint)reductionRank,           // uint reductionRank
                (uint)maxTempMemSizeInSamples, // uint maxTempMemSizeInSamples
                name                           //  string name
            );

            if (useBias)
            {
                var bias = new Parameter(conv.Output.Shape, DataType.Float, biasInitializer);
                conv = CNTKLib.Plus(conv, bias);
            }

            conv = Helpers.ApplyActivation(conv, activation);

            return conv;
        }
    }
}
