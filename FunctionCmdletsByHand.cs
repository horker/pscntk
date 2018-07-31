using System;
using System.Management.Automation;

// DO NOT EDIT
// This file was automatically generated at 2018/08/01 01:18:56

namespace Horker.PSCNTK {

    [Cmdlet("New", "CNTKClassificationError")]
    [Alias("cntk.classificationerror")]
    public class NewCNTKClassificationError : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Prediction;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable Labels;

        [Parameter(Position = 2, Mandatory = true)]
        public UInt32 TopN;

        [Parameter(Position = 3, Mandatory = true)]
        public CNTK.Axis Axis;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ClassificationError(Prediction, Labels, TopN, Axis, Name);
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
        public CNTK.Axis Axis;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ClassificationError(Prediction, Labels, Axis, Name);
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
        public UInt32 TopN;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ClassificationError(Prediction, Labels, TopN, Name);
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

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ClassificationError(Prediction, Labels, Name);
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
        public CNTK.Variable AncestorInput;

        [Parameter(Position = 3, Mandatory = true)]
        public CNTK.Variable AncestorReferent;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Crop(NodeInput, NodeReferent, AncestorInput, AncestorReferent, Name);
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
        public UInt32 OffsetX;

        [Parameter(Position = 3, Mandatory = true)]
        public UInt32 OffsetY;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Crop(NodeInput, NodeReferent, OffsetX, OffsetY, Name);
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

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Crop(NodeInput, NodeReferent, Name);
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

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.CrossEntropyWithSoftmax(Prediction, Labels, Name);
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

    [Cmdlet("New", "CNTKELU")]
    [Alias("cntk.elu")]
    public class NewCNTKELU : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public double Alpha;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ELU(Operand, Alpha, Name);
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

    [Cmdlet("New", "CNTKFlatten")]
    [Alias("cntk.flatten")]
    public class NewCNTKFlatten : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Flatten(Operand, Name);
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

