using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Interop.VisualStudio.COM
{
    [Guid("B9F27534-9FD1-496A-B715-8F740BEC61C3")]
    public interface IPropertyView
    {
        string Title { get; }

        string Description { get; }

        string HelpKeyword { get; }

        string HelpFile { get; }

        int HelpContext { get; }

        IntPtr Handle { get; }

        Size Size { get; set; }

        Point Location { get; set; }

        void Initialize(IPropertyViewSite host);

        void LoadProperties(string[] configNames, IPropertyStore storage);

        void SaveProperties(string[] configNames, IPropertyStore storage);

        void Show();

        void Hide();
    }
}