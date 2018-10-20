using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKAxis")]
    [Alias("cntk.axis")]
    public class NewCNTKAxis : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public int StaticAxisIndex;

        protected override void EndProcessing()
        {
            var axis = new Axis(StaticAxisIndex);
            WriteObject(axis);
        }
    }

    [Cmdlet("New", "CNTKDynamicAxis")]
    [Alias("cntk.axis.dynamic")]
    public class NewCNTKDynamicAxis : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string Name;

        [Parameter(Position = 0, Mandatory = false)]
        public SwitchParameter NoOrderedDynamicAxis;

        protected override void EndProcessing()
        {
            var axis = new Axis(Name, !NoOrderedDynamicAxis);
            WriteObject(axis);
        }
    }

    [Cmdlet("New", "CNTKAllAxis")]
    [Alias("cntk.axis.all")]
    public class NewCNTKAllAxis : PSCmdlet
    {
        protected override void EndProcessing()
        {
            var axis = Axis.AllAxes();
            WriteObject(axis);
        }
    }

    [Cmdlet("New", "CNTKAllStaticAxis")]
    [Alias("cntk.axis.allStatic")]
    public class NewCNTKStaticAxis : PSCmdlet
    {
        protected override void EndProcessing()
        {
            var axis = Axis.AllStaticAxes();
            WriteObject(axis);
        }
    }

    [Cmdlet("New", "CNTKDefaultBatchAxis")]
    [Alias("cntk.axis.defaultBatch")]
    public class NewCNTKDefaultBatchAxis : PSCmdlet
    {
        protected override void EndProcessing()
        {
            var axis = Axis.DefaultBatchAxis();
            WriteObject(axis);
        }
    }

    [Cmdlet("New", "CNTKDefaultInputVariableDynamicAxes")]
    [Alias("cntk.axis.defaultInputVariableDynamic")]
    public class NewCNTKDefaultDynamicAxes : PSCmdlet
    {
        protected override void EndProcessing()
        {
            var axis = Axis.DefaultInputVariableDynamicAxes();
            WriteObject(axis);
        }
    }

    [Cmdlet("New", "CNTKEndStaticAxis")]
    [Alias("cntk.axis.endStatic")]
    public class NewCNTKEndStaticAxis : PSCmdlet
    {
        protected override void EndProcessing()
        {
            var axis = Axis.EndStaticAxis();
            WriteObject(axis);
        }
    }

    [Cmdlet("New", "CNTKUniqueDynamicAxis")]
    [Alias("cntk.axis.uniqueDynamic")]
    public class NewCNTKUniqueDyanamicAxis : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string AxisNamePrefix;

        [Parameter(Position = 0, Mandatory = false)]
        public SwitchParameter NoOrderedDynamicAxis;

        protected override void EndProcessing()
        {
            var axis = Axis.NewUniqueDynamicAxis(AxisNamePrefix, !NoOrderedDynamicAxis);
            WriteObject(axis);
        }
    }

    [Cmdlet("New", "CNTKOperandSequenceAxis")]
    [Alias("cntk.axis.operandSequence")]
    public class NewCNTKOperandSequenceAxis : PSCmdlet
    {
        protected override void EndProcessing()
        {
            var axis = Axis.OperandSequenceAxis();
            WriteObject(axis);
        }
    }

    [Cmdlet("New", "CNTKUnknownDynamicAxis")]
    [Alias("cntk.axis.unknownDynamic")]
    public class NewCNTKUnknownDynamicAxis : PSCmdlet
    {
        protected override void EndProcessing()
        {
            var axis = Axis.UnknownDynamicAxes();
            WriteObject(axis);
        }
    }
}
