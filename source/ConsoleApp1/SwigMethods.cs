using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CNTK;

namespace Horker.PSCNTK
{
    public class SwigMethods
    {
        public static IntPtr GetSwigPointerAddress<T>(T obj)
        {
            var f = typeof(T).GetMember("swigCPtr", MemberTypes.Field, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
            return (IntPtr)(HandleRef)((FieldInfo)(f[0])).GetValue(obj);
        }

        // std::shared_ptr memory layout
        // -----------------------------
        //
        // In xatomic0.h:
        //
        // typedef unsigned long _Uint4_t;
        // typedef _Uint4_t _Atomic_integral_t
        // typedef _Atomic_integral_t _Atomic_counter_t;
        //   :
        //
        // In memory.h:
        //
        // struct _Ref_count_base {
        //   _Atomic_counter_t _Uses;
        //   _Atomic_counter_t _Weaks;
        //   :
        //
        // class _Ptr_base {
        // private:
        //   element_type * _Ptr{nullptr};
        //   _Ref_count_base * _Rep{nullptr};
        //   :
        //
        // class shared_ptr : public _Ptr_base
        //   :

        public static IntPtr GetSharedPtrElementPointer<T>(T obj)
        {
            var pSharedPtr = GetSwigPointerAddress(obj);

            unsafe
            {
                return *(IntPtr*)pSharedPtr;
            }
        }

        public static int GetSharedPtrUseCount<T>(T obj)
        {
            var pSharedPtr = GetSwigPointerAddress(obj);

            int count;
            unsafe
            {
                var p = (IntPtr**)pSharedPtr;
                var pRefCountBase = *(p + 1);
                count = *(int*)(pRefCountBase + 1);
            }

            return count;
        }

        public static void AddSharedPtrUseCount<T>(T obj)
        {
            // Warning: NOT ATOMIC

            var pSharedPtr = GetSwigPointerAddress(obj);

            unsafe
            {
                var p = (IntPtr**)pSharedPtr;
                var pRefCountBase = *(p + 1);
                var pCount = (int*)(pRefCountBase + 1);
                *pCount = *pCount + 1;
            }
        }
    }
}
