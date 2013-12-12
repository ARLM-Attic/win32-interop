using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
namespace Interop.Core
{
    // ReSharper disable InconsistentNaming
    public static partial class UnsafeNativeMethods
    {
        [ComImport]
        [Guid(NativeMethods.IIDs.IID_IClassFactory)]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IClassFactory
        {
            [MethodImpl(MethodImplOptions.PreserveSig)]
            [return: MarshalAs(UnmanagedType.Error)]
            NativeMethods.HResult CreateInstance([MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObject);

            [MethodImpl(MethodImplOptions.PreserveSig)]
            [return: MarshalAs(UnmanagedType.Error)]
            NativeMethods.HResult LockServer([MarshalAs(UnmanagedType.Bool)] bool fLock);
        }
    }
}