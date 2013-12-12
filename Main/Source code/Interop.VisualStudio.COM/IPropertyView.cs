using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Interop.VisualStudio.COM
{
    [Guid(IIDs.IID_IPropertyView)]
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