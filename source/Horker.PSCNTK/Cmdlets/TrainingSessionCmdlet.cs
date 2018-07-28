using System;
using System.Management.Automation;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Reflection;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("Start", "CNTKTraining2")]
    [Alias("cntk.starttraining2")]
    public class StartCNTKTraining2 : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public Trainer Trainer;

        [Parameter(Position = 1, Mandatory = true)]
        public MinibatchDefinition MinibatchDefinition;

        [Parameter(Position = 2, Mandatory = false)]
        public Hashtable ParameterMap = null;

        [Parameter(Position = 3, Mandatory = false)]
        public int MaxIteration = 10000;

        [Parameter(Position = 4, Mandatory = false)]
        public int ProgressOutputStep = 100;

        [Parameter(Position = 5, Mandatory = false)]
        public DeviceDescriptor Device = null;

        protected override void EndProcessing()
        {
            var session = new TrainingSession2(Trainer, MinibatchDefinition, ParameterMap, Device);

            int sampleCount = 0;
            var loss = 0.0;
            var metric = 0.0;

            foreach (var t in session.GetSession(MaxIteration))
            {
                sampleCount += t.SampleCount;
                loss += t.Loss;
                metric += t.Metric;
                if (t.Iteration % ProgressOutputStep == 0 || t.Iteration == MaxIteration)
                {
                    var p = new TrainingProgress();
                    p.Epoch = t.Epoch;
                    p.Iteration = t.Iteration;
                    p.SampleCount = sampleCount;
                    p.Loss = Math.Round(loss / ProgressOutputStep, 5);
                    p.Metric = Math.Round(metric / ProgressOutputStep, 5);
                    p.Validation = t.GetValidationMetric();
                    WriteObject(p);

                    sampleCount = 0;
                    loss = 0.0;
                    metric = 0.0;
                }
            }
        }
    }

    [Cmdlet("New", "CNTKTrainingSession2")]
    [Alias("cntk.trainingsession2")]
    public class NewCNTKTrainingSession2 : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public Trainer Trainer;

        [Parameter(Position = 1, Mandatory = true)]
        public MinibatchDefinition MinibatchDefinition;

        [Parameter(Position = 2, Mandatory = false)]
        public Hashtable ParameterMap = null;

        [Parameter(Position = 3, Mandatory = false)]
        public DeviceDescriptor Device = null;

        protected override void EndProcessing()
        {
            var session = new TrainingSession2(Trainer, MinibatchDefinition, ParameterMap, Device);

            WriteObject(session);
        }
    }
}
