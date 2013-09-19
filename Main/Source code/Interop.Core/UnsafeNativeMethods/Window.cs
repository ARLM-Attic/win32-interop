using System.Runtime.InteropServices;

namespace Interop.Core
{
    public static partial class UnsafeNativeMethods
    {
        [DllImport(NativeMethods.ExternDll.User32, CharSet = CharSet.Unicode, BestFitMapping = false, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.U4)]
        public static extern NativeMethods.WindowMessage RegisterWindowMessage([MarshalAs(UnmanagedType.LPWStr)] string lpString);
    }
}