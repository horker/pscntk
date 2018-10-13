using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CNTK;
using Horker.PSCNTK;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class OnMemorySamplerTest
    {
        public OnMemorySamplerTest()
        {
            UnmanagedDllLoader.Load(@"..\..\..\..\lib");
            DeviceDescriptor.TrySetDefaultDevice(DeviceDescriptor.CPUDevice);
        }

        [TestMethod]
        public void TestMinibatch()
        {
            var features = DataSourceFactory.Create(new float[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new int[] { 2, 1, -1 });

            var dss = new Dictionary<string, IDataSource<float>>() { { "input", features } };
            var sampler = new OnMemorySampler(dss, 2, false, true);

            {
                var batch = sampler.GetNextMinibatch();
//                GC.Collect();
//                GC.Collect();
                var data = batch.Features["input"];
                // var c1 = SharedPtrMethods.GetUseCountOf(data);
                // var c2 = SharedPtrMethods.GetUseCountOf(data.data);
                // var c3 = SharedPtrMethods.GetUseCountOf(data.data.Data);
                var ds = DataSourceFactory.FromValue(data.data);
                CollectionAssert.AreEqual(new float[] { 0, 1, 2, 3 }, ds.TypedData);
                CollectionAssert.AreEqual(new int[] { 2, 1, 2 }, data.data.Shape.Dimensions.ToArray());
                Assert.AreEqual((uint)2, data.numberOfSamples);
                Assert.AreEqual((uint)2, data.numberOfSequences);
                Assert.AreEqual(false, data.sweepEnd);
            }

            {
                var batch = sampler.GetNextMinibatch();
//                GC.Collect();
                var data = batch.Features["input"];
                // var c1 = SharedPtrMethods.GetUseCountOf(data);
                // var c2 = SharedPtrMethods.GetUseCountOf(data.data);
                // var c3 = SharedPtrMethods.GetUseCountOf(data.data.Data);
                var ds = DataSourceFactory.FromValue(data.data);
                CollectionAssert.AreEqual(new float[] { 4, 5, 6, 7 }, ds.TypedData);
                CollectionAssert.AreEqual(new int[] { 2, 1, 2 }, data.data.Shape.Dimensions.ToArray());
                Assert.AreEqual((uint)2, data.numberOfSamples);
                Assert.AreEqual((uint)2, data.numberOfSequences);
                Assert.AreEqual(true, data.sweepEnd);
            }

            // When not randomized, remnant data that is smaller than the minibatch size is ignored.
            {
                var batch = sampler.GetNextMinibatch();
//                GC.Collect();
                var data = batch.Features["input"];
                var ds = DataSourceFactory.FromValue(data.data);
                CollectionAssert.AreEqual(new float[] { 0, 1, 2, 3 }, ds.TypedData);
                CollectionAssert.AreEqual(new int[] { 2, 1, 2 }, data.data.Shape.Dimensions.ToArray());
                Assert.AreEqual((uint)2, data.numberOfSamples);
                Assert.AreEqual((uint)2, data.numberOfSequences);
                Assert.AreEqual(false, data.sweepEnd);
            }
        }

        [TestMethod]
        public void TestSequence()
        {
            var features = DataSourceFactory.Create(new float[] { 0, 1, 2, 3, 4, 5, 6, 7 }, new int[] { 2, 2, -1 });

            var ds = new Dictionary<string, IDataSource<float>>() { { "input", features } };
            var sampler = new OnMemorySampler(ds, 2, false, true);

            var batch = sampler.GetNextMinibatch();
            var data = batch.Features["input"];
            CollectionAssert.AreEqual(new float[] { 0, 1, 2, 3, 4, 5, 6, 7 }, DataSourceFactory.FromValue(data.data).TypedData);
            CollectionAssert.AreEqual(new int[] { 2, 2, 2 }, data.data.Shape.Dimensions.ToArray());
            Assert.AreEqual((uint)4, data.numberOfSamples);
            Assert.AreEqual((uint)2, data.numberOfSequences);
            Assert.AreEqual(true, data.sweepEnd);
        }
    }
}