        [Parameter(Position = 2, Mandatory = false)]
        public UInt32 Offset = 1;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.FutureValue(Operand, InitialState, Offset, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKFutureValue")]
    [Alias("cntk.futurevalue")]
    public class NewCNTKFutureValue : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public UInt32 Offset = 1;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.FutureValue(Operand, Offset, Name);
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

        [Parameter(Position = 2, Mandatory = true)]
        public CNTK.Axis Axis;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.GatherOp(Indices, Reference, Axis, Name);
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

    [Cmdlet("New", "CNTKLogSoftmax")]
    [Alias("cntk.logsoftmax")]
    public class NewCNTKLogSoftmax : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.LogSoftmax(Operand, Axis, Name);
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

    [Cmdlet("New", "CNTKMeanVarianceNormalization")]
    [Alias("cntk.meanvariancenormalization")]
    public class NewCNTKMeanVarianceNormalization : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public bool UseStatsAcrossChannels = false;

        [Parameter(Position = 2, Mandatory = false)]
        public bool DoVarianceScaling = true;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.MeanVarianceNormalization(Operand, UseStatsAcrossChannels, DoVarianceScaling, Name);
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

        [Parameter(Position = 2, Mandatory = false)]
        public bool UseStatsAcrossChannels = false;

        [Parameter(Position = 3, Mandatory = false)]
        public bool DoVarianceScaling = true;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.MeanVarianceNormalization(Operand, Epsilon, UseStatsAcrossChannels, DoVarianceScaling, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKoperator-")]
    [Alias("cntk.operator-")]
    public class NewCNTKoperator- : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.operator-(Operand);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKoperator-")]
    [Alias("cntk.operator-")]
    public class NewCNTKoperator- : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable RightOperand;

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.operator-(LeftOperand, RightOperand);
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

        [Parameter(Position = 2, Mandatory = false)]
        public UInt32 Offset = 1;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.PastValue(Operand, InitialState, Offset, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKPastValue")]
    [Alias("cntk.pastvalue")]
    public class NewCNTKPastValue : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public UInt32 Offset = 1;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.PastValue(Operand, Offset, Name);
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
        public CNTK.NDArrayView Mean;

        [Parameter(Position = 2, Mandatory = true)]
        public CNTK.NDArrayView InvStdDev;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.PerDimMeanVarianceNormalize(Operand, Mean, InvStdDev, Name);
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

    [Cmdlet("New", "CNTKReduceLogSum")]
    [Alias("cntk.reducelogsum")]
    public class NewCNTKReduceLogSum : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis[] Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceLogSum(Operand, new CNTK.AxisVector(Axis), Name);
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

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceMax(Operand, Name);
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
        public CNTK.Axis[] Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceMax(Operand, new CNTK.AxisVector(Axis), Name);
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
        public CNTK.Axis[] Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceMean(Operand, new CNTK.AxisVector(Axis), Name);
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
        public CNTK.Axis[] Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceMin(Operand, new CNTK.AxisVector(Axis), Name);
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
        public CNTK.Axis[] Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceProd(Operand, new CNTK.AxisVector(Axis), Name);
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

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceSum(Operand, Name);
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

    [Cmdlet("New", "CNTKReduceSum")]
    [Alias("cntk.reducesum")]
    public class NewCNTKReduceSum : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis[] Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ReduceSum(Operand, new CNTK.AxisVector(Axis), Name);
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
        public int[] NewShape;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Reshape(Operand, NewShape, Name);
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
        public CNTK.Axis[] Axis;

        [Parameter(Position = 2, Mandatory = true)]
        public int[] BeginIndex;

        [Parameter(Position = 3, Mandatory = true)]
        public int[] EndIndex;

        [Parameter(Position = 4, Mandatory = true)]
        public int[] Strides;

        [Parameter(Position = 5, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Slice(Operand, new CNTK.AxisVector(Axis), BeginIndex, EndIndex, Strides, Name);
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
        public CNTK.Axis[] Axis;

        [Parameter(Position = 2, Mandatory = true)]
        public int[] BeginIndex;

        [Parameter(Position = 3, Mandatory = true)]
        public int[] EndIndex;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Slice(Operand, new CNTK.AxisVector(Axis), BeginIndex, EndIndex, Name);
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
        public int BeginIndex;

        [Parameter(Position = 2, Mandatory = true)]
        public int EndIndex;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Slice(Operand, BeginIndex, EndIndex, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKSoftmax")]
    [Alias("cntk.softmax")]
    public class NewCNTKSoftmax : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Softmax(Operand, Axis, Name);
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

    [Cmdlet("New", "CNTKSqueeze")]
    [Alias("cntk.squeeze")]
    public class NewCNTKSqueeze : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis[] Axis;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Squeeze(Operand, new CNTK.AxisVector(Axis), Name);
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
        public UInt32 OutputRank;

        [Parameter(Position = 3, Mandatory = true)]
        public int InferInputRankToMap;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Times(LeftOperand, RightOperand, OutputRank, InferInputRankToMap, Name);
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
        public UInt32 OutputRank;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Times(LeftOperand, RightOperand, OutputRank, Name);
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

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Times(LeftOperand, RightOperand, Name);
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
        public UInt32 K;

        [Parameter(Position = 2, Mandatory = true)]
        public CNTK.Axis Axis;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.TopK(Operand, K, Axis, Name);
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
        public UInt32 K;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.TopK(Operand, K, Name);
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

    [Cmdlet("New", "CNTKToSequence")]
    [Alias("cntk.tosequence")]
    public class NewCNTKToSequence : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable SequenceLengths;

        [Parameter(Position = 2, Mandatory = true)]
        public string SequenceAxisNamePrefix;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.ToSequence(Operand, SequenceLengths, SequenceAxisNamePrefix, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKTranspose")]
    [Alias("cntk.transpose")]
    public class NewCNTKTranspose : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Axis[] Permutation;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.Transpose(Operand, new CNTK.AxisVector(Permutation), Name);
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

    [Cmdlet("New", "CNTKTransposeTimes")]
    [Alias("cntk.transposetimes")]
    public class NewCNTKTransposeTimes : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable LeftOperand;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTK.Variable RightOperand;

        [Parameter(Position = 2, Mandatory = true)]
        public UInt32 OutputRank;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.TransposeTimes(LeftOperand, RightOperand, OutputRank, Name);
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

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = CNTK.CNTKLib.TransposeTimes(LeftOperand, RightOperand, Name);
            WriteObject(result);
        }
    }
}

