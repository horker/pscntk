using System;
using System.Management.Automation;

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
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKAssign")]
    [Alias("cntk.assign")]
    public class NewCNTKAssign : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Arg0;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Assign(Arg0, Operand, Name);
            WriteObject(result);
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
            WriteObject(result);
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

        [Parameter(Position = 7, Mandatory = true)]
        public double NormalizationTimeConstant;

        [Parameter(Position = 8, Mandatory = true)]
        public double BlendTimeConstant;

        [Parameter(Position = 9, Mandatory = true)]
        public double Epsilon;

        [Parameter(Position = 10, Mandatory = true)]
        public bool UseCuDNNEngine;

        [Parameter(Position = 11, Mandatory = true)]
        public bool DisableRegularization;

        [Parameter(Position = 12, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.BatchNormalization(Operand, Scale, Bias, RunningMean, RunningInvStd, RunningCount, Spatial, NormalizationTimeConstant, BlendTimeConstant, Epsilon, UseCuDNNEngine, DisableRegularization, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKBernoulliRandom")]
    [Alias("cntk.bernoullirandom")]
    public class NewCNTKBernoulliRandom : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.NDShape Shape;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.DataType DataType;

        [Parameter(Position = 2, Mandatory = true)]
        public double Mean;

        [Parameter(Position = 3, Mandatory = true)]
        public int Seed;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.BernoulliRandom(Shape, DataType, Mean, (uint)Seed, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKBernoulliRandomLike")]
    [Alias("cntk.bernoullirandomlike")]
    public class NewCNTKBernoulliRandomLike : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public double Mean;

        [Parameter(Position = 2, Mandatory = true)]
        public int Seed;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.BernoulliRandomLike(Operand, Mean, (uint)Seed, Name);
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
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKClassificationError")]
    [Alias("cntk.classificationerror")]
    public class NewCNTKClassificationError : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Prediction;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable Labels;

        [Parameter(Position = 2, Mandatory = true)]
        public int TopN;

        [Parameter(Position = 3, Mandatory = true)]
        public CNTK.Axis Axis;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ClassificationError(Prediction, Labels, (uint)TopN, Axis, Name);
            WriteObject(result);
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
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKCombine")]
    [Alias("cntk.combine")]
    public class NewCNTKCombine : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.VariableVector Operands;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Combine(Operands, Name);
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

        [Parameter(Position = 2, Mandatory = true)]
        public CNTK.NDShape Strides;

        [Parameter(Position = 3, Mandatory = true)]
        public CNTK.BoolVector Sharing;

        [Parameter(Position = 4, Mandatory = true)]
        public CNTK.BoolVector AutoPadding;

        [Parameter(Position = 5, Mandatory = true)]
        public CNTK.NDShape Dilation;

        [Parameter(Position = 6, Mandatory = true)]
        public int ReductionRank;

        [Parameter(Position = 7, Mandatory = true)]
        public int Groups;

        [Parameter(Position = 8, Mandatory = true)]
        public int MaxTempMemSizeInSamples;

        [Parameter(Position = 9, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Convolution(ConvolutionMap, Operand, Strides, Sharing, AutoPadding, Dilation, (uint)ReductionRank, (uint)Groups, (uint)MaxTempMemSizeInSamples, Name);
            WriteObject(result);
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

        [Parameter(Position = 2, Mandatory = true)]
        public CNTK.NDShape Strides;

        [Parameter(Position = 3, Mandatory = true)]
        public CNTK.BoolVector Sharing;

        [Parameter(Position = 4, Mandatory = true)]
        public CNTK.BoolVector AutoPadding;

        [Parameter(Position = 5, Mandatory = true)]
        public CNTK.NDShape OutputShape;

        [Parameter(Position = 6, Mandatory = true)]
        public CNTK.NDShape Dilation;

        [Parameter(Position = 7, Mandatory = true)]
        public int ReductionRank;

        [Parameter(Position = 8, Mandatory = true)]
        public int MaxTempMemSizeInSamples;

        [Parameter(Position = 9, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ConvolutionTranspose(ConvolutionMap, Operand, Strides, Sharing, AutoPadding, OutputShape, Dilation, (uint)ReductionRank, (uint)MaxTempMemSizeInSamples, Name);
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
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
        public int ShiftWindow;

        [Parameter(Position = 3, Mandatory = true)]
        public int NumberOfNegativeSamples;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.CosineDistanceWithNegativeSamples(LeftOperand, RightOperand, (uint)ShiftWindow, (uint)NumberOfNegativeSamples, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKCrop")]
    [Alias("cntk.crop")]
    public class NewCNTKCrop : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable NodeInput;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable NodeReferent;

        [Parameter(Position = 2, Mandatory = true)]
        public int OffsetX;

        [Parameter(Position = 3, Mandatory = true)]
        public int OffsetY;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Crop(NodeInput, NodeReferent, (uint)OffsetX, (uint)OffsetY, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKCrossEntropyWithSoftmax")]
    [Alias("cntk.crossentropywithsoftmax")]
    public class NewCNTKCrossEntropyWithSoftmax : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Prediction;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable Labels;

        [Parameter(Position = 2, Mandatory = true)]
        public CNTK.Axis Axis;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.CrossEntropyWithSoftmax(Prediction, Labels, Axis, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKDepthToSpace")]
    [Alias("cntk.depthtospace")]
    public class NewCNTKDepthToSpace : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public int BlockSize;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.DepthToSpace(Operand, (uint)BlockSize, Name);
            WriteObject(result);
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

        [Parameter(Position = 2, Mandatory = true)]
        public int Seed;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Dropout(Operand, DropoutRate, (uint)Seed, Name);
            WriteObject(result);
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
        public CNTK.SizeTVector TokensToIgnore;

        [Parameter(Position = 7, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.EditDistanceError(Prediction, Labels, SubstitutionPenalty, DeletionPenalty, InsertionPenalty, SquashInputs, TokensToIgnore, Name);
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
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

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ElementMax(LeftOperand, RightOperand, Name);
            WriteObject(result);
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

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ElementMin(LeftOperand, RightOperand, Name);
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKELU")]
    [Alias("cntk.elu")]
    public class NewCNTKELU : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ELU(Operand, Name);
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKFlatten")]
    [Alias("cntk.flatten")]
    public class NewCNTKFlatten : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Flatten(Operand, Axis, Name);
            WriteObject(result);
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
            WriteObject(result);
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
        public int BlankTokenId;

        [Parameter(Position = 3, Mandatory = true)]
        public int DelayConstraint;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ForwardBackward(Graph, Features, (uint)BlankTokenId, DelayConstraint, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKFutureValue")]
    [Alias("cntk.futurevalue")]
    public class NewCNTKFutureValue : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable InitialState;

        [Parameter(Position = 2, Mandatory = true)]
        public int Offset;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.FutureValue(Operand, InitialState, (uint)Offset, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKGatherOp")]
    [Alias("cntk.gatherop")]
    public class NewCNTKGatherOp : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Indices;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable Reference;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.GatherOp(Indices, Reference, Name);
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
            WriteObject(result);
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
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKGroupConvolution")]
    [Alias("cntk.groupconvolution")]
    public class NewCNTKGroupConvolution : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable ConvolutionMap;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 2, Mandatory = true)]
        public CNTK.NDShape Strides;

        [Parameter(Position = 3, Mandatory = true)]
        public CNTK.BoolVector Sharing;

        [Parameter(Position = 4, Mandatory = true)]
        public CNTK.BoolVector AutoPadding;

        [Parameter(Position = 5, Mandatory = true)]
        public CNTK.NDShape Dilation;

        [Parameter(Position = 6, Mandatory = true)]
        public int Groups;

        [Parameter(Position = 7, Mandatory = true)]
        public int MaxTempMemSizeInSamples;

        [Parameter(Position = 8, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.GroupConvolution(ConvolutionMap, Operand, Strides, Sharing, AutoPadding, Dilation, (uint)Groups, (uint)MaxTempMemSizeInSamples, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKGumbelRandom")]
    [Alias("cntk.gumbelrandom")]
    public class NewCNTKGumbelRandom : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.NDShape Shape;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.DataType DataType;

        [Parameter(Position = 2, Mandatory = true)]
        public double Loc;

        [Parameter(Position = 3, Mandatory = true)]
        public double Scale;

        [Parameter(Position = 4, Mandatory = true)]
        public int Seed;

        [Parameter(Position = 5, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.GumbelRandom(Shape, DataType, Loc, Scale, (uint)Seed, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKGumbelRandomLike")]
    [Alias("cntk.gumbelrandomlike")]
    public class NewCNTKGumbelRandomLike : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public double Loc;

        [Parameter(Position = 2, Mandatory = true)]
        public double Scale;

        [Parameter(Position = 3, Mandatory = true)]
        public int Seed;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.GumbelRandomLike(Operand, Loc, Scale, (uint)Seed, Name);
            WriteObject(result);
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
            WriteObject(result);
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
        public CNTK.FloatVector Biases;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ImageScaler(Operand, Scaler, Biases, Name);
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKLocalResponseNormalization")]
    [Alias("cntk.localresponsenormalization")]
    public class NewCNTKLocalResponseNormalization : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public int DepthRadius;

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
            var result = CNTK.CNTKLib.LocalResponseNormalization(Operand, (uint)DepthRadius, Bias, Alpha, Beta, Name);
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKLogSoftmax")]
    [Alias("cntk.logsoftmax")]
    public class NewCNTKLogSoftmax : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.LogSoftmax(Operand, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKMean")]
    [Alias("cntk.mean")]
    public class NewCNTKMean : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.VariableVector Operands;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Mean(Operands, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKMeanVarianceNormalization")]
    [Alias("cntk.meanvariancenormalization")]
    public class NewCNTKMeanVarianceNormalization : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public double Epsilon;

        [Parameter(Position = 2, Mandatory = true)]
        public bool UseStatsAcrossChannels;

        [Parameter(Position = 3, Mandatory = true)]
        public bool DoVarianceScaling;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.MeanVarianceNormalization(Operand, Epsilon, UseStatsAcrossChannels, DoVarianceScaling, Name);
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKNormalRandom")]
    [Alias("cntk.normalrandom")]
    public class NewCNTKNormalRandom : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.NDShape Shape;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.DataType DataType;

        [Parameter(Position = 2, Mandatory = true)]
        public double Mean;

        [Parameter(Position = 3, Mandatory = true)]
        public double Scale;

        [Parameter(Position = 4, Mandatory = true)]
        public int Seed;

        [Parameter(Position = 5, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.NormalRandom(Shape, DataType, Mean, Scale, (uint)Seed, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKNormalRandomLike")]
    [Alias("cntk.normalrandomlike")]
    public class NewCNTKNormalRandomLike : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public double Mean;

        [Parameter(Position = 2, Mandatory = true)]
        public double Scale;

        [Parameter(Position = 3, Mandatory = true)]
        public int Seed;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.NormalRandomLike(Operand, Mean, Scale, (uint)Seed, Name);
            WriteObject(result);
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
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKOneHotOp")]
    [Alias("cntk.onehotop")]
    public class NewCNTKOneHotOp : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public int NumClass;

        [Parameter(Position = 2, Mandatory = true)]
        public bool OutputSparse;

        [Parameter(Position = 3, Mandatory = true)]
        public CNTK.Axis Axis;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.OneHotOp(Operand, (uint)NumClass, OutputSparse, Axis, Name);
            WriteObject(result);
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
            WriteObject(result);
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
        public CNTK.SizeTVector Head;

        [Parameter(Position = 3, Mandatory = true)]
        public CNTK.SizeTVector Foot;

        [Parameter(Position = 4, Mandatory = true)]
        public double ConstantValue;

        [Parameter(Position = 5, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Pad(Operand, Mode, Head, Foot, ConstantValue, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKPastValue")]
    [Alias("cntk.pastvalue")]
    public class NewCNTKPastValue : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable InitialState;

        [Parameter(Position = 2, Mandatory = true)]
        public int Offset;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.PastValue(Operand, InitialState, (uint)Offset, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKPerDimMeanVarianceNormalize")]
    [Alias("cntk.perdimmeanvariancenormalize")]
    public class NewCNTKPerDimMeanVarianceNormalize : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable Mean;

        [Parameter(Position = 2, Mandatory = true)]
        public CNTK.Variable InvStdDev;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.PerDimMeanVarianceNormalize(Operand, Mean, InvStdDev, Name);
            WriteObject(result);
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
            WriteObject(result);
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
        public CNTK.NDShape PoolingWindowShape;

        [Parameter(Position = 3, Mandatory = true)]
        public CNTK.NDShape Strides;

        [Parameter(Position = 4, Mandatory = true)]
        public CNTK.BoolVector AutoPadding;

        [Parameter(Position = 5, Mandatory = true)]
        public bool CeilOutDim;

        [Parameter(Position = 6, Mandatory = true)]
        public bool IncludePad;

        [Parameter(Position = 7, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Pooling(Operand, PoolingType, PoolingWindowShape, Strides, AutoPadding, CeilOutDim, IncludePad, Name);
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKRandomSample")]
    [Alias("cntk.randomsample")]
    public class NewCNTKRandomSample : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public int NumSamples;

        [Parameter(Position = 2, Mandatory = true)]
        public bool AllowDuplicates;

        [Parameter(Position = 3, Mandatory = true)]
        public int Seed;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.RandomSample(Operand, (uint)NumSamples, AllowDuplicates, (uint)Seed, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKRandomSampleInclusionFrequency")]
    [Alias("cntk.randomsampleinclusionfrequency")]
    public class NewCNTKRandomSampleInclusionFrequency : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public int NumSamples;

        [Parameter(Position = 2, Mandatory = true)]
        public bool AllowDuplicates;

        [Parameter(Position = 3, Mandatory = true)]
        public int Seed;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.RandomSampleInclusionFrequency(Operand, (uint)NumSamples, AllowDuplicates, (uint)Seed, Name);
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKReduceL1")]
    [Alias("cntk.reducel1")]
    public class NewCNTKReduceL1 : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.AxisVector Axes;

        [Parameter(Position = 2, Mandatory = true)]
        public bool KeepDims;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceL1(Operand, Axes, KeepDims, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKReduceL2")]
    [Alias("cntk.reducel2")]
    public class NewCNTKReduceL2 : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.AxisVector Axes;

        [Parameter(Position = 2, Mandatory = true)]
        public bool KeepDims;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceL2(Operand, Axes, KeepDims, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKReduceLogSum")]
    [Alias("cntk.reducelogsum")]
    public class NewCNTKReduceLogSum : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceLogSum(Operand, Axis, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKReduceMax")]
    [Alias("cntk.reducemax")]
    public class NewCNTKReduceMax : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceMax(Operand, Axis, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKReduceMean")]
    [Alias("cntk.reducemean")]
    public class NewCNTKReduceMean : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceMean(Operand, Axis, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKReduceMin")]
    [Alias("cntk.reducemin")]
    public class NewCNTKReduceMin : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceMin(Operand, Axis, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKReduceProd")]
    [Alias("cntk.reduceprod")]
    public class NewCNTKReduceProd : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceProd(Operand, Axis, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKReduceSum")]
    [Alias("cntk.reducesum")]
    public class NewCNTKReduceSum : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceSum(Operand, Axis, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKReduceSumSquare")]
    [Alias("cntk.reducesumsquare")]
    public class NewCNTKReduceSumSquare : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.AxisVector Axes;

        [Parameter(Position = 2, Mandatory = true)]
        public bool KeepDims;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceSumSquare(Operand, Axes, KeepDims, Name);
            WriteObject(result);
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
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKReshape")]
    [Alias("cntk.reshape")]
    public class NewCNTKReshape : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.NDShape ReplacementShape;

        [Parameter(Position = 2, Mandatory = true)]
        public CNTK.Axis BeginAxis;

        [Parameter(Position = 3, Mandatory = true)]
        public CNTK.Axis EndAxis;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Reshape(Operand, ReplacementShape, BeginAxis, EndAxis, Name);
            WriteObject(result);
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
        public CNTK.NDShape RoiOutputShape;

        [Parameter(Position = 4, Mandatory = true)]
        public double SpatialScale;

        [Parameter(Position = 5, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ROIPooling(Operand, Rois, PoolingType, RoiOutputShape, SpatialScale, Name);
            WriteObject(result);
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
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKSELU")]
    [Alias("cntk.selu")]
    public class NewCNTKSELU : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public double Gamma;

        [Parameter(Position = 2, Mandatory = true)]
        public double Alpha;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SELU(Operand, Gamma, Alpha, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKSequenceBroadcastAs")]
    [Alias("cntk.sequencebroadcastas")]
    public class NewCNTKSequenceBroadcastAs : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable BroadcastAs;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SequenceBroadcastAs(Operand, BroadcastAs, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKSequenceFirst")]
    [Alias("cntk.sequencefirst")]
    public class NewCNTKSequenceFirst : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SequenceFirst(Operand, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKSequenceGather")]
    [Alias("cntk.sequencegather")]
    public class NewCNTKSequenceGather : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable Condition;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SequenceGather(Operand, Condition, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKSequenceIsFirst")]
    [Alias("cntk.sequenceisfirst")]
    public class NewCNTKSequenceIsFirst : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SequenceIsFirst(Operand, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKSequenceIsLast")]
    [Alias("cntk.sequenceislast")]
    public class NewCNTKSequenceIsLast : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SequenceIsLast(Operand, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKSequenceLast")]
    [Alias("cntk.sequencelast")]
    public class NewCNTKSequenceLast : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SequenceLast(Operand, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKSequenceReduceMax")]
    [Alias("cntk.sequencereducemax")]
    public class NewCNTKSequenceReduceMax : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SequenceReduceMax(Operand, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKSequenceReduceSum")]
    [Alias("cntk.sequencereducesum")]
    public class NewCNTKSequenceReduceSum : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SequenceReduceSum(Operand, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKSequenceScatter")]
    [Alias("cntk.sequencescatter")]
    public class NewCNTKSequenceScatter : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable Condition;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SequenceScatter(Operand, Condition, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKSequenceSlice")]
    [Alias("cntk.sequenceslice")]
    public class NewCNTKSequenceSlice : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public int BeginIndex;

        [Parameter(Position = 2, Mandatory = true)]
        public int EndIndex;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SequenceSlice(Operand, BeginIndex, EndIndex, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKSequenceSoftmax")]
    [Alias("cntk.sequencesoftmax")]
    public class NewCNTKSequenceSoftmax : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SequenceSoftmax(Operand, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKSequenceUnpack")]
    [Alias("cntk.sequenceunpack")]
    public class NewCNTKSequenceUnpack : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public double PaddingValue;

        [Parameter(Position = 2, Mandatory = true)]
        public bool SupressMaskOutput;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SequenceUnpack(Operand, PaddingValue, SupressMaskOutput, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKSequenceWhere")]
    [Alias("cntk.sequencewhere")]
    public class NewCNTKSequenceWhere : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Condition;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SequenceWhere(Condition, Name);
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKSinh")]
    [Alias("cntk.sinh")]
    public class NewCNTKSinh : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Sinh(Operand);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKSlice")]
    [Alias("cntk.slice")]
    public class NewCNTKSlice : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.AxisVector Axis;

        [Parameter(Position = 2, Mandatory = true)]
        public CNTK.IntVector BeginIndex;

        [Parameter(Position = 3, Mandatory = true)]
        public CNTK.IntVector EndIndex;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Slice(Operand, Axis, BeginIndex, EndIndex, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKSoftmax")]
    [Alias("cntk.softmax")]
    public class NewCNTKSoftmax : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Softmax(Operand, Name);
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKSpaceToDepth")]
    [Alias("cntk.spacetodepth")]
    public class NewCNTKSpaceToDepth : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public int BlockSize;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SpaceToDepth(Operand, (uint)BlockSize, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKSpatialConvolution")]
    [Alias("cntk.spatialconvolution")]
    public class NewCNTKSpatialConvolution : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable ConvolutionMap;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 2, Mandatory = true)]
        public CNTK.NDShape Strides;

        [Parameter(Position = 3, Mandatory = true)]
        public CNTK.BoolVector Sharing;

        [Parameter(Position = 4, Mandatory = true)]
        public CNTK.BoolVector AutoPadding;

        [Parameter(Position = 5, Mandatory = true)]
        public CNTK.NDShape Dilation;

        [Parameter(Position = 6, Mandatory = true)]
        public int MaxTempMemSizeInSamples;

        [Parameter(Position = 7, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.SpatialConvolution(ConvolutionMap, Operand, Strides, Sharing, AutoPadding, Dilation, (uint)MaxTempMemSizeInSamples, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKSplice")]
    [Alias("cntk.splice")]
    public class NewCNTKSplice : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.VariableVector Operands;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Splice(Operands, Axis, Name);
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
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
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKSqueeze")]
    [Alias("cntk.squeeze")]
    public class NewCNTKSqueeze : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Squeeze(Operand, Name);
            WriteObject(result);
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
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKSum")]
    [Alias("cntk.sum")]
    public class NewCNTKSum : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.VariableVector Operands;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Sum(Operands, Name);
            WriteObject(result);
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
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKTimes")]
    [Alias("cntk.times")]
    public class NewCNTKTimes : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable RightOperand;

        [Parameter(Position = 2, Mandatory = true)]
        public int OutputRank;

        [Parameter(Position = 3, Mandatory = true)]
        public int InferInputRankToMap;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Times(LeftOperand, RightOperand, (uint)OutputRank, InferInputRankToMap, Name);
            WriteObject(result);
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
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKTopK")]
    [Alias("cntk.topk")]
    public class NewCNTKTopK : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public int K;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.TopK(Operand, (uint)K, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKToSequence")]
    [Alias("cntk.tosequence")]
    public class NewCNTKToSequence : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public string SequenceAxisNamePrefix;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ToSequence(Operand, SequenceAxisNamePrefix, Name);
            WriteObject(result);
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
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKTranspose")]
    [Alias("cntk.transpose")]
    public class NewCNTKTranspose : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Transpose(Operand, Name);
            WriteObject(result);
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
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKTransposeTimes")]
    [Alias("cntk.transposetimes")]
    public class NewCNTKTransposeTimes : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable RightOperand;

        [Parameter(Position = 2, Mandatory = true)]
        public int OutputRank;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.TransposeTimes(LeftOperand, RightOperand, (uint)OutputRank, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKUniformRandom")]
    [Alias("cntk.uniformrandom")]
    public class NewCNTKUniformRandom : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.NDShape Shape;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.DataType DataType;

        [Parameter(Position = 2, Mandatory = true)]
        public double Low;

        [Parameter(Position = 3, Mandatory = true)]
        public double High;

        [Parameter(Position = 4, Mandatory = true)]
        public int Seed;

        [Parameter(Position = 5, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.UniformRandom(Shape, DataType, Low, High, (uint)Seed, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKUniformRandomLike")]
    [Alias("cntk.uniformrandomlike")]
    public class NewCNTKUniformRandomLike : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public double Low;

        [Parameter(Position = 2, Mandatory = true)]
        public double High;

        [Parameter(Position = 3, Mandatory = true)]
        public int Seed;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.UniformRandomLike(Operand, Low, High, (uint)Seed, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKUnpackBatch")]
    [Alias("cntk.unpackbatch")]
    public class NewCNTKUnpackBatch : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.UnpackBatch(Operand, Name);
            WriteObject(result);
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
        public CNTK.NDShape UnpoolingWindowShape;

        [Parameter(Position = 4, Mandatory = true)]
        public CNTK.NDShape Strides;

        [Parameter(Position = 5, Mandatory = true)]
        public CNTK.BoolVector AutoPadding;

        [Parameter(Position = 6, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Unpooling(Operand, PoolingInput, UnpoolingType, UnpoolingWindowShape, Strides, AutoPadding, Name);
            WriteObject(result);
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
            WriteObject(result);
        }
    }
}

