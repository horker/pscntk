using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CNTK;
using Horker.PSCNTK;

namespace UnitTest
{
    [TestClass]
    public class TrainingSessionTest
    {
        public TrainingSessionTest()
        {
            var path = Environment.ExpandEnvironmentVariables(@"..\..\..\..\lib");
            UnmanagedDllLoader.Load(path);
        }

        [TestMethod]
        public void TestTrainingSession2()
        {
            // Prepare data

            var features = new DataSource<float>(new float[] { 0, 0, 0, 1, 1, 0, 1, 1, 3, 4, 3, 5, 4, 4, 4, 5 }, new int[] { 2, 1, -1 });
            var labels   = new DataSource<float>(new float[] { 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0 }, new int[] { 2, 1, -1 });

            var sampler = new OnMemorySampler(new Dictionary<string, DataSource<float>>()
            {
                { "input", features },
                { "label", labels }
            }, 2);

            // Build a model

            var input = CNTKLib.InputVariable(new int[] { 2 }, false, DataType.Float, "input");
            var h = Composite.Dense(input, new int[] { 100 }, CNTKLib.HeNormalInitializer(), true, null, "relu", "");
            h = Composite.Dense(h, new int[] { 2 }, CNTKLib.GlorotNormalInitializer(), true, null, "sigmoid", "");
            var output = h;

            var label = CNTKLib.InputVariable(new int[] { 2 }, DataType.Float, "label");

            // Train

            var lr = new TrainingParameterScheduleDouble(.01);
            var m = new TrainingParameterScheduleDouble(.9);

            var learner = Learner.MomentumSGDLearner(output.Parameters(), lr, m, true);

            var loss = CNTKLib.BinaryCrossEntropy(output, label);
            var error = CNTKLib.ClassificationError(output, label);

            var trainer = Trainer.CreateTrainer(output, loss, error, new List<Learner>() { learner });

            var session = new TrainingSession(trainer, sampler);
            var iteration = session.GetSession().GetEnumerator();

            for (var i = 0; i < 1000; ++i)
            {
                iteration.MoveNext();
                var dummy = iteration.Current;
                var valid = session.GetValidationMetric();
            }

            Assert.IsTrue(session.Metric < 0.1);
        }
    }
}
