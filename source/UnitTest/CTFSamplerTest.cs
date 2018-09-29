using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using CNTK;
using Horker.PSCNTK;

namespace UnitTest
{
    [TestClass]
    public class CTFSamplerTest
    {
        public CTFSamplerTest()
        {
            UnmanagedDllLoader.Load(@"..\..\..\..\lib");
            DeviceDescriptor.TrySetDefaultDevice(DeviceDescriptor.CPUDevice);
        }

        [TestMethod]
        public void TestGuessTextFormat()
        {
            var file = @"..\..\TestData\CTFTest1.txt";

            var result = CTFSampler.GuessDataFormat(file, 1);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(5, result["f1"]);
            Assert.AreEqual(3, result["f2"]);
        }

        [TestMethod]
        public void TestGetNextBatch()
        {
            var file = @"..\..\TestData\CTFTest1.txt";

            var sampler = new CTFSampler(file, 1, false);

            var m = sampler.GetNextMinibatch();

            Assert.AreEqual(2, m.Features.Count);
            Assert.AreEqual(1, (int)m.Features["f1"].numberOfSamples);
            Assert.AreEqual(1, (int)m.Features["f2"].numberOfSamples);

            CollectionAssert.AreEqual(new float[] { 1, 2, 3, 4, 5 }, DataSourceFactory.FromValue(m.Features["f1"].data).ToArray());

            m = sampler.GetNextMinibatch();

            Assert.AreEqual(2, m.Features.Count);
            Assert.AreEqual(1, (int)m.Features["f1"].numberOfSamples);
            Assert.AreEqual(1, (int)m.Features["f2"].numberOfSamples);

            CollectionAssert.AreEqual(new float[] { 6, 5, 4 }, DataSourceFactory.FromValue(m.Features["f2"].data).ToArray());
        }
    }
}
