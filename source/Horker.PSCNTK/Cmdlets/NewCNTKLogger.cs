using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKLogger")]
    [Alias("cntk.logger")]
    [OutputType(typeof(Logger))]
    public class NewCNTKLogger : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string LogFile;

        [Parameter(Position = 1, Mandatory = false)]
        public SwitchParameter Append = false;

        [Parameter(Position = 2, Mandatory = false)]
        public string DefaultSource = "";

        protected override void EndProcessing()
        {
            var logger = new Logger(IO.GetAbsolutePath(this, LogFile), Append, DefaultSource);
            WriteObject(logger);
        }
    }
}
