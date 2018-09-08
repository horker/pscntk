using System;
using System.Management.Automation;

// DO NOT EDIT
// This file was automatically generated at 2018/09/09 01:26:50

namespace Horker.PSCNTK {

    [Cmdlet("New", "CNTKAbs")]
    [Alias("cntk.abs")]
    public class NewCNTKAbs : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKAcos : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKAlias : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKArgmax : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKArgmin : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKAsComposite : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Function RootFunction;

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
    public class NewCNTKAsin : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKAsinh : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKAssign : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Ref;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKAtanh : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKBatchNormalization : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable Scale;

        [Parameter(Position = 2, Mandatory = true)]
        public CNTK.Variable Bias;

        [Parameter(Position = 3, Mandatory = true)]
        public CNTK.Variable RunningMean;

        [Parameter(Position = 4, Mandatory = true)]
        public CNTK.Variable RunningInvStd;

        [Parameter(Position = 5, Mandatory = true)]
        public CNTK.Variable RunningCount;

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
    public class NewCNTKBernoulliRandom : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public int[] Shape;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.DataType DataType;

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
    public class NewCNTKBernoulliRandomLike : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKBinaryCrossEntropy : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Prediction;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable Targets;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.BinaryCrossEntropy(Prediction, Targets, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKCast")]
    [Alias("cntk.cast")]
    public class NewCNTKCast : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable NodeInput;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.DataType OutputType;

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
    public class NewCNTKCeil : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKClip : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable Min;

        [Parameter(Position = 2, Mandatory = true)]
        public CNTK.Variable Max;

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
    public class NewCNTKConvolution : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable ConvolutionMap;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKConvolutionTranspose : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable ConvolutionMap;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKCos : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKCosh : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKCosineDistance : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable RightOperand;

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
    public class NewCNTKCosineDistanceWithNegativeSamples : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable RightOperand;

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
    public class NewCNTKDepthToSpace : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKDropout : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKEditDistanceError : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Prediction;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable Labels;

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
    public class NewCNTKElementAnd : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable RightOperand;

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
    public class NewCNTKElementDivide : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable RightOperand;

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
    public class NewCNTKElementMax : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable RightOperand;

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
    public class NewCNTKElementMin : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable RightOperand;

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
    public class NewCNTKElementNot : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKElementOr : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable RightOperand;

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
    public class NewCNTKElementSelect : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Condition;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable ThenOperand;

        [Parameter(Position = 2, Mandatory = true)]
        public CNTK.Variable ElseOperand;

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
    public class NewCNTKElementTimes : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable RightOperand;

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
    public class NewCNTKElementXor : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable RightOperand;

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
    public class NewCNTKEqual : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable RightOperand;

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
    public class NewCNTKExp : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKExpandDims : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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

    [Cmdlet("New", "CNTKFloor")]
    [Alias("cntk.floor")]
    public class NewCNTKFloor : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKForwardBackward : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Graph;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable Features;

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
    public class NewCNTKGreater : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable RightOperand;

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
    public class NewCNTKGreaterEqual : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable RightOperand;

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
    public class NewCNTKGumbelRandom : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public int[] Shape;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.DataType DataType;

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
    public class NewCNTKGumbelRandomLike : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKHardmax : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKHardSigmoid : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKImageScaler : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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

    [Cmdlet("New", "CNTKLabelsToGraph")]
    [Alias("cntk.labelstograph")]
    public class NewCNTKLabelsToGraph : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Labels;

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
    public class NewCNTKLambdaRank : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Prediction;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable Gains;

        [Parameter(Position = 2, Mandatory = true)]
        public CNTK.Variable GroupId;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.LambdaRank(Prediction, Gains, GroupId, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKLatticeSequenceWithSoftmax")]
    [Alias("cntk.latticesequencewithsoftmax")]
    public class NewCNTKLatticeSequenceWithSoftmax : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Labels;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable Prediction;

        [Parameter(Position = 2, Mandatory = true)]
        public CNTK.Variable ScaledLogLikelihood;

        [Parameter(Position = 3, Mandatory = true)]
        public CNTK.Variable Lattice;

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
    public class NewCNTKLeakyReLU : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKLess : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable RightOperand;

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
    public class NewCNTKLessEqual : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable RightOperand;

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
    public class NewCNTKLocalResponseNormalization : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKLog : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKLogAddExp : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable RightOperand;

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
    public class NewCNTKMinus : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable RightOperand;

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
    public class NewCNTKNDCGAt1 : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Prediction;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable Gains;

        [Parameter(Position = 2, Mandatory = true)]
        public CNTK.Variable GroupId;

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
    public class NewCNTKNegate : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKNormalRandom : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public int[] Shape;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.DataType DataType;

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
    public class NewCNTKNormalRandomLike : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKNotEqual : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable RightOperand;

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
    public class NewCNTKOneHotOp : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKOnesLike : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKPad : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKPlus : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable RightOperand;

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
    public class NewCNTKPooling : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKPow : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable RightOperand;

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
    public class NewCNTKPReLU : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Alpha;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKRandomSample : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKRandomSampleInclusionFrequency : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKReciprocal : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKReconcileDynamicAxes : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable AxesAsOperand;

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
    public class NewCNTKReduceL1 : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKReduceL2 : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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

    [Cmdlet("New", "CNTKReduceSumSquare")]
    [Alias("cntk.reducesumsquare")]
    public class NewCNTKReduceSumSquare : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKReLU : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKROIPooling : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable Rois;

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
    public class NewCNTKRound : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKSELU : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKSigmoid : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKSin : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKSinh : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Sinh(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSoftplus")]
    [Alias("cntk.softplus")]
    public class NewCNTKSoftplus : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKSoftsign : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKSpaceToDepth : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKSqrt : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKSquare : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKSquaredError : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Prediction;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable Targets;

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
    public class NewCNTKStopGradient : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKTanh : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKToBatch : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKToSequenceLike : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable DynamicAxesLike;

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
    public class NewCNTKTransposeAxes : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKUniformRandom : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public int[] Shape;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.DataType DataType;

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
    public class NewCNTKUniformRandomLike : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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

    [Cmdlet("New", "CNTKUnpackBatch")]
    [Alias("cntk.unpackbatch")]
    public class NewCNTKUnpackBatch : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

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
    public class NewCNTKUnpooling : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable PoolingInput;

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
    public class NewCNTKWeightedBinaryCrossEntropy : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Prediction;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable Targets;

        [Parameter(Position = 2, Mandatory = true)]
        public CNTK.Variable Weights;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.WeightedBinaryCrossEntropy(Prediction, Targets, Weights, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKXavierInitializer")]
    [Alias("cntk.init.xavier", "cntk.xavier")]
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
    public class NewCNTKZerosLike : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ZerosLike(Operand, Name);
            WriteObject(new Horker.PSCNTK.WrappedFunction(result));
        }
    }
}

