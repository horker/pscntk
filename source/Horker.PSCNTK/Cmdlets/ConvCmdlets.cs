using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKConv")]
    [Alias("cntk.conv")]
    [OutputType(typeof(WrappedFunction))]
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

        [Parameter(Position = 4, Mandatory = true)]
        public bool[] Padding = new bool[] { false };

        [Parameter(Position = 5, Mandatory = false)]
        public CNTKDictionary Initializer = null;

        [Parameter(Position = 6, Mandatory = false)]
        public string Activation = null;

        [Parameter(Position = 7, Mandatory = false)]
        public bool Bias = true;

        [Parameter(Position = 8, Mandatory = false)]
        public CNTKDictionary biasInitializer = null;

        [Parameter(Position = 9, Mandatory = false)]
        int[] Dilation = new int[] { 1 };

        [Parameter(Position = 10, Mandatory = false)]
        public int ReductionRank = 1;

        [Parameter(Position = 11, Mandatory = false)]
        public int MaxTempMemSizeInSamples = 0;

        [Parameter(Position = 12, Mandatory = false)]
        public bool Sequential = false;

        [Parameter(Position = 13, Mandatory = false)]
        public string Name = "conv";

        protected override void EndProcessing()
        {
            var result = Composite.Convolution(
                Input,                   // Variable input
                FilterShape,             // int[] filterShape
                NumFilters,              // int numFilters
                Activation,              // string activation
                Initializer,             // CNTKDictionary initializer
                Bias,                    // bool hasBias
                biasInitializer,         // CNTKDictionary biasInitializer
                Strides,                 // int[] strides
                Padding,                 // bool[] padding
                Dilation,                // int[] dilation
                ReductionRank,           // int reductionRank
                1,                       // int groups
                MaxTempMemSizeInSamples, // int maxTempMemSizeInSamples
                Sequential,              // int sequential (from v2.6.0)
                Name                     // string name
            );

            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKConv1D")]
    [Alias("cntk.conv1d")]
    [OutputType(typeof(WrappedFunction))]
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
        public bool[] Padding = new bool[] { false };

        [Parameter(Position = 5, Mandatory = false)]
        public CNTKDictionary Initializer = null;

        [Parameter(Position = 6, Mandatory = false)]
        public string Activation = null;

        [Parameter(Position = 7, Mandatory = false)]
        public bool Bias = true;

        [Parameter(Position = 8, Mandatory = false)]
        public CNTKDictionary biasInitializer = null;

        [Parameter(Position = 9, Mandatory = false)]
        int[] Dilation = new int[] { 1 };

        [Parameter(Position = 10, Mandatory = false)]
        public int ReductionRank = 1;

        [Parameter(Position = 11, Mandatory = false)]
        public int MaxTempMemSizeInSamples = 0;

        [Parameter(Position = 12, Mandatory = false)]
        public bool Sequential = false;

        [Parameter(Position = 13, Mandatory = false)]
        public string Name = "conv1d";

        [Parameter(Position = 14, Mandatory = false)]
        public SwitchParameter ChannelFirst = false;

        protected override void EndProcessing()
        {
            var result = Composite.ConvolutionxD(
                1,
                ChannelFirst,
                Input,                   // Variable input
                FilterShape,             // int[] filterShape
                NumFilters,              // int numFilters
                Activation,              // string activation
                Initializer,             // CNTKDictionary initializer
                Bias,                    // bool hasBias
                biasInitializer,         // CNTKDictionary biasInitializer
                Strides,                 // int[] strides
                Padding,                 // bool[] padding
                Dilation,                // int[] dilation
                ReductionRank,           // int reductionRank
                1,                       // int groups
                MaxTempMemSizeInSamples, // int maxTempMemSizeInSamples
                Sequential,              // bool sequential (from v2.6.0)
                Name                     // string name
            );

            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKConv2D")]
    [Alias("cntk.conv2d")]
    [OutputType(typeof(WrappedFunction))]
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
        public bool[] Padding = new bool[] { false };

        [Parameter(Position = 5, Mandatory = false)]
        public CNTKDictionary Initializer = null;

        [Parameter(Position = 6, Mandatory = false)]
        public string Activation = null;

        [Parameter(Position = 7, Mandatory = false)]
        public bool Bias = true;

        [Parameter(Position = 8, Mandatory = false)]
        public CNTKDictionary biasInitializer = null;

        [Parameter(Position = 9, Mandatory = false)]
        int[] Dilation = new int[] { 1 };

        [Parameter(Position = 10, Mandatory = false)]
        public int ReductionRank = 1;

        [Parameter(Position = 11, Mandatory = false)]
        public int MaxTempMemSizeInSamples = 0;

        [Parameter(Position = 12, Mandatory = false)]
        public bool Sequential = false;

        [Parameter(Position = 13, Mandatory = false)]
        public string Name = "conv2d";

        [Parameter(Position = 14, Mandatory = false)]
        public SwitchParameter ChannelFirst = false;

        protected override void EndProcessing()
        {
            var result = Composite.ConvolutionxD(
                2,
                ChannelFirst,
                Input,                   // Variable input
                FilterShape,             // int[] filterShape
                NumFilters,              // int numFilters
                Activation,              // string activation
                Initializer,             // CNTKDictionary initializer
                Bias,                    // bool hasBias
                biasInitializer,         // CNTKDictionary biasInitializer
                Strides,                 // int[] strides
                Padding,                 // bool[] padding
                Dilation,                // int[] dilation
                ReductionRank,           // int reductionRank
                1,                       // int groups
                MaxTempMemSizeInSamples, // int maxTempMemSizeInSamples
                Sequential,              // bool sequential (from v2.6.0)
                Name                     // string name
            );

            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKConv3D")]
    [Alias("cntk.conv3d")]
    [OutputType(typeof(WrappedFunction))]
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
        public bool[] Padding = new bool[] { false };

        [Parameter(Position = 5, Mandatory = false)]
        public CNTKDictionary Initializer = null;

        [Parameter(Position = 6, Mandatory = false)]
        public string Activation = null;

        [Parameter(Position = 7, Mandatory = false)]
        public bool Bias = true;

        [Parameter(Position = 8, Mandatory = false)]
        public CNTKDictionary biasInitializer = null;

        [Parameter(Position = 9, Mandatory = false)]
        int[] Dilation = new int[] { 1 };

        [Parameter(Position = 10, Mandatory = false)]
        public int ReductionRank = 1;

        [Parameter(Position = 11, Mandatory = false)]
        public int MaxTempMemSizeInSamples = 0;

        [Parameter(Position = 12, Mandatory = false)]
        public bool Sequential = false;

        [Parameter(Position = 13, Mandatory = false)]
        public string Name = "conv3d";

        [Parameter(Position = 14, Mandatory = false)]
        public SwitchParameter ChannelFirst = false;

        protected override void EndProcessing()
        {
            var result = Composite.ConvolutionxD(
                3,
                ChannelFirst,
                Input,                   // Variable input
                FilterShape,             // int[] filterShape
                NumFilters,              // int numFilters
                Activation,              // string activation
                Initializer,             // CNTKDictionary initializer
                Bias,                    // bool hasBias
                biasInitializer,         // CNTKDictionary biasInitializer
                Strides,                 // int[] strides
                Padding,                 // bool[] padding
                Dilation,                // int[] dilation
                ReductionRank,           // int reductionRank
                1,                       // int groups
                MaxTempMemSizeInSamples, // int maxTempMemSizeInSamples
                Sequential,              // bool sequential (from v2.6.0)
                Name                     // string name
            );

            WriteObject(new WrappedFunction(result));
        }
    }
}
