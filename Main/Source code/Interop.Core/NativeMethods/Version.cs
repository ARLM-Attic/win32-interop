using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Interop.Core
{
    // ReSharper disable InconsistentNaming
    public static partial class NativeMethods
    {
        private static readonly Dictionary<WINVER, string> WindowsVersions = new Dictionary<WINVER, string>
        {
            { WINVER.WindowsXP, "Windows XP" },
            { WINVER.WindowsXPSP2, "Windows XP SP2" },
            { WINVER.WindowsServer2003, "Windows Server 2003" },
            { WINVER.WindowsServer2003SP1, "Windows Server 2003 SP1" },
            { WINVER.WindowsVista, "Windows Vista" },
            { WINVER.WindowsServer2008, "Windows Server 2008" },
            { WINVER.Windows7, "Windows 7" },
            { WINVER.WindowsServer2008R2, "Windows Server 2008 R2" },
            { WINVER.Windows8, "Windows 8" },
            { WINVER.WindowsServer2012, "Windows Server 2012" }
        };

        public enum WINVER
        {
            None = 0,
            WindowsXP = 0x0501,
            WindowsXPSP2 = 0x0502,
            WindowsServer2003 = 0x0501,
            WindowsServer2003SP1 = 0x0502,
            WindowsVista = 0x0600,
            WindowsServer2008 = 0x0600,
            Windows7 = 0x0601,
            WindowsServer2008R2 = 0x0601,
            Windows8 = 0x0602,
            WindowsServer2012 = 0x0602
        }

        public static string ToString(this WINVER winver)
        {
            return WindowsVersions[winver];
        }
    }
}