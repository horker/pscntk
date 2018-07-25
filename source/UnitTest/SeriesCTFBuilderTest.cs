using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Horker.PSCNTK;

namespace UnitTest
{
    [TestClass]
    class SeriesCTFBuilderTest
    {
        [TestMethod]
        public void TestSeriesCTFBuilder()
        {
            var builder = new SeriesCTFBuilder(3);

            builder.AddFeature("a", new double[] { 1, 2, 3, 4, 5 });
            builder.AddFeature("b", new double[] { 10, 20, 30, 40, 50 });
            builder.AddLabel("label", new double[] { -1, -2, -3, -4, -5, -6, -7, -8, -9, -10 });

            var writer = new StringWriter();
            builder.Write(writer);

            var s = writer.ToString();

            string expected =
                "0\t|a 1\t|b 10\r\n" +
                "0\t|a 2\t|b 20\r\n" +
                "0\t|a 3\t|b 30\t|label -3\r\n" +
                "1\t|a 2\t|b 20\r\n" +
                "1\t|a 3\t|b 30\r\n" +
                "1\t|a 4\t|b 40\t|label -4\r\n" +
                "2\t|a 3\t|b 30\r\n" +
                "2\t|a 4\t|b 40\r\n" +
                "2\t|a 5\t|b 50\t|label -5";

            Assert.AreEqual(expected, s);
        }
    }
}
