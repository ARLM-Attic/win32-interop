
using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;

[assembly: AssemblyTitle("Interop.Core.Tests.dll")]
[assembly: AssemblyDescription("Unit tests for Interop.Core.dll")]
[assembly: AssemblyProduct("Interop")]
[assembly: AssemblyCopyright("Copyright © Aleksandr Vishnyakov & Codeplex community 2013")]

#if !SILVERLIGHT
[assembly: SecurityRules(SecurityRuleSet.Level2)]
#endif
//[assembly: AllowPartiallyTrustedCallers]

[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]

[assembly: NeutralResourcesLanguage("en-us")]

#if NETFX4
[assembly: AssemblyVersion("1.0.7.0")]
[assembly: AssemblyFileVersion("1.0.7.0")]
#elif NETFX45
[assembly: AssemblyVersion("1.0.5.0")]
[assembly: AssemblyFileVersion("1.0.5.0")]
#elif SL5
[assembly: AssemblyVersion("1.0.9.0")]
[assembly: AssemblyFileVersion("1.0.9.0")]
#endif
[assembly: AssemblyInformationalVersion("1.0 EAP")]