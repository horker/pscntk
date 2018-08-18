using System;
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
    public class CompositeFunctionTest
    {
        public CompositeFunctionTest()
        {
            UnmanagedDllLoader.Load(@"..\..\..\..\lib");
        }

        [TestMethod]
        public void TestConv()
        {
            var inputData = new DataSource<float>(new float[] { 1, 2, 3, 4, 1, 2, 3, 4 }, new int[] { 2, 2, 2 });

            var input = CNTKLib.InputVariable(new int[] { 2, 2, 2 }, DataType.Float);

            var f = Composite.Convolution(input, new int[] { 2, 2, 2 }, 4, null, CNTKLib.ConstantInitializer(1), false, null, new int[] { 1, 1, 2 }, new bool[] { true }, new int[] { 1 }, 1, 1, 0, "");

            var inputs = new Dictionary<Variable, Value>() { { input, inputData.ToValue() } };
            var outputs = new Dictionary<Variable, Value>() { { f.Output, null } };

            f.Evaluate(inputs, outputs, DeviceDescriptor.UseDefaultDevice());
            var result = DataSource<float>.FromValue(outputs[f.Output]);

            CollectionAssert.AreEqual(new int[] { 2, 2, 4, 1, 1 }, result.Shape.Dimensions);
            Assert.AreEqual(20, result.Data[0]);
            Assert.AreEqual(12, result.Data[1]);
            Assert.AreEqual(14, result.Data[2]);
            Assert.AreEqual(8, result.Data[3]);
        }

        [TestMethod]
        public void TestConvolutionTranspose()
        {
            var inputData = new DataSource<float>(new float[] { 1, 2, 3, 4 }, new int[] { 2, 2, 1 });

            var input = CNTKLib.InputVariable(new int[] { 2, 2, 1 }, DataType.Float);

            var f = Composite.ConvolutionTranspose(input, new int[] { 2, 2, 1 }, 1, null, CNTKLib.ConstantInitializer(1), new bool[] { true }, new int[] { 2, 2, 1 }, false, null, new int[] { 0 }, new int[] { 1 }, 1, 0, "");

            var inputs = new Dictionary<Variable, Value>() { { input, inputData.ToValue() } };
            var outputs = new Dictionary<Variable, Value>() { { f.Output, null } };

            f.Evaluate(inputs, outputs, DeviceDescriptor.UseDefaultDevice());
            var result = DataSource<float>.FromValue(outputs[f.Output]);

            CollectionAssert.AreEqual(new int[] { 3, 3, 1, 1, 1 }, result.Shape.Dimensions);
            Assert.AreEqual(1, result.Data[0]);
            Assert.AreEqual(1, result.Data[1]);
            Assert.AreEqual(2, result.Data[2]);
            Assert.AreEqual(1, result.Data[3]);
        }
    }
}
