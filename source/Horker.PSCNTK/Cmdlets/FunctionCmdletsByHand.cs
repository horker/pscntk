using System;
using System.Management.Automation;

namespace Horker.PSCNTK {

    [Cmdlet("New", "CNTKClassificationError")]
    [Alias("cntk.classificationerror")]
    public class NewCNTKClassificationError : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Prediction;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable Labels;

        [Parameter(Position = 2, Mandatory = false)]
        public UInt32? TopN;

        [Parameter(Position = 3, Mandatory = false)]
        public CNTK.Axis Axis;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            CNTK.Function result;

            if (TopN.HasValue)
                if (Axis != null)
                    result = CNTK.CNTKLib.ClassificationError(Prediction, Labels, TopN.Value, Axis, Name);
                else
                    result = CNTK.CNTKLib.ClassificationError(Prediction, Labels, TopN.Value, Name);
            else
                if (Axis != null)
                    result = CNTK.CNTKLib.ClassificationError(Prediction, Labels, Axis, Name);
                else
                    result = CNTK.CNTKLib.ClassificationError(Prediction, Labels, Name);

            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKCrop")]
    [Alias("cntk.crop")]
    public class NewCNTKCrop : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable NodeInput;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable NodeReferent;

        [Parameter(Position = 2, Mandatory = true)]
        public WrappedVariable AncestorInput;

        [Parameter(Position = 3, Mandatory = true)]
        public WrappedVariable AncestorReferent;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Crop(NodeInput, NodeReferent, AncestorInput, AncestorReferent, Name);
            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKCrop2")]
    [Alias("cntk.crop2")]
    public class NewCNTKCrop2 : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable NodeInput;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable NodeReferent;

        [Parameter(Position = 2, Mandatory = true)]
        public UInt32 OffsetX;

        [Parameter(Position = 3, Mandatory = true)]
        public UInt32 OffsetY;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Crop(NodeInput, NodeReferent, OffsetX, OffsetY, Name);
            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKCrop3")]
    [Alias("cntk.crop3")]
    public class NewCNTKCrop3 : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable NodeInput;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable NodeReferent;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Crop(NodeInput, NodeReferent, Name);
            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKCrossEntropyWithSoftmax")]
    [Alias("cntk.crossentropywithsoftmax")]
    public class NewCNTKCrossEntropyWithSoftmax : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Prediction;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable Labels;

        [Parameter(Position = 2, Mandatory = false)]
        public CNTK.Axis Axis;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            CNTK.Function result;

            if (Axis != null)
                result = CNTK.CNTKLib.CrossEntropyWithSoftmax(Prediction, Labels, Axis, Name);
            else
                result = CNTK.CNTKLib.CrossEntropyWithSoftmax(Prediction, Labels, Name);

            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKELU")]
    [Alias("cntk.elu")]
    public class NewCNTKELU : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            // ELU() has multiple signatures in C++, but not in C#.
            var result = CNTK.CNTKLib.ELU(Operand, Name);
            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKFlatten")]
    [Alias("cntk.flatten")]
    public class NewCNTKFlatten : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public CNTK.Axis Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            CNTK.Function result;

            if (Axis != null)
                result = CNTK.CNTKLib.Flatten(Operand, Axis, Name);
            else
                result = CNTK.CNTKLib.Flatten(Operand, Name);

            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKFutureValue")]
    [Alias("cntk.futurevalue")]
    public class NewCNTKFutureValue : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public WrappedVariable InitialState;

        [Parameter(Position = 2, Mandatory = false)]
        public UInt32 Offset = 1;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            CNTK.Function result;

            if (InitialState != null)
                result = CNTK.CNTKLib.FutureValue(Operand, InitialState, Offset, Name);
            else
                result = CNTK.CNTKLib.FutureValue(Operand, Offset, Name);

            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKGatherOp")]
    [Alias("cntk.gatherop")]
    public class NewCNTKGatherOp : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Indices;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable Reference;

