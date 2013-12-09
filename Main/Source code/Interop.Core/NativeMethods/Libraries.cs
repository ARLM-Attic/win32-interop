using JetBrains.Annotations;

namespace Interop.Core
{
    public static partial class NativeMethods
    {
        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
        public static class ExternDll
        {
            public const string Kernel = "kernel32.dll";
            public const string User32 = "user32.dll";
            public const string GDI32 = "user32.dll";
            public const string DWMAPI = "dwmapi.dll";
            public const string Shell32 = "shell32.dll";
        }
    }
}