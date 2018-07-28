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
    public class MinibatchDefinitionTest
    {
        public MinibatchDefinitionTest()
        {
            var path = Environment.ExpandEnvironmentVariables("%HOME%\\work\\pscntk\\lib");
            UnmanagedDllLoader.Load(path);
        }

        [TestMethod]
        public void TestMinibatch()
        {
            var features = new DataSource<float>(new float[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new int[] { 2, 1, -1 });

            var ds = new Dictionary<string, DataSource<float>>() { { "input", features } };
            var minibatchDef = new MinibatchDefinition(ds, 2, 0, false);

            var batch = minibatchDef.GetNextBatch();
            var data = batch.Features["input"];
            CollectionAssert.AreEqual(new float[] { 0, 1, 2, 3 }, DataSource<float>.FromValue(data.data).Data);
            CollectionAssert.AreEqual(new int[] { 2, 1, 2 }, data.data.Shape.Dimensions.ToArray());
            Assert.AreEqual((uint)2, data.numberOfSamples);
            Assert.AreEqual((uint)2, data.numberOfSequences);
            Assert.AreEqual(false, data.sweepEnd);

            batch = minibatchDef.GetNextBatch();
            data = batch.Features["input"];
            CollectionAssert.AreEqual(new float[] { 4, 5, 6, 7 }, DataSource<float>.FromValue(data.data).Data);
            CollectionAssert.AreEqual(new int[] { 2, 1, 2 }, data.data.Shape.Dimensions.ToArray());
            Assert.AreEqual((uint)2, data.numberOfSamples);
            Assert.AreEqual((uint)2, data.numberOfSequences);
            Assert.AreEqual(true, data.sweepEnd);

            // When not randomized, remnant data that is smaller than the minibatch size is ignored.

            batch = minibatchDef.GetNextBatch();
            data = batch.Features["input"];
            CollectionAssert.AreEqual(new float[] { 0, 1, 2, 3 }, DataSource<float>.FromValue(data.data).Data);
            CollectionAssert.AreEqual(new int[] { 2, 1, 2 }, data.data.Shape.Dimensions.ToArray());
            Assert.AreEqual((uint)2, data.numberOfSamples);
            Assert.AreEqual((uint)2, data.numberOfSequences);
            Assert.AreEqual(false, data.sweepEnd);
        }

        [TestMethod]
        public void TestValidation()
        {
            var features = new DataSource<float>(new float[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new int[] { 2, 1, -1 });

            var ds = new Dictionary<string, DataSource<float>>() { { "input", features } };
            var minibatchDef = new MinibatchDefinition(ds, 2, .4, false);

            var batch = minibatchDef.GetNextBatch();
            var data = batch.Features["input"];
            CollectionAssert.AreEqual(new float[] { 0, 1, 2, 3 }, DataSource<float>.FromValue(data.data).Data);
            CollectionAssert.AreEqual(new int[] { 2, 1, 2 }, data.data.Shape.Dimensions.ToArray());
            Assert.AreEqual((uint)2, data.numberOfSamples);
            Assert.AreEqual((uint)2, data.numberOfSequences);
            Assert.AreEqual(true, data.sweepEnd);

            batch = minibatchDef.GetNextBatch();
            data = batch.Features["input"];
            CollectionAssert.AreEqual(new float[] { 0, 1, 2, 3 }, DataSource<float>.FromValue(data.data).Data);
            CollectionAssert.AreEqual(new int[] { 2, 1, 2 }, data.data.Shape.Dimensions.ToArray());
            Assert.AreEqual((uint)2, data.numberOfSamples);
            Assert.AreEqual((uint)2, data.numberOfSequences);
            Assert.AreEqual(true, data.sweepEnd);

            batch = minibatchDef.GetValidationBatch();
            data = batch.Features["input"];
            CollectionAssert.AreEqual(new float[] { 6, 7, 8, 9 }, DataSource<float>.FromValue(data.data).Data);
            CollectionAssert.AreEqual(new int[] { 2, 1, 2 }, data.data.Shape.Dimensions.ToArray());
            Assert.AreEqual((uint)2, data.numberOfSamples);
            Assert.AreEqual((uint)2, data.numberOfSequences);
            Assert.AreEqual(false, data.sweepEnd);
        }

        [TestMethod]
        public void TestSequence()
        {
            var features = new DataSource<float>(new float[] { 0, 1, 2, 3, 4, 5, 6, 7 }, new int[] { 2, 2, -1 });

            var ds = new Dictionary<string, DataSource<float>>() { { "input", features } };
            var minibatchDef = new MinibatchDefinition(ds, 2, .4, false);

            var batch = minibatchDef.GetNextBatch();
            var data = batch.Features["input"];
            CollectionAssert.AreEqual(new float[] { 0, 1, 2, 3, 4, 5, 6, 7 }, DataSource<float>.FromValue(data.data).Data);
            CollectionAssert.AreEqual(new int[] { 2, 2, 2 }, data.data.Shape.Dimensions.ToArray());
            Assert.AreEqual((uint)4, data.numberOfSamples);
            Assert.AreEqual((uint)2, data.numberOfSequences);
            Assert.AreEqual(true, data.sweepEnd);
        }

        [TestMethod]
        public void TestSequenceRandomization()
        {
            var features = new DataSource<float>(new float[] { 0, 1, 2, 3, 4, 5, 6, 7 }, new int[] { 1, 2, -1 });

            var ds = new Dictionary<string, DataSource<float>>() { { "input", features } };
            var minibatchDef = new MinibatchDefinition(ds, 2, .5, true);

            var batch = minibatchDef.GetValidationBatch();
            var data = batch.Features["input"];
            CollectionAssert.AreEqual(new float[] { 4, 5, 6, 7 }, DataSource<float>.FromValue(data.data).Data);
        }
    }
}