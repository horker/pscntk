using System;
using System.Management.Automation;

// DO NOT EDIT
// This file was automatically generated at 2018/10/20 19:35:57

namespace Horker.PSCNTK {

    [Cmdlet("New", "CNTKAbs")]
    [Alias("cntk.abs")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKAbs : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Abs(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKAcos")]
    [Alias("cntk.acos")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKAcos : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Acos(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKAlias")]
    [Alias("cntk.alias")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKAlias : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Alias(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKArgmax")]
    [Alias("cntk.argmax")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKArgmax : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Argmax(Operand, Axis, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKArgmin")]
    [Alias("cntk.argmin")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKArgmin : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Argmin(Operand, Axis, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKAsComposite")]
    [Alias("cntk.ascomposite")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKAsComposite : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedFunction RootFunction;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.AsComposite(RootFunction, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKAsin")]
    [Alias("cntk.asin")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKAsin : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Asin(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKAsinh")]
    [Alias("cntk.asinh")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKAsinh : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Asinh(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKAssign")]
    [Alias("cntk.assign")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKAssign : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Ref;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Assign(Ref, Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKAtanh")]
    [Alias("cntk.atanh")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKAtanh : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Atanh(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKBatchNormalization")]
    [Alias("cntk.batchnormalization")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKBatchNormalization : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable Scale;

        [Parameter(Position = 2, Mandatory = true)]
        public WrappedVariable Bias;

        [Parameter(Position = 3, Mandatory = true)]
        public WrappedVariable RunningMean;

        [Parameter(Position = 4, Mandatory = true)]
        public WrappedVariable RunningInvStd;

        [Parameter(Position = 5, Mandatory = true)]
        public WrappedVariable RunningCount;

        [Parameter(Position = 6, Mandatory = true)]
        public bool Spatial;

        [Parameter(Position = 7, Mandatory = false)]
        public double NormalizationTimeConstant = 0;

        [Parameter(Position = 8, Mandatory = false)]
        public double BlendTimeConstant = 0;

        [Parameter(Position = 9, Mandatory = false)]
        public double Epsilon = 0.00001;

        [Parameter(Position = 10, Mandatory = false)]
        public bool UseCuDNNEngine = true;

        [Parameter(Position = 11, Mandatory = false)]
        public bool DisableRegularization = false;

        [Parameter(Position = 12, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.BatchNormalization(Operand, Scale, Bias, RunningMean, RunningInvStd, RunningCount, Spatial, NormalizationTimeConstant, BlendTimeConstant, Epsilon, UseCuDNNEngine, DisableRegularization, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKBernoulliRandom")]
    [Alias("cntk.bernoullirandom")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKBernoulliRandom : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public int[] Shape;

        [Parameter(Position = 1, Mandatory = false)]
        public CNTK.DataType DataType = CNTK.DataType.Float;

        [Parameter(Position = 2, Mandatory = false)]
        public double Mean = 0.5;

        [Parameter(Position = 3, Mandatory = false)]
        public UInt32 Seed = Constants.SentinelValueForAutoSelectRandomSeed;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.BernoulliRandom(Shape, DataType, Mean, Seed, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKBernoulliRandomLike")]
    [Alias("cntk.bernoullirandomlike")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKBernoulliRandomLike : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public double Mean = 0.5;

        [Parameter(Position = 2, Mandatory = false)]
        public UInt32 Seed = Constants.SentinelValueForAutoSelectRandomSeed;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.BernoulliRandomLike(Operand, Mean, Seed, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKBilinearInitializer")]
    [Alias("cntk.init.bilinear")]
    [OutputType(typeof(CNTK.CNTKDictionary))]
    public class NewCNTKBilinearInitializer : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public UInt32 KernelWidth;

        [Parameter(Position = 1, Mandatory = true)]
        public UInt32 KernelHeight;

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.BilinearInitializer(KernelWidth, KernelHeight);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKBinaryCrossEntropy")]
    [Alias("cntk.binarycrossentropy")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKBinaryCrossEntropy : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Prediction;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable Targets;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.BinaryCrossEntropy(Prediction, Targets, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSequenceBroadcastAs")]
    [Alias("cntk.sequence.broadcastas")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKSequenceBroadcastAs : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable BroadcastAs;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SequenceBroadcastAs(Operand, BroadcastAs, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKCast")]
    [Alias("cntk.cast")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKCast : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable NodeInput;

        [Parameter(Position = 1, Mandatory = false)]
        public CNTK.DataType OutputType = CNTK.DataType.Float;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Cast(NodeInput, OutputType, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKCeil")]
    [Alias("cntk.ceil")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKCeil : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Ceil(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKClip")]
    [Alias("cntk.clip")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKClip : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable Min;

        [Parameter(Position = 2, Mandatory = true)]
        public WrappedVariable Max;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Clip(Operand, Min, Max, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKCombine")]
    [Alias("cntk.combine")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKCombine : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable[] Operands;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Combine(new CNTK.VariableVector(Operands), Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKConstantInitializer")]
    [Alias("cntk.init.constant")]
    [OutputType(typeof(CNTK.CNTKDictionary))]
    public class NewCNTKConstantInitializer : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = false)]
        public double Value = 0.0;

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ConstantInitializer(Value);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKConvolution")]
    [Alias("cntk.convolution")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKConvolution : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable ConvolutionMap;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 2, Mandatory = false)]
        public int[] Strides = new int[] { 1 };

        [Parameter(Position = 3, Mandatory = false)]
        public bool[] Sharing = new bool[] { true };

        [Parameter(Position = 4, Mandatory = false)]
        public bool[] AutoPadding = new bool[] { true };

        [Parameter(Position = 5, Mandatory = false)]
        public int[] Dilation = new int[] { 1 };

        [Parameter(Position = 6, Mandatory = false)]
        public UInt32 ReductionRank = 1;

        [Parameter(Position = 7, Mandatory = false)]
        public UInt32 Groups = 1;

        [Parameter(Position = 8, Mandatory = false)]
        public UInt32 MaxTempMemSizeInSamples = 0;

        [Parameter(Position = 9, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Convolution(ConvolutionMap, Operand, Strides, new CNTK.BoolVector(Sharing), new CNTK.BoolVector(AutoPadding), Dilation, ReductionRank, Groups, MaxTempMemSizeInSamples, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKConvolutionTranspose")]
    [Alias("cntk.convolutiontranspose")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKConvolutionTranspose : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable ConvolutionMap;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 2, Mandatory = false)]
        public int[] Strides = new int[] { 1 };

        [Parameter(Position = 3, Mandatory = false)]
        public bool[] Sharing = new bool[] { true };

        [Parameter(Position = 4, Mandatory = false)]
        public bool[] AutoPadding = new bool[] { true };

        [Parameter(Position = 5, Mandatory = false)]
        public int[] OutputShape = new int[] { 0 };

        [Parameter(Position = 6, Mandatory = false)]
        public int[] Dilation = new int[] { 1 };

        [Parameter(Position = 7, Mandatory = false)]
        public UInt32 ReductionRank = 1;

        [Parameter(Position = 8, Mandatory = false)]
        public UInt32 MaxTempMemSizeInSamples = 0;

        [Parameter(Position = 9, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ConvolutionTranspose(ConvolutionMap, Operand, Strides, new CNTK.BoolVector(Sharing), new CNTK.BoolVector(AutoPadding), OutputShape, Dilation, ReductionRank, MaxTempMemSizeInSamples, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKCos")]
    [Alias("cntk.cos")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKCos : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Cos(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKCosh")]
    [Alias("cntk.cosh")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKCosh : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Cosh(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKCosineDistance")]
    [Alias("cntk.cosinedistance")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKCosineDistance : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable RightOperand;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.CosineDistance(LeftOperand, RightOperand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKCosineDistanceWithNegativeSamples")]
    [Alias("cntk.cosinedistancewithnegativesamples")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKCosineDistanceWithNegativeSamples : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable RightOperand;

        [Parameter(Position = 2, Mandatory = true)]
        public UInt32 ShiftWindow;

        [Parameter(Position = 3, Mandatory = true)]
        public UInt32 NumberOfNegativeSamples;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.CosineDistanceWithNegativeSamples(LeftOperand, RightOperand, ShiftWindow, NumberOfNegativeSamples, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKDepthToSpace")]
    [Alias("cntk.depthtospace")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKDepthToSpace : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public UInt32 BlockSize;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.DepthToSpace(Operand, BlockSize, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKDropout")]
    [Alias("cntk.dropout")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKDropout : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public double DropoutRate;

        [Parameter(Position = 2, Mandatory = false)]
        public UInt32 Seed = Constants.SentinelValueForAutoSelectRandomSeed;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Dropout(Operand, DropoutRate, Seed, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKEditDistanceError")]
    [Alias("cntk.editdistanceerror")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKEditDistanceError : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Prediction;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable Labels;

        [Parameter(Position = 2, Mandatory = true)]
        public float SubstitutionPenalty;

        [Parameter(Position = 3, Mandatory = true)]
        public float DeletionPenalty;

        [Parameter(Position = 4, Mandatory = true)]
        public float InsertionPenalty;

        [Parameter(Position = 5, Mandatory = true)]
        public bool SquashInputs;

        [Parameter(Position = 6, Mandatory = true)]
        public UInt32[] TokensToIgnore;

        [Parameter(Position = 7, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.EditDistanceError(Prediction, Labels, SubstitutionPenalty, DeletionPenalty, InsertionPenalty, SquashInputs, new CNTK.SizeTVector(TokensToIgnore), Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKElementAnd")]
    [Alias("cntk.elementand")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKElementAnd : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable RightOperand;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ElementAnd(LeftOperand, RightOperand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKElementDivide")]
    [Alias("cntk.elementdivide")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKElementDivide : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable RightOperand;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ElementDivide(LeftOperand, RightOperand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKElementMax")]
    [Alias("cntk.elementmax")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKElementMax : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable RightOperand;

        [Parameter(Position = 2, Mandatory = true)]
        public string Name;

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ElementMax(LeftOperand, RightOperand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKElementMin")]
    [Alias("cntk.elementmin")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKElementMin : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable RightOperand;

        [Parameter(Position = 2, Mandatory = true)]
        public string Name;

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ElementMin(LeftOperand, RightOperand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKElementNot")]
    [Alias("cntk.elementnot")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKElementNot : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ElementNot(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKElementOr")]
    [Alias("cntk.elementor")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKElementOr : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable RightOperand;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ElementOr(LeftOperand, RightOperand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKElementSelect")]
    [Alias("cntk.elementselect")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKElementSelect : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Condition;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable ThenOperand;

        [Parameter(Position = 2, Mandatory = true)]
        public WrappedVariable ElseOperand;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ElementSelect(Condition, ThenOperand, ElseOperand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKElementTimes")]
    [Alias("cntk.elementtimes")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKElementTimes : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable RightOperand;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ElementTimes(LeftOperand, RightOperand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKElementXor")]
    [Alias("cntk.elementxor")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKElementXor : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable RightOperand;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ElementXor(LeftOperand, RightOperand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKEqual")]
    [Alias("cntk.equal")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKEqual : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable RightOperand;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Equal(LeftOperand, RightOperand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKExp")]
    [Alias("cntk.exp")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKExp : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Exp(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKExpandDims")]
    [Alias("cntk.expanddims")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKExpandDims : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ExpandDims(Operand, Axis, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSequenceFirst")]
    [Alias("cntk.sequence.first")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKSequenceFirst : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SequenceFirst(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKFloor")]
    [Alias("cntk.floor")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKFloor : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Floor(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKForwardBackward")]
    [Alias("cntk.forwardbackward")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKForwardBackward : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Graph;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable Features;

        [Parameter(Position = 2, Mandatory = true)]
        public UInt32 BlankTokenId;

        [Parameter(Position = 3, Mandatory = true)]
        public int DelayConstraint;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ForwardBackward(Graph, Features, BlankTokenId, DelayConstraint, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKGlorotNormalInitializer")]
    [Alias("cntk.init.glorotnormal", "cntk.glorotnormal")]
    [OutputType(typeof(CNTK.CNTKDictionary))]
    public class NewCNTKGlorotNormalInitializer : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = false)]
        public double Scale = Constants.DefaultParamInitScale;

        [Parameter(Position = 1, Mandatory = false)]
        public int OutputRank = Constants.SentinelValueForInferParamInitRank;

        [Parameter(Position = 2, Mandatory = false)]
        public int FilterRank = Constants.SentinelValueForInferParamInitRank;

        [Parameter(Position = 3, Mandatory = false)]
        public UInt32 Seed = Constants.SentinelValueForAutoSelectRandomSeed;

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.GlorotNormalInitializer(Scale, OutputRank, FilterRank, Seed);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKGlorotUniformInitializer")]
    [Alias("cntk.init.glorotuniform", "cntk.glorotuniform")]
    [OutputType(typeof(CNTK.CNTKDictionary))]
    public class NewCNTKGlorotUniformInitializer : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = false)]
        public double Scale = Constants.DefaultParamInitScale;

        [Parameter(Position = 1, Mandatory = false)]
        public int OutputRank = Constants.SentinelValueForInferParamInitRank;

        [Parameter(Position = 2, Mandatory = false)]
        public int FilterRank = Constants.SentinelValueForInferParamInitRank;

        [Parameter(Position = 3, Mandatory = false)]
        public UInt32 Seed = Constants.SentinelValueForAutoSelectRandomSeed;

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.GlorotUniformInitializer(Scale, OutputRank, FilterRank, Seed);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKGreater")]
    [Alias("cntk.greater")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKGreater : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable RightOperand;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Greater(LeftOperand, RightOperand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKGreaterEqual")]
    [Alias("cntk.greaterequal")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKGreaterEqual : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable RightOperand;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.GreaterEqual(LeftOperand, RightOperand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKGumbelRandom")]
    [Alias("cntk.gumbelrandom")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKGumbelRandom : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public int[] Shape;

        [Parameter(Position = 1, Mandatory = false)]
        public CNTK.DataType DataType = CNTK.DataType.Float;

        [Parameter(Position = 2, Mandatory = false)]
        public double Loc = 0.0;

        [Parameter(Position = 3, Mandatory = false)]
        public double Scale = 1.0;

        [Parameter(Position = 4, Mandatory = false)]
        public UInt32 Seed = Constants.SentinelValueForAutoSelectRandomSeed;

        [Parameter(Position = 5, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.GumbelRandom(Shape, DataType, Loc, Scale, Seed, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKGumbelRandomLike")]
    [Alias("cntk.gumbelrandomlike")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKGumbelRandomLike : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public double Loc = 0.0;

        [Parameter(Position = 2, Mandatory = false)]
        public double Scale = 1.0;

        [Parameter(Position = 3, Mandatory = false)]
        public UInt32 Seed = Constants.SentinelValueForAutoSelectRandomSeed;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.GumbelRandomLike(Operand, Loc, Scale, Seed, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKHardmax")]
    [Alias("cntk.hardmax")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKHardmax : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Hardmax(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKHardSigmoid")]
    [Alias("cntk.hardsigmoid")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKHardSigmoid : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public float Alpha;

        [Parameter(Position = 2, Mandatory = true)]
        public float Beta;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.HardSigmoid(Operand, Alpha, Beta, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKHeNormalInitializer")]
    [Alias("cntk.init.henormal", "cntk.henormal")]
    [OutputType(typeof(CNTK.CNTKDictionary))]
    public class NewCNTKHeNormalInitializer : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = false)]
        public double Scale = Constants.DefaultParamInitScale;

        [Parameter(Position = 1, Mandatory = false)]
        public int OutputRank = Constants.SentinelValueForInferParamInitRank;

        [Parameter(Position = 2, Mandatory = false)]
        public int FilterRank = Constants.SentinelValueForInferParamInitRank;

        [Parameter(Position = 3, Mandatory = false)]
        public UInt32 Seed = Constants.SentinelValueForAutoSelectRandomSeed;

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.HeNormalInitializer(Scale, OutputRank, FilterRank, Seed);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKHeUniformInitializer")]
    [Alias("cntk.init.heuniform", "cntk.heuniform")]
    [OutputType(typeof(CNTK.CNTKDictionary))]
    public class NewCNTKHeUniformInitializer : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = false)]
        public double Scale = Constants.DefaultParamInitScale;

        [Parameter(Position = 1, Mandatory = false)]
        public int OutputRank = Constants.SentinelValueForInferParamInitRank;

        [Parameter(Position = 2, Mandatory = false)]
        public int FilterRank = Constants.SentinelValueForInferParamInitRank;

        [Parameter(Position = 3, Mandatory = false)]
        public UInt32 Seed = Constants.SentinelValueForAutoSelectRandomSeed;

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.HeUniformInitializer(Scale, OutputRank, FilterRank, Seed);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKImageScaler")]
    [Alias("cntk.imagescaler")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKImageScaler : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public float Scaler;

        [Parameter(Position = 2, Mandatory = true)]
        public float[] Biases;

        [Parameter(Position = 3, Mandatory = true)]
        public string Name;

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ImageScaler(Operand, Scaler, new CNTK.FloatVector(Biases), Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSequenceIsFirst")]
    [Alias("cntk.sequence.isfirst")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKSequenceIsFirst : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SequenceIsFirst(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSequenceIsLast")]
    [Alias("cntk.sequence.islast")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKSequenceIsLast : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SequenceIsLast(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKLabelsToGraph")]
    [Alias("cntk.labelstograph")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKLabelsToGraph : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Labels;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.LabelsToGraph(Labels, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKLambdaRank")]
    [Alias("cntk.lambdarank")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKLambdaRank : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Prediction;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable Gains;

        [Parameter(Position = 2, Mandatory = true)]
        public WrappedVariable GroupId;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.LambdaRank(Prediction, Gains, GroupId, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSequenceLast")]
    [Alias("cntk.sequence.last")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKSequenceLast : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SequenceLast(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKLatticeSequenceWithSoftmax")]
    [Alias("cntk.latticesequencewithsoftmax")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKLatticeSequenceWithSoftmax : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Labels;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable Prediction;

        [Parameter(Position = 2, Mandatory = true)]
        public WrappedVariable ScaledLogLikelihood;

        [Parameter(Position = 3, Mandatory = true)]
        public WrappedVariable Lattice;

        [Parameter(Position = 4, Mandatory = true)]
        public string SymbolListPath;

        [Parameter(Position = 5, Mandatory = true)]
        public string PhonePath;

        [Parameter(Position = 6, Mandatory = true)]
        public string StateListPath;

        [Parameter(Position = 7, Mandatory = true)]
        public string TransitionProbabilityPath;

        [Parameter(Position = 8, Mandatory = true)]
        public string ConfigFilePath;

        [Parameter(Position = 9, Mandatory = true)]
        public float SmoothingWeight;

        [Parameter(Position = 10, Mandatory = true)]
        public float FrameDropThreshold;

        [Parameter(Position = 11, Mandatory = true)]
        public bool DoReferenceAlign;

        [Parameter(Position = 12, Mandatory = true)]
        public bool GammarUsesMBR;

        [Parameter(Position = 13, Mandatory = true)]
        public float GammarAMF;

        [Parameter(Position = 14, Mandatory = true)]
        public float GammarLMF;

        [Parameter(Position = 15, Mandatory = true)]
        public float GammarBMMIFactor;

        [Parameter(Position = 16, Mandatory = true)]
        public float GammarWordPenalty;

        [Parameter(Position = 17, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.LatticeSequenceWithSoftmax(Labels, Prediction, ScaledLogLikelihood, Lattice, SymbolListPath, PhonePath, StateListPath, TransitionProbabilityPath, ConfigFilePath, SmoothingWeight, FrameDropThreshold, DoReferenceAlign, GammarUsesMBR, GammarAMF, GammarLMF, GammarBMMIFactor, GammarWordPenalty, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKLeakyReLU")]
    [Alias("cntk.leakyrelu")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKLeakyReLU : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public double Alpha;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.LeakyReLU(Operand, Alpha, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKLess")]
    [Alias("cntk.less")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKLess : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable RightOperand;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Less(LeftOperand, RightOperand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKLessEqual")]
    [Alias("cntk.lessequal")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKLessEqual : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable RightOperand;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.LessEqual(LeftOperand, RightOperand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKLocalResponseNormalization")]
    [Alias("cntk.localresponsenormalization")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKLocalResponseNormalization : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public UInt32 DepthRadius;

        [Parameter(Position = 2, Mandatory = true)]
        public double Bias;

        [Parameter(Position = 3, Mandatory = true)]
        public double Alpha;

        [Parameter(Position = 4, Mandatory = true)]
        public double Beta;

        [Parameter(Position = 5, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.LocalResponseNormalization(Operand, DepthRadius, Bias, Alpha, Beta, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKLog")]
    [Alias("cntk.log")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKLog : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Log(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKLogAddExp")]
    [Alias("cntk.logaddexp")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKLogAddExp : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable RightOperand;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.LogAddExp(LeftOperand, RightOperand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKMean")]
    [Alias("cntk.mean")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKMean : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable[] Operands;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Mean(new CNTK.VariableVector(Operands), Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKMinus")]
    [Alias("cntk.minus")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKMinus : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable RightOperand;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Minus(LeftOperand, RightOperand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKNDCGAt1")]
    [Alias("cntk.ndcgat1")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKNDCGAt1 : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Prediction;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable Gains;

        [Parameter(Position = 2, Mandatory = true)]
        public WrappedVariable GroupId;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.NDCGAt1(Prediction, Gains, GroupId, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKNegate")]
    [Alias("cntk.negate")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKNegate : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Negate(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKNormalInitializer")]
    [Alias("cntk.init.normal", "cntk.normal")]
    [OutputType(typeof(CNTK.CNTKDictionary))]
    public class NewCNTKNormalInitializer : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public double Scale;

        [Parameter(Position = 1, Mandatory = false)]
        public int OutputRank = Constants.SentinelValueForInferParamInitRank;

        [Parameter(Position = 2, Mandatory = false)]
        public int FilterRank = Constants.SentinelValueForInferParamInitRank;

        [Parameter(Position = 3, Mandatory = false)]
        public UInt32 Seed = Constants.SentinelValueForAutoSelectRandomSeed;

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.NormalInitializer(Scale, OutputRank, FilterRank, Seed);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKNormalRandom")]
    [Alias("cntk.normalrandom")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKNormalRandom : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public int[] Shape;

        [Parameter(Position = 1, Mandatory = false)]
        public CNTK.DataType DataType = CNTK.DataType.Float;

        [Parameter(Position = 2, Mandatory = false)]
        public double Mean = 0.0;

        [Parameter(Position = 3, Mandatory = false)]
        public double Scale = 1.0;

        [Parameter(Position = 4, Mandatory = false)]
        public UInt32 Seed = Constants.SentinelValueForAutoSelectRandomSeed;

        [Parameter(Position = 5, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.NormalRandom(Shape, DataType, Mean, Scale, Seed, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKNormalRandomLike")]
    [Alias("cntk.normalrandomlike")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKNormalRandomLike : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public double Mean = 0.0;

        [Parameter(Position = 2, Mandatory = false)]
        public double Scale = 1.0;

        [Parameter(Position = 3, Mandatory = false)]
        public UInt32 Seed = Constants.SentinelValueForAutoSelectRandomSeed;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.NormalRandomLike(Operand, Mean, Scale, Seed, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKNotEqual")]
    [Alias("cntk.notequal")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKNotEqual : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable RightOperand;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.NotEqual(LeftOperand, RightOperand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKOneHotOp")]
    [Alias("cntk.onehotop")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKOneHotOp : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public UInt32 NumClass;

        [Parameter(Position = 2, Mandatory = true)]
        public bool OutputSparse;

        [Parameter(Position = 3, Mandatory = true)]
        public CNTK.Axis Axis;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.OneHotOp(Operand, NumClass, OutputSparse, Axis, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKOnesLike")]
    [Alias("cntk.oneslike")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKOnesLike : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.OnesLike(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKPad")]
    [Alias("cntk.pad")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKPad : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.PaddingMode Mode;

        [Parameter(Position = 2, Mandatory = true)]
        public UInt32[] Head;

        [Parameter(Position = 3, Mandatory = true)]
        public UInt32[] Foot;

        [Parameter(Position = 4, Mandatory = false)]
        public double ConstantValue = 0;

        [Parameter(Position = 5, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Pad(Operand, Mode, new CNTK.SizeTVector(Head), new CNTK.SizeTVector(Foot), ConstantValue, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKPlus")]
    [Alias("cntk.plus")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKPlus : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable RightOperand;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Plus(LeftOperand, RightOperand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKPooling")]
    [Alias("cntk.pooling")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKPooling : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.PoolingType PoolingType;

        [Parameter(Position = 2, Mandatory = true)]
        public int[] PoolingWindowShape;

        [Parameter(Position = 3, Mandatory = false)]
        public int[] Strides = new int[] { 1 };

        [Parameter(Position = 4, Mandatory = false)]
        public bool[] AutoPadding = new bool[] { false };

        [Parameter(Position = 5, Mandatory = false)]
        public bool CeilOutDim = false;

        [Parameter(Position = 6, Mandatory = false)]
        public bool IncludePad = false;

        [Parameter(Position = 7, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Pooling(Operand, PoolingType, PoolingWindowShape, Strides, new CNTK.BoolVector(AutoPadding), CeilOutDim, IncludePad, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKPow")]
    [Alias("cntk.pow")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKPow : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable RightOperand;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Pow(LeftOperand, RightOperand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKPReLU")]
    [Alias("cntk.prelu")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKPReLU : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Alpha;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.PReLU(Alpha, Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKRandomSample")]
    [Alias("cntk.randomsample")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKRandomSample : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public UInt32 NumSamples;

        [Parameter(Position = 2, Mandatory = true)]
        public bool AllowDuplicates;

        [Parameter(Position = 3, Mandatory = false)]
        public UInt32 Seed = Constants.SentinelValueForAutoSelectRandomSeed;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.RandomSample(Operand, NumSamples, AllowDuplicates, Seed, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKRandomSampleInclusionFrequency")]
    [Alias("cntk.randomsampleinclusionfrequency")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKRandomSampleInclusionFrequency : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public UInt32 NumSamples;

        [Parameter(Position = 2, Mandatory = true)]
        public bool AllowDuplicates;

        [Parameter(Position = 3, Mandatory = false)]
        public UInt32 Seed = Constants.SentinelValueForAutoSelectRandomSeed;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.RandomSampleInclusionFrequency(Operand, NumSamples, AllowDuplicates, Seed, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKReciprocal")]
    [Alias("cntk.reciprocal")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKReciprocal : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Reciprocal(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKReconcileDynamicAxes")]
    [Alias("cntk.reconciledynamicaxes")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKReconcileDynamicAxes : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable AxesAsOperand;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReconcileDynamicAxes(Operand, AxesAsOperand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKReduceL1")]
    [Alias("cntk.reducel1")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKReduceL1 : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis[] Axes;

        [Parameter(Position = 2, Mandatory = false)]
        public bool KeepDims = true;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceL1(Operand, new CNTK.AxisVector(Axes), KeepDims, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKReduceL2")]
    [Alias("cntk.reducel2")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKReduceL2 : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis[] Axes;

        [Parameter(Position = 2, Mandatory = false)]
        public bool KeepDims = true;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceL2(Operand, new CNTK.AxisVector(Axes), KeepDims, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSequenceReduceMax")]
    [Alias("cntk.sequence.reducemax")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKSequenceReduceMax : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SequenceReduceMax(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSequenceReduceSum")]
    [Alias("cntk.sequence.reducesum")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKSequenceReduceSum : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SequenceReduceSum(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKReduceSumSquare")]
    [Alias("cntk.reducesumsquare")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKReduceSumSquare : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis[] Axes;

        [Parameter(Position = 2, Mandatory = false)]
        public bool KeepDims = true;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceSumSquare(Operand, new CNTK.AxisVector(Axes), KeepDims, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKReLU")]
    [Alias("cntk.relu")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKReLU : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReLU(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKROIPooling")]
    [Alias("cntk.roipooling")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKROIPooling : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable Rois;

        [Parameter(Position = 2, Mandatory = true)]
        public CNTK.PoolingType PoolingType;

        [Parameter(Position = 3, Mandatory = true)]
        public int[] RoiOutputShape;

        [Parameter(Position = 4, Mandatory = true)]
        public double SpatialScale;

        [Parameter(Position = 5, Mandatory = true)]
        public string Name;

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ROIPooling(Operand, Rois, PoolingType, RoiOutputShape, SpatialScale, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKRound")]
    [Alias("cntk.round")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKRound : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Round(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSELU")]
    [Alias("cntk.selu")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKSELU : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public double Gamma = 1.0507009873554804934193349852946;

        [Parameter(Position = 2, Mandatory = false)]
        public double Alpha = 1.6732632423543772848170429916717;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SELU(Operand, Gamma, Alpha, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSigmoid")]
    [Alias("cntk.sigmoid")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKSigmoid : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Sigmoid(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSin")]
    [Alias("cntk.sin")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKSin : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Sin(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSinh")]
    [Alias("cntk.sinh")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKSinh : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Sinh(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSequenceSlice")]
    [Alias("cntk.sequence.slice")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKSequenceSlice : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public int BeginIndex;

        [Parameter(Position = 2, Mandatory = true)]
        public int EndIndex;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SequenceSlice(Operand, BeginIndex, EndIndex, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSequenceSoftmax")]
    [Alias("cntk.sequence.softmax")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKSequenceSoftmax : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SequenceSoftmax(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSoftplus")]
    [Alias("cntk.softplus")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKSoftplus : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Softplus(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSoftsign")]
    [Alias("cntk.softsign")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKSoftsign : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Softsign(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSpaceToDepth")]
    [Alias("cntk.spacetodepth")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKSpaceToDepth : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public UInt32 BlockSize;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SpaceToDepth(Operand, BlockSize, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSplice")]
    [Alias("cntk.splice")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKSplice : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable[] Operands;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Splice(new CNTK.VariableVector(Operands), Axis, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSqrt")]
    [Alias("cntk.sqrt")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKSqrt : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Sqrt(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSquare")]
    [Alias("cntk.square")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKSquare : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Square(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSquaredError")]
    [Alias("cntk.squarederror")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKSquaredError : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Prediction;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable Targets;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SquaredError(Prediction, Targets, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKStopGradient")]
    [Alias("cntk.stopgradient")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKStopGradient : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.StopGradient(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSum")]
    [Alias("cntk.sum")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKSum : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable[] Operands;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Sum(new CNTK.VariableVector(Operands), Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKTanh")]
    [Alias("cntk.tanh")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKTanh : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Tanh(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKToBatch")]
    [Alias("cntk.tobatch")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKToBatch : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ToBatch(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKToSequenceLike")]
    [Alias("cntk.tosequencelike")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKToSequenceLike : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable DynamicAxesLike;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ToSequenceLike(Operand, DynamicAxesLike, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKTransposeAxes")]
    [Alias("cntk.transposeaxes")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKTransposeAxes : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis Axis1;

        [Parameter(Position = 2, Mandatory = true)]
        public CNTK.Axis Axis2;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.TransposeAxes(Operand, Axis1, Axis2, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKTruncatedNormalInitializer")]
    [Alias("cntk.init.truncatednormal", "cntk.truncatednormal")]
    [OutputType(typeof(CNTK.CNTKDictionary))]
    public class NewCNTKTruncatedNormalInitializer : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = false)]
        public double Scale = Constants.DefaultParamInitScale;

        [Parameter(Position = 1, Mandatory = false)]
        public UInt32 Seed = Constants.SentinelValueForAutoSelectRandomSeed;

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.TruncatedNormalInitializer(Scale, Seed);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKUniformInitializer")]
    [Alias("cntk.init.uniform", "cntk.uniform")]
    [OutputType(typeof(CNTK.CNTKDictionary))]
    public class NewCNTKUniformInitializer : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public double Scale;

        [Parameter(Position = 1, Mandatory = false)]
        public UInt32 Seed = Constants.SentinelValueForAutoSelectRandomSeed;

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.UniformInitializer(Scale, Seed);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKUniformRandom")]
    [Alias("cntk.uniformrandom")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKUniformRandom : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public int[] Shape;

        [Parameter(Position = 1, Mandatory = false)]
        public CNTK.DataType DataType = CNTK.DataType.Float;

        [Parameter(Position = 2, Mandatory = false)]
        public double Low = 0.0;

        [Parameter(Position = 3, Mandatory = false)]
        public double High = 1.0;

        [Parameter(Position = 4, Mandatory = false)]
        public UInt32 Seed = Constants.SentinelValueForAutoSelectRandomSeed;

        [Parameter(Position = 5, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.UniformRandom(Shape, DataType, Low, High, Seed, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKUniformRandomLike")]
    [Alias("cntk.uniformrandomlike")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKUniformRandomLike : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public double Low = 0.0;

        [Parameter(Position = 2, Mandatory = false)]
        public double High = 1.0;

        [Parameter(Position = 3, Mandatory = false)]
        public UInt32 Seed = Constants.SentinelValueForAutoSelectRandomSeed;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.UniformRandomLike(Operand, Low, High, Seed, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSequenceUnpack")]
    [Alias("cntk.sequence.unpack")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKSequenceUnpack : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public double PaddingValue;

        [Parameter(Position = 2, Mandatory = true)]
        public bool SupressMaskOutput;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SequenceUnpack(Operand, PaddingValue, SupressMaskOutput, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKUnpackBatch")]
    [Alias("cntk.unpackbatch")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKUnpackBatch : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public string Name;

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.UnpackBatch(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKUnpooling")]
    [Alias("cntk.unpooling")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKUnpooling : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable PoolingInput;

        [Parameter(Position = 2, Mandatory = true)]
        public CNTK.PoolingType UnpoolingType;

        [Parameter(Position = 3, Mandatory = true)]
        public int[] UnpoolingWindowShape;

        [Parameter(Position = 4, Mandatory = false)]
        public int[] Strides = new int[] { 1 };

        [Parameter(Position = 5, Mandatory = false)]
        public bool[] AutoPadding = new bool[] { false };

        [Parameter(Position = 6, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Unpooling(Operand, PoolingInput, UnpoolingType, UnpoolingWindowShape, Strides, new CNTK.BoolVector(AutoPadding), Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKWeightedBinaryCrossEntropy")]
    [Alias("cntk.weightedbinarycrossentropy")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKWeightedBinaryCrossEntropy : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Prediction;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable Targets;

        [Parameter(Position = 2, Mandatory = true)]
        public WrappedVariable Weights;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.WeightedBinaryCrossEntropy(Prediction, Targets, Weights, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSequenceWhere")]
    [Alias("cntk.sequence.where")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKSequenceWhere : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Condition;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SequenceWhere(Condition, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKXavierInitializer")]
    [Alias("cntk.init.xavier", "cntk.xavier")]
    [OutputType(typeof(CNTK.CNTKDictionary))]
    public class NewCNTKXavierInitializer : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = false)]
        public double Scale = Constants.DefaultParamInitScale;

        [Parameter(Position = 1, Mandatory = false)]
        public int OutputRank = Constants.SentinelValueForInferParamInitRank;

        [Parameter(Position = 2, Mandatory = false)]
        public int FilterRank = Constants.SentinelValueForInferParamInitRank;

        [Parameter(Position = 3, Mandatory = false)]
        public UInt32 Seed = Constants.SentinelValueForAutoSelectRandomSeed;

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.XavierInitializer(Scale, OutputRank, FilterRank, Seed);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKZerosLike")]
    [Alias("cntk.zeroslike")]
    [OutputType(typeof(Horker.PSCNTK.WrappedFunction))]
    public class NewCNTKZerosLike : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ZerosLike(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }
}

