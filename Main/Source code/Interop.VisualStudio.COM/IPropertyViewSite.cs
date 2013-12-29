using System.Runtime.InteropServices;

using JetBrains.Annotations;

namespace Interop.VisualStudio.COM
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [Guid(IIDs.IID_IPropertyViewSite)]
    public interface IPropertyViewSite
    {
        void PropertiesChanged();
    }
}