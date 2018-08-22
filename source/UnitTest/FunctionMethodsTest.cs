using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CNTK;
using Horker.PSCNTK;

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

        /*
        [TestMethod]
        public void TestInvoke()
        {
            var input = CNTK.Variable.InputVariable(new int[] { 1 }, CNTK.DataType.Float, "input");
            var f = CNTK.CNTKLib.ReduceSum(input, CNTK.Axis.AllStaticAxes());

            var obj = new PSObject(f);

            var output = FunctionMethods.Invoke(
                new PSObject(f),
                new Hashtable() { { "input", new DataSource<float>(new float[] { 1, 3, 5 }, new int[] { 3 }).ToValue() } }
            );

            var result = DataSource<float>.FromValue(output);

            CollectionAssert.AreEqual(new int[] { 1 }, result.Shape.Dimensions);
            CollectionAssert.AreEqual(new float[] { 9 }, result.Data.ToArray());
        }
    }
    */

        [TestMethod]
        public void TestFind()
        {
            var input = Variable.InputVariable(new int[] { 1, 2 }, CNTK.DataType.Float, "input");
            var f = Composite.Dense(input, new int[] { 5 }, null, true, null, "sigmoid", "TestFind");

            var obj = new PSObject(f);

            var result = FunctionMethods.Find(new PSObject(f), "TestFind_w");

            Assert.AreEqual("TestFind_w", ((Variable)result).Name);
        }
    }
}
