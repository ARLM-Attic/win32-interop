using System;
using System.Runtime.InteropServices;

using Interop.Helpers;

// ReSharper disable once CheckNamespace
namespace Interop.Core
{
    // ReSharper disable InconsistentNaming
    public static partial class UnsafeWrappers
    {
        public static object CoCreateInstance(Guid rclsid, object pUnkOuter, NativeMethods.CLSCTX dwClsContext, Guid riid)
        {
            object ppv;
            ErrorHelper.ThrowIfNotZero(UnsafeNativeMethods.CoCreateInstance(rclsid, pUnkOuter, dwClsContext, riid, out ppv));
            return ppv;
        }

        public static int CoRegisterClassObject(Guid rclsid, object pUnk, NativeMethods.CLSCTX dwClsContext, NativeMethods.REGCLS flags)
        {
            int lpdwRegister;
            ErrorHelper.ThrowIfNotZero(UnsafeNativeMethods.CoRegisterClassObject(rclsid, pUnk, dwClsContext, flags, out lpdwRegister));
            return lpdwRegister;
        }

        public static void CoRevokeClassObject(int dwRegister)
        {
            ErrorHelper.ThrowIfNotZero(UnsafeNativeMethods.CoRevokeClassObject(dwRegister));
        }
    }
}