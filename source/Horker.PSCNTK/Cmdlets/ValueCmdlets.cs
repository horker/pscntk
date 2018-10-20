using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKBatchOfSequences")]
    [Alias("cntk.batchOfSequences")]
    public class NewCNTKBatchOfSequenses : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public int[] Dimensions;

        [Parameter(Position = 1, Mandatory = true)]
        public float[][] Values;

        [Parameter(Position = 2, Mandatory = false)]
        public DeviceDescriptor Device = null;

        protected override void EndProcessing()
        {
            if (Device == null)
                Device = DeviceDescriptor.UseDefaultDevice();

            var values = Value.CreateBatchOfSequences(Dimensions, Values, Device, false);
            WriteObject(values);
        }
    }
}
