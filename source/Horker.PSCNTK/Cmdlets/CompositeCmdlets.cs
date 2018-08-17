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
            var output = Composite.OptimizedRNNStack(Input, HiddenSize, LayerSize, Bidirectional, CellType, Name);

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
        public int[] Shape;

        [Parameter(Position = 2, Mandatory = false)]
        public CNTKDictionary Initializer = null;

        [Parameter(Position = 3, Mandatory = false)]
        public string Activation = null;

        [Parameter(Position = 4, Mandatory = false)]
        public SwitchParameter NoBias = false;

        [Parameter(Position = 5, Mandatory = false)]
        public CNTKDictionary BiasInitializer = null;

        [Parameter(Position = 6, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var output = Composite.Dense(
                Input,           // Variable input,
                Shape,           // Shape outputShape,
                Initializer,     // CNTKDictionary initializer,
                !NoBias,         // bool hasBias,
                BiasInitializer, // CNTKDictionary biasInitializer,
                Activation,      // string activation
                Name             // string name
            );

            WriteObject(output);
        }
    }

    [Cmdlet("New", "CNTKConv")]
    [Alias("cntk.conv")]
    public class NewCNTKConv : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Input;

        [Parameter(Position = 1, Mandatory = true)]
        public int[] FilterShape;

        [Parameter(Position = 2, Mandatory = true)]
        public int NumFilters;

        [Parameter(Position = 3, Mandatory = false)]
        public CNTKDictionary Initializer = null;

        [Parameter(Position = 4, Mandatory = false)]
        public string Activation = null;

        [Parameter(Position = 5, Mandatory = false)]
        public bool[] Pad = new bool[] { true };

        [Parameter(Position = 6, Mandatory = false)]
        public int[] Strides = new int[] { 1 };

        [Parameter(Position = 7, Mandatory = false)]
        public bool[] Sharing = new bool[] { true };

        [Parameter(Position = 8, Mandatory = false)]
        public SwitchParameter NoBias = false;

        [Parameter(Position = 9, Mandatory = false)]
        public CNTKDictionary biasInitializer = null;

        [Parameter(Position = 10, Mandatory = false)]
        public int ReductionRank = 1;

        [Parameter(Position = 11, Mandatory = false)]
        public int MaxTempMemSizeInSamples = 0;

        [Parameter(Position = 12, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var output = Composite.Convolution(
                Input,                   // Variable input
                FilterShape,             // int[] filterShape
                NumFilters,              // int numFilters
                Activation,              // string activation
                Initializer,             // CNTKDictionary initializer
                !NoBias,                 // bool hasBias
                biasInitializer,         // CNTKDictionary biasInitializer
                Strides,                 // int[] strides
                Sharing,                 // bool[] sharing
                Pad,                     // bool[] padding
                new int[] { 1 },         // int[] dilation
                ReductionRank,           // int reductionRank
                1,                       // int groups
                MaxTempMemSizeInSamples, // int maxTempMemSizeInSamples
                Name                     // string name
            );

            WriteObject(output);
        }
    }

    [Cmdlet("New", "CNTKMaxPooling")]
    [Alias("cntk.maxpooling")]
    public class NewCNTKMaxPooling : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Input;

        [Parameter(Position = 1, Mandatory = true)]
        public int[] FilterShape;

        [Parameter(Position = 2, Mandatory = false)]
        public int[] Strides = new int[] { 1 };

        [Parameter(Position = 3, Mandatory = false)]
        public bool[] AutoPadding = new bool[] { true };

        [Parameter(Position = 4, Mandatory = false)]
        public bool CeilOutDim = false;

        [Parameter(Position = 5, Mandatory = false)]
        public bool IncludePad = false;

        [Parameter(Position = 6, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Pooling(Input, PoolingType.Max, FilterShape, Strides, new CNTK.BoolVector(AutoPadding), CeilOutDim, IncludePad, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKAveragePooling")]
    [Alias("cntk.averagepooling")]
    public class NewCNTKAveragePooling : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Input;

        [Parameter(Position = 1, Mandatory = true)]
        public int[] FilterShape;

        [Parameter(Position = 2, Mandatory = false)]
        public int[] Strides = new int[] { 1 };

        [Parameter(Position = 3, Mandatory = false)]
        public bool[] AutoPadding = new bool[] { true };

        [Parameter(Position = 4, Mandatory = false)]
        public bool CeilOutDim = false;

        [Parameter(Position = 5, Mandatory = false)]
        public bool IncludePad = false;

        [Parameter(Position = 6, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Pooling(Input, PoolingType.Average, FilterShape, Strides, new CNTK.BoolVector(AutoPadding), CeilOutDim, IncludePad, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKBatchNorm")]
    [Alias("cntk.batchnorm")]
    public class NewCNTKBatchNorm : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public Variable Input;

        [Parameter(Position = 1, Mandatory = false)]
        public SwitchParameter Spatial = false;

        [Parameter(Position = 2, Mandatory = false)]
        public double InitialScale = 1.0;

        [Parameter(Position = 3, Mandatory = false)]
        public double NormalizationTimeConstant = 5000;

        [Parameter(Position = 4, Mandatory = false)]
        public double BlendTimeConstant = 0;

        [Parameter(Position = 5, Mandatory = false)]
        public double Epsilon = 0.00001;

        [Parameter(Position = 6, Mandatory = false)]
        public SwitchParameter UseCNTKEngine = false;

        [Parameter(Position = 7, Mandatory = false)]
        public SwitchParameter DisableRegularization = false;

        [Parameter(Position = 8, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var output = Composite.BatchNormalization(Input, Spatial, InitialScale, NormalizationTimeConstant, BlendTimeConstant, Epsilon, UseCNTKEngine, DisableRegularization, Name);

            WriteObject(output);
        }
    }
}
