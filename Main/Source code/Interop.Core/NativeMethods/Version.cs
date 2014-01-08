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
            { WINVER.WindowsServer2003, "Windows Server 2003" },
            { WINVER.WindowsVista, "Windows Vista / Windows Server 2008" },
            { WINVER.Windows7, "Windows 7 / Windows Server 2008 R2" },
            { WINVER.Windows8, "Windows 8 / Windows Server 2012" },
            { WINVER.Windows81, "Windows 8.1 / Windows Server 2012 R2" }
        };

        public enum WINVER
        {
            None = 0,
            WindowsXP = 0x0501,
            WindowsServer2003 = 0x0502,
            WindowsVista = 0x0600,
            Windows7 = 0x0601,
            Windows8 = 0x0602,
            Windows81 = 0x0603
        }

        public static string ToString(this WINVER winver)
        {
            return WindowsVersions[winver];
        }
    }
}