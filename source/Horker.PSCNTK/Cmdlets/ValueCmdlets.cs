using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKValue")]
    [Alias("cntk.value")]
    [OutputType(typeof(Value))]
    public class NewCNTKValue : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public float[] Values;

        [Parameter(Position = 1, Mandatory = false)]
        public int[] Dimensions = null;

        [Parameter(Position = 2, Mandatory = false)]
        public DeviceDescriptor Device = null;

        protected override void EndProcessing()
        {
            if (Dimensions == null)
                Dimensions = new int[] { Values.Length };

            var value = ValueMethods.SafeCreate(Dimensions, Values, Device);
            WriteObject(value);
        }
    }

    [Cmdlet("New", "CNTKBatchOfSequences")]
    [Alias("cntk.batchOfSequences")]
    [OutputType(typeof(Value))]
    public class NewCNTKBatchOfSequenses : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public float[][] Values;

        [Parameter(Position = 1, Mandatory = true)]
        public int[] Dimensions;

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
