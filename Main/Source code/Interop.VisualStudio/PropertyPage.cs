using System;
using System.Drawing;
using System.Runtime.InteropServices;

using Interop.Core;

using Microsoft.VisualStudio.OLE.Interop;

using IServiceProvider = System.IServiceProvider;

namespace Interop.VisualStudio
{
    public class PropertyPage : IPropertyPage, IPropertyTabHost, IPropertyStorage
    {
        private IPropertyTab _tab;
        private IPropertyPageSite _site;
        private bool _isInitializing;
        private bool _isChanged;
        private string[] _configNames;

        public PropertyPage(IPropertyTab tab)
        {
            _tab = tab;
            tab.Initialize(this);
        }

        public void SetPageSite(IPropertyPageSite pPageSite)
        {
            _site = pPageSite;
        }

        public void Activate(IntPtr hWndParent, RECT[] pRect, int bModal)
        {
            throw new NotImplementedException();
        }

        public void Deactivate()
        {
        }

        public void GetPageInfo(PROPPAGEINFO[] pPageInfo)
        {
            if (pPageInfo == null || pPageInfo.Length <= 0)
            {
                throw new ArgumentException();
            }
            var pageInfo = pPageInfo[0];
            pageInfo.cb = (uint)Marshal.SizeOf(typeof(PROPPAGEINFO));
            if (_tab != null)
            {
                pageInfo.pszTitle = _tab.Title;
                pageInfo.pszDocString = _tab.Description;
                pageInfo.pszHelpFile = _tab.HelpFile;
                pageInfo.dwHelpContext = (uint)_tab.HelpContext;
                pageInfo.SIZE.cx = _tab.Size.Width;
                pageInfo.SIZE.cy = _tab.Size.Height;
            }
            else
            {
                pageInfo.pszTitle = string.Empty;
                pageInfo.pszDocString = null;
                pageInfo.pszHelpFile = null;
                pageInfo.dwHelpContext = 0U;
                pageInfo.SIZE.cx = 0;
                pageInfo.SIZE.cy = 0;
            }
            pPageInfo[0] = pageInfo;
        }

        public void SetObjects(uint cObjects, object[] ppunk)
        {
            throw new NotImplementedException();
        }

        public void Show(uint nCmdShow)
        {
            throw new NotImplementedException();
        }

        public void Move(RECT[] pRect)
        {
            throw new NotImplementedException();
        }

        public int IsPageDirty()
        {
            return (int)(_isChanged ? NativeMethods.HResult.S_OK : NativeMethods.HResult.S_FALSE);
        }

        public int Apply()
        {
            if (_tab != null)
            {
                _isInitializing = true;
                _tab.SaveProperties(_configNames, this);
                _isInitializing = false;
                PropertiesChanged(false);
            }
            return (int)NativeMethods.HResult.S_OK;
        }

        public void Help(string pszHelpDir)
        {
// ReSharper disable SuspiciousTypeConversion.Global
            var serviceProvider = _site as IServiceProvider;
            if (serviceProvider != null)
            {
                var service = serviceProvider as Microsoft.VisualStudio.VSHelp80.Help2;
                if (service != null)
                {
                    service.DisplayTopicFromF1Keyword(pszHelpDir);
                }
            }
// ReSharper restore SuspiciousTypeConversion.Global
        }

        public int TranslateAccelerator(MSG[] pMsg)
        {
            return _site != null ? _site.TranslateAccelerator(pMsg) : (int)NativeMethods.HResult.S_OK;
        }

        public string GetProperty(bool perUser, string configName, string propertyName, string defaultValue)
        {
            throw new NotImplementedException();
        }

        public string GetProperties(bool perUser, string[] configNames, string propertyName, string defaultValue)
        {
            throw new NotImplementedException();
        }

        public void SetProperty(bool perUser, string configName, string propertyName, string value)
        {
            throw new NotImplementedException();
        }

        public void SetProperties(bool perUser, string[] configNames, string propertyName, string value)
        {
            throw new NotImplementedException();
        }

        public void RemoveProperty(bool perUser, string configName, string propertyName)
        {
            throw new NotImplementedException();
        }

        public void RemoveProperties(bool perUser, string[] configNames, string propertyName)
        {
            throw new NotImplementedException();
        }

        public void PropertiesChanged()
        {
            throw new NotImplementedException();
        }

        private void PropertiesChanged(bool isChanged)
        {
            if (_isChanged == isChanged || _isInitializing)
            {
                return;
            }
            _isChanged = isChanged;
            if (_site != null)
            {
                _site.OnStatusChange(isChanged ? (uint)PROPPAGESTATUS.PROPPAGESTATUS_DIRTY | (uint)PROPPAGESTATUS.PROPPAGESTATUS_VALIDATE : (uint)PROPPAGESTATUS.PROPPAGESTATUS_CLEAN);
            }
        }
    }
}