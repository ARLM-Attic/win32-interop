using System;
using System.Diagnostics;

namespace Interop.Core
{
    [DebuggerNonUserCode]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Method | AttributeTargets.Field)]
    public class UndocumentedFeatureAttribute : WarningAttribute
    {
        private const string UndocumentedFeature = "Undocumented feature";

        public UndocumentedFeatureAttribute()
        {
            Check();
            Initialize(UndocumentedFeature);
        }

        [Conditional("UNDOCUMENTED")]
        private void Check()
        {
            Ignored = true;
        }
    }
}