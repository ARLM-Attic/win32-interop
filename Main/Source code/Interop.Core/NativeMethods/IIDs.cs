using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace Interop.Core
{
    // ReSharper disable InconsistentNaming
    public static partial class NativeMethods
    {
        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
        public static class IIDs
        {
            public const string IID_IUnknown = "00000000-0000-0000-C000-000000000046";
            public const string IID_IClassFactory = "00000001-0000-0000-C000-000000000046";
            public const string IID_IPropertyPage = "B196B28D-BAB4-101A-B69C-00AA00341D07";
        }
    }
}