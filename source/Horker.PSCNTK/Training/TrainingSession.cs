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

        public Learner Learner { get; private set; }
        public ILearningScheduler LearningRateScheduler { get; private set; }

        public Trainer Trainer { get; private set; }

        public ISampler Sampler { get; private set; }
        public ISampler ValidationSampler { get; private set; }
        public DataNameToInputMap DataNameToInputMap { get; private set; }

        public DeviceDescriptor TrainingDevice { get; private set; }
        public DeviceDescriptor TestDevice { get; private set; }

        public int Epoch { get; private set; }
        public int Iteration { get; private set; }
        public bool EpochIncremented { get; private set; }

        public Minibatch Minibatch { get; private set; }

        public int SampleCount { get; private set; }
        public double Loss { get; private set; }
        public double Metric { get; private set; }

        public ICallback[] Callbacks;

        private bool _stop;

        public TrainingSession(WrappedFunction model, WrappedFunction lossFunction, WrappedFunction evaluationFunction, Learner learner, ILearningScheduler scheduler, ISampler sampler, ISampler validationSampler, Hashtable dataNameToInputMap = null, DeviceDescriptor trainingDevice = null, DeviceDescriptor testDevice = null, ICallback[] callbacks = null)
        {
            Learner = learner;
            LearningRateScheduler = scheduler;

            Trainer = Trainer.CreateTrainer(model, lossFunction, evaluationFunction, new Learner[] { learner });

            Sampler = sampler;
            ValidationSampler = validationSampler;

            TrainingDevice = trainingDevice;
            if (TrainingDevice == null)
                TrainingDevice = DeviceDescriptor.UseDefaultDevice();

            TestDevice = testDevice;
            if (TestDevice == null)
                TestDevice = DeviceDescriptor.UseDefaultDevice();

            DataNameToInputMap = new DataNameToInputMap(
                new Function[] { model, lossFunction, evaluationFunction },
                dataNameToInputMap);

            if (callbacks == null)
                Callbacks = new ICallback[0];
            else
                Callbacks = callbacks;
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

        public IEnumerable<TrainingSession> GetIterator(int maxIteration = int.MaxValue)
        {
            _stopwatch = Stopwatch.StartNew();
            _stop = false;

            Epoch = 1;
            EpochIncremented = false;

            if (LearningRateScheduler != null)
                Learner.ResetLearningRate(new TrainingParameterScheduleDouble(LearningRateScheduler.LearningRate));

            for (Iteration = 1; Iteration <= maxIteration; ++Iteration)
            {
                var minibatch = Sampler.GetNextMinibatch(TrainingDevice);
                if (minibatch == null)
                    break;

                DataNameToInputMap.InitializeByMinibatch(minibatch);

                var arguments = DataNameToInputMap.GetVariableValueMap(minibatch);

                Trainer.TrainMinibatch(arguments, minibatch.SweepEnd, TrainingDevice);

                SampleCount = (int)Trainer.PreviousMinibatchSampleCount();
                Loss = Trainer.PreviousMinibatchLossAverage();
                if (Trainer.EvaluationFunction() != null)
                    Metric = Trainer.PreviousMinibatchEvaluationAverage();

                foreach (var cb in Callbacks)
                    cb.Run(this);

                if (_stop)
                    break;

                yield return this;

                if (LearningRateScheduler != null)
                {
                    bool update = LearningRateScheduler.UpdateLearningRate(Epoch, Iteration, Loss);
                    if (update)
                        Learner.ResetLearningRate(new TrainingParameterScheduleDouble(LearningRateScheduler.LearningRate));
                }

                EpochIncremented = false;
                if (minibatch.SweepEnd)
                {
                    ++Epoch;
                    EpochIncremented = true;
                }
            }
        }

        public IEnumerator<TrainingSession> GetEnumerator(int maxIteration = int.MaxValue)
        {
            // In Powershell we can't call GetEnumerator()
            return GetIterator(maxIteration).GetEnumerator();
        }

        public double GetValidationMetric()
        {
            if (ValidationSampler == null || Trainer.EvaluationFunction() == null)
                return 0.0;

            double metric = 0.0;
            int count = 0;

            Minibatch testData;
            do
            {
                testData = ValidationSampler.GetNextMinibatch(TestDevice);
                if (testData == null)
                    break;

                var arguments = DataNameToInputMap.GetVariableValueMapAsCNTKUnorderedMap(testData);
                metric += Trainer.TestMinibatch(arguments, TestDevice);
                ++count;
            }
            while (!testData.SweepEnd);

            return metric / count;
        }

        void Stop()
        {
            _stop = true;
        }
    }
}
