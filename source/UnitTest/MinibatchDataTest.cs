using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Horker.PSCNTK;
using System.Management.Automation;
using CNTK;
using System.Runtime.InteropServices;

namespace UnitTest
{
    [TestClass]
    public class MinibatchDataTest
    {
        public MinibatchDataTest()
        {
            UnmanagedDllLoader.Load(@"..\..\..\..\lib");
            DeviceDescriptor.TrySetDefaultDevice(DeviceDescriptor.CPUDevice);
        }

        [TestMethod]
        public void TestDataIngegrityAfterGarbageCollection()
        {
            for (var i = 0; i < 100; ++i)
            {
                var data = new float[] { 1, 2, 3, 4, 5, 6 };

                MinibatchData m;
                IntPtr sharedPtrAddress;
                IntPtr valueAddress;
                int c1;
                unsafe
                {
                    fixed (float* d = data)
                    {
                        var a = new NDArrayView(new int[] { 3, 2 }, data, DeviceDescriptor.CPUDevice);
                        var a2 = a.DeepClone();
                        var value = new Value(a2);
                        c1 = SwigMethods.GetSharedPtrUseCount(value);
                        sharedPtrAddress = SwigMethods.GetSwigPointerAddress(value);
                        valueAddress = SwigMethods.GetSharedPtrElementPointer(value);

                        m = new MinibatchData(value);
                    }
                }

                var c2 = SwigMethods.GetSharedPtrUseCount(m.data);
                var sharedPtrAddress2 = SwigMethods.GetSwigPointerAddress(m.data);
                var valueAddress2 = SwigMethods.GetSharedPtrElementPointer(m.data);
                var c3 = SwigMethods.GetSharedPtrUseCount(m.data);

                GC.Collect();
                GC.Collect();
                GC.Collect();

                var c4 = SwigMethods.GetSharedPtrUseCount(m.data);
                var sharedPtrAddress3 = SwigMethods.GetSwigPointerAddress(m.data);
                var valueAddress3 = SwigMethods.GetSharedPtrElementPointer(m.data);

                var ds = DataSourceFactory.FromValue(m.data);
                Assert.AreEqual(6, ds.Data.Count);
                CollectionAssert.AreEqual(new int[] { 3, 2 }, ds.Shape.Dimensions);
                CollectionAssert.AreEqual(new float[] { 1, 2, 3, 4, 5, 6 }, ds.TypedData);
            }
        }
    }
}
