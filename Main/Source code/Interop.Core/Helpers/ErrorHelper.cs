using System;
#if !SILVERLIGHT
using System.ComponentModel;
#endif
using System.Diagnostics.Contracts;
#if SILVERLIGHT
using System.Runtime.InteropServices;
#endif
using System.Security;

namespace Interop.Core.Helpers
{
    internal static class ErrorHelper
    {
#if SILVERLIGHT
        private const string UnknownError = "Unknown error (0x{0})";
#endif

        [SecuritySafeCritical]
        internal static IntPtr ThrowIfZero(IntPtr result)
        {
            Contract.Ensures(Contract.Result<IntPtr>() != IntPtr.Zero);

            if (result == IntPtr.Zero)
            {
#if SILVERLIGHT
                throw new ExternalException(string.Format(UnknownError, Convert.ToString(Marshal.GetLastWin32Error(), 16)));
#else
                throw new Win32Exception();
#endif
            }
            return result;
        }

        [SecuritySafeCritical]
        internal static int ThrowIfZero(int result)
        {
            Contract.Ensures(Contract.Result<int>() != 0);

            if (result == 0)
            {
#if SILVERLIGHT
                throw new ExternalException(string.Format(UnknownError, Convert.ToString(Marshal.GetLastWin32Error(), 16)));
#else
                throw new Win32Exception();
#endif
            }
            return result;
        }

        [SecuritySafeCritical]
        internal static long ThrowIfZero(long result)
        {
            Contract.Ensures(Contract.Result<long>() != 0L);

            if (result == 0L)
            {
#if SILVERLIGHT
                throw new ExternalException(string.Format(UnknownError, Convert.ToString(Marshal.GetLastWin32Error(), 16)));
#else
                throw new Win32Exception();
#endif
            }
            return result;
        }

        [SecuritySafeCritical]
        internal static NativeMethods.WindowMessage ThrowIfZero(NativeMethods.WindowMessage result)
        {
            Contract.Ensures(Contract.Result<NativeMethods.WindowMessage>() != 0);
            
            if (result == 0)
            {
#if SILVERLIGHT
                throw new ExternalException(string.Format(UnknownError, Convert.ToString(Marshal.GetLastWin32Error(), 16)));
#else
                throw new Win32Exception();
#endif
            }
            return result;
        }
    }
}