using System;
using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKInput")]
    [Alias("cntk.input")]
    public class NewCNTKInput : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public int[] Dimensions;

        [Parameter(Position = 1, Mandatory = false)]
        public string Name = "";

        [Parameter(Position = 2, Mandatory = false)]
        public Axis[] DynamicAxes = null;

        [Parameter(Position = 3, Mandatory = false)]
        public SwitchParameter Sparse = false;

        [Parameter(Position = 4, Mandatory = false)]
        public SwitchParameter NeedsGradient = false;

        [Parameter(Position = 5, Mandatory = false)]
        public DataType DataType = DataType.Float;

        [Parameter(Position = 6, Mandatory = false)]
        public SwitchParameter WithSequenceAxis = false;

        protected override void EndProcessing()
        {
            // Variable.InputVariable() creates two dynamic axes, a default dynamic axis and a default batch axis, when no axes are specified.
            // However, a default dynamic axis is not necessary for non-sequential model.
            // More often than not, its existence is troublesome because it often leads miscalculation of the shapes of minibatch data without any error.
            // Thus we change the default behavior to create a default batch axis only unless the -WithSequenceAxis switch is set.

            if (DynamicAxes != null && WithSequenceAxis)
                throw new ArgumentException("-WithSequenceAxis and -DynamicAxes should not be specified at the same time");

            if (DynamicAxes == null && !WithSequenceAxis)
                DynamicAxes = new Axis[] { Axis.DefaultBatchAxis() };

            var result = Variable.InputVariable(Dimensions, DataType, Name, DynamicAxes, Sparse, NeedsGradient);
            WriteObject(new WrappedVariable(result));
        }
    }

    [Cmdlet("New", "CNTKPlaceholder")]
    [Alias("cntk.placeholder")]
    public class NewCNTKPlaceholder : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public int[] Dimensions;

        [Parameter(Position = 1, Mandatory = false)]
        public Axis[] DynamicAxes = null;

        protected override void EndProcessing()
        {
            var result = Variable.PlaceholderVariable(Dimensions, DynamicAxes);
            WriteObject(new WrappedVariable(result));
        }
    }

    [Cmdlet("New", "CNTKParameter")]
    [Alias("cntk.parameter")]
    public class NewCNTKParameter : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public int[] Dimensions;

        [Parameter(Position = 1, Mandatory = true)]
        public CNTKDictionary Initializer;

        [Parameter(Position = 2, Mandatory = false)]
        public DataType DataType = DataType.Float;

        [Parameter(Position = 3, Mandatory = false)]
        public DeviceDescriptor Device = DeviceDescriptor.UseDefaultDevice();

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = new Parameter(Dimensions, DataType, Initializer, Device, Name);
            WriteObject(new WrappedVariable(result));
        }
    }

    [Cmdlet("New", "CNTKConstant")]
    [Alias("cntk.constant")]
    public class NewCNTKConstant : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public int[] Dimensions;

        [Parameter(Position = 1, Mandatory = false)]
        public float[] InitValue = new float[] { 0.0f };

        [Parameter(Position = 2, Mandatory = false)]
        public DataType DataType = DataType.Float;

        [Parameter(Position = 3, Mandatory = false)]
        public DeviceDescriptor Device = DeviceDescriptor.UseDefaultDevice();

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            CNTK.Constant result;

            if (InitValue.Length == 1)
                result = new Constant(Dimensions, DataType, InitValue[0], Device, Name);
            else
            {
                var array = NDArrayViewMethods.SafeCreate(Dimensions, InitValue, Device);
                result = new Constant(array, Name);
            }

            WriteObject(new WrappedVariable(result));
        }
    }
}
