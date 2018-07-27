using System;
using System.Management.Automation;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Reflection;
using CNTK;

namespace Horker.PSCNTK
{
    [Cmdlet("New", "CNTKTrainer")]
    [Alias("cntk.trainer")]
    public class NewCNTKTrainer : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public Variable Model;

        [Parameter(Position = 1, Mandatory = true)]
        public Variable Label;

        [Parameter(Position = 2, Mandatory = true)]
        public string LossFunctionName;

        [Parameter(Position = 3, Mandatory = true)]
        public string ErrorFunctionName;

        [Parameter(Position = 4, Mandatory = true)]
        public Learner[] Learners;

        private MethodInfo FindMethod(string name)
        {
            var methods = typeof(CNTKLib).GetMethods(BindingFlags.Public | BindingFlags.Static);

            foreach (var m in methods)
            {
                if (m.Name == name)
                {
                    var p = m.GetParameters();
                    if (p.Length == 2 && p[0].ParameterType == typeof(Variable) && p[1].ParameterType == typeof(Variable))
                    {
                        return m;
                    }
                }
            }

            throw new ArgumentException("LossFunctionName doesn't indicate the proper CNTK function name");
        }

        protected override void EndProcessing()
        {
            MethodInfo lossMethod = FindMethod(LossFunctionName);
            MethodInfo errorMethod = FindMethod(ErrorFunctionName);

            var loss = (Function)lossMethod.Invoke(null, new object[] { Model, Label });
            var error = (Function)errorMethod.Invoke(null, new object[] { Model, Label });

            var trainer = Trainer.CreateTrainer(Model, loss, error, Learners.ToList());
            WriteObject(trainer);
        }
    }

    public class TrainingProgress
    {
        public int Epoch;
        public int Iteration;
        public UInt32 SampleCount;
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
        public MinibatchSource TrainingData;

        [Parameter(Position = 2, Mandatory = true)]
        public Dictionary<object, Value> ValidationData;

        [Parameter(Position = 3, Mandatory = true)]
        public UInt32 MinibatchSize;

        [Parameter(Position = 4, Mandatory = true)]
        public int ValidationSampleSize;

        [Parameter(Position = 5, Mandatory = false)]
        public Hashtable ParameterMap = null;

        [Parameter(Position = 6, Mandatory = false)]
        public int MaxIteration = 10000;

        [Parameter(Position = 7, Mandatory = false)]
        public int ProgressOutputStep = 100;

        protected override void EndProcessing()
        {
            var session = new TrainingSession(Trainer, TrainingData, ValidationData, MinibatchSize, ValidationSampleSize, ParameterMap);

            UInt32 sampleCount = 0;
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

    [Cmdlet("New", "CNTKTrainingSession")]
    [Alias("cntk.trainingsession")]
    public class NewCNTKTrainingSession : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public Trainer Trainer;

        [Parameter(Position = 1, Mandatory = true)]
        public MinibatchSource TrainingData;

        [Parameter(Position = 2, Mandatory = true)]
        public Dictionary<object, Value> ValidationData;

        [Parameter(Position = 3, Mandatory = true)]
        public UInt32 MinibatchSize;

        [Parameter(Position = 4, Mandatory = false)]
        public int ValidationSampleSize = 0;

        [Parameter(Position = 5, Mandatory = false)]
        public Hashtable ParameterMap = null;

        protected override void EndProcessing()
        {
            var session = new TrainingSession(Trainer, TrainingData, ValidationData, MinibatchSize, ValidationSampleSize, ParameterMap).GetSession();

            WriteObject(session);
        }
    }
}
