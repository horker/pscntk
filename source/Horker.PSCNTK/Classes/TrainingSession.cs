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
        public MinibatchSource MinibatchSource { get; private set; }
        public IDictionary<object, CNTK.Value> ValidationData { get; private set; }
        public UInt32 MinibatchSize { get; private set; }
        public int ValidationSampleSize { get; private set; }
        public Dictionary<string, Variable> ParameterMap { get; private set; }
        public DeviceDescriptor Device { get; private set; }

        public int Epoch { get; private set; }
        public int Iteration { get; private set; }
        public UInt32 SampleCount { get; private set; }
        public double Loss { get; private set; }
        public double Metric { get; private set; }

        private ValidationMetric _validationMetric;

        public TrainingSession(Trainer trainer, MinibatchSource minibatchSource, IDictionary<object, Value> validationData, UInt32 minibatchSize, int validationSampleSize, Hashtable parameterMap = null, DeviceDescriptor device = null)
        {
            Trainer = trainer;
            MinibatchSource = minibatchSource;
            ValidationData = validationData;
            MinibatchSize = minibatchSize;
            ValidationSampleSize = validationSampleSize;

            if (parameterMap != null)
            {
                ParameterMap = new Dictionary<string, Variable>();

                var model = Trainer.Model();
                var loss = Trainer.LossFunction();
                var error = Trainer.LossFunction();
                foreach (DictionaryEntry entry in parameterMap)
                {
                    if (entry.Value is Variable)
                    {
                        ParameterMap.Add(entry.Key.ToString(), entry.Value as Variable);
                    }
                    else
                    {
                        var va = CNTKFunctionHelper.Get(model, entry.Value.ToString());
                        if (va != null)
                            ParameterMap.Add(entry.Key.ToString(), va);
                        else
                        {
                            va = CNTKFunctionHelper.Get(loss, entry.Value.ToString());
                            if (va != null)
                                ParameterMap.Add(entry.Key.ToString(), va);
                            else
                            {
                                va = CNTKFunctionHelper.Get(error, entry.Value.ToString());
                                if (va != null)
                                    ParameterMap.Add(entry.Key.ToString(), va);
                                else
                                    throw new ArgumentException(string.Format("Pair ({0}, {1}) in parameterMap doesn't match any variable in the model", entry.Key, entry.Value.ToString()));
                            }
                        }
                    }
                }
            }

            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();
            Device = device;
        }

        private Dictionary<Variable, Value> GetArguments(UnorderedMapStreamInformationMinibatchData batch)
        {
            var arguments = new Dictionary<Variable, Value>();
            foreach (var entry in batch)
            {
                var info = entry.Key;
                var data = entry.Value;

                var va = ParameterMap[info.m_name];

                arguments.Add(va, data.data);
            }

            return arguments;
        }

        public IEnumerable<TrainingSession> GetSession(int maxIteration = int.MaxValue)
        {
            Epoch = 0;
            SampleCount = 0;
            Loss = 0.0f;
            Metric = 0.0f;

            for (Iteration = 1; Iteration <= maxIteration; ++Iteration)
            {
                var batch = MinibatchSource.GetNextMinibatch(MinibatchSize, Device);
                foreach (var entry in batch)
                    Debug.Print(DataSource<float>.FromValue(batch[entry.Key].data).AsString());

                if (ParameterMap == null)
                {
                    ParameterMap = new Dictionary<string, Variable>();
                    var model = Trainer.Model();
                    var loss = Trainer.LossFunction();
                    var error = Trainer.LossFunction();
                    foreach (var info in batch.Keys)
                    {
                        var name = info.m_name;
                        var va = CNTKFunctionHelper.Get(model, name);
                        if (va != null)
                            ParameterMap.Add(name, va);
                        else
                        {
                            va = CNTKFunctionHelper.Get(loss, name);
                            if (va != null)
                                ParameterMap.Add(name, va);
                            else
                            {
                                va = CNTKFunctionHelper.Get(error, name);
                                if (va != null)
                                    ParameterMap.Add(name, va);
                            }
                        }
                    }
                }

                var arguments = GetArguments(batch);
                Trainer.TrainMinibatch(arguments, false, Device);

                SampleCount = Trainer.PreviousMinibatchSampleCount();
                Loss = Trainer.PreviousMinibatchLossAverage();
                Metric = Trainer.PreviousMinibatchEvaluationAverage();

                yield return this;
            }
        }

        private void EnsureValidationMetric()
        {
            if (_validationMetric == null)
                _validationMetric = new ValidationMetric(Trainer, MinibatchSource, ParameterMap, ValidationSampleSize, Device);
        }

        public void UpdateValidateData()
        {
            EnsureValidationMetric();
            _validationMetric.UpdateValidationData();
        }

        public double GetValidationMetric()
        {
            EnsureValidationMetric();
            return _validationMetric.GetValidationMetric();
        }
    }

    public class ValidationMetric
    {
        public Trainer Trainer;
        public MinibatchSource MinibatchSource;
        public Dictionary<string, Variable> ParameterMap;
        public int SampleSize;
        public DeviceDescriptor Device;

        private UnorderedMapVariableMinibatchData _validationArguments;
        private UnorderedMapStreamInformationMinibatchData _validationBatch;

        public ValidationMetric(Trainer trainer, MinibatchSource minibatchSource, Dictionary<string, Variable> parameterMap, int sampleSize, DeviceDescriptor device)
        {
            Trainer = trainer;
            MinibatchSource = minibatchSource;
            ParameterMap = parameterMap;
            SampleSize = sampleSize;
            Device = device;
        }

        public void UpdateValidationData()
        {
            _validationBatch = MinibatchSource.GetNextMinibatch((uint)SampleSize, Device);
            _validationArguments = null;
        }

        public double GetValidationMetric()
        {
            if (_validationArguments == null)
            {
                if (_validationBatch == null)
                    UpdateValidationData();

                _validationArguments = new UnorderedMapVariableMinibatchData();
                var eval = Trainer.EvaluationFunction();
                foreach (var info in _validationBatch.Keys)
                {
                    var name = info.m_name;
                    if (ParameterMap.ContainsKey(name))
                        _validationArguments.Add(ParameterMap[name], _validationBatch[info]);
                    else
                    {
                        var va = CNTKFunctionHelper.Get(eval, name);
                        if (va != null)
                            _validationArguments.Add(va, _validationBatch[info]);
                    }
                }
            }

            foreach (var entry in _validationArguments)
                Debug.Print(DataSource<float>.FromValue(entry.Value.data).AsString());
            return Trainer.TestMinibatch(_validationArguments, Device);
        }
    }
}
