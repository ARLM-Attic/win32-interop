using System;
using System.Globalization;
#if !SILVERLIGHT
using System.ComponentModel;
#else
using System.Runtime.InteropServices;
#endif
using System.Security;

namespace Interop.Core.Helpers
{
    internal static class ErrorHelper
    {
        [SecuritySafeCritical]
        internal static IntPtr ThrowIfZero(IntPtr result)
        {
            if (result == IntPtr.Zero)
            {
#if SILVERLIGHT
                throw new ExternalException("Unknown error (0x" + Convert.ToString(Marshal.GetLastWin32Error(), 16) + ")");
#else
                throw new Win32Exception();
#endif
            }
            return result;
        }

        [SecuritySafeCritical]
        internal static T ThrowIfZero<T>(T result)
        {
            if (Convert.ToInt32(result, CultureInfo.InvariantCulture) == 0)
            {
#if SILVERLIGHT
                throw new ExternalException("Unknown error (0x" + Convert.ToString(Marshal.GetLastWin32Error(), 16) + ")");
#else
                throw new Win32Exception();
#endif
            }
            return result;
        }
    }
}