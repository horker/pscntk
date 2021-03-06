﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CNTK;
using Horker.PSCNTK;

namespace UnitTest
{
    [TestClass]
    public class ExpressionSamplerTest
    {
        public ExpressionSamplerTest()
        {
            UnmanagedDllLoader.Load(@"..\..\..\..\lib");
            DeviceDescriptor.TrySetDefaultDevice(DeviceDescriptor.CPUDevice);
        }

        [TestMethod]
        public void TestWithoutParameter()
        {
            var exp = new Constant(new int[] { 2, 3 }, DataType.Float, 0) + Constant.Scalar(DataType.Float, 27);
            var sampler = new ExpressionSampler("value", exp, null);

            var batch = sampler.GetNextMinibatch();

            Assert.AreEqual(batch.Features.Count, 1);

            var data = batch["value"];

            CollectionAssert.AreEqual(data.Shape.Dimensions.ToArray(), new int[] { 2, 3 });

            var ds = DataSourceFactory.FromValue(data);
            CollectionAssert.AreEqual(ds.TypedData, new float[] { 27, 27, 27, 27, 27, 27 });
        }

        [TestMethod]
        public void TestWithParameter()
        {
            var input = Variable.InputVariable(new int[] { 1 }, DataType.Float, "input", new Axis[] { Axis.DefaultBatchAxis() });
            var exp = input + Constant.Scalar(DataType.Float, 1);

            var sampler = new ExpressionSampler("value", exp, input, 3);

            var batch = sampler.GetNextMinibatch();
            var data = batch["value"];
            CollectionAssert.AreEqual(data.Shape.Dimensions.ToArray(), new int[] { 1, 3 });
            var ds = DataSourceFactory.FromValue(data);
            CollectionAssert.AreEqual(ds.TypedData, new float[] { 1, 1, 1 });

            batch = sampler.GetNextMinibatch();
            data = batch["value"];
            CollectionAssert.AreEqual(data.Shape.Dimensions.ToArray(), new int[] { 1, 3 });
            ds = DataSourceFactory.FromValue(data);
            CollectionAssert.AreEqual(ds.TypedData, new float[] { 2, 2, 2 });

            batch = sampler.GetNextMinibatch();
            data = batch["value"];
            CollectionAssert.AreEqual(data.Shape.Dimensions.ToArray(), new int[] { 1, 3 });
            ds = DataSourceFactory.FromValue(data);
            CollectionAssert.AreEqual(ds.TypedData, new float[] { 3, 3, 3 });
        }
    }
}
