using System.Runtime.InteropServices;

using JetBrains.Annotations;

namespace Interop.VisualStudio.COM
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [Guid(IIDs.IID_IPropertyStore)]
    public interface IPropertyStore
    {
        [CanBeNull]
        object GetProperty(bool perUser, [NotNull] string configName, [NotNull] string propertyName, [CanBeNull] object defaultValue);

        [CanBeNull]
        object GetProperties(bool perUser, [NotNull] string[] configNames, [NotNull] string propertyName, [CanBeNull] object defaultValue);

        void SetProperty(bool perUser, [NotNull] string configName, [NotNull] string propertyName, [CanBeNull] object value);

        void SetProperties(bool perUser, [NotNull] string[] configNames, [NotNull] string propertyName, [CanBeNull] object value);

        void RemoveProperty(bool perUser, [NotNull] string configName, [NotNull] string propertyName);

        void RemoveProperties(bool perUser, [NotNull] string[] configNames, [NotNull] string propertyName);
    }
}