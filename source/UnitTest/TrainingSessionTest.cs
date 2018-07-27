﻿using System;
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
        [TestMethod]
        public void TestTrainingSession()
        {
            var path = Environment.ExpandEnvironmentVariables("%HOME%\\work\\pscntk\\lib");
            UnmanagedDllLoader.Load(path);

            // Prepare data

            var trainingFeatures = new DataSource<float>(new float[] { 0, 0, 0, 1, 1, 0, 1, 1, 3, 4, 3, 5, 4, 4, 4, 5 }, new int[] { 2, 2, -1 });
            var trainingLabels   = new DataSource<float>(new float[] { 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0 }, new int[] { 2, 2, -1 });

            var validFeatures = new DataSource<float>(new float[] { 6, 6, 7, 7, 8, 8, 9, 9 }, new int[] { 2, 2, -1 });
            var validLabels   = new DataSource<float>(new float[] { 1, 0, 1, 0, 1, 0, 1, 0 }, new int[] { 2, 2, -1 });

            var file = Path.GetTempFileName();
            using (var writer = new StreamWriter(file, false, new UTF8Encoding(false)))
            {
                DataSourceCTFBuilder.Write(writer, new DataSource<float>[] { trainingFeatures, trainingFeatures }, new string[] { "input", "label" });
            }

            var training = MinibatchSource.TextFormatMinibatchSource(file, new List<StreamConfiguration>()
            {
                new StreamConfiguration("input", 2),
                new StreamConfiguration("label", 2)
            });

            var validation = new Dictionary<object, Value>()
            {
                { "input", validFeatures.ToValue() },
                { "label", validLabels.ToValue() }
            };

            // Build a model

            var input = CNTKLib.InputVariable(new int[] { 2 }, false, DataType.Float, "input");
            var h = Layers.Dense(input, 100, CNTKLib.HeNormalInitializer());
            h = CNTKLib.ReLU(h);
            h = Layers.Dense(h, 2, CNTKLib.GlorotNormalInitializer());
            var output = CNTKLib.Sigmoid(h);

            var labels = CNTKLib.InputVariable(new int[] { 2 }, DataType.Float, "label");

            // Train

            var lr = new TrainingParameterScheduleDouble(.01);
            var m = new TrainingParameterScheduleDouble(.9);

            var learner = Learner.MomentumSGDLearner(output.Parameters(), lr, m, true);

            var loss = CNTKLib.BinaryCrossEntropy(output, labels);
            var error = CNTKLib.ClassificationError(output, labels);

            var trainer = Trainer.CreateTrainer(output, loss, error, new List<Learner>() { learner });

            var session = new TrainingSession(trainer, training, validation, 20, 1, null);
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
