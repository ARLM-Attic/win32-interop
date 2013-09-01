using System.Security;

using Interop.Core.Helpers;

namespace Interop.Core
{
    public static partial class UnsafeWrappers
    {
        [SecurityCritical]
        public static NativeMethods.WindowMessage RegisterWindowMessage(string lpString)
        {
            return ErrorHelper.ThrowIfZero(UnsafeNativeMethods.RegisterWindowMessage(lpString));
        }
    }
}