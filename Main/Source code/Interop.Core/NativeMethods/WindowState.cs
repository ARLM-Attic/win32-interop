using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace Interop.Core
{
    // ReSharper disable InconsistentNaming
    public static partial class NativeMethods
    {
        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
        public enum WindowState
        {
            SW_HIDE = 0,

            SW_SHOW = 5,

            SW_SHOWNOACTIVATE = 4,

            SW_SHOWNA = 8,

            SW_SHOWDEFAULT = 10,

            SW_SHOWNORMAL = 1,

            SW_SHOWMINIMIZED = 2,

            SW_SHOWMINNOACTIVE = 7,

            SW_SHOWMAXIMIZED = 3,

            SW_NORMAL = 1,

            SW_MINIMIZE = 6,

            SW_MAXIMIZE = 3,

            SW_RESTORE = 9,

            SW_FORCEMINIMIZE = 11,

            SW_MAX = 11
        }
    }
}