
using System;
using System.Security;

using Interop.Helpers;

// ReSharper disable once CheckNamespace
namespace Interop.Core
{
    // ReSharper disable InconsistentNaming
    public static partial class UnsafeWrappers
    {
        [SecurityCritical]
        public static NativeMethods.WindowMessage RegisterWindowMessage(string lpString)
        {
            return ErrorHelper.ThrowIfZero(UnsafeNativeMethods.RegisterWindowMessage(lpString));
        }

        [SecurityCritical]
        public static IntPtr SetParent(IntPtr hWnd, IntPtr hWndParent)
        {
            return ErrorHelper.ThrowIfZero(UnsafeNativeMethods.SetParent(hWnd, hWndParent));
        }
    }
}