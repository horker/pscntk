using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Horker.PSCNTK;

namespace UnitTest
{
    [TestClass]
    public class OneCycleSchedulerTest
    {
        [TestMethod]
        public void TestOneCycleScheduler()
        {
            var sche = new OneCycleScheduler(0, 10, -1, 5);

            Assert.AreEqual(0.0, sche.LearningRate);

            Assert.AreEqual(true, sche.UpdateLearningRate(1, 1, 0.0));
            Assert.AreEqual(2.0, sche.LearningRate);
            Assert.AreEqual(true, sche.UpdateLearningRate(1, 2, 0.0));
            Assert.AreEqual(4.0, sche.LearningRate);
            Assert.AreEqual(true, sche.UpdateLearningRate(1, 3, 0.0));
            Assert.AreEqual(6.0, sche.LearningRate);
            Assert.AreEqual(true, sche.UpdateLearningRate(1, 4, 0.0));
            Assert.AreEqual(8.0, sche.LearningRate);
            Assert.AreEqual(true, sche.UpdateLearningRate(1, 5, 0.0));
            Assert.AreEqual(10.0, sche.LearningRate);

            Assert.AreEqual(true, sche.UpdateLearningRate(1, 6, 0.0));
            Assert.AreEqual(8.0, sche.LearningRate);
            Assert.AreEqual(true, sche.UpdateLearningRate(1, 7, 0.0));
            Assert.AreEqual(6.0, sche.LearningRate);
            Assert.AreEqual(true, sche.UpdateLearningRate(1, 8, 0.0));
            Assert.AreEqual(4.0, sche.LearningRate);
            Assert.AreEqual(true, sche.UpdateLearningRate(1, 9, 0.0));
            Assert.AreEqual(2.0, sche.LearningRate);
            Assert.AreEqual(true, sche.UpdateLearningRate(1, 10, 0.0));
            Assert.AreEqual(0.0, sche.LearningRate);

            Assert.AreEqual(true, sche.UpdateLearningRate(1, 11, 0.0));
            Assert.AreEqual(-1.0, sche.LearningRate);
            Assert.AreEqual(true, sche.UpdateLearningRate(1, 12, 0.0));
            Assert.AreEqual(-1.0, sche.LearningRate);
        }
    }
}
