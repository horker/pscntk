using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using CNTK;
using Horker.PSCNTK;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class MsgPackSamplerTest
    {
        public MsgPackSamplerTest()
        {
            UnmanagedDllLoader.Load(@"..\..\..\..\lib");
            DeviceDescriptor.TrySetDefaultDevice(DeviceDescriptor.CPUDevice);
        }

        [TestMethod]
        public void TestMsgPackSampler()
        {
            const int NUM_SAMPLES = 1000;

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

            using (var sampler = new MsgPackSampler(NUM_SAMPLES, 10, false, 100))
            {
                sampler.StartLoading(file);

                for (var i = 0; i < 10; ++i)
                {
                    for (var j = 0; j < NUM_SAMPLES; ++j)
                    {
                        var batch = sampler.GetNextMinibatch();
                        Assert.AreEqual(1, batch.Features.Count);
                        Assert.IsTrue(batch.Features.ContainsKey("a"));
                        var b = batch["a"];
                        CollectionAssert.AreEqual(new int[] { 3, 1, 1 }, b.data.Shape.Dimensions.ToArray());

                        var ds = DataSourceFactory.FromValue(b.data).ToArray();
                        CollectionAssert.AreEqual(new float[] { j, j * 10, j * 100 }, ds);

                        // Assert.AreEqual((i * 1000 + j + 1) % 3 == 0, b.sweepEnd);
                    }
                }
            }
        }

        [TestMethod]
        public void TestMsgPackSamplerReuseSamples()
        {
            var file = Path.GetTempFileName();
            using (var stream = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                for (var i = 0; i < 1000; ++i)
                {
                    var a = DataSourceFactory.Create(new float[] { i, i * 10, i * 100 }, new int[] { 3, 1, 1 });
                    var dss = new DataSourceSet();
                    dss.Add("a", a);
                    MsgPackSerializer.Serialize(dss, stream);
                }
            }

            using (var sampler = new MsgPackSampler(100, 100, true, 1000))
            {
                sampler.StartLoading(file);

                for (var i = 0; i < 10000; ++i)
                {
                    var batch = sampler.GetNextMinibatch();
                    Assert.IsTrue(batch.Features.ContainsKey("a"));
                }
            }
        }
    }
}
