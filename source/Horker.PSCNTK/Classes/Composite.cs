using System;
using System.Linq;
using System.Reflection;
using CNTK;

namespace Horker.PSCNTK
{
    public class Composite
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

            var weight = new Parameter(new int[] { hiddenSize, inDim }, DataType.Float, initializer);

            Function output;

            if (useBias)
            {
                var bias = new Parameter(new int[] { hiddenSize }, DataType.Float, biasInitializer);
                output = CNTKLib.Plus(CNTKLib.Times(weight, input), bias);
            }
            else
                output = CNTKLib.Times(weight, input);

            if (outputShape.Rank > 1)
                output = CNTKLib.Reshape(output, outputShape.Dimensions);

            output = Helpers.ApplyActivation(output, activation);

            return output;
        }

        public static Function Convolution(Variable input, int[] filterShape, int numFilters, string activation, CNTKDictionary initializer, bool useBias, CNTKDictionary biasInitializer, int[] strides, bool[] sharing, bool[] padding, int[] dilation, int reductionRank, int groups, int maxTempMemSizeInSamples, string name)
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
            // convolutionMap = CNTK.Parameter (kernelWidth, kernelHeight, numInputChannels, featureMapCount) initializer

            var convDims = new int[filterShape.Length + 2];
            filterShape.CopyTo(convDims, 0);
            convDims[filterShape.Length] = input.Shape.Dimensions[filterShape.Length]; // number of channels
            convDims[filterShape.Length + 1] = numFilters; // feature map count

            var convolutionMap = new Parameter(convDims, DataType.Float, initializer);

            // Strides

            var st = new int[input.Shape.Rank]; // plus channel
            if (strides.Length == 1)
                for (var i = 0; i < input.Shape.Rank - 1; ++i)
                    st[i] = strides[0];
            else
                strides.CopyTo(st, 0);

            st[input.Shape.Rank - 1] = numFilters;

            var conv = CNTKLib.Convolution(
                convolutionMap,                // CNTK.Variable convolutionMap
                input,                         // CNTK.Variable operand
                st,                            // CNTK.NDShape strides
                new BoolVector(sharing),       // CNTK.BoolVector sharing
                new BoolVector(padding),       // CNTK.BoolVector autoPadding
                dilation,                      // CNTK.NDShape dilation
                (uint)reductionRank,           // uint reductionRank
                (uint)groups,                  // uint groups
                (uint)maxTempMemSizeInSamples, // uint maxTempMemSizeInSamples
                name                           // string name
            );

            if (useBias)
            {
                var bias = new Parameter(conv.Output.Shape, DataType.Float, biasInitializer);
                conv = CNTKLib.Plus(conv, bias);
            }

            conv = Helpers.ApplyActivation(conv, activation);

            return conv;
        }

        public static Function BatchNormalization(Variable input, bool spatial, double initScale, double normalizationTimeConstant, double blendTimeConstant, double epsilon, bool useCNTKEngine, bool disableRegularization, string name)
        {
            var normShape = new int[] { CNTK.NDShape.InferredDimension };
            var scale = new Parameter(normShape, DataType.Float, initScale);
            var bias = new Parameter(normShape, DataType.Float, 0);
            var runningMean = new Constant(normShape, 0.0f, DeviceDescriptor.UseDefaultDevice());
            var runningInvStd = new Constant(normShape, 0.0f, DeviceDescriptor.UseDefaultDevice());
            var runningCount = Constant.Scalar(0.0f, DeviceDescriptor.UseDefaultDevice());

            var output = CNTKLib.BatchNormalization(
                input,                     // CNTK.Variable operand
                scale,                     // CNTK.Variable scale
                bias,                      // CNTK.Variable bias
                runningMean,               // CNTK.Variable runningMean
                runningInvStd,             // CNTK.Variable runningInvStd
                runningCount,              // CNTK.Variable runningCount
                spatial,                   // bool spatial
                normalizationTimeConstant, // double normalizationTimeConstant
                blendTimeConstant,         // double blendTimeConstant
                epsilon,                   // double epsilon
                !useCNTKEngine,            // bool useCuDNNEngine
                disableRegularization,     // bool disableRegularization
                name                       // string name
            );

            return output;
        }

        public static Function OptimizedRNNStack(Variable input, int hiddenSize, int layerSize = 1, bool bidirectional = false, string cellType = "lstm", string name = "")
        {
            var dim = input.Shape.Dimensions[0];

            var weightSize = (dim - 1) * 4 * hiddenSize;
            weightSize += (layerSize - 1) * (8 * hiddenSize * hiddenSize + 8 * hiddenSize);
            weightSize += 4 * hiddenSize * hiddenSize + 12 * hiddenSize;

            var w = new Parameter(new int[] { weightSize }, DataType.Float, CNTKLib.GlorotUniformInitializer());

            var rnn = CNTKLib.OptimizedRNNStack(input, w, (uint)hiddenSize, (uint)layerSize, bidirectional, cellType, name);

            var output = CNTKLib.SequenceLast(rnn);

            return output;
        }
    }
}
