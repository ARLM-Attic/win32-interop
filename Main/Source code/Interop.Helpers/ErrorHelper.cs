using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Security;

namespace Interop.Helpers
{
    public static class ErrorHelper
    {
        [SecuritySafeCritical]
        public static IntPtr ThrowIfZero(IntPtr result)
        {
            Contract.Ensures(Contract.Result<IntPtr>() != IntPtr.Zero);

            if (result == IntPtr.Zero)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
            }
            return result;
        }

        [SecuritySafeCritical]
        public static T ThrowIfZero<T>(T result)
            where T : struct, IComparable
        {
            Contract.Ensures(Contract.Result<T>().CompareTo(0) != 0);
            
            if (result.CompareTo(0) == 0)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
            }
            return result;
        }

        [SecuritySafeCritical]
        public static void ThrowIfNotZero(IntPtr result)
        {
            if (result != IntPtr.Zero)
            {
                Marshal.ThrowExceptionForHR(result.ToInt32());
            }
        }

        [SecuritySafeCritical]
        public static void ThrowIfNotZero<T>(T result)
            where T : struct, IComparable
        {
            if (result.CompareTo(0) != 0)
            {
                Marshal.ThrowExceptionForHR(Convert.ToInt32(result));
            }
        }
    }
}