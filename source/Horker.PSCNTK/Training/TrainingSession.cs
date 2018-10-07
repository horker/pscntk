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
        private Stopwatch _stopwatch;
        public TimeSpan Elapsed => _stopwatch.Elapsed;

        private Minibatch _validationData;

        public Trainer Trainer { get; private set; }
        public ISampler Sampler { get; private set; }
        public DataNameToInputMap DataNameToInputMap { get; private set; }

        public int Epoch { get; private set; }
        public int Iteration { get; private set; }
        public bool EpochIncremented { get; private set; }

        public bool KeepMinibatch { get; private set; }
        public Dictionary<string, Value> Minibatch { get; private set; }

        public int SampleCount { get; private set; }
        public double Loss { get; private set; }
        public double Metric { get; private set; }

        public TrainingSession(Trainer trainer, ISampler sampler, Hashtable dataNameToInputMap = null, bool keepMinibatch = false)
        {
            Trainer = trainer;
            Sampler = sampler;
            KeepMinibatch = keepMinibatch;

            _validationData = null;

            DataNameToInputMap = new DataNameToInputMap(
                new Function[] { trainer.Model(), trainer.LossFunction(), trainer.EvaluationFunction() },
                dataNameToInputMap);
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

        public IEnumerable<TrainingSession> GetIterator(int maxIteration = int.MaxValue, DeviceDescriptor device = null)
        {
            _stopwatch = Stopwatch.StartNew();

            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();

            Epoch = 1;
            EpochIncremented = false;

            for (Iteration = 1; Iteration <= maxIteration; ++Iteration)
            {
                var minibatch = Sampler.GetNextMinibatch(device);
                if (minibatch == null)
                    break;

                if (KeepMinibatch)
                    Minibatch = minibatch.GetCopy();

                DataNameToInputMap.InitializeByMinibatch(minibatch);

                var arguments = DataNameToInputMap.GetVariableMinibatchDataMap(minibatch);

                Trainer.TrainMinibatch(arguments, device);

                SampleCount = (int)Trainer.PreviousMinibatchSampleCount();
                Loss = Trainer.PreviousMinibatchLossAverage();
                Metric = Trainer.PreviousMinibatchEvaluationAverage();

                yield return this;

                EpochIncremented = false;
                if (minibatch.SweepEnd)
                {
                    ++Epoch;
                    EpochIncremented = true;
                }
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
                return 0.0;

            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();

//            if (_validationData == null)
//            {
                _validationData = Sampler.GetValidationMinibatch(device);

                if (_validationData == null)
                    return 0.0;
//            }

            var map = new UnorderedMapVariableMinibatchData();
            var arguments = DataNameToInputMap.GetVariableMinibatchDataMap(_validationData);
            foreach (var entry in arguments)
                map.Add(entry.Key, entry.Value);

            return Trainer.TestMinibatch(map, device);
        }
    }
}
