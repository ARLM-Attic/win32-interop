
using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;

[assembly: AssemblyTitle("Interop.Core.Tests.dll")]
[assembly: AssemblyDescription("Unit tests for Interop.Core.dll")]
[assembly: AssemblyProduct("Interop")]
[assembly: AssemblyCopyright("Copyright © Aleksandr Vishnyakov & Codeplex community 2013")]

[assembly: SecurityRules(SecurityRuleSet.Level2)]
//[assembly: AllowPartiallyTrustedCallers]

[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]

[assembly: NeutralResourcesLanguage("en-us")]

#if NETFX4
[assembly: AssemblyVersion("1.0.23.0")]
[assembly: AssemblyFileVersion("1.0.23.0")]
#elif NETFX45
[assembly: AssemblyVersion("1.0.17.0")]
[assembly: AssemblyFileVersion("1.0.17.0")]
#endif
[assembly: AssemblyInformationalVersion("1.0 EAP")]