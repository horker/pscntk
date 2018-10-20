using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKAccuracy")]
    [Alias("cntk.accuracy")]
    public class NewCNTKAccuracy : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedVariable Prediction;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedVariable Labels;

        [Parameter(Position = 2, Mandatory = false)]
        public UInt32? TopN;

        [Parameter(Position = 3, Mandatory = false)]
        public Axis Axis;

        [Parameter(Position = 4, Mandatory = false)]
        public string Name = "";

        protected override void EndProcessing()
        {
            Function result;

            if (TopN.HasValue)
                if (Axis != null)
                    result = CNTKLib.ClassificationError(Prediction, Labels, TopN.Value, Axis, Name);
                else
                    result = CNTKLib.ClassificationError(Prediction, Labels, TopN.Value, Name);
            else
                if (Axis != null)
                    result = CNTKLib.ClassificationError(Prediction, Labels, Axis, Name);
                else
                    result = CNTKLib.ClassificationError(Prediction, Labels, Name);

            WriteObject(CNTKLib.Minus(Constant.Scalar(DataType.Float, 1.0f), result));
        }
        
    }
}
