using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Interop.Core
{
    [DebuggerNonUserCode]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Method | AttributeTargets.Field)]
    public class VersionAttribute : WarningAttribute
    {
        private static readonly NativeMethods.WINVER[] WindowsVersions =
        {
            NativeMethods.WINVER.None,
            NativeMethods.WINVER.WindowsXP,
            NativeMethods.WINVER.WindowsServer2003,
            NativeMethods.WINVER.WindowsXPSP2,
            NativeMethods.WINVER.WindowsServer2003SP1,
            NativeMethods.WINVER.WindowsVista,
            NativeMethods.WINVER.WindowsServer2008,
            NativeMethods.WINVER.Windows7,
            NativeMethods.WINVER.WindowsServer2008R2,
            NativeMethods.WINVER.Windows8,
            NativeMethods.WINVER.WindowsServer2012
        };

        private readonly HashSet<NativeMethods.WINVER> _declaredWindowsVersions;

        public VersionAttribute(string name, NativeMethods.WINVER minimum, NativeMethods.WINVER maximum)
        {
            Name = name;
            Minimum = minimum;
            Maximum = maximum;

            var possibleWindowsVersions = new HashSet<NativeMethods.WINVER>();
            _declaredWindowsVersions = new HashSet<NativeMethods.WINVER>();

            var minimumPosition = minimum != NativeMethods.WINVER.None ? Array.IndexOf(WindowsVersions, minimum) : 0;
            var maximumPosition = maximum != NativeMethods.WINVER.None ? Array.IndexOf(WindowsVersions, maximum) : WindowsVersions.Length - 1;
            for (var i = minimumPosition; i <= maximumPosition - minimumPosition + 1; i++)
            {
                possibleWindowsVersions.Add(WindowsVersions[i]);
            }

            Check();

            if (_declaredWindowsVersions.IsSubsetOf(possibleWindowsVersions))
            {
                Ignored = true;
            }

            Initialize(name + " not available on " + string.Join(", ", _declaredWindowsVersions.Except(possibleWindowsVersions).Select(NativeMethods.ToString)));
        }

        public string Name { get; private set; }

        public NativeMethods.WINVER Minimum { get; private set; }

        public NativeMethods.WINVER Maximum { get; private set; }

        private void Check()
        {
            CheckWindowsXP();
            CheckWindowsServer2003();
            CheckWindowsXPSP2();
            CheckWindowsServer2003SP1();
            CheckWindowsVista();
            CheckWindowsServer2008();
            CheckWindows7();
            CheckWindowsServer2008R2();
            CheckWindows8();
            CheckWindowsServer2012();
        }

        [Conditional("WINXP")]
        private void CheckWindowsXP()
        {
            _declaredWindowsVersions.Add(NativeMethods.WINVER.WindowsXP);
        }

        [Conditional("WS2003")]
        private void CheckWindowsServer2003()
        {
            _declaredWindowsVersions.Add(NativeMethods.WINVER.WindowsServer2003);
        }

        [Conditional("WINXPSP2")]
        private void CheckWindowsXPSP2()
        {
            _declaredWindowsVersions.Add(NativeMethods.WINVER.WindowsXPSP2);
        }

        [Conditional("WS2003SP1")]
        private void CheckWindowsServer2003SP1()
        {
            _declaredWindowsVersions.Add(NativeMethods.WINVER.WindowsServer2003SP1);
        }

        [Conditional("WINVISTA")]
        private void CheckWindowsVista()
        {
            _declaredWindowsVersions.Add(NativeMethods.WINVER.WindowsVista);
        }

        [Conditional("WS2008")]
        private void CheckWindowsServer2008()
        {
            _declaredWindowsVersions.Add(NativeMethods.WINVER.WindowsServer2008);
        }

        [Conditional("WIN7")]
        private void CheckWindows7()
        {
            _declaredWindowsVersions.Add(NativeMethods.WINVER.Windows7);
        }

        [Conditional("WS2008R2")]
        private void CheckWindowsServer2008R2()
        {
            _declaredWindowsVersions.Add(NativeMethods.WINVER.WindowsServer2008R2);
        }

        [Conditional("WIN8")]
        private void CheckWindows8()
        {
            _declaredWindowsVersions.Add(NativeMethods.WINVER.Windows8);
        }

        [Conditional("WS2012")]
        private void CheckWindowsServer2012()
        {
            _declaredWindowsVersions.Add(NativeMethods.WINVER.WindowsServer2012);
        }
    }
}