using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Horker.PSCNTK;

namespace UnitTest
{
    [TestClass]
    public class DataSourceCTFBuilderTest
    {
        [TestMethod]
        public void TestDataSourceCTFBuilider()
        {
            var writer = new StringWriter();

            var ds1 = DataSourceFactory.Create(new float[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 }, new int[] { 2, 3, 3 });
            var ds2 = DataSourceFactory.Create(new float[] { 0, 1, 2, 3, 4, 5 }, new int[] { 2, 1, 3 });

            var dss = new DataSourceSet();
            dss.Features.Add("data", ds1);
            dss.Features.Add("label", ds2);

            DataSourceSetCTFBuilder.Write(writer, dss, true);
            var s = writer.ToString();

            var expected =
                "0\t|data 0 1\t|label 0 1\r\n" +
                "0\t|data 2 3\r\n" +
                "0\t|data 4 5\r\n" +
                "1\t|data 6 7\t|label 2 3\r\n" +
                "1\t|data 8 9\r\n" +
                "1\t|data 10 11\r\n" +
                "2\t|data 12 13\t|label 4 5\r\n" +
                "2\t|data 14 15\r\n" +
                "2\t|data 16 17";

            Assert.AreEqual(expected, s);
        }
    }
}
