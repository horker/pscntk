using System;
using System.Management.Automation;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Reflection;
using CNTK;

namespace Horker.PSCNTK
{
    public class TrainingProgress
    {
        public int Epoch;
        public int Iteration;
        public int SampleCount;
        public double Loss;
        public double Metric;
        public double Validation;
    }

    [Cmdlet("Start", "CNTKTraining")]
    [Alias("cntk.starttraining")]
    public class StartCNTKTraining : PSCmdlet
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
            var session = new TrainingSession(Trainer, MinibatchDefinition, ParameterMap, Device);

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
                    p.Validation = Math.Round(t.GetValidationMetric(), 5);
                    WriteObject(p);

                    sampleCount = 0;
                    loss = 0.0;
                    metric = 0.0;
                }
            }
        }
    }

    [Cmdlet("New", "CNTKTrainingSession")]
    [Alias("cntk.trainingsession")]
    public class NewCNTKTrainingSession : PSCmdlet
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
            var session = new TrainingSession(Trainer, MinibatchDefinition, ParameterMap, Device);

            WriteObject(session);
        }
    }
}
