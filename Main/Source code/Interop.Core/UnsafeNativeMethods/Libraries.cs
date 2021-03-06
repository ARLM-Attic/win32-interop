﻿using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

// ReSharper disable once CheckNamespace
namespace Interop.Core
{
    // ReSharper disable InconsistentNaming
    [SecurityCritical]
    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    public static partial class UnsafeNativeMethods
    {
        [DllImport(NativeMethods.ExternDll.Kernel32, CharSet = CharSet.Unicode, BestFitMapping = false, ThrowOnUnmappableChar = true, SetLastError = true)]
        public static extern IntPtr GetModuleHandle([MarshalAs(UnmanagedType.LPWStr)] string lpModuleName);

        [DllImport(NativeMethods.ExternDll.Kernel32, ExactSpelling = true, CharSet = CharSet.Ansi, BestFitMapping = false, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] string lpProcName);

        [DllImport(NativeMethods.ExternDll.User32, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.SysInt)]
        public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, [MarshalAs(UnmanagedType.U4)] NativeMethods.WindowMessage Msg,
                                                   [MarshalAs(UnmanagedType.SysUInt)] IntPtr wParam, [MarshalAs(UnmanagedType.SysInt)] IntPtr lParam);
    }
}