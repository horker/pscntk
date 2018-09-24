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
        public DataNameToInputMap DataNameToInputMap { get; private set; }

        public int Epoch { get; private set; }
        public int Iteration { get; private set; }
        public bool EpochIncremented { get; private set; }

        public Minibatch Minibatch { get; private set; }

        public int SampleCount => (int)Trainer.PreviousMinibatchSampleCount();

        public double Loss
        {
            get
            {
                if (Trainer.LossFunction() == null)
                    return 0;

                return Trainer.PreviousMinibatchLossAverage();
            }
        }

        public double Metric
        {
            get
            {
                if (Trainer.EvaluationFunction() == null)
                    return 0;

                return Trainer.PreviousMinibatchEvaluationAverage();
            }
        }

        private Stopwatch _stopwatch;
        public TimeSpan Elapsed => _stopwatch.Elapsed;

        private UnorderedMapVariableMinibatchData _validationData;

        public TrainingSession(Trainer trainer, ISampler sampler, Hashtable dataNameToInputMap = null)
        {
            Trainer = trainer;
            Sampler = sampler;

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
                Minibatch = Sampler.GetNextMinibatch(device);
                if (Minibatch == null)
                    break;

                DataNameToInputMap.InitializeByMinibatch(Minibatch);

                var arguments = DataNameToInputMap.GetVariableMinibatchDataMap(Minibatch);

                Trainer.TrainMinibatch(arguments, device);

                yield return this;

                EpochIncremented = false;
                if (Minibatch.SweepEnd)
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

            if (_validationData == null)
            {
                var batch = Sampler.GetValidationMinibatch(device);

                if (batch == null)
                    return 0.0;

                _validationData = new UnorderedMapVariableMinibatchData();
                var arguments = DataNameToInputMap.GetVariableMinibatchDataMap(batch);
                foreach (var entry in arguments)
                    _validationData.Add(entry.Key, entry.Value);
            }

            return Trainer.TestMinibatch(_validationData, device);
        }
    }
}
