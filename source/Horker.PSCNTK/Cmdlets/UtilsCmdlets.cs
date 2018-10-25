using System.Management.Automation;
using System.Runtime.InteropServices;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("Get", "CNTKMaxNumCPUThreads")]
    [Alias("cntk.getMaxCpuThreads", "cntk.maxCpuThreads")]
    public class GetCNTKMaxCPUThreadCount : PSCmdlet
    {
        protected override void EndProcessing()
        {
            var result = Utils.GetMaxNumCPUThreads();
            WriteObject(result);
        }
    }

    [Cmdlet("Set", "CNTKMaxCPUThreadCount")]
    [Alias("cntk.setMaxCpuThreads")]
    public class SetCNTKMaxCPUThreadCount : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public int NumCPUThreads;

        protected override void EndProcessing()
        {
            Utils.SetMaxNumCPUThreads(NumCPUThreads);
        }
    }

    [Cmdlet("Get", "CNTKTraceLevel")]
    [Alias("cntk.getTraceLevel", "cntk.traceLevel")]
    public class GetCNTKTraceLvel : PSCmdlet
    {
        protected override void EndProcessing()
        {
            var result = Utils.GetTraceLevel();
            WriteObject(result);
        }
    }

    [Cmdlet("Set", "CNTKTraceLevel")]
    [Alias("cntk.setTraceLevel")]
    public class SetCNTTraceLevel : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public TraceLevel TraceLevel;

        protected override void EndProcessing()
        {
            Utils.SetTraceLevel(TraceLevel);
        }
    }

    [Cmdlet("Set", "CNTKRandomSeed")]
    [Alias("cntk.setRandomSeed")]
    public class SetCNTRandomSeed : PSCmdlet
    {
        [DllImport("Cntk.Core-2.6.dll", EntryPoint = "?SetFixedRandomSeed@Internal@CNTK@@YAXK@Z", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SetFixedRandomSeed(uint value);

        [Parameter(Position = 0, Mandatory = false)]
        public int Value = 1234;

        protected override void EndProcessing()
        {
            SetFixedRandomSeed((uint)Value);
        }
    }

    [Cmdlet("Reset", "CNTKRandomSeed")]
    [Alias("cntk.resetRandomSeed")]
    public class ResetCNTRandomSeed : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = false)]
        public int Value = 0;

        protected override void EndProcessing()
        {
            if (MyInvocation.BoundParameters.ContainsKey("Value"))
                CNTKLib.ResetRandomSeed((uint)Value);
            else
                CNTKLib.ResetRandomSeed();
        }
    }

    [Cmdlet("Get", "CNTKRandomSeed")]
    [Alias("cntk.getRandomSeed", "cntk.randomSeed")]
    public class GetCNTRandomSeed : PSCmdlet
    {
        protected override void EndProcessing()
        {
            WriteObject(CNTKLib.GetRandomSeed());
        }
    }

    [Cmdlet("Test", "CNTKRandomSeedFixed")]
    [Alias("cntk.testRandomSeedFixed", "cntk.isRandomSeedFixed")]
    public class TestCNTRandomSeedFixed : PSCmdlet
    {
        protected override void EndProcessing()
        {
            WriteObject(CNTKLib.IsRandomSeedFixed());
        }
    }

    // Unimplemented:
    // void DisableCPUEvalOptimization()
    // GenerateRandomSeed
    // RandomInitializerWithRank
    // SetMathLibTraceLevel
    // UseSparseGradientAggregationInDataParallelSGD

}
