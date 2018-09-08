using Microsoft.VisualStudio.TestTools.UnitTesting;
using Horker.PSCNTK;
using CNTK;

namespace UnitTest
{
    [TestClass]
    public class WrapperTest
    {
        [TestMethod]
        public void TestOperator()
        {
            var term1 = new WrappedFunction(CNTKLib.Log(new WrappedVariable(Constant.Scalar(DataType.Float, 3.0))));
            var term2 = new WrappedVariable(Variable.InputVariable(new int[] { 2, 2 }, DataType.Float));
            var exp = term1 + term2;

            Assert.IsTrue(exp is WrappedFunction);
            Assert.AreEqual(((Function)exp).RootFunction.OpName, "Plus");
        }
    }
}
