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
        public MinibatchDefinition Minibatch { get; private set; }
        public Dictionary<string, Variable> ParameterMap { get; private set; }
        public DeviceDescriptor Device { get; private set; }

        public int Epoch { get; private set; }
        public int Iteration { get; private set; }

        public int SampleCount { get; private set; }
        public double Loss { get; private set;}
        public double Metric { get; private set;}

        private UnorderedMapVariableMinibatchData _validationData;

        public TrainingSession(Trainer trainer, MinibatchDefinition minibatch, Hashtable parameterMap = null, DeviceDescriptor device = null)
        {
            Trainer = trainer;
            Minibatch = minibatch;

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
            else
            {
                foreach (var entry in minibatch.Features)
                {
                    var name = entry.Key.ToString();
                    var va = FindVariable(name);
                    if (va != null)
                        ParameterMap.Add(name, va);
                }
            }

            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();
            Device = device;
        }

        private Variable FindVariable(string name)
        {
            var model = Trainer.Model();
            var va = CNTKFunctionHelper.Get(model, name);
            if (va != null)
                return va;
            else
            {
                var loss = Trainer.LossFunction();
                va = CNTKFunctionHelper.Get(loss, name);
                if (va != null)
                    return va;
                else
                {
                    var error = Trainer.EvaluationFunction();
                    va = CNTKFunctionHelper.Get(error, name);
                    if (va != null)
                        return va;
                }
            }
            return null;
        }

        public IEnumerable<TrainingSession> GetSession(int maxIteration = int.MaxValue)
        {
            Epoch = 0;

            for (Iteration = 1; Iteration <= maxIteration; ++Iteration)
            {
                var batch = Minibatch.GetNextBatch();

                var arguments = new Dictionary<Variable, MinibatchData>();
                foreach (var entry in batch.Features)
                    arguments.Add(ParameterMap[entry.Key], entry.Value);

                Trainer.TrainMinibatch(arguments, Device);

                SampleCount = (int)Trainer.PreviousMinibatchSampleCount();
                Loss = Trainer.PreviousMinibatchLossAverage();
                Metric = Trainer.PreviousMinibatchEvaluationAverage();

                yield return this;

                if (batch.SweepEnd)
                    ++Epoch;
            }
        }

        public double GetValidationMetric()
        {
            if (_validationData == null)
            {
                var batch = Minibatch.GetNextBatch();

                _validationData = new UnorderedMapVariableMinibatchData();
                foreach (var entry in batch.Features)
                    _validationData.Add(ParameterMap[entry.Key], entry.Value);
            }

            return Trainer.TestMinibatch(_validationData, Device);
        }
    }
}
