using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using CNTK;

namespace Horker.PSCNTK
{
    public class TrainingSession
    {
        public Trainer Trainer { get; private set; }
        public ISampler Sampler { get; private set; }
        public Dictionary<string, Variable> ParameterMap { get; private set; }

        public int Epoch { get; private set; }
        public int Iteration { get; private set; }

        public int SampleCount { get; private set; }
        public double Loss { get; private set;}
        public double Metric { get; private set;}

        private Stopwatch _stopwatch;
        public TimeSpan Elapsed { get; private set; }

        private UnorderedMapVariableMinibatchData _validationData;

        public TrainingSession(Trainer trainer, ISampler sampler, Hashtable parameterMap = null)
        {
            _stopwatch = Stopwatch.StartNew();

            Trainer = trainer;
            Sampler = sampler;

            ParameterMap = new Dictionary<string, Variable>();
            if (parameterMap != null)
            {
                foreach (DictionaryEntry entry in parameterMap)
                {
                    if (entry.Value is Variable)
                        ParameterMap.Add(entry.Key.ToString(), entry.Value as Variable);
                    else
                    {
                        var va = FindVariable(entry.Value.ToString());
                        if (va == null)
                            throw new ArgumentException(string.Format("Pair ({0}, {1}) in parameterMap doesn't match any variable in the model", entry.Key, entry.Value.ToString()));
                        ParameterMap.Add(entry.Key.ToString(), va);
                    }
                }
            }
        }

        private Variable FindVariable(string name)
        {
            var model = Trainer.Model();
            var va = CNTKFunctionHelper.Find(model, name);
            if (va != null)
                return va;
            else
            {
                var loss = Trainer.LossFunction();
                va = CNTKFunctionHelper.Find(loss, name);
                if (va != null)
                    return va;
                else
                {
                    var error = Trainer.EvaluationFunction();
                    va = CNTKFunctionHelper.Find(error, name);
                    if (va != null)
                        return va;
                }
            }
            return null;
        }

        private void InitializeParameterMap(Minibatch batch)
        {
            if (ParameterMap.Count > 0)
                return;

            foreach (var entry in batch.Features)
            {
                var name = entry.Key.ToString();
                var va = FindVariable(name);
                if (va != null)
                    ParameterMap.Add(name, va);
            }
        }

        public IEnumerable<TrainingSession> GetSession(int maxIteration = int.MaxValue, DeviceDescriptor device = null)
        {
            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();

            Epoch = 1;

            for (Iteration = 1; Iteration <= maxIteration; ++Iteration)
            {
                var batch = Sampler.GetNextBatch(device);
                if (batch == null)
                    break;

                InitializeParameterMap(batch);

                var arguments = new Dictionary<Variable, MinibatchData>();
                foreach (var entry in batch.Features)
                    arguments.Add(ParameterMap[entry.Key], entry.Value);

                Trainer.TrainMinibatch(arguments, device);

                SampleCount = (int)Trainer.PreviousMinibatchSampleCount();

                if (Trainer.LossFunction() != null)
                    Loss = Trainer.PreviousMinibatchLossAverage();
                else
                    Loss = 0;

                if (Trainer.EvaluationFunction() != null)
                    Metric = Trainer.PreviousMinibatchEvaluationAverage();
                else
                    Metric = 0;

                Elapsed = _stopwatch.Elapsed;

                yield return this;

                if (batch.SweepEnd)
                    ++Epoch;
            }
        }

        public double GetValidationMetric(DeviceDescriptor device = null)
        {
            if (Trainer.EvaluationFunction() == null)
                return 0;

            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();

            if (_validationData == null)
            {
                var batch = Sampler.GetValidationBatch(device);

                if (batch == null)
                    return 0.0;

                _validationData = new UnorderedMapVariableMinibatchData();
                foreach (var entry in batch.Features)
                    _validationData.Add(ParameterMap[entry.Key], entry.Value);
            }

            return Trainer.TestMinibatch(_validationData, device);
        }
    }
}
