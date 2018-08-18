﻿using System;
using System.Linq;
using System.Reflection;
using CNTK;

namespace Horker.PSCNTK
{
    public partial class Composite
    {
        public static Function Convolution(Variable input, int[] filterShape, int numFilters, string activation, CNTKDictionary initializer, bool useBias, CNTKDictionary biasInitializer, int[] strides, bool[] padding, int[] dilation, int reductionRank, int groups, int maxTempMemSizeInSamples, string name)
        {
            // Initializers

            if (initializer == null)
                initializer = CNTKLib.GlorotUniformInitializer();

            if (useBias && biasInitializer == null)
                biasInitializer = CNTKLib.ConstantInitializer(0);

            // Convolution map
            // (kernelWidth, kernelHeight, kernelInputChannels, featureMapCount)

            var convDims = new int[filterShape.Length + 1];
            filterShape.CopyTo(convDims, 0);
            convDims[filterShape.Length] = numFilters; // feature map count

            var convolutionMap = new Parameter(convDims, DataType.Float, initializer);

            var conv = CNTKLib.Convolution(
                convolutionMap,                      // CNTK.Variable convolutionMap
                input,                               // CNTK.Variable operand
                strides,                             // CNTK.NDShape strides
                new BoolVector(new bool[] { true }), // CNTK.BoolVector sharing (false is not supported)
                new BoolVector(padding),             // CNTK.BoolVector autoPadding
                dilation,                            // CNTK.NDShape dilation
                (uint)reductionRank,                 // uint reductionRank
                (uint)groups,                        // uint groups
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

        private static int[] FillShapeArray(int[] shape, int numDimensions, Variable input, bool channelFirst)
        {
            var result = new int[numDimensions + 1];

            var offset = channelFirst ? 0 : 1;

            result[offset * numDimensions] = input.Shape.Dimensions[offset * numDimensions];

            shape.CopyTo(result, 1 - offset);
            for (var i = shape.Length; i < numDimensions; ++i)
                result[i + offset] = shape[shape.Length - 1];

            return result;
        }

        private static Function ConvolutionxD(int numDimensions, bool channelFirst, Variable input, int[] filterShape, int numFilters, string activation, CNTKDictionary initializer, bool useBias, CNTKDictionary biasInitializer, int[] strides, bool[] padding, int[] dilation, int reductionRank, int groups, int maxTempMemSizeInSamples, string name)
        {
            if (strides.Length != 1 && filterShape.Length != numDimensions)
                throw new ArgumentException("Dimensions of filterShape should be " + numDimensions);

            if (strides.Length != 1 && strides.Length != 2)
                throw new ArgumentException("Dimensions of strides should be 1 or " + numDimensions);

            var fil = FillShapeArray(filterShape, numDimensions, input, channelFirst);
            var st = FillShapeArray(strides, numDimensions, input, channelFirst);

            return Convolution(input, fil, numFilters, activation, initializer, useBias, biasInitializer, st, padding, dilation, reductionRank, groups, maxTempMemSizeInSamples, name);
        }

        public static Function Convolution1D(bool channelFirst, Variable input, int[] filterShape, int numFilters, string activation, CNTKDictionary initializer, bool useBias, CNTKDictionary biasInitializer, int[] strides, bool[] padding, int[] dilation, int reductionRank, int groups, int maxTempMemSizeInSamples, string name)
        {
            return ConvolutionxD(1, channelFirst, input, filterShape, numFilters, activation, initializer, useBias, biasInitializer, strides, padding, dilation, reductionRank, groups, maxTempMemSizeInSamples, name);
        }

        public static Function Convolution2D(bool channelFirst, Variable input, int[] filterShape, int numFilters, string activation, CNTKDictionary initializer, bool useBias, CNTKDictionary biasInitializer, int[] strides, bool[] padding, int[] dilation, int reductionRank, int groups, int maxTempMemSizeInSamples, string name)
        {
            return ConvolutionxD(2, channelFirst, input, filterShape, numFilters, activation, initializer, useBias, biasInitializer, strides, padding, dilation, reductionRank, groups, maxTempMemSizeInSamples, name);
        }

        public static Function Convolution3D(bool channelFirst, Variable input, int[] filterShape, int numFilters, string activation, CNTKDictionary initializer, bool useBias, CNTKDictionary biasInitializer, int[] strides, bool[] padding, int[] dilation, int reductionRank, int groups, int maxTempMemSizeInSamples, string name)
        {
            return ConvolutionxD(3, channelFirst, input, filterShape, numFilters, activation, initializer, useBias, biasInitializer, strides, padding, dilation, reductionRank, groups, maxTempMemSizeInSamples, name);
        }
    }
}