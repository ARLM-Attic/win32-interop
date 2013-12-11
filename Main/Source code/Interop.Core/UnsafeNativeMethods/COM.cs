using System;
using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
namespace Interop.Core
{
    // ReSharper disable InconsistentNaming
    public static partial class UnsafeNativeMethods
    {
        [DllImport(NativeMethods.ExternDll.OLE32)]
        [return: MarshalAs(UnmanagedType.Error)]
        public static extern NativeMethods.HResult CoCreateInstance([In, MarshalAs(UnmanagedType.LPStruct)] Guid rclsid, [MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter,
                                                                    [MarshalAs(UnmanagedType.U4)] NativeMethods.CLSCTX dwClsContext,
                                                                    [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);

        [DllImport(NativeMethods.ExternDll.OLE32)]
        [return: MarshalAs(UnmanagedType.Error)]
        public static extern NativeMethods.HResult CoRegisterClassObject([In, MarshalAs(UnmanagedType.LPStruct)] Guid rclsid, [MarshalAs(UnmanagedType.IUnknown)] object pUnk,
                                                                         [MarshalAs(UnmanagedType.U4)] NativeMethods.CLSCTX dwClsContext, [MarshalAs(UnmanagedType.U4)] NativeMethods.REGCLS flags,
                                                                         [MarshalAs(UnmanagedType.U4)] out int lpdwRegister);

        [DllImport(NativeMethods.ExternDll.OLE32)]
        [return: MarshalAs(UnmanagedType.Error)]
        public static extern NativeMethods.HResult CoRevokeClassObject([MarshalAs(UnmanagedType.U4)] int dwRegister);
    }
}