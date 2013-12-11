using System;
using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
namespace Interop.Core
{
    // ReSharper disable InconsistentNaming
    public static partial class NativeMethods
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.SysInt)]
        public delegate IntPtr WndProc(IntPtr hwnd, [MarshalAs(UnmanagedType.U4)] WindowMessage Msg, [MarshalAs(UnmanagedType.SysUInt)] IntPtr wParam, [MarshalAs(UnmanagedType.SysInt)] IntPtr lParam);

        public delegate IntPtr WndProcHook(IntPtr hwnd, WindowMessage Msg, IntPtr wParam, IntPtr lParam, ref bool handled);
    }
}