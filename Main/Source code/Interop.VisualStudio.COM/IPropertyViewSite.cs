using System.Runtime.InteropServices;

namespace Interop.VisualStudio.COM
{
    [Guid(IIDs.IID_IPropertyViewSite)]
    public interface IPropertyViewSite
    {
        void PropertiesChanged();
    }
}