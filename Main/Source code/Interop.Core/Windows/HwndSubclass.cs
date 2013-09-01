using System;
using System.Security;

namespace Interop.Core.Windows
{
    public class HwndSubclass : IDisposable
    {
        #region Constants

        public const string DetachMessageName = "Interop.Core.Windows.HwndSubclass.DetachMessage";

        private const string DefWindowProcName = "DefWindowProcW";

        #endregion

        #region Constructors

        [SecurityCritical]
        static HwndSubclass()
        {
            DetachMessage = UnsafeWrappers.RegisterWindowMessage(DetachMessageName);
            var user32Module = UnsafeWrappers.GetModuleHandle(NativeMethods.ExternDll.User32);
            _defWndProc = UnsafeWrappers.GetProcAddress(user32Module, DefWindowProcName);
        }

        public HwndSubclass(NativeMethods.WndProcHook wndProcHook)
        {
            
        }

        #endregion

        #region Window Messages

        public static readonly NativeMethods.WindowMessage DetachMessage;

        [SecurityCritical]
        private static NativeMethods.WndProc _defWndProcStub = DefWndProcWrapper;

        [SecurityCritical]
        private static IntPtr _defWndProc;

        // Workaround for bug described in http://support.microsoft.com/kb/319740
        [SecurityCritical]
        private static IntPtr DefWndProcWrapper(IntPtr hwnd, NativeMethods.WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            return UnsafeNativeMethods.CallWindowProc(_defWndProc, hwnd, msg, wParam, lParam);
        }

        #endregion

        #region Destructors

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}