using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Text;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKPredeterminedScheduler")]
    [Alias("cntk.scheduler.predetermined")]
    [OutputType(typeof(PredeterminedScheduler))]
    public class NewCNTKPredeterminedSchedule : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public double[] Schedule;

        protected override void BeginProcessing()
        {
            if (Schedule.Length % 2 != 0)
            {
                WriteError(new ErrorRecord(new ArgumentException("Schedule should be sets of pairs of iteration size and learning rate"), "", ErrorCategory.InvalidArgument, null));
                return;
            }

            var scheduler = new PredeterminedScheduler();

            for (var i = 0; i < Schedule.Length; i += 2)
                scheduler.AddLearningSchedule((int)Schedule[i], Schedule[i + 1]);

            WriteObject(scheduler);
        }
    }

    [Cmdlet("New", "CNTKExponentialScheduler")]
    [Alias("cntk.scheduler.exponential")]
    [OutputType(typeof(ExponentialScheduler))]
    public class NewCNTKExponentialScheduler : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public double InitialRate;

        [Parameter(Position = 1, Mandatory = true)]
        public double TimeScale;

        protected override void BeginProcessing()
        {
            var scheduler = new ExponentialScheduler(InitialRate, TimeScale);
            WriteObject(scheduler);
        }
    }

    [Cmdlet("New", "CNTKPowerScheduler")]
    [Alias("cntk.scheduler.power")]
    [OutputType(typeof(PowerScheduler))]
    public class NewCNTKPowerScheduler : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public double InitialRate;

        [Parameter(Position = 1, Mandatory = true)]
        public double TimeScale;

        [Parameter(Position = 2, Mandatory = false)]
        public double Exponent = 1;

        protected override void BeginProcessing()
        {
            var scheduler = new PowerScheduler(InitialRate, TimeScale, Exponent);
            WriteObject(scheduler);
        }
    }

    [Cmdlet("New", "CNTKPerformanceScheduler")]
    [Alias("cntk.scheduler.performance")]
    [OutputType(typeof(PerformanceScheduler))]
    public class NewCNTKPerformanceScheduler : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public double InitialRate;

        [Parameter(Position = 1, Mandatory = true)]
        public double DecayRate;

        [Parameter(Position = 2, Mandatory = true)]
        public int UpdateInterval;

        [Parameter(Position = 3, Mandatory = false)]
        public double Smoothing = double.NaN;

        protected override void BeginProcessing()
        {
            var scheduler = new PerformanceScheduler(InitialRate, DecayRate, UpdateInterval, Smoothing);
            WriteObject(scheduler);
        }
    }

    [Cmdlet("New", "CNTKCosineAnnealingScheduler")]
    [Alias("cntk.scheduler.cosineAnnealing")]
    [OutputType(typeof(CosineAnealingScheduler))]
    public class NewCNTKCosineAnnealingScheduler : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public double MinimumRate;

        [Parameter(Position = 1, Mandatory = true)]
        public double MaximumRate;

        [Parameter(Position = 2, Mandatory = true)]
        public double Step;

        protected override void BeginProcessing()
        {
            var scheduler = new CosineAnealingScheduler(MinimumRate, MaximumRate, Step);
            WriteObject(scheduler);
        }
    }

    [Cmdlet("New", "CNTKOneCycleScheduler")]
    [Alias("cntk.scheduler.oneCycle")]
    [OutputType(typeof(OneCycleScheduler))]
    public class NewCNTKOneCycleScheduler : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public double InitialRate;

        [Parameter(Position = 1, Mandatory = true)]
        public double MaximumRate;

        [Parameter(Position = 2, Mandatory = true)]
        public double MinimumRate;

        [Parameter(Position = 3, Mandatory = true)]
        public int Step;

        protected override void BeginProcessing()
        {
            var scheduler = new OneCycleScheduler(InitialRate, MaximumRate, MinimumRate, Step);
            WriteObject(scheduler);
        }
    }

    [Cmdlet("New", "CNTKCombinedScheduler")]
    [Alias("cntk.scheduler.combined")]
    [OutputType(typeof(CombinedScheduler))]
    public class NewCNTKCombinedScheduler : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public ILearningScheduler[] Schedulers;

        protected override void BeginProcessing()
        {
            var scheduler = new CombinedScheduler(Schedulers);
            WriteObject(scheduler);
        }
    }
}
