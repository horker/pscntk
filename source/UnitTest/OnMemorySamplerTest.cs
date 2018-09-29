﻿using System;
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

            var ds = new Dictionary<string, IDataSource<float>>() { { "input", features } };
            var sampler = new OnMemorySampler(ds, 2, 0, false, true);

            var batch = sampler.GetNextMinibatch();
            GC.Collect();
            var data = batch.Features["input"];
            CollectionAssert.AreEqual(new float[] { 0, 1, 2, 3 }, DataSourceFactory.FromValue(data.data).TypedData);
            CollectionAssert.AreEqual(new int[] { 2, 1, 2 }, data.data.Shape.Dimensions.ToArray());
            Assert.AreEqual((uint)2, data.numberOfSamples);
            Assert.AreEqual((uint)2, data.numberOfSequences);
            Assert.AreEqual(false, data.sweepEnd);

            batch = sampler.GetNextMinibatch();
            GC.Collect();
            data = batch.Features["input"];
            CollectionAssert.AreEqual(new float[] { 4, 5, 6, 7 }, DataSourceFactory.FromValue(data.data).TypedData);
            CollectionAssert.AreEqual(new int[] { 2, 1, 2 }, data.data.Shape.Dimensions.ToArray());
            Assert.AreEqual((uint)2, data.numberOfSamples);
            Assert.AreEqual((uint)2, data.numberOfSequences);
            Assert.AreEqual(true, data.sweepEnd);

            // When not randomized, remnant data that is smaller than the minibatch size is ignored.

            batch = sampler.GetNextMinibatch();
            GC.Collect();
            data = batch.Features["input"];
            CollectionAssert.AreEqual(new float[] { 0, 1, 2, 3 }, DataSourceFactory.FromValue(data.data).TypedData);
            CollectionAssert.AreEqual(new int[] { 2, 1, 2 }, data.data.Shape.Dimensions.ToArray());
            Assert.AreEqual((uint)2, data.numberOfSamples);
            Assert.AreEqual((uint)2, data.numberOfSequences);
            Assert.AreEqual(false, data.sweepEnd);
        }

        [TestMethod]
        public void TestValidation()
        {
            var features = DataSourceFactory.Create(new float[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new int[] { 2, 1, -1 });

            var ds = new Dictionary<string, IDataSource<float>>() { { "input", features } };
            var sampler = new OnMemorySampler(ds, 2, .4, false, true);

            var batch = sampler.GetNextMinibatch();
            var data = batch.Features["input"];
            CollectionAssert.AreEqual(new float[] { 0, 1, 2, 3 }, DataSourceFactory.FromValue(data.data).TypedData);
            CollectionAssert.AreEqual(new int[] { 2, 1, 2 }, data.data.Shape.Dimensions.ToArray());
            Assert.AreEqual((uint)2, data.numberOfSamples);
            Assert.AreEqual((uint)2, data.numberOfSequences);
            Assert.AreEqual(true, data.sweepEnd);

            batch = sampler.GetNextMinibatch();
            data = batch.Features["input"];
            CollectionAssert.AreEqual(new float[] { 0, 1, 2, 3 }, DataSourceFactory.FromValue(data.data).TypedData);
            CollectionAssert.AreEqual(new int[] { 2, 1, 2 }, data.data.Shape.Dimensions.ToArray());
            Assert.AreEqual((uint)2, data.numberOfSamples);
            Assert.AreEqual((uint)2, data.numberOfSequences);
            Assert.AreEqual(true, data.sweepEnd);

            batch = sampler.GetValidationMinibatch();
            data = batch.Features["input"];
            CollectionAssert.AreEqual(new float[] { 6, 7, 8, 9 }, DataSourceFactory.FromValue(data.data).TypedData);
            CollectionAssert.AreEqual(new int[] { 2, 1, 2 }, data.data.Shape.Dimensions.ToArray());
            Assert.AreEqual((uint)2, data.numberOfSamples);
            Assert.AreEqual((uint)2, data.numberOfSequences);
            Assert.AreEqual(false, data.sweepEnd);
        }

        [TestMethod]
        public void TestSequence()
        {
            var features = DataSourceFactory.Create(new float[] { 0, 1, 2, 3, 4, 5, 6, 7 }, new int[] { 2, 2, -1 });

            var ds = new Dictionary<string, IDataSource<float>>() { { "input", features } };
            var sampler = new OnMemorySampler(ds, 2, .4, false, true);

            var batch = sampler.GetNextMinibatch();
            var data = batch.Features["input"];
            CollectionAssert.AreEqual(new float[] { 0, 1, 2, 3, 4, 5, 6, 7 }, DataSourceFactory.FromValue(data.data).TypedData);
            CollectionAssert.AreEqual(new int[] { 2, 2, 2 }, data.data.Shape.Dimensions.ToArray());
            Assert.AreEqual((uint)4, data.numberOfSamples);
            Assert.AreEqual((uint)2, data.numberOfSequences);
            Assert.AreEqual(true, data.sweepEnd);
        }

        [TestMethod]
        public void TestSequenceRandomization()
        {
            var features = DataSourceFactory.Create(new float[] { 0, 1, 2, 3, 4, 5, 6, 7 }, new int[] { 1, 2, -1 });

            var ds = new Dictionary<string, IDataSource<float>>() { { "input", features } };
            var sampler = new OnMemorySampler(ds, 2, .5, true, true);

            var batch = sampler.GetValidationMinibatch();
            var data = batch.Features["input"];
            CollectionAssert.AreEqual(new float[] { 4, 5, 6, 7 }, DataSourceFactory.FromValue(data.data).TypedData);
        }
    }
}