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
            UnmanagedDllLoader.Load(@"..\..\..\..\lib");
            DeviceDescriptor.TrySetDefaultDevice(DeviceDescriptor.CPUDevice);
        }

        [TestMethod]
        public void TestTrainingSession2()
        {
            // Data

            var features = DataSourceFactory.Create(new float[] { 0, 0, 0, 1, 1, 0, 1, 1, 3, 4, 3, 5, 4, 4, 4, 5 }, new int[] { 2, 1, -1 });
            var labels   = DataSourceFactory.Create(new float[] { 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0 }, new int[] { 2, 1, -1 });

            var sampler = new OnMemorySampler(new Dictionary<string, IDataSource<float>>()
            {
                { "input", features },
                { "label", labels }
            }, 2);

            // Model

            var input = CNTKLib.InputVariable(new int[] { 2 }, false, DataType.Float, "input");
            var h = Composite.Dense(input, new int[] { 100 }, CNTKLib.HeNormalInitializer(), true, null, false, 4, "relu", DeviceDescriptor.UseDefaultDevice(), "");
            h = Composite.Dense(h, new int[] { 2 }, CNTKLib.GlorotNormalInitializer(), true, null, false, 4, "sigmoid", DeviceDescriptor.UseDefaultDevice(), "");
            var output = h;

            var label = CNTKLib.InputVariable(new int[] { 2 }, DataType.Float, "label");

            // Loss and metric functions

            var loss = CNTKLib.BinaryCrossEntropy(output, label);
            var error = CNTKLib.ClassificationError(output, label);

            // Train

            var lr = new TrainingParameterScheduleDouble(.01);
            var m = new TrainingParameterScheduleDouble(.9);

            var learner = Learner.MomentumSGDLearner(output.Parameters(), lr, m, true);

            var session = new TrainingSession(output, loss, error, learner, null, sampler, null);
            var iteration = session.GetEnumerator();

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
