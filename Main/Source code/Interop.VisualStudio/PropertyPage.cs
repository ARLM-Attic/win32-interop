using System;

using Interop.Core;

using Microsoft.VisualStudio.OLE.Interop;

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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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