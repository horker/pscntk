using System.Linq;
using CNTK;

namespace Horker.PSCNTK
{
    public class Layers
    {
        public static Function Dense(Variable input, int hiddenSize, CNTKDictionary initializer)
        {
            if (input.Shape.Rank > 1)
            {
                int newDim = input.Shape.Dimensions.Aggregate((d1, d2) => d1 * d2);
                input = CNTKLib.Reshape(input, new int[] { newDim });
            }

            var inDim = input.Shape.Dimensions[0];

            var weight = new Parameter(new int[] { hiddenSize, inDim }, DataType.Float, initializer);
            var bias = new Parameter(new int[] { hiddenSize }, DataType.Float, initializer);

            var output = CNTKLib.Plus(CNTKLib.Times(weight, input), bias);

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
