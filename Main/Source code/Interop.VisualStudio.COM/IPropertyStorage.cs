using System.Runtime.InteropServices;

namespace Interop.VisualStudio
{
    [Guid("5B1F894B-4680-4E63-8186-80E3F92060D8")]
    public interface IPropertyStorage
    {
        string GetProperty(bool perUser, string configName, string propertyName, string defaultValue);

        string GetProperties(bool perUser, string[] configNames, string propertyName, string defaultValue);

        void SetProperty(bool perUser, string configName, string propertyName, string value);

        void SetProperties(bool perUser, string[] configNames, string propertyName, string value);

        void RemoveProperty(bool perUser, string configName, string propertyName);

        void RemoveProperties(bool perUser, string[] configNames, string propertyName);
    }
}