using System;
using System.Management.Automation;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Reflection;
using CNTK;

namespace Horker.PSCNTK
{

    [Cmdlet("Start", "CNTKTraining")]
    [Alias("cntk.starttraining")]
    [OutputType(typeof(TrainingProgress))]
    public class StartCNTKTraining : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public Trainer Trainer;

        [Parameter(Position = 1, Mandatory = true)]
        public ISampler Sampler;

        [Parameter(Position = 2, Mandatory = false)]
        public Hashtable DataToInputMap = null;

        [Parameter(Position = 3, Mandatory = false)]
        public int MaxIteration = 10000;

        [Parameter(Position = 4, Mandatory = false)]
        public int ProgressOutputStep = 100;

        protected override void EndProcessing()
        {
            var session = new TrainingSession(Trainer, Sampler, DataToInputMap);
            foreach (var progress in TrainingLoop.Start(session, MaxIteration, ProgressOutputStep))
                WriteObject(progress);
        }
    }

    [Cmdlet("New", "CNTKTrainingSession")]
    [Alias("cntk.trainingsession")]
    [OutputType(typeof(TrainingSession))]
    public class NewCNTKTrainingSession : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public Trainer Trainer;

        [Parameter(Position = 1, Mandatory = true)]
        public ISampler Sampler;

        [Parameter(Position = 2, Mandatory = false)]
        public Hashtable DataToInputMap = null;

        protected override void EndProcessing()
        {
            var session = new TrainingSession(Trainer, Sampler, DataToInputMap);
            WriteObject(session);
        }
    }
}
