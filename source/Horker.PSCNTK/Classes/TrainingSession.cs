using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNTK;

namespace Horker.PSCNTK
{
    public class TrainingSession
    {
        public Trainer Trainer { get; private set; }
        public MinibatchSource MinibatchSource { get; private set; }
        public UInt32 MinibatchSize { get; private set; }
        public Dictionary<string, Variable> ParameterMap { get; private set; }

        public Dictionary<Variable, Value> Arguments { get; private set; }

        public int Epoch { get; private set; }
        public int Iteration { get; private set; }
        public UInt32 SampleCount { get; private set; }
        public double Loss { get; private set; }
        public double Metric { get; private set; }

        public TrainingSession(Trainer trainer, MinibatchSource minibatchSource, UInt32 minibatchSize, Hashtable parameterMap = null)
        {
            Trainer = trainer;
            MinibatchSource = minibatchSource;
            MinibatchSize = minibatchSize;

            if (parameterMap != null)
            {
                ParameterMap = new Dictionary<string, Variable>();

                var model = Trainer.Model();
                foreach (DictionaryEntry entry in parameterMap)
                {
                    if (entry.Value is Variable)
                    {
                        ParameterMap.Add(entry.Key.ToString(), entry.Value as Variable);
                    }
                    else
                    {
                        var va = CNTKFunctionHelper.Get(model, entry.Value.ToString());
                        if (va == null)
                            throw new ArgumentException(string.Format("Pair ({0}, {1}) in parameterMap doesn't match any variable in the model", entry.Key, entry.Value.ToString()));

                        ParameterMap.Add(entry.Key.ToString(), va);
                    }
                }
            }
        }

        public IEnumerable<TrainingSession> GetSession(int maxIteration = int.MaxValue, DeviceDescriptor device = null)
        {
            if (device == null)
                device = DeviceDescriptor.UseDefaultDevice();

            Epoch = 0;
            SampleCount = 0;
            Loss = 0.0;
            Metric = 0.0;

            Arguments = new Dictionary<Variable, Value>();

            for (Iteration = 1; Iteration <= maxIteration; ++Iteration)
            {
                var batch = MinibatchSource.GetNextMinibatch(MinibatchSize);

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

                Arguments.Clear();
                foreach (var entry in batch)
                {
                    var info = entry.Key;
                    var data = entry.Value;

                    var va = ParameterMap[info.m_name];

                    Arguments.Add(va, data.data);
                }

                Trainer.TrainMinibatch(Arguments, false, device);

                SampleCount = Trainer.PreviousMinibatchSampleCount();
                Loss = Trainer.PreviousMinibatchLossAverage();
                Metric = Trainer.PreviousMinibatchEvaluationAverage();

                yield return this;
            }
        }
    }
}
