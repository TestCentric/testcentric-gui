// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using TestCentric.Gui.Model.Settings;
namespace TestCentric.Gui.Presenters
{
    // TODO: With latest changes, this is no longer used.
    // Keeping it for a while, as we may end up wanting
    // to write more tests that use it.
    class UserSettingsFake : ISettings
    {
        private Dictionary<string, object> _settings = new Dictionary<string, object>();

        public event SettingsEventHandler Changed;

        public object GetSetting(string settingName)
        {
            if (_settings.ContainsKey(settingName))
                return _settings[settingName];

            return null;
        }

        public T GetSetting<T>(string settingName, T defaultValue)
        {
            object result = GetSetting(settingName);

            if (result == null)
                return defaultValue;

            if (result is T)
                return (T)result;

            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter == null)
                    return defaultValue;

                return (T)converter.ConvertFrom(result);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public void RemoveSetting(string settingName)
        {
            _settings.Remove(settingName);

            if (Changed != null)
                Changed(this, new SettingsEventArgs(settingName));
        }

        public void RemoveGroup(string groupName)
        {
            throw new NotImplementedException();
        }

        public void SaveSetting(string settingName, object settingValue)
        {
            _settings[settingName] = settingValue;

            Changed?.Invoke(this, new SettingsEventArgs(settingName));
        }
    }
}
