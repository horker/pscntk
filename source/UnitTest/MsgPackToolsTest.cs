using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Horker.PSCNTK;
using System.Management.Automation;

namespace UnitTest
{
    [TestClass]
    public class MsgPackToolsTest
    {
        [TestMethod]
        public void TestReadDataSourceSet()
        {
            const int NUM_SAMPLES = 10;

            var file = Path.GetTempFileName();
            using (var stream = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                for (var i = 0; i < NUM_SAMPLES; ++i)
                {
                    var a = DataSourceFactory.Create(new float[] { i, i * 10, i * 100 }, new int[] { 3, 1, 1 });
                    var dss = new DataSourceSet();
                    dss.Add("a", a);
                    MsgPackSerializer.Serialize(dss, stream);
                }
            }

            using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                var total = MsgPackTools.GetTotalSampleCount(stream);
                stream.Position = 0;

                Assert.AreEqual(NUM_SAMPLES, total);

                var reader = MsgPackTools.ReadDataSourceSet(stream, total, 4).GetEnumerator();

                var hasNext = reader.MoveNext();
                Assert.AreEqual(true, hasNext);
                var dss = reader.Current;
                Assert.AreEqual(4, dss.SampleCount);
                CollectionAssert.AreEqual(new int[] { 3, 1, 4 }, dss["a"].Shape.Dimensions);
                CollectionAssert.AreEqual(new float[] { 0, 0, 0, 1, 10, 100, 2, 20, 200, 3, 30, 300 }, dss["a"].Data.ToArray());

                hasNext = reader.MoveNext();
                Assert.AreEqual(true, hasNext);
                dss = reader.Current;
                Assert.AreEqual(4, dss.SampleCount);
                CollectionAssert.AreEqual(new int[] { 3, 1, 4 }, dss["a"].Shape.Dimensions);
                CollectionAssert.AreEqual(new float[] { 4, 40, 400, 5, 50, 500, 6, 60, 600, 7, 70, 700 }, dss["a"].Data.ToArray());

                hasNext = reader.MoveNext();
                Assert.AreEqual(true, hasNext);
                dss = reader.Current;
                Assert.AreEqual(2, dss.SampleCount);
                CollectionAssert.AreEqual(new int[] { 3, 1, 2 }, dss["a"].Shape.Dimensions);
                CollectionAssert.AreEqual(new float[] { 8, 80, 800, 9, 90, 900 }, dss["a"].Data.ToArray());

                hasNext = reader.MoveNext();
                Assert.AreEqual(false, hasNext);
            }
        }
    }
}
