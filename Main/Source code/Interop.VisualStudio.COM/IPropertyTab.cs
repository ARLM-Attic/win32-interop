using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Interop.VisualStudio
{
    [Guid("B9F27534-9FD1-496A-B715-8F740BEC61C3")]
    public interface IPropertyTab
    {
        string Title { get; }

        string Description { get; }

        string HelpKeyword { get; }

        string HelpFile { get; }

        int HelpContext { get; }

        IntPtr Handle { get; }

        Size Size { get; set; }

        Point Location { get; set; }

        void Initialize(IPropertyTabHost host);

        void LoadProperties(string[] configNames, IPropertyStorage storage);

        void SaveProperties(string[] configNames, IPropertyStorage storage);

        void Show();

        void Hide();
    }
}