using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Horker.PSCNTK;

namespace UnitTest
{
    [TestClass]
    public class PerformanceSchedulerTest
    {
        [TestMethod]
        public void TestSmoothing1()
        {
            var sche = new PerformanceScheduler(.1, .1, 3, 1.0);

            Assert.AreEqual(false, sche.UpdateLearningRate(1, 1, .5));
            Assert.AreEqual(.1, sche.LearningRate, 1e-5);
            Assert.AreEqual(false, sche.UpdateLearningRate(1, 2, .5));
            Assert.AreEqual(.1, sche.LearningRate, 1e-5);
            Assert.AreEqual(false, sche.UpdateLearningRate(1, 3, .5));
            Assert.AreEqual(.1, sche.LearningRate, 1e-5);

            Assert.AreEqual(false, sche.UpdateLearningRate(1, 4, .8));
            Assert.AreEqual(.1, sche.LearningRate, 1e-5);
            Assert.AreEqual(false, sche.UpdateLearningRate(1, 5, .8));
            Assert.AreEqual(.1, sche.LearningRate, 1e-5);
            Assert.AreEqual(false, sche.UpdateLearningRate(1, 6, .4));
            Assert.AreEqual(.1, sche.LearningRate, 1e-5);

            Assert.AreEqual(false, sche.UpdateLearningRate(1, 7, .3));
            Assert.AreEqual(.1, sche.LearningRate, 1e-5);
            Assert.AreEqual(false, sche.UpdateLearningRate(1, 8, .3));
            Assert.AreEqual(.1, sche.LearningRate, 1e-5);
            Assert.AreEqual(true, sche.UpdateLearningRate(1, 9, .5));
            Assert.AreEqual(.01, sche.LearningRate, 1e-5);
        }

        [TestMethod]
        public void TestDefaultSmoothing()
        {
            var sche = new PerformanceScheduler(.1, .1, 3);

            Assert.AreEqual(2.0 / (3 + 1), sche.Smoothing, 1e-5);

            Assert.AreEqual(false, sche.UpdateLearningRate(1, 1, .5));
            Assert.AreEqual(.1, sche.LearningRate, 1e-5);
            Assert.AreEqual(false, sche.UpdateLearningRate(1, 2, .5));
            Assert.AreEqual(.1, sche.LearningRate, 1e-5);
            Assert.AreEqual(false, sche.UpdateLearningRate(1, 3, .5));
            Assert.AreEqual(.1, sche.LearningRate, 1e-5);

            Assert.AreEqual(false, sche.UpdateLearningRate(1, 4, .8));
            Assert.AreEqual(.1, sche.LearningRate, 1e-5);
            Assert.AreEqual(false, sche.UpdateLearningRate(1, 5, .8));
            Assert.AreEqual(.1, sche.LearningRate, 1e-5);
            Assert.AreEqual(true, sche.UpdateLearningRate(1, 6, .4));
            Assert.AreEqual(.01, sche.LearningRate, 1e-5);

            Assert.AreEqual(false, sche.UpdateLearningRate(1, 7, .3));
            Assert.AreEqual(.01, sche.LearningRate, 1e-5);
            Assert.AreEqual(false, sche.UpdateLearningRate(1, 8, .3));
            Assert.AreEqual(.01, sche.LearningRate, 1e-5);
            Assert.AreEqual(false, sche.UpdateLearningRate(1, 9, .5));
            Assert.AreEqual(.01, sche.LearningRate, 1e-5);
        }
    }
}
