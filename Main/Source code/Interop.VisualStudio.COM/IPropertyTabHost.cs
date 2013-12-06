using System.Runtime.InteropServices;

namespace Interop.VisualStudio
{
    [Guid("DD7DE377-7A48-40F6-9B91-BAE545856C97")]
    public interface IPropertyTabHost
    {
        void PropertiesChanged();
    }
}