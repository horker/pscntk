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
/*
        [TestMethod]
        public void TestConvTrans()
        {
            // input:
            //  | 2 1 |
            //  | 4 4 |
            //
            // kernel:
            //  | 1 4 1 |
            //  | 1 4 3 |
            //  | 3 3 1 |
            //
            // output:
            //  |  2  9  6  1 |
            //  |  6 29 30  7 |
            //  | 10 29 33 13 |
            //  | 12 24 16  4 |

            Variable input = CNTKLib.InputVariable(new int[] { 3, 3, 1 }, DataType.Float);
            int[] filterShape = new int[] { 2, 2 };
            int numFilters = 1;
            string activation = null;
            CNTKDictionary initializer = CNTKLib.ConstantInitializer(0);
            bool[] padding = new bool[] { false };
            int[] strides = new int[] { 1, 1 };
            bool[] sharing = new bool[] { true };
            bool useBias = false;
            CNTKDictionary biasInitializer = null;
            int[] outputShape = new int[] { 0 };
            int[] dilation = new int[] { 1 };
            int reductionRank = 1;
            int maxTempMemSizeInSamples = 0;
            string name = "";

            var f = Composite.ConvolutionTranspose(input, filterShape, numFilters, activation, initializer, padding, strides, sharing, useBias, biasInitializer, outputShape, dilation, reductionRank, maxTempMemSizeInSamples, name);

            var inputValue = new DataSource<float>(new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, new int[] { 1, 3, 3 }).ToValue();
            var inputMap = new Dictionary<Variable, Value>()
            {
                { input, inputValue }
            };

            var outputMap = new Dictionary<Variable, Value>()
            {
                { f.Output, null }
            };

            f.Evaluate(inputMap, outputMap, DeviceDescriptor.UseDefaultDevice());

            var result = DataSource<float>.FromVariable(f.Output);
            CollectionAssert.AreEqual(new int[] { 1, 4, 6, 1 }, result.Shape.Dimensions);
        }
    */
    }
}
