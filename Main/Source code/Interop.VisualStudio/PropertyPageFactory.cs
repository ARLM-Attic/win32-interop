using System;
using System.Runtime.InteropServices;

using Interop.Core;
using Interop.VisualStudio.COM;

namespace Interop.VisualStudio
{
    internal class PropertyPageFactory : UnsafeNativeMethods.IClassFactory
    {
        private readonly Guid _clsid;

        public PropertyPageFactory(Guid clsid)
        {
            _clsid = clsid;
        }

        public NativeMethods.HResult CreateInstance(object pUnkOuter, Guid riid, out object ppvObject)
        {
            ppvObject = null;
            if (pUnkOuter != null)
            {
                Marshal.ThrowExceptionForHR((int)NativeMethods.HResult.CLASS_E_NOAGGREGATION);
            }
            else if (riid.ToString() == NativeMethods.IIDs.IID_IUnknown || riid.ToString() == NativeMethods.IIDs.IID_IPropertyPage)
            {
                var instance = UnsafeWrappers.CoCreateInstance(_clsid, null, NativeMethods.CLSCTX.CLSCTX_INPROC_SERVER, riid) as IPropertyView;
                if (instance == null)
                {
                    Marshal.ThrowExceptionForHR((int)NativeMethods.HResult.E_NOINTERFACE);
                }
                else
                {
                    ppvObject = new PropertyPage(instance);
                }
            }
            else
            {
                Marshal.ThrowExceptionForHR((int)NativeMethods.HResult.E_NOINTERFACE);
            }
            return NativeMethods.HResult.S_OK;
        }

        public NativeMethods.HResult LockServer(bool fLock)
        {
            return NativeMethods.HResult.S_OK;
        }
    }
}