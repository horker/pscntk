using System;
using System.Management.Automation;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Reflection;
using CNTK;
using System.IO;
using System.Text;

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

        [Parameter(Position = 2, Mandatory = true)]
        public ISampler ValidationSampler;

        [Parameter(Position = 3, Mandatory = false)]
        public Hashtable DataToInputMap = null;

        [Parameter(Position = 4, Mandatory = false)]
        public int MaxIteration = 10000;

        [Parameter(Position = 5, Mandatory = false)]
        public int ProgressOutputStep = 100;

        [Parameter(Position = 6, Mandatory = false)]
        public string LogFile = null;

        protected override void EndProcessing()
        {
            Logger logger = null;
            if (LogFile != null)
            {
                var writer = new StreamWriter(IO.GetAbsolutePath(this, LogFile), true, new UTF8Encoding(false));
                logger = new Logger(writer);
            }

            try
            {
                var session = new TrainingSession(Trainer, Sampler, ValidationSampler, DataToInputMap, null, null, false);
                foreach (var progress in TrainingLoop.Start(session, MaxIteration, ProgressOutputStep, logger))
                    WriteObject(progress);
            }
            finally
            {
                if (logger != null)
                    logger.Writer.Close();
            }
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

        [Parameter(Position = 2, Mandatory = true)]
        public ISampler ValidationSampler;

        [Parameter(Position = 3, Mandatory = false)]
        public Hashtable DataToInputMap = null;

        [Parameter(Position = 4, Mandatory = false)]
        public SwitchParameter KeepMinibatch;

        protected override void EndProcessing()
        {
            var session = new TrainingSession(Trainer, Sampler, ValidationSampler, DataToInputMap, null, null, KeepMinibatch);
            WriteObject(session);
        }
    }
}
