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
        }

        [TestMethod]
        public void TestWithoutParameter()
        {
            var exp = new Constant(new int[] { 2, 3 }, DataType.Float, 0) + CNTKLib.Pow(Constant.Scalar(DataType.Float, 3), Constant.Scalar(DataType.Float, 3));
            var sampler = new ExpressionSampler("value", exp, null);

            var batch = sampler.GetNextBatch();

            Assert.AreEqual(batch.Features.Count, 1);

            var data = batch["value"];

            CollectionAssert.AreEqual(data.data.Shape.Dimensions.ToArray(), new int[] { 2, 3 });

            CollectionAssert.AreEqual(DataSource<float>.FromValue(data.data).Data, new float[] { 27, 27, 27, 27, 27, 27 });
        }

        [TestMethod]
        public void TestWithParameter()
        {
            var input = Variable.InputVariable(new int[] { 1 }, DataType.Float, "input");
            var exp = input + Constant.Scalar(DataType.Float, 1);

            var sampler = new ExpressionSampler("value", exp, input);

            var batch = sampler.GetNextBatch();
            var data = batch["value"];
            CollectionAssert.AreEqual(data.data.Shape.Dimensions.ToArray(), new int[] { 1, 1, 1 });
            var ds = DataSource<float>.FromValue(data.data);
            CollectionAssert.AreEqual(ds.Data, new float[] { 1 });

            batch = sampler.GetNextBatch();
            data = batch["value"];
            CollectionAssert.AreEqual(data.data.Shape.Dimensions.ToArray(), new int[] { 1, 1, 1 });
            ds = DataSource<float>.FromValue(data.data);
            CollectionAssert.AreEqual(ds.Data, new float[] { 2 });

            batch = sampler.GetNextBatch();
            data = batch["value"];
            CollectionAssert.AreEqual(data.data.Shape.Dimensions.ToArray(), new int[] { 1, 1, 1 });
            ds = DataSource<float>.FromValue(data.data);
            CollectionAssert.AreEqual(ds.Data, new float[] { 3 });
        }
    }
}
