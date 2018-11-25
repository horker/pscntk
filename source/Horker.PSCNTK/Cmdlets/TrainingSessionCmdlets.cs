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
    [Alias("cntk.startTraining")]
    [OutputType(typeof(TrainingProgress))]
    public class StartCNTKTraining : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedFunction Model;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedFunction LossFunction;

        [Parameter(Position = 2, Mandatory = false)]
        [AllowNull()]
        public WrappedFunction EvaluationFunction;

        [Parameter(Position = 3, Mandatory = true)]
        public Learner Learner;

        [Parameter(Position = 4, Mandatory = false)]
        public ILearningScheduler LearningScheduler;

        [Parameter(Position = 5, Mandatory = true)]
        public ISampler Sampler;

        [Parameter(Position = 6, Mandatory = true)]
        public ISampler ValidationSampler;

        [Parameter(Position = 7, Mandatory = false)]
        public Hashtable DataToInputMap = null;

        [Parameter(Position = 8, Mandatory = false)]
        public int MaxIteration = int.MaxValue;

        [Parameter(Position = 9, Mandatory = false)]
        public int ProgressOutputStep = 100;

        [Parameter(Position = 10, Mandatory = false)]
        public Logger Logger = null;

        protected override void EndProcessing()
        {
            try
            {
                var session = new TrainingSession(Model, LossFunction, EvaluationFunction, Learner, LearningScheduler, Sampler, ValidationSampler, DataToInputMap, null, null);
                foreach (var progress in TrainingLoop.Start(session, MaxIteration, ProgressOutputStep, Logger))
                    WriteObject(progress);
            }
            finally
            {
                Sampler.Dispose();
                ValidationSampler.Dispose();
            }
        }
    }

    [Cmdlet("New", "CNTKTrainingSession")]
    [Alias("cntk.trainingSession")]
    [OutputType(typeof(TrainingSession))]
    public class NewCNTKTrainingSession : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public WrappedFunction Model;

        [Parameter(Position = 1, Mandatory = true)]
        public WrappedFunction LossFunction;

        [Parameter(Position = 2, Mandatory = false)]
        [AllowNull()]
        public WrappedFunction EvaluationFunction;

        [Parameter(Position = 3, Mandatory = true)]
        public Learner Learner;

        [Parameter(Position = 4, Mandatory = false)]
        public ILearningScheduler LearningScheduler;

        [Parameter(Position = 5, Mandatory = true)]
        public ISampler Sampler;

        [Parameter(Position = 6, Mandatory = true)]
        public ISampler ValidationSampler;

        [Parameter(Position = 7, Mandatory = false)]
        public Hashtable DataToInputMap = null;

        protected override void EndProcessing()
        {
            var session = new TrainingSession(Model, LossFunction, EvaluationFunction, Learner, LearningScheduler, Sampler, ValidationSampler, DataToInputMap, null, null);
            WriteObject(session);
        }
    }
}