        [Parameter(Position = 2, Mandatory = false)]
        public CNTK.Axis Axis;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            CNTK.Function result;

            if (Axis != null)
                result = CNTK.CNTKLib.GatherOp(Indices, Reference, Axis, Name);
            else
                result = CNTK.CNTKLib.GatherOp(Indices, Reference, Name);

            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKLogSoftmax")]
    [Alias("cntk.logsoftmax")]
    public class NewCNTKLogSoftmax : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public CNTK.Axis Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            CNTK.Function result;

            if (Axis != null)
                result = CNTK.CNTKLib.LogSoftmax(Operand, Axis, Name);
            else
                result = CNTK.CNTKLib.LogSoftmax(Operand, Name);

            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKMeanVarianceNormalization")]
    [Alias("cntk.meanvariancenormalization")]
    public class NewCNTKMeanVarianceNormalization : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public double? Epsilon;

        [Parameter(Position = 2, Mandatory = false)]
        public bool UseStatsAcrossChannels = false;

        [Parameter(Position = 3, Mandatory = false)]
        public bool DoVarianceScaling = true;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            CNTK.Function result;

            if (Epsilon.HasValue)
                result = CNTK.CNTKLib.MeanVarianceNormalization(Operand, Epsilon.Value, UseStatsAcrossChannels, DoVarianceScaling, Name);
            else
                result = CNTK.CNTKLib.MeanVarianceNormalization(Operand, UseStatsAcrossChannels, DoVarianceScaling, Name);

            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKPastValue")]
    [Alias("cntk.pastvalue")]
    public class NewCNTKPastValue : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public WrappedVariable InitialState;

        [Parameter(Position = 2, Mandatory = false)]
        public UInt32 Offset = 1;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            CNTK.Function result;

            if (InitialState != null)
                result = CNTK.CNTKLib.PastValue(Operand, InitialState, Offset, Name);
            else
                result = CNTK.CNTKLib.PastValue(Operand, Offset, Name);

            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKPerDimMeanVarianceNormalize")]
    [Alias("cntk.perdimmeanvariancenormalize")]
    public class NewCNTKPerDimMeanVarianceNormalize : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.NDArrayView Mean;

        [Parameter(Position = 2, Mandatory = true)]
        public CNTK.NDArrayView InvStdDev;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.PerDimMeanVarianceNormalize(Operand, Mean, InvStdDev, Name);
            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKPerDimMeanVarianceNormalize2")]
    [Alias("cntk.perdimmeanvariancenormalize2")]
    public class NewCNTKPerDimMeanVarianceNormalize2 : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable Mean;

        [Parameter(Position = 2, Mandatory = true)]
        public WrappedVariable InvStdDev;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.PerDimMeanVarianceNormalize(Operand, Mean, InvStdDev, Name);
            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKReduceLogSum")]
    [Alias("cntk.reducelogsum")]
    public class NewCNTKReduceLogSum : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis[] Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceLogSum(Operand, new CNTK.AxisVector(Axis), Name);
            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKReduceMax")]
    [Alias("cntk.reducemax")]
    public class NewCNTKReduceMax : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis[] Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceMax(Operand, new CNTK.AxisVector(Axis), Name);
            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKReduceMean")]
    [Alias("cntk.reducemean")]
    public class NewCNTKReduceMean : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis[] Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceMean(Operand, new CNTK.AxisVector(Axis), Name);
            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKReduceMin")]
    [Alias("cntk.reducemin")]
    public class NewCNTKReduceMin : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis[] Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceMin(Operand, new CNTK.AxisVector(Axis), Name);
            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKReduceProd")]
    [Alias("cntk.reduceprod")]
    public class NewCNTKReduceProd : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis[] Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceProd(Operand, new CNTK.AxisVector(Axis), Name);
            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKReduceSum")]
    [Alias("cntk.reducesum")]
    public class NewCNTKReduceSum : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis[] Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceSum(Operand, new CNTK.AxisVector(Axis), Name);
            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKReshape2")]
    [Alias("cntk.reshape2")]
    public class NewCNTKReshape2 : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public int[] ReplacementShape;

