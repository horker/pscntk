using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Horker.PSCNTK;

namespace UnitTest
{
    [TestClass]
    public class CTFMinibatchDefinitionTest
    {
        public CTFMinibatchDefinitionTest()
        {
            UnmanagedDllLoader.Load(@"..\..\..\..\lib");
        }

        [TestMethod]
        public void TestGuessTextFormat()
        {
            var file = @"..\..\TestData\CTFTest1.txt";

            var result = CTFMinibatchDefinition.GuessDataFormat(file, 1);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(5, result["f1"]);
            Assert.AreEqual(3, result["f2"]);
        }

        [TestMethod]
        public void TestGetNextBatch()
        {
            var file = @"..\..\TestData\CTFTest1.txt";

            var minibatchDef = new CTFMinibatchDefinition(file, 1, false);

            var m = minibatchDef.GetNextBatch();

            Assert.AreEqual(2, m.Features.Count);
            Assert.AreEqual(1, (int)m.Features["f1"].numberOfSamples);
            Assert.AreEqual(1, (int)m.Features["f2"].numberOfSamples);

            CollectionAssert.AreEqual(new float[] { 1, 2, 3, 4, 5 }, DataSource<float>.FromValue(m.Features["f1"].data).ToArray());

            m = minibatchDef.GetNextBatch();

            Assert.AreEqual(2, m.Features.Count);
            Assert.AreEqual(1, (int)m.Features["f1"].numberOfSamples);
            Assert.AreEqual(1, (int)m.Features["f2"].numberOfSamples);

            CollectionAssert.AreEqual(new float[] { 6, 5, 4 }, DataSource<float>.FromValue(m.Features["f2"].data).ToArray());
        }
    }
}
