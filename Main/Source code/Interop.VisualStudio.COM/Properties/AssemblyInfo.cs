
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

[assembly: AssemblyTitle("Interop.VisualStudio.COM.dll")]
[assembly: AssemblyDescription("COM exportable API for Visual Studio API wrappers & helpers")]
[assembly: AssemblyProduct("Interop")]
[assembly: AssemblyCopyright("Copyright © Aleksandr Vishnyakov & Codeplex community 2013")]

[assembly: SecurityRules(SecurityRuleSet.Level2, SkipVerificationInFullTrust = true)]
[assembly: AllowPartiallyTrustedCallers]

[assembly: ComVisible(true)]
[assembly: CLSCompliant(true)]

#if NETFX4
[assembly: Guid("0FD2A011-F544-45D9-8C32-AA91A80EB89A")]
#elif NETFX45
[assembly: Guid("CA4FC603-149E-445F-A7B5-C58B37E6647F")]
#endif

#if NETFX4
[assembly: AssemblyVersion("1.0.4.0")]
[assembly: AssemblyFileVersion("1.0.4.0")]
#elif NETFX45
[assembly: AssemblyVersion("1.0.7.0")]
[assembly: AssemblyFileVersion("1.0.7.0")]
#endif
[assembly: AssemblyInformationalVersion("1.0 EAP")]