        [Parameter(Position = 2, Mandatory = true)]
        public CNTK.Axis BeginAxis;

        [Parameter(Position = 3, Mandatory = true)]
        public CNTK.Axis EndAxis;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Reshape(Operand, ReplacementShape, BeginAxis, EndAxis, Name);
            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKReshape")]
    [Alias("cntk.reshape")]
    public class NewCNTKReshape : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public int[] NewShape;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Reshape(Operand, NewShape, Name);
            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSlice")]
    [Alias("cntk.slice")]
    public class NewCNTKSlice : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis[] Axis;

        [Parameter(Position = 2, Mandatory = true)]
        public int[] BeginIndex;

        [Parameter(Position = 3, Mandatory = true)]
        public int[] EndIndex;

        [Parameter(Position = 4, Mandatory = false)]
        public int[] Strides;

        [Parameter(Position = 5, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            CNTK.Function result;

            if (Strides != null)
                result = CNTK.CNTKLib.Slice(Operand, new CNTK.AxisVector(Axis), new CNTK.IntVector(BeginIndex), new CNTK.IntVector(EndIndex), new CNTK.IntVector(Strides), Name);
            else
                result = CNTK.CNTKLib.Slice(Operand, new CNTK.AxisVector(Axis), new CNTK.IntVector(BeginIndex), new CNTK.IntVector(EndIndex), Name);

            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSoftmax")]
    [Alias("cntk.softmax")]
    public class NewCNTKSoftmax : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public CNTK.Axis Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            CNTK.Function result;

            if (Axis != null)
                result = CNTK.CNTKLib.Softmax(Operand, Axis, Name);
            else
                result = CNTK.CNTKLib.Softmax(Operand, Name);

            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKSqueeze")]
    [Alias("cntk.squeeze")]
    public class NewCNTKSqueeze : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public CNTK.Axis[] Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            CNTK.Function result;

            if (Axis != null)
                result = CNTK.CNTKLib.Squeeze(Operand, new CNTK.AxisVector(Axis), Name);
            else
                result = CNTK.CNTKLib.Squeeze(Operand, Name);

            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKTimes")]
    [Alias("cntk.times")]
    public class NewCNTKTimes : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable RightOperand;

        [Parameter(Position = 2, Mandatory = false)]
        public UInt32? OutputRank;

        [Parameter(Position = 3, Mandatory = false)]
        public int? InferInputRankToMap;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            CNTK.Function result;

            if (OutputRank.HasValue)
                if (InferInputRankToMap.HasValue)
                    result = CNTK.CNTKLib.Times(LeftOperand, RightOperand, OutputRank.Value, InferInputRankToMap.Value, Name);
                else
                    result = CNTK.CNTKLib.Times(LeftOperand, RightOperand, OutputRank.Value, Name);
            else
                if (InferInputRankToMap.HasValue)
                    throw new ArgumentException("InferInputRankTopMap is not effective when OutputRank is not specified");
                else
                    result = CNTK.CNTKLib.Times(LeftOperand, RightOperand, Name);

            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKTopK")]
    [Alias("cntk.topk")]
    public class NewCNTKTopK : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public UInt32 K;

        [Parameter(Position = 2, Mandatory = false)]
        public CNTK.Axis Axis;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            CNTK.Function result;

            if (Axis != null)
                result = CNTK.CNTKLib.TopK(Operand, K, Axis, Name);
            else
                result = CNTK.CNTKLib.TopK(Operand, K, Name);

            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKToSequence")]
    [Alias("cntk.tosequence")]
    public class NewCNTKToSequence : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public WrappedVariable SequenceLengths;

        [Parameter(Position = 2, Mandatory = true)]
        public string SequenceAxisNamePrefix;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            CNTK.Function result;

            if (SequenceLengths != null)
                result = CNTK.CNTKLib.ToSequence(Operand, SequenceLengths, SequenceAxisNamePrefix, Name);
            else
                result = CNTK.CNTKLib.ToSequence(Operand, SequenceAxisNamePrefix, Name);

            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKTranspose")]
    [Alias("cntk.transpose")]
    public class NewCNTKTranspose : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public CNTK.Axis[] Permutation;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            CNTK.Function result;

            if (Permutation != null)
                result = CNTK.CNTKLib.Transpose(Operand, new CNTK.AxisVector(Permutation), Name);
            else
                result = CNTK.CNTKLib.Transpose(Operand, Name);

            WriteObject(new WrappedFunction(result));
        }
    }

