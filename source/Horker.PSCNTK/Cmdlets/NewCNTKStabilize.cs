using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKStabilize")]
    [Alias("cntk.stabilize")]
    public class NewCNTKStabilize : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public double Steepness = 4;

        [Parameter(Position = 2, Mandatory = false)]
        public DeviceDescriptor Device = null;

        [Parameter(Position = 3, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            if (Device == null)
                Device = DeviceDescriptor.UseDefaultDevice();

            var result = Composite.Stabilize(Operand, Steepness, Device, Name);

            WriteObject(new WrappedFunction(result));
        }
    }
}
