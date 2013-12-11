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
            int CreateInstance(IntPtr pUnkOuter, ref Guid riid, out IntPtr ppvObject);

            [MethodImpl(MethodImplOptions.PreserveSig)]
            int LockServer(bool fLock);
        }
    }
}