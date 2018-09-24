using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Horker.PSCNTK;
using System.Management.Automation;
using CNTK;

namespace UnitTest
{
    [TestClass]
    public class NDArrayViewTest
    {
        [TestMethod]
        public void TestPrematureMemoryRelease()
        {
            for (var i = 0; i < 1000; ++i)
            {
                var a = new NDArrayView(new int[] { 3, 2 }, new float[] { 1, 2, 3, 4, 5, 6 }, DeviceDescriptor.CPUDevice);
                var a2 = a.DeepClone();

                GC.Collect();

                var ds = DataSource<float>.FromValue(new Value(a2));
                Assert.AreEqual(6, ds.Data.Length);
                Assert.AreEqual(1, ds.Data[0]);
            }
        }
    }
}
