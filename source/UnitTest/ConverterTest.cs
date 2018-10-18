using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using CNTK;
using Horker.PSCNTK;
using System.Collections;

namespace UnitTest
{
    [TestClass]
    public class ConverterTest
    {
        public ConverterTest()
        {
            UnmanagedDllLoader.Load(@"..\..\..\..\lib");
            DeviceDescriptor.TrySetDefaultDevice(DeviceDescriptor.CPUDevice);
        }

        [TestMethod]
        public void TestHashtableToDictionary()
        {
            var v1 = Variable.InputVariable(new int[] { 1, 2 }, DataType.Float);
            var v2 = Variable.InputVariable(new int[] { 1, 2 }, DataType.Float);
            var ht = new Hashtable();
            ht.Add(v1, v2);

            var d = Converter.HashtableToDictionary<Variable, Variable>(ht);

            Assert.AreEqual(v2, d[v1]);
        }
    }
}
