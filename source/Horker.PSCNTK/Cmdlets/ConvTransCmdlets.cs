using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKConvTrans")]
    [Alias("cntk.convTrans")]
    public class NewCNTKConvTrans : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public Variable Input;

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
        public CNTKDictionary BiasInitializer = null;

        [Parameter(Position = 9, Mandatory = false)]
        public int[] OutputShape = new int[] { 0 };

        [Parameter(Position = 10, Mandatory = false)]
        public int[] Dilation = new int[] { 1 };

        [Parameter(Position = 11, Mandatory = false)]
        public int ReductionRank = 1;

        [Parameter(Position = 12, Mandatory = false)]
        public int MaxTempMemSizeInSamples = 0;

        [Parameter(Position = 13, Mandatory = false)]
        public string Name = "convTrans";

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
                Bias,                    // bool useBias
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

    [Cmdlet("New", "CNTKConvTrans1D")]
    [Alias("cntk.convTrans1d")]
    public class NewCNTKConvTrans1D : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public Variable Input;

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
        public CNTKDictionary BiasInitializer = null;

        [Parameter(Position = 9, Mandatory = false)]
        public int[] OutputShape = new int[] { 0 };

        [Parameter(Position = 10, Mandatory = false)]
        public int[] Dilation = new int[] { 1 };

        [Parameter(Position = 11, Mandatory = false)]
        public int ReductionRank = 1;

        [Parameter(Position = 12, Mandatory = false)]
        public int MaxTempMemSizeInSamples = 0;

        [Parameter(Position = 13, Mandatory = false)]
        public string Name = "convTrans1d";

        [Parameter(Position = 14, Mandatory = false)]
        public SwitchParameter ChannelFirst = false;

        protected override void EndProcessing()
        {
            var result = Composite.ConvolutionTransposexD(
                1,
                ChannelFirst,
                Input,                   // Variable input
                FilterShape,             // int[] filterShape
                NumFilters,              // int numFilters
                Activation,              // string activation
                Initializer,             // CNTKDictionary initializer
                Padding,                 // bool[] padding
                Strides,                 // int[] strides
                Bias,                    // bool useBias
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

    [Cmdlet("New", "CNTKConvTrans2D")]
    [Alias("cntk.convTrans2d")]
    public class NewCNTKConvTrans2D : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public Variable Input;

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
        public CNTKDictionary BiasInitializer = null;

        [Parameter(Position = 9, Mandatory = false)]
        public int[] OutputShape = new int[] { 0 };

        [Parameter(Position = 10, Mandatory = false)]
        public int[] Dilation = new int[] { 1 };

        [Parameter(Position = 11, Mandatory = false)]
        public int ReductionRank = 1;

        [Parameter(Position = 12, Mandatory = false)]
        public int MaxTempMemSizeInSamples = 0;

        [Parameter(Position = 13, Mandatory = false)]
        public string Name = "convTrans2d";

        [Parameter(Position = 14, Mandatory = false)]
        public SwitchParameter ChannelFirst = false;

        protected override void EndProcessing()
        {
            var result = Composite.ConvolutionTransposexD(
                2,
                ChannelFirst,
                Input,                   // Variable input
                FilterShape,             // int[] filterShape
                NumFilters,              // int numFilters
                Activation,              // string activation
                Initializer,             // CNTKDictionary initializer
                Padding,                 // bool[] padding
                Strides,                 // int[] strides
                Bias,                    // bool useBias
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

    [Cmdlet("New", "CNTKConvTrans3D")]
    [Alias("cntk.convTrans3d")]
    public class NewCNTKConvTrans3D : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public Variable Input;

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
        public CNTKDictionary BiasInitializer = null;

        [Parameter(Position = 9, Mandatory = false)]
        public int[] OutputShape = new int[] { 0 };

        [Parameter(Position = 10, Mandatory = false)]
        public int[] Dilation = new int[] { 1 };

        [Parameter(Position = 11, Mandatory = false)]
        public int ReductionRank = 1;

        [Parameter(Position = 12, Mandatory = false)]
        public int MaxTempMemSizeInSamples = 0;

        [Parameter(Position = 13, Mandatory = false)]
        public string Name = "convTrans3d";

        [Parameter(Position = 14, Mandatory = false)]
        public SwitchParameter ChannelFirst = false;

        protected override void EndProcessing()
        {
            var result = Composite.ConvolutionTransposexD(
                3,
                ChannelFirst,
                Input,                   // Variable input
                FilterShape,             // int[] filterShape
                NumFilters,              // int numFilters
                Activation,              // string activation
                Initializer,             // CNTKDictionary initializer
                Padding,                 // bool[] padding
                Strides,                 // int[] strides
                Bias,                    // bool useBias
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
}