    [Cmdlet("New", "CNTKTransposeTimes")]
    [Alias("cntk.transposetimes")]
    public class NewCNTKTransposeTimes : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable RightOperand;

        [Parameter(Position = 2, Mandatory = false)]
        public UInt32? OutputRank;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            CNTK.Function result;

            if (OutputRank.HasValue)
                result = CNTK.CNTKLib.TransposeTimes(LeftOperand, RightOperand, OutputRank.Value, Name);
            else
                result = CNTK.CNTKLib.TransposeTimes(LeftOperand, RightOperand, Name);

            WriteObject(new WrappedFunction(result));
        }
    }

    // Signatures:
    // public static CNTK.Function SequenceGather(CNTK.Variable operand, CNTK.Variable condition)
    // public static CNTK.Function SequenceGather(CNTK.Variable operand, CNTK.Variable condition, CNTK.PairSizeTInt newDerivedSequenceAxisScalingAndAdditiveFactor)
    // public static CNTK.Function SequenceGather(CNTK.Variable operand, CNTK.Variable condition, CNTK.PairSizeTInt newDerivedSequenceAxisScalingAndAdditiveFactor, string name)
    // public static CNTK.Function SequenceGather(CNTK.Variable operand, CNTK.Variable condition, string name)
    [Cmdlet("New", "CNTKSequenceGather")]
    [Alias("cntk.sequence.gather")]
    public class NewCNTKSequenceGather : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable Condition;

        [Parameter(Position = 2, Mandatory = false)]
        public CNTK.PairSizeTInt NewDerivedSequenceAxisScalingAndAdditiveFactor;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            CNTK.Function result;

            if (NewDerivedSequenceAxisScalingAndAdditiveFactor == null)
                result = CNTK.CNTKLib.SequenceGather(Operand, Condition, Name);
            else
                result = CNTK.CNTKLib.SequenceGather(Operand, Condition, NewDerivedSequenceAxisScalingAndAdditiveFactor, Name);

            WriteObject(new WrappedFunction(result));
        }
    }

    // Signatures:
    // public static CNTK.Function SequenceScatter(CNTK.Variable operand, CNTK.Variable condition)
    // public static CNTK.Function SequenceScatter(CNTK.Variable operand, CNTK.Variable condition, CNTK.PairSizeTInt newDerivedSequenceAxisScalingAndAdditiveFactor)
    // public static CNTK.Function SequenceScatter(CNTK.Variable operand, CNTK.Variable condition, CNTK.PairSizeTInt newDerivedSequenceAxisScalingAndAdditiveFactor, string name)
    // public static CNTK.Function SequenceScatter(CNTK.Variable operand, CNTK.Variable condition, string name)
    [Cmdlet("New", "CNTKSequenceScatter")]
    [Alias("cntk.sequence.scatter")]
    public class NewCNTKSequenceScatter : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable Condition;

        [Parameter(Position = 2, Mandatory = false)]
        public CNTK.PairSizeTInt NewDerivedSequenceAxisScalingAndAdditiveFactor;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            CNTK.Function result;

            if (NewDerivedSequenceAxisScalingAndAdditiveFactor == null)
                result = CNTK.CNTKLib.SequenceScatter(Operand, Condition);
            else
                result = CNTK.CNTKLib.SequenceScatter(Operand, Condition, NewDerivedSequenceAxisScalingAndAdditiveFactor, Name);

            WriteObject(new WrappedFunction(result));
        }
    }
}

