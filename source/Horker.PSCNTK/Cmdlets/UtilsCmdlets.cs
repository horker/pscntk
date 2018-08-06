using System.Management.Automation;

namespace Horker.PSCNTK
{
    [Cmdlet("Get", "CNTKMaxNumCPUThreads")]
    [Alias("cntk.maxcputhreads")]
    public class GetCNTKMaxCPUThreadCount : PSCmdlet
    {
        protected override void EndProcessing()
        {
            var result = CNTK.Utils.GetMaxNumCPUThreads();
            WriteObject(result);
        }
    }

    [Cmdlet("Set", "CNTKMaxCPUThreadCount")]
    [Alias("cntk.setmaxcputhreads")]
    public class SetCNTKMaxCPUThreadCount : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public int NumCPUThreads;

        protected override void EndProcessing()
        {
            CNTK.Utils.SetMaxNumCPUThreads(NumCPUThreads);
        }
    }

    [Cmdlet("Get", "CNTKTraceLevel")]
    [Alias("cntk.tracelevel")]
    public class GetCNTKTraceLvel : PSCmdlet
    {
        protected override void EndProcessing()
        {
            var result = CNTK.Utils.GetTraceLevel();
            WriteObject(result);
        }
    }

    [Cmdlet("Set", "CNTKTraceLevel")]
    [Alias("cntk.settracelevel")]
    public class SetCNTTraceLevel : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public CNTK.TraceLevel TraceLevel;

        protected override void EndProcessing()
        {
            CNTK.Utils.SetTraceLevel(TraceLevel);
        }
    }
}
