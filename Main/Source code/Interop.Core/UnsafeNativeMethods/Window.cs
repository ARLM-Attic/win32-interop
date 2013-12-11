using System;
using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
namespace Interop.Core
{
    // ReSharper disable InconsistentNaming
    public static partial class UnsafeNativeMethods
    {
        [DllImport(NativeMethods.ExternDll.User32, CharSet = CharSet.Unicode, BestFitMapping = false, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.U4)]
        public static extern NativeMethods.WindowMessage RegisterWindowMessage([MarshalAs(UnmanagedType.LPWStr)] string lpString);

        [DllImport(NativeMethods.ExternDll.User32, SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWnd, IntPtr hWndParent);
    }
}