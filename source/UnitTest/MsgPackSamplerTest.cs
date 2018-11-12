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

            using (var sampler = new MsgPackSampler(file, 1, false, 3, 10, false, 100))
            {
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

            using (var sampler = new MsgPackSampler(file, 1, false, 100, 100, true, 1000))
            {
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

            using (var sampler = new MsgPackSampler(file, 1, false, 7, 10, false, 100))
            {
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
                    var a = DataSourceFactory.Create(new float[] { i, i, i }, new int[] { 1, 1, 3 });
                    var dss = new DataSourceSet();
                    dss.Add("a", a);
                    MsgPackSerializer.Serialize(dss, stream);
                }
            }

            using (var sampler = new MsgPackSampler(file, 5, false, 7, 10, false, 100))
            {
                int count = 0;
                for (var i = 0; i < 10; ++i)
                {
                    for (var j = 0; j < NUM_SAMPLES * 3 / 5; ++j)
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
                            j * 5 / 3,
                            (j * 5 + 1) / 3,
                            (j * 5 + 2) / 3,
                            (j * 5 + 3) / 3,
                            (j * 5 + 4) / 3
                        }, ds);
                        count += 5;
                    }
                }
            }
        }

        [TestMethod]
        public void TestMsgPackSamplerRandomize()
        {
            const int NUM_CHUNKS = 30;
            const int CHUNK_SIZE = 6;
            const int MINIBATCH_SIZE = 2;

            var data = new float[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            int FEATURE_DIM = data.Length / CHUNK_SIZE;

            var file = Path.GetTempFileName();
            using (var stream = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                for (var i = 0; i < NUM_CHUNKS; ++i)
                {
                    var a = DataSourceFactory.Create(data, new int[] { FEATURE_DIM, CHUNK_SIZE });
                    var dss = new DataSourceSet();
                    dss.Add("a", a);
                    MsgPackSerializer.Serialize(dss, stream);
                }
            }

            using (var sampler = new MsgPackSampler(file, MINIBATCH_SIZE, true, 10, 100, false, 100))
            {
                for (var i = 0; i < NUM_CHUNKS; ++i)
                {
                    var values = new float[data.Length];
                    for (var j = 0; j < data.Length; j += FEATURE_DIM * MINIBATCH_SIZE)
                    {
                        var batch = sampler.GetNextMinibatch();
                        var value = DataSourceFactory.FromValue(batch["a"]);
                        CollectionAssert.AreEqual(new int[] { FEATURE_DIM, MINIBATCH_SIZE }, value.Shape.Dimensions.ToArray());
                        for (var k = 0; k < FEATURE_DIM * MINIBATCH_SIZE; ++k)
                            values[j + k] = value[k];
                    }

                    CollectionAssert.AreNotEqual(data, values);
                    var sorted = values.ToList();
                    sorted.Sort();
                    CollectionAssert.AreEqual(data, sorted);
                }
            }
        }

        [TestMethod]
        public void TestMsgPackSamplerSampleCountPerEpoch()
        {
            const int NUM_SAMPLES = 3;
            const int NUM_CHUNKS = 100;

            var file = Path.GetTempFileName();
            using (var stream = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                for (var i = 0; i < NUM_CHUNKS; ++i)
                {
                    var a = DataSourceFactory.Create(new float[] { i, i * 10, i * 100 }, new int[] { 1, 1, NUM_SAMPLES });
                    var dss = new DataSourceSet();
                    dss.Add("a", a);
                    MsgPackSerializer.Serialize(dss, stream);
                }
            }

            using (var sampler = new MsgPackSampler(file, 1, false, -1, 10, false, 100))
            {
                Assert.AreEqual(NUM_SAMPLES * NUM_CHUNKS, sampler.SampleCountPerEpoch);
            }
        }
    }
}
