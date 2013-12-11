using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

using Interop.Core;
using Interop.Helpers;

using JetBrains.Annotations;

using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell.Interop;

using IServiceProvider = System.IServiceProvider;

namespace Interop.VisualStudio
{
    public class PropertyPage : IPropertyPage, IPropertyTabHost, IPropertyStorage
    {
        [NotNull]
        private readonly IPropertyTab _tab;

        [CanBeNull]
        private IPropertyPageSite _site;

        private bool _isInitializing;
        private bool _isChanged;

        private object[] _configs;
        private string[] _configNames;
        private IVsBuildPropertyStorage _buildStorage;

        public PropertyPage([NotNull] IPropertyTab tab)
        {
            ValidationHelper.NotNull(tab, "tab");
            _tab = tab;
            tab.Initialize(this);
        }

        public int Apply()
        {
            _isInitializing = true;
            _tab.SaveProperties(_configNames, this);
            _isInitializing = false;
            PropertiesChanged(false);
            return (int)NativeMethods.HResult.S_OK;
        }

        public void Activate(IntPtr hWndParent, [CanBeNull] RECT[] pRect, int bModal)
        {
            UnsafeWrappers.SetParent(_tab.Handle, hWndParent);
            Move(pRect);
        }

        public void Move([CanBeNull] RECT[] pRect)
        {
            ValidationHelper.NotNull(pRect, "pRect");
            ValidationHelper.NotZeroLength(pRect, "pRect");
            var rect = pRect[0];
            _tab.Location = new Point(rect.left, rect.top);
            _tab.Size = new Size(rect.right - rect.left, rect.bottom - rect.top);
        }

        public void GetPageInfo([CanBeNull] PROPPAGEINFO[] pPageInfo)
        {
            ValidationHelper.NotNull(pPageInfo, "pPageInfo");
            ValidationHelper.NotZeroLength(pPageInfo, "pPageInfo");
            var pageInfo = pPageInfo[0];
            pageInfo.cb = (uint)Marshal.SizeOf(typeof(PROPPAGEINFO));
            pageInfo.pszTitle = _tab.Title;
            pageInfo.pszDocString = _tab.Description;
            pageInfo.pszHelpFile = _tab.HelpFile;
            pageInfo.dwHelpContext = (uint)_tab.HelpContext;
            pageInfo.SIZE.cx = _tab.Size.Width;
            pageInfo.SIZE.cy = _tab.Size.Height;
            pPageInfo[0] = pageInfo;
        }

        public int IsPageDirty()
        {
            return _isChanged ? (int)NativeMethods.HResult.S_OK : (int)NativeMethods.HResult.S_FALSE;
        }

        public void SetPageSite([CanBeNull] IPropertyPageSite pPageSite)
        {
            _site = pPageSite;
        }

        public void SetObjects(uint cObjects, object[] ppunk)
        {
            _configs = null;
            _configNames = null;
            _buildStorage = null;
            if (ppunk != null)
            {
                _configs = ppunk;
                SetBuildStorage();
                SetConfigurationNames();
                _isInitializing = true;
                _tab.LoadProperties(_configNames, this);
                _isInitializing = false;
                PropertiesChanged(false);
            }
        }

        private void SetBuildStorage()
        {
            foreach (var config in _configs.Where(config => config != null))
            {
                var vsCfgBrowseObject = config as IVsCfgBrowseObject;
                IVsHierarchy pHier = null;
                uint pItemid;
                if (vsCfgBrowseObject != null)
                {
                    vsCfgBrowseObject.GetProjectItem(out pHier, out pItemid);
                }
                else
                {
                    var vsBrowseObject = config as IVsBrowseObject;
                    if (vsBrowseObject != null)
                    {
                        vsBrowseObject.GetProjectItem(out pHier, out pItemid);
                    }
                }
                _buildStorage = pHier as IVsBuildPropertyStorage;
                if (_buildStorage != null)
                {
                    break;
                }
            }
        }

        private void SetConfigurationNames()
        {
            _configNames = new string[_configs.Length];
            for (var index = 0; index < _configs.Length; index++)
            {
                var vsCfg = _configs[index] as IVsCfg;
                if (vsCfg != null)
                {
                    vsCfg.get_DisplayName(out _configNames[index]);
                }
                if (_configNames[index] == null)
                {
                    _configNames[index] = string.Empty;
                }
            }
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
                    service.DisplayTopicFromF1Keyword(_tab.HelpKeyword);
                }
            }
// ReSharper restore SuspiciousTypeConversion.Global
        }

        public void Deactivate()
        {
        }



        public void Show(uint nCmdShow)
        {
            throw new NotImplementedException();
        }

        public int TranslateAccelerator(MSG[] pMsg)
        {
            return _site != null ? _site.TranslateAccelerator(pMsg) : (int)NativeMethods.HResult.E_NOTIMPL;
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
            if (_isChanged != isChanged && !_isInitializing)
            {
                _isChanged = isChanged;
                if (_site != null)
                {
                    _site.OnStatusChange(isChanged ? (uint)PROPPAGESTATUS.PROPPAGESTATUS_DIRTY | (uint)PROPPAGESTATUS.PROPPAGESTATUS_VALIDATE : (uint)PROPPAGESTATUS.PROPPAGESTATUS_CLEAN);
                }
            }
        }
    }
}