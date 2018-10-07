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
        public void TestDataIngegrityAfterGarbageCollection()
        {
            for (var i = 0; i < 100; ++i)
            {
                NDArrayView a2;
                using (var a = new NDArrayView(new int[] { 3, 2 }, new float[] { 1, 2, 3, 4, 5, 6 }, DeviceDescriptor.CPUDevice))
                {
                    a2 = a.DeepClone();
                }

                GC.Collect();
                GC.Collect();
                GC.Collect();

                var value = new Value(a2);

                GC.Collect();
                GC.Collect();
                GC.Collect();

                var ds = DataSourceFactory.FromValue(value);
                Assert.AreEqual(6, ds.Data.Count);
                CollectionAssert.AreEqual(new int[] { 3, 2 }, ds.Shape.Dimensions);
                CollectionAssert.AreEqual(new float[] { 1, 2, 3, 4, 5, 6 }, ds.TypedData);
            }
        }
    }
}
