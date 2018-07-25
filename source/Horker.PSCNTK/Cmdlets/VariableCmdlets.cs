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
        SwitchParameter Sparse = false;

        [Parameter(Position = 4, Mandatory = false)]
        SwitchParameter NeedsGradient = false;

        [Parameter(Position = 5, Mandatory = false)]
        DataType DataType = DataType.Float;

        protected override void EndProcessing()
        {
            var result = Variable.InputVariable(Dimensions, DataType, Name, DynamicAxes, Sparse, NeedsGradient);
            WriteObject(result);
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
            WriteObject(result);
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
        DataType DataType = DataType.Float;

        [Parameter(Position = 3, Mandatory = false)]
        DeviceDescriptor Device = DeviceDescriptor.UseDefaultDevice();

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = new Parameter(Dimensions, DataType, Initializer, Device, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("New", "CNTKConstant")]
    [Alias("cntk.constant")]
    public class NewCNTKConstant : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public int[] Dimensions;

        [Parameter(Position = 1, Mandatory = false)]
        public float InitValue = 0;

        [Parameter(Position = 2, Mandatory = false)]
        public DataType DataType = DataType.Float;

        [Parameter(Position = 3, Mandatory = false)]
        public DeviceDescriptor Device = DeviceDescriptor.UseDefaultDevice();

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            var result = new Constant(Dimensions, DataType, InitValue, Device, Name);
            WriteObject(result);
        }
    }

    [Cmdlet("Restore", "CNTKFunction")]
    [Alias("cntk.restore", "cntk.load")]
    public class LoadCNTKFunction : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string Path;

        [Parameter(Position = 1, Mandatory = false)]
        public DeviceDescriptor Device = DeviceDescriptor.UseDefaultDevice();

        [Parameter(Position = 2, Mandatory = false)]
        public ModelFormat Format = ModelFormat.CNTKv2;

        protected override void EndProcessing()
        {
            var result = Function.Load(Path, Device, Format);
            WriteObject(result);
        }
    }
}
