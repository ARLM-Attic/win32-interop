using System.Runtime.InteropServices;

namespace Interop.VisualStudio.COM
{
    [Guid("5B1F894B-4680-4E63-8186-80E3F92060D8")]
    public interface IPropertyStore
    {
        object GetProperty(bool perUser, string configName, string propertyName, object defaultValue);

        object GetProperties(bool perUser, string[] configNames, string propertyName, object defaultValue);

        void SetProperty(bool perUser, string configName, string propertyName, object value);

        void SetProperties(bool perUser, string[] configNames, string propertyName, object value);

        void RemoveProperty(bool perUser, string configName, string propertyName);

        void RemoveProperties(bool perUser, string[] configNames, string propertyName);
    }
}