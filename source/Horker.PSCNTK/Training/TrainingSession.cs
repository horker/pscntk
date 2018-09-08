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

        public Minibatch Minibatch { get; private set; }

        public int SampleCount { get; private set; }
        public double Loss { get; private set;}
        public double Metric { get; private set;}

        private Stopwatch _stopwatch;
        public TimeSpan Elapsed { get; private set; }

        private UnorderedMapVariableMinibatchData _validationData;

        public TrainingSession(Trainer trainer, ISampler sampler, Hashtable dataNameToInputMap = null)
        {
            _stopwatch = Stopwatch.StartNew();

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
            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();

            Epoch = 1;

            for (Iteration = 1; Iteration <= maxIteration; ++Iteration)
            {
                Minibatch = Sampler.GetNextBatch(device);
                if (Minibatch == null)
                    break;

                DataNameToInputMap.InitializeByMinibatch(Minibatch);

                var arguments = DataNameToInputMap.GetVariableMinibatchDataMap(Minibatch);

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

                if (Minibatch.SweepEnd)
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
                return 0.0;

            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();

            if (_validationData == null)
            {
                var batch = Sampler.GetValidationBatch(device);

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
