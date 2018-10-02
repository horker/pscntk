using System;
using System.Management.Automation;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKLstm")]
    [Alias("cntk.lstm")]
    public class NewCNTKLstm : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Input;

        [Parameter(Position = 1, Mandatory = true)]
        public int LstmDimension;

        [Parameter(Position = 2, Mandatory = true)]
        public int CellDimension;

        [Parameter(Position = 3, Mandatory = false)]
        public SwitchParameter ReturnSequences = false;

        [Parameter(Position = 4, Mandatory = false)]
        public SwitchParameter Stabilize = false;

        [Parameter(Position = 5, Mandatory = false)]
        public CNTK.DeviceDescriptor Device = null;

        [Parameter(Position = 6, Mandatory = false)]
        public string Name = "LSTM";

        protected override void EndProcessing()
        {
            if (Device == null)
                Device = CNTK.DeviceDescriptor.UseDefaultDevice();

            var lstm = Microsoft.LSTMSequenceClassifierNet.Create(Input, LstmDimension, CellDimension, ReturnSequences, Stabilize, Device, Name);

            WriteObject(lstm);
        }
    }

    [Cmdlet("New", "CNTKStabilize")]
    [Alias("cntk.stabilize")]
    public class NewCNTKStabilize : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.Variable Operand;

        [Parameter(Position = 1, Mandatory = false)]
        public CNTK.DeviceDescriptor Device = null;

        [Parameter(Position = 2, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            if (Device == null)
                Device = CNTK.DeviceDescriptor.UseDefaultDevice();

            var result = Microsoft.LSTMSequenceClassifierNet.Stabilize<float>(Operand, Device, Name);

            WriteObject(result);
        }
    }
}
