using System;
using System.Drawing;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

namespace Interop.VisualStudio.COM
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [Guid(IIDs.IID_IPropertyView)]
    public interface IPropertyView
    {
        [NotNull]
        string Title { get; }

        [CanBeNull]
        string Description { get; }

        [CanBeNull]
        string HelpKeyword { get; }

        [CanBeNull]
        string HelpFile { get; }

        int HelpContext { get; }

        IntPtr Handle { get; }

        Size Size { get; set; }

        Point Location { get; set; }

        void Initialize([NotNull] IPropertyViewSite host);

        void LoadProperties([NotNull] string[] configNames, [NotNull] IPropertyStore storage);

        void SaveProperties([NotNull] string[] configNames, [NotNull] IPropertyStore storage);

        void Show();

        void Hide();
    }
}