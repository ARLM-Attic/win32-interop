using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;

using Interop.Core;
using Interop.Helpers;
using Interop.VisualStudio.COM;

using JetBrains.Annotations;

using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell.Interop;

using IServiceProvider = System.IServiceProvider;

namespace Interop.VisualStudio
{
    [PublicAPI]
    public class PropertyPage : IPropertyPage, IPropertyViewSite, IPropertyStore
    {
        [NotNull]
        private readonly IPropertyView _view;

        [CanBeNull]
        private IPropertyPageSite _site;

        private bool _isInitializing;
        private bool _isChanged;

        private object[] _configs;
        private string[] _configNames;
        private IVsBuildPropertyStorage _buildStorage;

        public PropertyPage([NotNull] IPropertyView tab)
        {
            ValidationHelper.NotNull(tab, "tab");
            _view = tab;
            tab.Initialize(this);
        }

        public int Apply()
        {
            _isInitializing = true;
            _view.SaveProperties(_configNames, this);
            _isInitializing = false;
            PropertiesChanged(false);
            return (int)NativeMethods.HResult.S_OK;
        }

        public void Activate(IntPtr hWndParent, [CanBeNull] RECT[] pRect, int bModal)
        {
            ValidationHelper.NotNull(pRect, "pRect");
            ValidationHelper.NotZeroLength(pRect, "pRect");
            UnsafeWrappers.SetParent(_view.Handle, hWndParent);
            Move(pRect);
        }

        public void Move([CanBeNull] RECT[] pRect)
        {
            ValidationHelper.NotNull(pRect, "pRect");
            ValidationHelper.NotZeroLength(pRect, "pRect");
            var rect = pRect[0];
            _view.Location = new Point(rect.left, rect.top);
            _view.Size = new Size(rect.right - rect.left, rect.bottom - rect.top);
        }

        public void GetPageInfo([CanBeNull] PROPPAGEINFO[] pPageInfo)
        {
            ValidationHelper.NotNull(pPageInfo, "pPageInfo");
            ValidationHelper.NotZeroLength(pPageInfo, "pPageInfo");
            var pageInfo = pPageInfo[0];
            pageInfo.cb = (uint)Marshal.SizeOf(typeof(PROPPAGEINFO));
            pageInfo.pszTitle = _view.Title;
            pageInfo.pszDocString = _view.Description;
            pageInfo.pszHelpFile = _view.HelpFile;
            pageInfo.dwHelpContext = (uint)_view.HelpContext;
            pageInfo.SIZE.cx = _view.Size.Width;
            pageInfo.SIZE.cy = _view.Size.Height;
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
                _view.LoadProperties(_configNames, this);
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

        public object GetProperty(bool perUser, string configName, string propertyName, object defaultValue)
        {
            object result = null;
            string pbstrPropValue;
            if (_buildStorage != null && _buildStorage.GetPropertyValue(propertyName, configName, !perUser ? (uint)_PersistStorageType.PST_PROJECT_FILE : (uint)_PersistStorageType.PST_USER_FILE, out pbstrPropValue) == (int)NativeMethods.HResult.S_OK)
            {
                result = pbstrPropValue;
            }
            if (result == null)
            {
                return defaultValue;
            }
            if (defaultValue == null)
            {
                return result;
            }
            try
            {
                return Convert.ChangeType(result, defaultValue.GetType(), CultureInfo.InvariantCulture);
            }
            catch (InvalidCastException)
            {
                return defaultValue;
            }
        }

        public object GetProperties(bool perUser, string[] configNames, string propertyName, object defaultValue)
        {
            ValidationHelper.NotNull(configNames, "configNames");
            ValidationHelper.NotZeroLength(configNames, "configNames");
            object result = null;
            foreach (var property in configNames.Select(configName => GetProperty(perUser, configName, propertyName, defaultValue)))
            {
                if (property == null)
                {
                    return null;
                }
                if (result == null)
                {
                    result = property;
                }
                else if (!property.Equals(result))
                {
                    return null;
                }
            }
            return result;
        }

        public void SetProperty(bool perUser, string configName, string propertyName, object value)
        {
            ValidationHelper.NotNull(value, "value");
            if (_buildStorage != null)
            {
                ErrorHelper.ThrowIfNotZero(_buildStorage.SetPropertyValue(propertyName, configName, !perUser ? (uint)_PersistStorageType.PST_PROJECT_FILE : (uint)_PersistStorageType.PST_USER_FILE, value.ToString()));
            }
        }

        public void SetProperties(bool perUser, string[] configNames, string propertyName, object value)
        {
            ValidationHelper.NotNull(configNames, "configNames");
            ValidationHelper.NotZeroLength(configNames, "configNames");
            foreach (var configName in configNames)
            {
                SetProperty(perUser, configName, propertyName, value);
            }
        }

        public void RemoveProperty(bool perUser, string configName, string propertyName)
        {
            if (_buildStorage != null)
            {
                ErrorHelper.ThrowIfNotZero(_buildStorage.RemoveProperty(propertyName, configName, !perUser ? (uint)_PersistStorageType.PST_PROJECT_FILE : (uint)_PersistStorageType.PST_USER_FILE));
            }
        }

        public void RemoveProperties(bool perUser, string[] configNames, string propertyName)
        {
            ValidationHelper.NotNull(configNames, "configNames");
            ValidationHelper.NotZeroLength(configNames, "configNames");
            foreach (var configName in configNames)
            {
                RemoveProperty(perUser, configName, propertyName);
            }
        }

        public void PropertiesChanged()
        {
            PropertiesChanged(true);
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

        public int TranslateAccelerator(MSG[] pMsg)
        {
            return _site != null ? _site.TranslateAccelerator(pMsg) : (int)NativeMethods.HResult.E_NOTIMPL;
        }

        public void Show(uint nCmdShow)
        {
            switch (nCmdShow)
            {
                case (uint)NativeMethods.WindowState.SW_SHOWNORMAL:
                case (uint)NativeMethods.WindowState.SW_SHOW:
                    _view.Show();
                    break;
                case (uint)NativeMethods.WindowState.SW_HIDE:
                    _view.Hide();
                    break;
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
                    service.DisplayTopicFromF1Keyword(_view.HelpKeyword);
                }
            }
// ReSharper restore SuspiciousTypeConversion.Global
        }

        public void Deactivate()
        {
        }
    }
}