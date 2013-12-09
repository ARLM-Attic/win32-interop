using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Security;

namespace Interop.Core.Helpers
{
    internal static class ErrorHelper
    {
        [SecuritySafeCritical]
        internal static IntPtr ThrowIfZero(IntPtr result)
        {
            Contract.Ensures(Contract.Result<IntPtr>() != IntPtr.Zero);

            if (result == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            return result;
        }

        [SecuritySafeCritical]
        internal static int ThrowIfZero(int result)
        {
            Contract.Ensures(Contract.Result<int>() != 0);

            if (result == 0)
            {
                throw new Win32Exception();
            }
            return result;
        }

        [SecuritySafeCritical]
        internal static long ThrowIfZero(long result)
        {
            Contract.Ensures(Contract.Result<long>() != 0L);

            if (result == 0L)
            {
                throw new Win32Exception();
            }
            return result;
        }

        [SecuritySafeCritical]
        internal static NativeMethods.WindowMessage ThrowIfZero(NativeMethods.WindowMessage result)
        {
            Contract.Ensures(Contract.Result<NativeMethods.WindowMessage>() != 0);
            
            if (result == 0)
            {
                throw new Win32Exception();
            }
            return result;
        }
    }
}