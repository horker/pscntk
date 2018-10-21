using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Horker.PSCNTK;
using System.Management.Automation;
using System.Text.RegularExpressions;

namespace UnitTest
{
    [TestClass]
    public class MsgPackSerializerTest
    {
        [TestMethod]
        public void TestSerializeDeserialize()
        {
            var a = DataSourceFactory.Create(new float[] { 1, 2, 3 }, new int[] { 3, 1, 1 });
            var b = DataSourceFactory.Create(new float[] { 4, 5, 6, 7 }, new int[] { 2, 2, 1 });
            var dss = new DataSourceSet();
            dss.Add("a", a);
            dss.Add("b", b);

            var stream = new MemoryStream();
            MsgPackSerializer.Serialize(dss, stream);

            stream.Position = 0;
            var result = MsgPackSerializer.Deserialize(stream);

            Assert.AreEqual(2, result.Features.Count);

            var x = result["a"];
            CollectionAssert.AreEqual(new int[] { 3, 1, 1 }, x.Shape.Dimensions);
            CollectionAssert.AreEqual(new float[] { 1, 2, 3 }, x.Data.ToArray());

            var y = result["b"];
            CollectionAssert.AreEqual(new int[] { 2, 2, 1 }, y.Shape.Dimensions);
            CollectionAssert.AreEqual(new float[] { 4, 5, 6, 7 }, y.Data.ToArray());
        }

        [TestMethod]
        public void TestMultipleObjects()
        {
            var a = DataSourceFactory.Create(new float[] { 1, 2, 3 }, new int[] { 3, 1, 1 });
            var dss = new DataSourceSet();
            dss.Add("a", a);

            var dss2 = new DataSourceSet();
            var b = DataSourceFactory.Create(new float[] { 4, 5, 6, 7 }, new int[] { 2, 2, 1 });
            dss2.Add("b", b);

            var stream = new MemoryStream();
            MsgPackSerializer.Serialize(dss, stream);
            MsgPackSerializer.Serialize(dss2, stream);

            stream.Position = 0;
            var result = MsgPackSerializer.Deserialize(stream);
            var result2 = MsgPackSerializer.Deserialize(stream);

            Assert.AreEqual(1, result.Features.Count);

            var x = result["a"];
            CollectionAssert.AreEqual(new int[] { 3, 1, 1 }, x.Shape.Dimensions);
            CollectionAssert.AreEqual(new float[] { 1, 2, 3 }, x.Data.ToArray());

            Assert.AreEqual(1, result2.Features.Count);

            var y = result2["b"];
            CollectionAssert.AreEqual(new int[] { 2, 2, 1 }, y.Shape.Dimensions);
            CollectionAssert.AreEqual(new float[] { 4, 5, 6, 7 }, y.Data.ToArray());
        }
    }
}
