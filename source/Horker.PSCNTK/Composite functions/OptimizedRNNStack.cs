using System;
using System.Linq;
using System.Reflection;
using CNTK;

namespace Horker.PSCNTK
{
    public partial class Composite
    {
        public static Function OptimizedRNNStack(Variable input, int hiddenSize, int layerSize = 1, bool bidirectional = false, string cellType = "lstm", string name = "")
        {
            var dim = input.Shape.Dimensions[0];

            var weightSize = (dim - 1) * 4 * hiddenSize;
            weightSize += (layerSize - 1) * (8 * hiddenSize * hiddenSize + 8 * hiddenSize);
            weightSize += 4 * hiddenSize * hiddenSize + 12 * hiddenSize;

            var w = new Parameter(new int[] { weightSize }, DataType.Float, CNTKLib.GlorotUniformInitializer(), DeviceDescriptor.UseDefaultDevice(), name + "_w");

            var rnn = CNTKLib.OptimizedRNNStack(input, w, (uint)hiddenSize, (uint)layerSize, bidirectional, cellType, name + "_rnn");

            var output = CNTKLib.SequenceLast(rnn);

            output.RootFunction.SetName(name);

            return output;
        }
    }
}
