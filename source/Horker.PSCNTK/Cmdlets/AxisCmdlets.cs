using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKAxis")]
    [Alias("cntk.axis")]
    [OutputType(typeof(Axis))]
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
    [OutputType(typeof(Axis))]
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
    [OutputType(typeof(Axis))]
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
    [OutputType(typeof(Axis))]
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
    [OutputType(typeof(Axis))]
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
    [OutputType(typeof(AxisVector))]
    public class NewCNTKDefaultDynamicAxes : PSCmdlet
    {
        protected override void EndProcessing()
        {
            var axes = Axis.DefaultInputVariableDynamicAxes();
            WriteObject(axes);
        }
    }

    [Cmdlet("New", "CNTKEndStaticAxis")]
    [Alias("cntk.axis.endStatic")]
    [OutputType(typeof(Axis))]
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
    [OutputType(typeof(Axis))]
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
    [OutputType(typeof(Axis))]
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
    [OutputType(typeof(AxisVector))]
    public class NewCNTKUnknownDynamicAxis : PSCmdlet
    {
        protected override void EndProcessing()
        {
            var axis = Axis.UnknownDynamicAxes();
            WriteObject(axis);
        }
    }
}
