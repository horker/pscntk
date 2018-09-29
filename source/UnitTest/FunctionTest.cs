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
    public class FunctionTest
    {
        public FunctionTest()
        {
            UnmanagedDllLoader.Load(@"..\..\..\..\lib");
            DeviceDescriptor.TrySetDefaultDevice(DeviceDescriptor.CPUDevice);
        }

        [TestMethod]
        public void TestConvolution()
        {
            // input:
            // <ch. 1> <ch. 2>
            // | 1 3 | | 1 3 |
            // | 2 4 | | 2 4 |
            //
            // kernel:
            // <ch. 1> <ch. 2>
            // | 1 3 | | 1 3 |
            // | 2 4 | | 2 4 |
            //
            // output:
            //             <ch. 1>                       <ch. 2>
            // | 20 14 | = | (1 + 2 + 3 + 4) (3 + 4) | + | (1 + 2 + 3 + 4) (3 + 4) |
            // | 12  8 | = | (2 + 4)         4       | + | (2 + 4)         4       |


            var inputData = DataSourceFactory.Create(new float[] { 1, 2, 3, 4, 1, 2, 3, 4 }, new int[] { 2, 2, 2 });
            var kernelData = DataSourceFactory.Create(new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, new int[] { 2, 2, 2, 4 });

            var input = CNTKLib.InputVariable(new int[] { 2, 2, 2 }, DataType.Float);

            var convolutionMap = new Parameter(kernelData.ToNDArrayView());

            var f = CNTKLib.Convolution(convolutionMap, input, new int[] { 1, 1, 2 }, new BoolVector(new bool[] { true }), new BoolVector(new bool[] { true }), new int[] { 1 }, 1, 1);

            var inputs = new Dictionary<Variable, Value>() { { input, inputData.ToValue() } };
            var outputs = new Dictionary<Variable, Value>() { { f.Output, null } };

            f.Evaluate(inputs, outputs, DeviceDescriptor.UseDefaultDevice());
            var result = DataSourceFactory.FromValue(outputs[f.Output]);

            CollectionAssert.AreEqual(new int[] { 2, 2, 4, 1, 1 }, result.Shape.Dimensions);
            Assert.AreEqual(20, result.Data[0]);
            Assert.AreEqual(12, result.Data[1]);
            Assert.AreEqual(14, result.Data[2]);
            Assert.AreEqual(8, result.Data[3]);
        }

        [TestMethod]
        public void TestConvolutionTranspose()
        {
            // input:
            //                           | 0 0 0 0 0 |
            // | 1 3 |  strides = 2, 2   | 0 1 0 3 0 |
            // | 2 4 | ================> | 0 0 0 0 0 |
            //                           | 0 2 0 4 0 |
            //                           | 0 0 0 0 0 |
            //
            // kernel:
            // | 1 1 |    transpose      | 0 0 |
            // | 0 0 | ================> | 1 1 |
            //
            // output:
            //                           | 1 1 3 |
            //                           | 0 0 0 |
            //                           | 2 2 4 |

            var inputData = DataSourceFactory.Create(new float[] { 1, 2, 3, 4 }, new int[] { 2, 2, 1 });
            var convData = DataSourceFactory.Create(new float[] { 1, 0, 1, 0 }, new int[] { 2, 2, 1, 1 });

            var input = CNTKLib.InputVariable(new int[] { 2, 2, 1 }, DataType.Float);

            var convolutionMap = new Parameter(convData.ToNDArrayView());

            var f = CNTKLib.ConvolutionTranspose(convolutionMap, input, new int[] { 2, 2, 1 }, new BoolVector(new bool[] { true }), new BoolVector(new bool[] { true }), new int[] { 0 }, new int[] { 1 }, 1, 0, "");

            var inputs = new Dictionary<Variable, Value>() { { input, inputData.ToValue() } };
            var outputs = new Dictionary<Variable, Value>() { { f.Output, null } };

            f.Evaluate(inputs, outputs, DeviceDescriptor.UseDefaultDevice());
            var result = DataSourceFactory.FromValue(outputs[f.Output]);

            CollectionAssert.AreEqual(new int[] { 3, 3, 1, 1, 1 }, result.Shape.Dimensions);
            Assert.AreEqual(1, result.Data[0]);
            Assert.AreEqual(0, result.Data[1]);
            Assert.AreEqual(2, result.Data[2]);
            Assert.AreEqual(1, result.Data[3]);
        }

        [TestMethod]
        public void TestConvolutionTranspose2()
        {
            var inputData = DataSourceFactory.Create(new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new int[] { 2, 2, 3 });
            // var inputData = DataSourceFactory.Create(new float[] { 1, 2, 3, 4 }, new int[] { 2, 2, 1 });

            var input = CNTKLib.InputVariable(new int[] { 2, 2, 3 }, DataType.Float);

            var convolutionMap = new Parameter(new int[] { 2, 2, 6, 3 }, DataType.Float, CNTKLib.ConstantInitializer(1));

            var f = CNTKLib.ConvolutionTranspose(convolutionMap, input, new int[] { 2, 2, 3 }, new BoolVector(new bool[] { true }), new BoolVector(new bool[] { true }), new int[] { 0 }, new int[] { 1 }, 1, 0, "");

            var inputs = new Dictionary<Variable, Value>() { { input, inputData.ToValue() } };
            var outputs = new Dictionary<Variable, Value>() { { f.Output, null } };

            f.Evaluate(inputs, outputs, DeviceDescriptor.UseDefaultDevice());
            var result = DataSourceFactory.FromValue(outputs[f.Output]);

            CollectionAssert.AreEqual(new int[] { 3, 3, 6, 1, 1 }, result.Shape.Dimensions);
            Assert.AreEqual(15, result.Data[0]);
            Assert.AreEqual(15, result.Data[1]);
            Assert.AreEqual(18, result.Data[2]);
            Assert.AreEqual(15, result.Data[3]);
        }
    }
}
