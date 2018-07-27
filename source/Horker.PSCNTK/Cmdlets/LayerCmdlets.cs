using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKOptimizedRNNStack")]
    [Alias("cntk.rnnstack")]
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
            var output = Layers.OptimizedRNNStack(Input, HiddenSize, LayerSize, Bidirectional, CellType, Name);

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
            var output = Layers.Dense(Input, HiddenSize, Initializer);

            WriteObject(output);
        }
    }
}
