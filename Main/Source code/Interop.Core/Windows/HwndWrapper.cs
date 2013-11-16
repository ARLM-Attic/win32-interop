using System;
using System.Diagnostics.CodeAnalysis;
using System.Security;
using System.Windows;
using System.Windows.Threading;

namespace Interop.Core.Windows
{
    public sealed class HwndWrapper : DispatcherObject, IDisposable
    {
        private string _className;

        public IntPtr Handle
        {
            [SecurityCritical]
            get;

            [SecurityCritical]
            private set;
        }

        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "WndProc is a multicast delegate.")]
        public NativeMethods.WndProc WndProc;

        #region Constructors

        public HwndWrapper()
        {
            _className = "MessageWindow" + Guid.NewGuid();
        }

        #endregion

        #region Destructors

        private bool _disposed;

        ~HwndWrapper()
        {
            Dispose(false, false);
        }

        public void Dispose()
        {
            Dispose(true, false);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing, bool isHwndBeingDestroyed)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // TODO: Free managed resources
            }

            // TODO: Free unmanaged resources
            _disposed = true;
        }

        #endregion
    }
}