using System;
using System.Runtime.InteropServices;
using System.Security;
#if !SILVERLIGHT
using System.Security.Permissions;
#endif

namespace Interop.Core
{
    [SecurityCritical]
#if !SILVERLIGHT
    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
#endif
    public static partial class UnsafeNativeMethods
    {
// ReSharper disable InconsistentNaming
        [DllImport(NativeMethods.ExternDll.Kernel32, CharSet = CharSet.Unicode, BestFitMapping = false,
#if !SILVERLIGHT
            ThrowOnUnmappableChar = true,
#endif
            SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport(NativeMethods.ExternDll.Kernel32, ExactSpelling = true, CharSet = CharSet.Ansi, BestFitMapping = false, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport(NativeMethods.ExternDll.User32, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.SysInt)]
        public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, [MarshalAs(UnmanagedType.U4)] NativeMethods.WindowMessage Msg, [MarshalAs(UnmanagedType.SysUInt)] IntPtr wParam, [MarshalAs(UnmanagedType.SysInt)] IntPtr lParam);

// ReSharper restore InconsistentNaming
    }
}