using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
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
            WriteObject(new WrappedFunction(result));
        }
    }
}
