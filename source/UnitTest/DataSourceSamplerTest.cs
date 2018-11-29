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
    public class DataSourceSamplerTest
    {
        public DataSourceSamplerTest()
        {
            UnmanagedDllLoader.Load(@"..\..\..\..\lib");
            DeviceDescriptor.TrySetDefaultDevice(DeviceDescriptor.CPUDevice);
        }

        [TestMethod]
        public void TestMinibatch()
        {
            var features = DataSourceFactory.Create(new float[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new int[] { 2, 1, -1 });

            var dss = new Dictionary<string, IDataSource<float>>() { { "input", features } };
            var sampler = new DataSourceSampler(dss, 2, false, true);

            {
                var batch = sampler.GetNextMinibatch();
                //                GC.Collect();
                //                GC.Collect();

                Assert.AreEqual(2, batch.SampleCount);
                Assert.AreEqual(false, batch.SweepEnd);

                var data = batch.Features["input"];
                // var c1 = SharedPtrMethods.GetUseCountOf(data);
                // var c2 = SharedPtrMethods.GetUseCountOf(data.data);
                // var c3 = SharedPtrMethods.GetUseCountOf(data.data.Data);
                var ds = DataSourceFactory.FromValue(data);
                CollectionAssert.AreEqual(new float[] { 0, 1, 2, 3 }, ds.TypedData);
                CollectionAssert.AreEqual(new int[] { 2, 1, 2 }, data.Shape.Dimensions.ToArray());
            }

            {
                var batch = sampler.GetNextMinibatch();
//                GC.Collect();
                Assert.AreEqual(2, batch.SampleCount);
                Assert.AreEqual(true, batch.SweepEnd);
                var data = batch.Features["input"];
                // var c1 = SharedPtrMethods.GetUseCountOf(data);
                // var c2 = SharedPtrMethods.GetUseCountOf(data.data);
                // var c3 = SharedPtrMethods.GetUseCountOf(data.data.Data);
                var ds = DataSourceFactory.FromValue(data);
                CollectionAssert.AreEqual(new float[] { 4, 5, 6, 7 }, ds.TypedData);
                CollectionAssert.AreEqual(new int[] { 2, 1, 2 }, data.Shape.Dimensions.ToArray());
            }

            // When not randomized, remnant data that is smaller than the minibatch size is ignored.
            {
                var batch = sampler.GetNextMinibatch();
//                GC.Collect();
                Assert.AreEqual(2, batch.SampleCount);
                Assert.AreEqual(false, batch.SweepEnd);
                var data = batch.Features["input"];
                var ds = DataSourceFactory.FromValue(data);
                CollectionAssert.AreEqual(new float[] { 0, 1, 2, 3 }, ds.TypedData);
                CollectionAssert.AreEqual(new int[] { 2, 1, 2 }, data.Shape.Dimensions.ToArray());
            }
        }

        [TestMethod]
        public void TestSequence()
        {
            var features = DataSourceFactory.Create(new float[] { 0, 1, 2, 3, 4, 5, 6, 7 }, new int[] { 2, 2, -1 });

            var ds = new Dictionary<string, IDataSource<float>>() { { "input", features } };
            var sampler = new DataSourceSampler(ds, 2, false, true);

            var batch = sampler.GetNextMinibatch();
            Assert.AreEqual(2, batch.SampleCount);
            Assert.AreEqual(true, batch.SweepEnd);
            var data = batch.Features["input"];
            CollectionAssert.AreEqual(new float[] { 0, 1, 2, 3, 4, 5, 6, 7 }, DataSourceFactory.FromValue(data).TypedData);
            CollectionAssert.AreEqual(new int[] { 2, 2, 2 }, data.Shape.Dimensions.ToArray());
        }
    }
}