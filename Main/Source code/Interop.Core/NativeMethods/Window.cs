using System;
using System.Runtime.InteropServices;

namespace Interop.Core
{
    public static partial class NativeMethods
    {
// ReSharper disable InconsistentNaming
        [return: MarshalAs(UnmanagedType.SysInt)]
        public delegate IntPtr WndProc(IntPtr hwnd, [MarshalAs(UnmanagedType.U4)] WindowMessage Msg, [MarshalAs(UnmanagedType.SysUInt)] IntPtr wParam, [MarshalAs(UnmanagedType.SysInt)] IntPtr lParam);

        public delegate IntPtr WndProcHook(IntPtr hwnd, WindowMessage Msg, IntPtr wParam, IntPtr lParam, ref bool handled);

// ReSharper restore InconsistentNaming
    }
}