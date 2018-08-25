using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management.Automation;
using CNTK;

namespace Horker.PSCNTK
{
    public class TrainingSession
    {
        public Trainer Trainer { get; private set; }
        public ISampler Sampler { get; private set; }
        public Dictionary<string, Variable> DataToInputMap { get; private set; }

        public int Epoch { get; private set; }
        public int Iteration { get; private set; }

        public int SampleCount { get; private set; }
        public double Loss { get; private set;}
        public double Metric { get; private set;}

        private Stopwatch _stopwatch;
        public TimeSpan Elapsed { get; private set; }

        private UnorderedMapVariableMinibatchData _validationData;

        public TrainingSession(Trainer trainer, ISampler sampler, Hashtable dataToInputMap = null)
        {
            _stopwatch = Stopwatch.StartNew();

            Trainer = trainer;
            Sampler = sampler;

            DataToInputMap = new Dictionary<string, Variable>();
            if (dataToInputMap != null)
            {
                foreach (DictionaryEntry entry in dataToInputMap)
                {
                    object value = entry.Value;
                    if (value is PSObject)
                        value = (value as PSObject).BaseObject;

                    if (value is Variable)
                        DataToInputMap.Add(entry.Key.ToString(), entry.Value as Variable);
                    else
                    {
                        var va = FindVariable(value.ToString());
                        if (va == null)
                            throw new ArgumentException(string.Format("Pair ({0}, {1}) in parameterMap doesn't match any variable in the model", entry.Key, value.ToString()));
                        DataToInputMap.Add(entry.Key.ToString(), va);
                    }
                }
            }
        }

        private Variable FindVariable(string name)
        {
            var model = Trainer.Model();
            var va = FunctionFind.FindVariable(model, name);
            if (va != null)
                return va;

            var loss = Trainer.LossFunction();
            if (loss != null)
            {
                va = FunctionFind.FindVariable(loss, name);
                if (va != null)
                    return va;
            }

            var error = Trainer.EvaluationFunction();
            if (error != null)
            {
                va = FunctionFind.FindVariable(error, name);
                if (va != null)
                    return va;
            }

            return null;
        }

        private void InitializeParameterMap(Minibatch batch)
        {
            if (DataToInputMap.Count > 0)
                return;

            foreach (var entry in batch.Features)
            {
                var name = entry.Key;
                var va = FindVariable(name);
                if (va != null)
                    DataToInputMap.Add(name, va);
            }
        }

        public IEnumerable<TrainingSession> GetIterator(int maxIteration = int.MaxValue, DeviceDescriptor device = null)
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
                {
                    Variable v = null;
                    if (DataToInputMap.TryGetValue(entry.Key, out v))
                        arguments.Add(v, entry.Value);
                }

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

        public IEnumerator<TrainingSession> GetEnumerator(int maxIteration = int.MaxValue, DeviceDescriptor device = null)
        {
            // In Powershell we can't call GetEnumerator()
            return GetIterator(maxIteration, device).GetEnumerator();
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
                    _validationData.Add(DataToInputMap[entry.Key], entry.Value);
            }

            return Trainer.TestMinibatch(_validationData, device);
        }
    }
}
