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
        public NDArrayViewTest()
        {
            UnmanagedDllLoader.Load(@"..\..\..\..\lib");
            DeviceDescriptor.TrySetDefaultDevice(DeviceDescriptor.CPUDevice);
        }

        [TestMethod]
        public void TestPrematureMemoryRelease()
        {
            for (var i = 0; i < 1000; ++i)
            {
                var a = new NDArrayView(new int[] { 3, 2 }, new float[] { 1, 2, 3, 4, 5, 6 }, DeviceDescriptor.CPUDevice);
                var a2 = a.DeepClone();

                GC.Collect();

                var ds = DataSourceFactory.FromValue(new Value(a2));
                Assert.AreEqual(6, ds.Data.Count);
                Assert.AreEqual(1, ds.Data[0]);
            }
        }
    }
}
