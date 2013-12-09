using System;
using System.Security;

using Interop.Helpers;

namespace Interop.Core
{
    public static partial class UnsafeWrappers
    {
        [SecurityCritical]
        public static IntPtr GetModuleHandle(string lpModuleName)
        {
            return ErrorHelper.ThrowIfZero(UnsafeNativeMethods.GetModuleHandle(lpModuleName));
        }

        [SecurityCritical]
        public static IntPtr GetProcAddress(IntPtr hModule, string lpProcName)
        {
            return ErrorHelper.ThrowIfZero(UnsafeNativeMethods.GetProcAddress(hModule, lpProcName));
        }
    }
}