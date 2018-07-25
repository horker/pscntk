using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Horker.PSCNTK;
using System.Management.Automation;
using System.Linq;

namespace UnitTest
{
    /*
    [TestClass]
    public class FunctionMethodsTest
    {
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
}
