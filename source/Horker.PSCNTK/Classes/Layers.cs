using System;
using System.Linq;
using System.Reflection;
using CNTK;

namespace Horker.PSCNTK
{
    public class Layers
    {
        public static Function Dense(Variable input, Shape outputShape, CNTKDictionary initializer = null, CNTKDictionary biasInitializer = null, bool hasBias = true, string activation = null)
        {
            if (initializer == null)
                initializer = CNTKLib.GlorotUniformInitializer();

            if (biasInitializer == null)
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

            if (hasBias)
            {
                var bias = new Parameter(new int[] { hiddenSize }, DataType.Float, biasInitializer);
                output = CNTKLib.Plus(CNTKLib.Times(weight, input), bias);
            }
            else
                output = CNTKLib.Times(weight, input);

            if (outputShape.Rank > 1)
                output = CNTKLib.Reshape(output, outputShape.Dimensions);

            if (activation != null)
            {
                var m = Helpers.GetCNTKLibMethod(activation);
                output = (Function)m.Invoke(null, new object[] { output });
            }

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
