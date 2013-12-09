using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace Interop.Core
{
    // ReSharper disable InconsistentNaming
    public static partial class NativeMethods
    {
        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
        public enum ScrollBoxMessage
        {
            SBM_SETPOS = 0x00E0,

            SBM_GETPOS = 0x00E1,

            SBM_SETRANGE = 0x00E2,

            SBM_GETRANGE = 0x00E3,

            SBM_ENABLE_ARROWS = 0x00E4,

            //// Not defined 0x00E5

            SBM_SETRANGEREDRAW = 0x00E6,

            //// Not defined 0x00E7

            //// Not defined 0x00E8

            SBM_SETSCROLLINFO = 0x00E9,

            SBM_GETSCROLLINFO = 0x00EA,

            SBM_GETSCROLLBARINFO = 0x00EB,
        }
    }
}