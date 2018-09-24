using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CNTK;

namespace Horker.PSCNTK
{
    public class PinnedAddressAllocator
    {
        [DllImport("kernel32.dll", SetLastError=false)]
        static extern IntPtr HeapAlloc(IntPtr hHeap, uint dwFlags, UIntPtr dwBytes);
        
        [DllImport("kernel32.dll", SetLastError=true)]
        static extern bool HeapFree(IntPtr hHeap, uint dwFlags, IntPtr lpMem);
/*
        public static float[] AllocateFloat(NDShape shape, float[] data, object owner)
        {
            NDArrayView result;
            unsafe
            {
                float* p = (float*)HeapAlloc((IntPtr)0, 0, (UIntPtr)(size * sizeof(float)));
                result = new NDArrayView
            }

        }
*/
    }
}
