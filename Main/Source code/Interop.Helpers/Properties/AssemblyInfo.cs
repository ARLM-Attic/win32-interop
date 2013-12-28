
using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;

[assembly: AssemblyTitle("Interop.VisualStudio.dll")]
[assembly: AssemblyDescription("Visual Studio API wrappers & helpers")]
[assembly: AssemblyProduct("Interop")]
[assembly: AssemblyCopyright("Copyright © Aleksandr Vishnyakov & Codeplex community 2013")]

[assembly: SecurityRules(SecurityRuleSet.Level2, SkipVerificationInFullTrust = true)]
[assembly: AllowPartiallyTrustedCallers]

[assembly: ComVisible(false)]
[assembly: CLSCompliant(false)]

#if NETFX4
[assembly: Guid("2AC0C66E-EFC4-45C1-B12C-754B9FB5E794")]
#elif NETFX45
[assembly: Guid("DA2B0FD7-A030-4CB6-85AA-D91D05F1E577")]
#endif

[assembly: NeutralResourcesLanguage("en-us")]

#if NETFX4
[assembly: AssemblyVersion("1.0.17.0")]
[assembly: AssemblyFileVersion("1.0.17.0")]
#elif NETFX45
[assembly: AssemblyVersion("1.0.23.0")]
[assembly: AssemblyFileVersion("1.0.23.0")]
#endif
[assembly: AssemblyInformationalVersion("1.0 EAP")]