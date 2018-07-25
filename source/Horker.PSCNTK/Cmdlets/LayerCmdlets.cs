using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKOptimizedRNNStack")]
    [Alias("cntk.rnn")]
    public class NewCNTKOptimizedRNNStack : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Input;

        [Parameter(Position = 1, Mandatory = true)]
        public int HiddenSize;

        [Parameter(Position = 2, Mandatory = false)]
        public int LayerSize = 1;

        [Parameter(Position = 3, Mandatory = false)]
        public SwitchParameter Bidirectional = false;

        [Parameter(Position = 4, Mandatory = false)]
        [ValidateSet("lstm", "gru", "rnnTanh", "rnnReLU")]
        public string CellType = "lstm";

        [Parameter(Position = 5, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var dim = Input.Shape.Dimensions[0];

            var weightSize = (dim - 1) * 4 * HiddenSize;
            weightSize += (LayerSize - 1) * (8 * HiddenSize * HiddenSize + 8 * HiddenSize);
            weightSize += 4 * HiddenSize * HiddenSize + 12 * HiddenSize;

            var w = new Parameter(new int[] { weightSize }, DataType.Float, CNTKLib.GlorotUniformInitializer());

            var rnn = CNTKLib.OptimizedRNNStack(Input, w, (uint)HiddenSize, (uint)LayerSize, Bidirectional, CellType, Name);

            var output = CNTKLib.SequenceLast(rnn);

            WriteObject(output);
        }
    }

    [Cmdlet("New", "CNTKDense")]
    [Alias("cntk.dense")]
    public class NewCNTKDense : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Input;

        [Parameter(Position = 1, Mandatory = true)]
        public int HiddenSize;

        [Parameter(Position = 2, Mandatory = true)]
        public CNTKDictionary Initializer;

        protected override void EndProcessing()
        {
            var inDim = Input.Shape.Dimensions[0];

            var weight = new Parameter(new int[] { HiddenSize, inDim }, DataType.Float, Initializer);
            var bias = new Parameter(new int[] { HiddenSize }, DataType.Float, Initializer);

            var output = CNTKLib.Plus(CNTKLib.Times(weight, Input), bias);

            WriteObject(output);
        }
    }
}
