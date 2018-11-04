using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using CNTK;
using Horker.PSCNTK;
using System.Linq;
using System.Diagnostics;

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

            using (var sampler = new MsgPackSampler(1, 3, 10, false, 100))
            {
                sampler.StartLoading(file);

                for (var i = 0; i < 10; ++i)
                {
                    for (var j = 0; j < NUM_SAMPLES; ++j)
                    {
                        var batch = sampler.GetNextMinibatch();
                        Assert.AreEqual(1, batch.Features.Count);
                        Assert.IsTrue(batch.Features.ContainsKey("a"));
                        Assert.AreEqual((i * NUM_SAMPLES + j + 1) % 3 == 0, batch.SweepEnd);

                        var value = batch["a"];
                        CollectionAssert.AreEqual(new int[] { 3, 1, 1 }, value.Shape.Dimensions.ToArray());

                        var ds = DataSourceFactory.FromValue(value).ToArray();
//                        Debug.WriteLine(string.Join(", ", ds));
                        CollectionAssert.AreEqual(new float[] { j, j * 10, j * 100 }, ds);

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

            using (var sampler = new MsgPackSampler(1, 100, 100, true, 1000))
            {
                sampler.StartLoading(file);

                for (var i = 0; i < 10000; ++i)
                {
                    var batch = sampler.GetNextMinibatch();
                    Assert.IsTrue(batch.Features.ContainsKey("a"));
                }
            }
        }

        [TestMethod]
        public void TestMsgPackSamplerSlicing1()
        {
            const int NUM_SAMPLES = 300;

            var file = Path.GetTempFileName();
            using (var stream = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                for (var i = 0; i < NUM_SAMPLES; ++i)
                {
                    var a = DataSourceFactory.Create(new float[] { i, i + 1, i + 2 }, new int[] { 1, 1, 3 });
                    var dss = new DataSourceSet();
                    dss.Add("a", a);
                    MsgPackSerializer.Serialize(dss, stream);
                }
            }

            using (var sampler = new MsgPackSampler(1, 7, 10, false, 100))
            {
                sampler.StartLoading(file);

                for (var i = 0; i < 10; ++i)
                {
                    for (var j = 0; j < NUM_SAMPLES * 3; ++j)
                    {
                        var batch = sampler.GetNextMinibatch();
                        Assert.AreEqual(1, batch.Features.Count);
                        Assert.IsTrue(batch.Features.ContainsKey("a"));
                        Assert.AreEqual((i * NUM_SAMPLES * 3 + j + 1) % 7 == 0, batch.SweepEnd);

                        var value = batch["a"];
                        CollectionAssert.AreEqual(new int[] { 1, 1, 1 }, value.Shape.Dimensions.ToArray());

                        var ds = DataSourceFactory.FromValue(value).ToArray();
//                        Debug.WriteLine(string.Join(", ", ds));
                        CollectionAssert.AreEqual(new float[] { j / 3 + j % 3 }, ds);

                    }
                }
            }
        }

        [TestMethod]
        public void TestMsgPackSamplerSlicing2()
        {
            const int NUM_SAMPLES = 300;

            var file = Path.GetTempFileName();
            using (var stream = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                for (var i = 0; i < NUM_SAMPLES; ++i)
                {
                    var a = DataSourceFactory.Create(new float[] { 0, 1, 2 }, new int[] { 1, 1, 3 });
                    var dss = new DataSourceSet();
                    dss.Add("a", a);
                    MsgPackSerializer.Serialize(dss, stream);
                }
            }

            using (var sampler = new MsgPackSampler(5, 7, 10, false, 100))
            {
                sampler.StartLoading(file);

                int count = 0;
                for (var i = 0; i < 10; ++i)
                {
                    for (var j = 0; j < NUM_SAMPLES * 3; ++j)
                    {
                        var batch = sampler.GetNextMinibatch();
                        Assert.AreEqual(1, batch.Features.Count);
                        Assert.IsTrue(batch.Features.ContainsKey("a"));

                        var value = batch["a"];
                        CollectionAssert.AreEqual(new int[] { 1, 1, 5 }, value.Shape.Dimensions.ToArray());

                        var ds = DataSourceFactory.FromValue(value).ToArray();
                        Debug.WriteLine(string.Join(", ", ds));
                        CollectionAssert.AreEqual(new float[]
                        {
                            count % 3,
                            (count + 1) % 3,
                            (count + 2) % 3,
                            (count + 3) % 3,
                            (count + 4) % 3,
                        }, ds);
                        count += 5;
                    }
                }
            }
        }
    }
}
