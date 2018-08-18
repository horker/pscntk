using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
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
            var result = Composite.Dense(
                Input,           // Variable input,
                Shape,           // Shape outputShape,
                Initializer,     // CNTKDictionary initializer,
                !NoBias,         // bool hasBias,
                BiasInitializer, // CNTKDictionary biasInitializer,
                Activation,      // string activation
                Name             // string name
            );

            WriteObject(result);
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
        public int[] Strides = new int[] { 1 };

        [Parameter(Position = 4, Mandatory = false)]
        public CNTKDictionary Initializer = null;

        [Parameter(Position = 5, Mandatory = false)]
        public string Activation = null;

        [Parameter(Position = 6, Mandatory = false)]
        public bool[] Padding = new bool[] { true };

        [Parameter(Position = 7, Mandatory = false)]
        public SwitchParameter NoBias = false;

        [Parameter(Position = 8, Mandatory = false)]
        public CNTKDictionary biasInitializer = null;

        [Parameter(Position = 9, Mandatory = false)]
        int[] Dilation = new int[] { 1 };

        [Parameter(Position = 10, Mandatory = false)]
        public int ReductionRank = 1;

        [Parameter(Position = 11, Mandatory = false)]
        public int MaxTempMemSizeInSamples = 0;

        [Parameter(Position = 12, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = Composite.Convolution(
                Input,                   // Variable input
                FilterShape,             // int[] filterShape
                NumFilters,              // int numFilters
                Activation,              // string activation
                Initializer,             // CNTKDictionary initializer
                !NoBias,                 // bool hasBias
                biasInitializer,         // CNTKDictionary biasInitializer
                Strides,                 // int[] strides
                Padding,                 // bool[] padding
                Dilation,                // int[] dilation
                ReductionRank,           // int reductionRank
                1,                       // int groups
                MaxTempMemSizeInSamples, // int maxTempMemSizeInSamples
                Name                     // string name
            );

            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKConv1D")]
    [Alias("cntk.conv1d")]
    public class NewCNTKConv1D : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Input;

        [Parameter(Position = 1, Mandatory = true)]
        public int[] FilterShape;

        [Parameter(Position = 2, Mandatory = true)]
        public int NumFilters;

        [Parameter(Position = 3, Mandatory = false)]
        public int[] Strides = new int[] { 1 };

        [Parameter(Position = 4, Mandatory = false)]
        public CNTKDictionary Initializer = null;

        [Parameter(Position = 5, Mandatory = false)]
        public string Activation = null;

        [Parameter(Position = 6, Mandatory = false)]
        public bool[] Padding = new bool[] { true };

        [Parameter(Position = 7, Mandatory = false)]
        public SwitchParameter NoBias = false;

        [Parameter(Position = 8, Mandatory = false)]
        public CNTKDictionary biasInitializer = null;

        [Parameter(Position = 9, Mandatory = false)]
        int[] Dilation = new int[] { 1 };

        [Parameter(Position = 10, Mandatory = false)]
        public int ReductionRank = 1;

        [Parameter(Position = 11, Mandatory = false)]
        public int MaxTempMemSizeInSamples = 0;

        [Parameter(Position = 12, Mandatory = false)]
        public string Name = "";

        [Parameter(Position = 13, Mandatory = false)]
        public SwitchParameter ChannelFirst = false;

        protected override void EndProcessing()
        {
            var result = Composite.Convolution1D(
                ChannelFirst,
                Input,                   // Variable input
                FilterShape,             // int[] filterShape
                NumFilters,              // int numFilters
                Activation,              // string activation
                Initializer,             // CNTKDictionary initializer
                !NoBias,                 // bool hasBias
                biasInitializer,         // CNTKDictionary biasInitializer
                Strides,                 // int[] strides
                Padding,                 // bool[] padding
                Dilation,                // int[] dilation
                ReductionRank,           // int reductionRank
                1,                       // int groups
                MaxTempMemSizeInSamples, // int maxTempMemSizeInSamples
                Name                     // string name
            );

            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKConv2D")]
    [Alias("cntk.conv2d")]
    public class NewCNTKConv2D : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Input;

        [Parameter(Position = 1, Mandatory = true)]
        public int[] FilterShape;

        [Parameter(Position = 2, Mandatory = true)]
        public int NumFilters;

        [Parameter(Position = 3, Mandatory = false)]
        public int[] Strides = new int[] { 1 };

        [Parameter(Position = 4, Mandatory = false)]
        public CNTKDictionary Initializer = null;

        [Parameter(Position = 5, Mandatory = false)]
        public string Activation = null;

        [Parameter(Position = 6, Mandatory = false)]
        public bool[] Padding = new bool[] { true };

        [Parameter(Position = 7, Mandatory = false)]
        public SwitchParameter NoBias = false;

        [Parameter(Position = 8, Mandatory = false)]
        public CNTKDictionary biasInitializer = null;

        [Parameter(Position = 9, Mandatory = false)]
        int[] Dilation = new int[] { 1 };

        [Parameter(Position = 10, Mandatory = false)]
        public int ReductionRank = 1;

        [Parameter(Position = 11, Mandatory = false)]
        public int MaxTempMemSizeInSamples = 0;

        [Parameter(Position = 12, Mandatory = false)]
        public string Name = "";

        [Parameter(Position = 13, Mandatory = false)]
        public SwitchParameter ChannelFirst = false;

        protected override void EndProcessing()
        {
            var result = Composite.Convolution2D(
                ChannelFirst,
                Input,                   // Variable input
                FilterShape,             // int[] filterShape
                NumFilters,              // int numFilters
                Activation,              // string activation
                Initializer,             // CNTKDictionary initializer
                !NoBias,                 // bool hasBias
                biasInitializer,         // CNTKDictionary biasInitializer
                Strides,                 // int[] strides
                Padding,                 // bool[] padding
                Dilation,                // int[] dilation
                ReductionRank,           // int reductionRank
                1,                       // int groups
                MaxTempMemSizeInSamples, // int maxTempMemSizeInSamples
                Name                     // string name
            );

            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKConv3D")]
    [Alias("cntk.conv3d")]
    public class NewCNTKConv3D : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Input;

        [Parameter(Position = 1, Mandatory = true)]
        public int[] FilterShape;

        [Parameter(Position = 2, Mandatory = true)]
        public int NumFilters;

        [Parameter(Position = 3, Mandatory = false)]
        public int[] Strides = new int[] { 1 };

        [Parameter(Position = 4, Mandatory = false)]
        public CNTKDictionary Initializer = null;

        [Parameter(Position = 5, Mandatory = false)]
        public string Activation = null;

        [Parameter(Position = 6, Mandatory = false)]
        public bool[] Padding = new bool[] { true };

        [Parameter(Position = 7, Mandatory = false)]
        public SwitchParameter NoBias = false;

        [Parameter(Position = 8, Mandatory = false)]
        public CNTKDictionary biasInitializer = null;

        [Parameter(Position = 9, Mandatory = false)]
        int[] Dilation = new int[] { 1 };

        [Parameter(Position = 10, Mandatory = false)]
        public int ReductionRank = 1;

        [Parameter(Position = 11, Mandatory = false)]
        public int MaxTempMemSizeInSamples = 0;

        [Parameter(Position = 12, Mandatory = false)]
        public string Name = "";

        [Parameter(Position = 13, Mandatory = false)]
        public SwitchParameter ChannelFirst = false;

        protected override void EndProcessing()
        {
            var result = Composite.Convolution3D(
                ChannelFirst,
                Input,                   // Variable input
                FilterShape,             // int[] filterShape
                NumFilters,              // int numFilters
                Activation,              // string activation
                Initializer,             // CNTKDictionary initializer
                !NoBias,                 // bool hasBias
                biasInitializer,         // CNTKDictionary biasInitializer
                Strides,                 // int[] strides
                Padding,                 // bool[] padding
                Dilation,                // int[] dilation
                ReductionRank,           // int reductionRank
                1,                       // int groups
                MaxTempMemSizeInSamples, // int maxTempMemSizeInSamples
                Name                     // string name
            );

            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKConvTrans")]
    [Alias("cntk.convtrans")]
    public class NewCNTKConvTrans : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public Variable Input;

        [Parameter(Position = 0, Mandatory = true)]
        public int[] FilterShape;

        [Parameter(Position = 0, Mandatory = true)]
        public int NumFilters;

        [Parameter(Position = 0, Mandatory = false)]
        public string Activation = null;

        [Parameter(Position = 0, Mandatory = false)]
        public CNTKDictionary Initializer = null;

        [Parameter(Position = 0, Mandatory = false)]
        public int[] Strides = new int[] { 1 };

        [Parameter(Position = 0, Mandatory = false)]
        public bool[] Padding = new bool[] { true };

        [Parameter(Position = 0, Mandatory = false)]
        public bool[] Sharing = new bool[] { true };

        [Parameter(Position = 0, Mandatory = false)]
        public SwitchParameter NoBias = false;

        [Parameter(Position = 0, Mandatory = false)]
        public CNTKDictionary BiasInitializer = null;

        [Parameter(Position = 0, Mandatory = false)]
        public int[] OutputShape = new int[] { 0 };

        [Parameter(Position = 0, Mandatory = false)]
        public int[] Dilation = new int[] { 1 };

        [Parameter(Position = 0, Mandatory = false)]
        public int ReductionRank = 1;

        [Parameter(Position = 0, Mandatory = false)]
        public int MaxTempMemSizeInSamples = 0;

        [Parameter(Position = 0, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = Composite.ConvolutionTranspose(
                Input,                   // Variable input
                FilterShape,             // int[] filterShape
                NumFilters,              // int numFilters
                Activation,              // string activation
                Initializer,             // CNTKDictionary initializer
                Padding,                 // bool[] padding
                Strides,                 // int[] strides
                Sharing,                 // bool[] sharing
                !NoBias,                 // bool useBias
                BiasInitializer,         // CNTKDictionary biasInitializer
                OutputShape,             // int[] outputShape
                Dilation,                // int[] dilation
                ReductionRank,           // int reductionRank
                MaxTempMemSizeInSamples, // int maxTempMemSizeInSamples
                Name                     // string name
            );

            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKMaxPooling")]
    [Alias("cntk.maxpooling")]
    public class NewCNTKMaxPooling : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public Variable Input;

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
            var result = CNTKLib.Pooling(Input, PoolingType.Max, FilterShape, Strides, new BoolVector(AutoPadding), CeilOutDim, IncludePad, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKAveragePooling")]
    [Alias("cntk.averagepooling")]
    public class NewCNTKAveragePooling : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public Variable Input;

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
            var result = CNTKLib.Pooling(Input, PoolingType.Average, FilterShape, Strides, new BoolVector(AutoPadding), CeilOutDim, IncludePad, Name);
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
            var result = Composite.BatchNormalization(Input, Spatial, InitialScale, NormalizationTimeConstant, BlendTimeConstant, Epsilon, UseCNTKEngine, DisableRegularization, Name);

            WriteObject(result);
        }
    }

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
            var result = Composite.OptimizedRNNStack(Input, HiddenSize, LayerSize, Bidirectional, CellType, Name);

            WriteObject(result);
        }
    }
}
