using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CNTK;
using Horker.PSCNTK;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class ProgressiveMinibatchDefinitionTest
    {
        public ProgressiveMinibatchDefinitionTest()
        {
            var path = Environment.ExpandEnvironmentVariables("%HOME%\\work\\pscntk\\lib");
            UnmanagedDllLoader.Load(path);
        }

        [TestMethod]
        public void TestProgressiveMinibatch()
        {
            var a = new DataSourceSet[]
            {
                new DataSourceSet(new Dictionary<string, DataSource<float>> { { "input", new DataSource<float>(new float[] { 1, 2, 3 }, new int[]{ 1, 1, -1 }) } }),
                new DataSourceSet(new Dictionary<string, DataSource<float>> { { "input", new DataSource<float>(new float[] { 4 }, new int[] { 1, 1, -1 }) } }),
                new DataSourceSet(new Dictionary<string, DataSource<float>> { { "input", new DataSource<float>(new float[] { 5, 6, 7 }, new int[] { 1, 1, -1 }) } })
            };

            var minibatchDef = new ProgressiveMinibatchDefinition(2, 4, 1, 10);

            minibatchDef.AddDataSourceSet(a[0]);

            var batch = minibatchDef.GetNextBatch();
            var data = batch.Features["input"];
            CollectionAssert.AreEqual(new int[] { 1, 1, 2 }, data.data.Shape.Dimensions.ToArray());
            CollectionAssert.AreEqual(new float[] { 1, 2 }, DataSource<float>.FromValue(data.data).Data);
            Assert.AreEqual((uint)2, data.numberOfSamples);
            Assert.AreEqual((uint)2, data.numberOfSequences);
            Assert.AreEqual(false, data.sweepEnd);

            minibatchDef.AddDataSourceSet(a[1]);
            minibatchDef.AddDataSourceSet(a[2]);

            batch = minibatchDef.GetNextBatch();
            data = batch.Features["input"];
            CollectionAssert.AreEqual(new int[] { 1, 1, 2 }, data.data.Shape.Dimensions.ToArray());
            CollectionAssert.AreEqual(new float[] { 3, 4 }, DataSource<float>.FromValue(data.data).Data);
            Assert.AreEqual((uint)2, data.numberOfSamples);
            Assert.AreEqual((uint)2, data.numberOfSequences);
            Assert.AreEqual(true, data.sweepEnd);

            batch = minibatchDef.GetNextBatch();
            data = batch.Features["input"];
            CollectionAssert.AreEqual(new int[] { 1, 1, 2 }, data.data.Shape.Dimensions.ToArray());
            CollectionAssert.AreEqual(new float[] { 5, 6 }, DataSource<float>.FromValue(data.data).Data);
            Assert.AreEqual((uint)2, data.numberOfSamples);
            Assert.AreEqual((uint)2, data.numberOfSequences);
            Assert.AreEqual(false, data.sweepEnd);
        }

        [TestMethod]
        public void TestProgressiveMinibatchValidation()
        {
            var a = new DataSourceSet[]
            {
                new DataSourceSet(new Dictionary<string, DataSource<float>> { { "input", new DataSource<float>(new float[] { 1, 2, 3 }, new int[]{ 1, 1, -1 }) } }),
                new DataSourceSet(new Dictionary<string, DataSource<float>> { { "input", new DataSource<float>(new float[] { 4 }, new int[] { 1, 1, -1 }) } }),
                new DataSourceSet(new Dictionary<string, DataSource<float>> { { "input", new DataSource<float>(new float[] { 5, 6, 7 }, new int[] { 1, 1, -1 }) } })
            };

            var minibatchDef = new ProgressiveMinibatchDefinition(2, 4, 3, 10);

            minibatchDef.AddDataSourceSet(a[0]);
            minibatchDef.AddDataSourceSet(a[1]);
            minibatchDef.AddDataSourceSet(a[2]);

            var batch = minibatchDef.GetNextBatch();
            var data = batch.Features["input"];
            CollectionAssert.AreEqual(new int[] { 1, 1, 2 }, data.data.Shape.Dimensions.ToArray());
            CollectionAssert.AreEqual(new float[] { 1, 2 }, DataSource<float>.FromValue(data.data).Data);
            Assert.AreEqual((uint)2, data.numberOfSamples);
            Assert.AreEqual((uint)2, data.numberOfSequences);
            Assert.AreEqual(false, data.sweepEnd);

            batch = minibatchDef.GetValidationBatch();
            data = batch.Features["input"];
            CollectionAssert.AreEqual(new int[] { 1, 1, 3 }, data.data.Shape.Dimensions.ToArray());
            CollectionAssert.AreEqual(new float[] { 4, 5, 6 }, DataSource<float>.FromValue(data.data).Data);
            Assert.AreEqual((uint)3, data.numberOfSamples);
            Assert.AreEqual((uint)3, data.numberOfSequences);
            Assert.AreEqual(false, data.sweepEnd);

            batch = minibatchDef.GetNextBatch();
            data = batch.Features["input"];
            CollectionAssert.AreEqual(new int[] { 1, 1, 2 }, data.data.Shape.Dimensions.ToArray());
            CollectionAssert.AreEqual(new float[] { 3, 7 }, DataSource<float>.FromValue(data.data).Data);
            Assert.AreEqual((uint)2, data.numberOfSamples);
            Assert.AreEqual((uint)2, data.numberOfSequences);
            Assert.AreEqual(true, data.sweepEnd);
        }

        [TestMethod]
        public void TestProgressiveMinibatchFeatures()
        {
            var a = new DataSourceSet[]
            {
                new DataSourceSet(new Dictionary<string, DataSource<float>>
                {
                    { "input", new DataSource<float>(new float[] { 1, 2, 3, 4, 5, 6 }, new int[]{ 2, 1, -1 }) },
                    { "label", new DataSource<float>(new float[] { 11, 12, 13 }, new int[]{ 1, 1, -1 }) }
                }),
                new DataSourceSet(new Dictionary<string, DataSource<float>>
                {
                    { "input", new DataSource<float>(new float[] { 7, 8 }, new int[] { 2, 1, -1 }) },
                    { "label", new DataSource<float>(new float[] { 14 }, new int[] { 1, 1, -1 }) }
                })
            };

            var minibatchDef = new ProgressiveMinibatchDefinition(2, 4, 3, 10);

            minibatchDef.AddDataSourceSet(a[0]);
            minibatchDef.AddDataSourceSet(a[1]);

            var batch = minibatchDef.GetNextBatch();

            var data = batch.Features["input"];
            CollectionAssert.AreEqual(new int[] { 2, 1, 2 }, data.data.Shape.Dimensions.ToArray());
            CollectionAssert.AreEqual(new float[] { 1, 2, 3, 4 }, DataSource<float>.FromValue(data.data).Data);
            Assert.AreEqual((uint)2, data.numberOfSamples);
            Assert.AreEqual((uint)2, data.numberOfSequences);
            Assert.AreEqual(false, data.sweepEnd);

            data = batch.Features["label"];
            CollectionAssert.AreEqual(new int[] { 1, 1, 2 }, data.data.Shape.Dimensions.ToArray());
            CollectionAssert.AreEqual(new float[] { 11, 12 }, DataSource<float>.FromValue(data.data).Data);
            Assert.AreEqual((uint)2, data.numberOfSamples);
            Assert.AreEqual((uint)2, data.numberOfSequences);
            Assert.AreEqual(false, data.sweepEnd);

            batch = minibatchDef.GetNextBatch();

            data = batch.Features["input"];
            CollectionAssert.AreEqual(new int[] { 2, 1, 2 }, data.data.Shape.Dimensions.ToArray());
            CollectionAssert.AreEqual(new float[] { 5, 6, 7, 8 }, DataSource<float>.FromValue(data.data).Data);
            Assert.AreEqual((uint)2, data.numberOfSamples);
            Assert.AreEqual((uint)2, data.numberOfSequences);
            Assert.AreEqual(true, data.sweepEnd);

            data = batch.Features["label"];
            CollectionAssert.AreEqual(new int[] { 1, 1, 2 }, data.data.Shape.Dimensions.ToArray());
            CollectionAssert.AreEqual(new float[] { 13, 14 }, DataSource<float>.FromValue(data.data).Data);
            Assert.AreEqual((uint)2, data.numberOfSamples);
            Assert.AreEqual((uint)2, data.numberOfSequences);
            Assert.AreEqual(true, data.sweepEnd);

        }

        [TestMethod]
        public void TestCancelAdding()
        {
            var minibatchDef = new ProgressiveMinibatchDefinition(1, 10, 10, 10);

            var producerStopped = false;

            var producer = Task.Run(() => {
                for (var i = 0; ; ++i)
                {
                    var dss = new DataSourceSet
                    (
                        new Dictionary<string, DataSource<float>> { { "input", new DataSource<float>(new float[] { i }, new int[] { 1, 1, -1 }) }
                    });

                    var cont = minibatchDef.AddDataSourceSet(dss);
                    if (!cont)
                        break;
                }

                producerStopped = true;
            });

            var batch = minibatchDef.GetNextBatch();
            var data = batch.Features["input"];
            CollectionAssert.AreEqual(new float[] { 0 }, DataSource<float>.FromValue(data.data).Data);
            Assert.AreEqual(false, producerStopped);

            batch = minibatchDef.GetNextBatch();
            data = batch.Features["input"];
            CollectionAssert.AreEqual(new float[] { 1 }, DataSource<float>.FromValue(data.data).Data);
            Assert.AreEqual(false, producerStopped);

            batch = minibatchDef.GetNextBatch();
            data = batch.Features["input"];
            CollectionAssert.AreEqual(new float[] { 2 }, DataSource<float>.FromValue(data.data).Data);
            Assert.AreEqual(false, producerStopped);

            minibatchDef.CancelAdding();

            producer.Wait();

            Assert.AreEqual(true, producerStopped);
        }
    }
}
