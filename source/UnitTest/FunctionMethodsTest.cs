using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CNTK;
using Horker.PSCNTK;
using System.Collections;

namespace UnitTest
{
    [TestClass]
    public class FunctionMethodsTest
    {
        public FunctionMethodsTest()
        {
            var path = Environment.ExpandEnvironmentVariables(@"..\..\..\..\lib");
            UnmanagedDllLoader.Load(path);
        }

        [TestMethod]
        public void TestInvoke()
        {
            var input = CNTK.Variable.InputVariable(new int[] { 3 }, CNTK.DataType.Float, "input");
            var f = CNTK.CNTKLib.ReduceSum(input, CNTK.Axis.AllStaticAxes());

            var obj = new PSObject(f);

            var output = FunctionMethods.Invoke(
                new PSObject(f),
                new Hashtable() { { "input", new DataSource<float>(new float[] { 1, 3, 5 }, new int[] { 3 }).ToValue() } }
            );

            var result = DataSource<float>.FromValue(output[0]);

            CollectionAssert.AreEqual(new int[] { 1, 1 }, result.Shape.Dimensions);
            CollectionAssert.AreEqual(new float[] { 9 }, result.Data.ToArray());
        }

        [TestMethod]
        public void TestInvoke2()
        {
            var input = CNTK.Variable.InputVariable(new int[] { 2, 2 }, CNTK.DataType.Float, "input");
            var f = CNTK.CNTKLib.ReduceSum(input, CNTK.Axis.AllStaticAxes());

            var obj = new PSObject(f);

            var output = FunctionMethods.Invoke(
                new PSObject(f),
                new Hashtable() { { "input", new object[] { new PSObject(1), 3, 5, 9 } } }
            );

            var result = DataSource<float>.FromValue(output[0]);

            CollectionAssert.AreEqual(new int[] { 1, 1 }, result.Shape.Dimensions);
            CollectionAssert.AreEqual(new float[] { 1 + 3 + 5 + 9 }, result.Data.ToArray());
        }

        [TestMethod]
        public void TestFind()
        {
            var input = Variable.InputVariable(new int[] { 1, 2 }, CNTK.DataType.Float, "input");
            var f = Composite.Dense(input, new int[] { 5 }, null, true, null, "sigmoid", "TestFind");

            var obj = new PSObject(f);

            var result = FunctionMethods.Find(new PSObject(f), "TestFind_w");

            Assert.AreEqual("TestFind_w", ((Variable)result).Name);
        }

        [TestMethod]
        public void TestAsTree()
        {
            var input = Variable.InputVariable(new int[] { 2 }, CNTK.DataType.Float, "input");
            var f = Composite.Dense(input, new int[] { 5 }, CNTKLib.ConstantInitializer(0.0), true, null, null, "test");

            var obj = new PSObject(f);

            var result = FunctionMethods.AsTree(obj, new Hashtable() { { input, new float[] { 1, 2 } } }, null, false, true);

            var expected =
                "0 CompositeFunctionOpName \r\n" +
                "    -> [5 x 1 x 1] [ [ [0 0 0 0 0] ] ]\r\n" +
                "  1 Plus <test>\r\n" +
                "      -> [5 x 1 x 1] [ [ [0 0 0 0 0] ] ]\r\n" +
                "    2 @Output [5]\r\n" +
                "      3 Times \r\n" +
                "          -> [5 x 1 x 1] [ [ [0 0 0 0 0] ] ]\r\n" +
                "        4 @Parameter [5 x 2] <test_w>\r\n" +
                "            -> [5 x 2] [0 0 0 0 0...]\r\n" +
                "        4 @Input [2] <input>\r\n" +
                "    2 @Parameter [5] <test_b>\r\n" +
                "        -> [5] [0 0 0 0 0]\r\n";

            Assert.AreEqual(expected, result);
        }
    }
}
