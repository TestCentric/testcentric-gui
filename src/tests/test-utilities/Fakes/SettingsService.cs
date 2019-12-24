// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using TestCentric.Engine;

namespace TestCentric.TestUtilities.Fakes
{
    public class SettingsService : ISettings
    {
        private Dictionary<string, object> _storage = new Dictionary<string, object>();

        public event SettingsEventHandler Changed;

        public object GetSetting(string settingName)
        {
            return _storage.ContainsKey(settingName)
                ? _storage[settingName]
                : null;
        }

        public T GetSetting<T>(string settingName, T defaultValue)
        {
            if (!_storage.ContainsKey(settingName))
                return defaultValue;

            try
            {
                return (T)_storage[settingName];
            }
            catch(Exception)
            {
                // Simulate engine action when conversion fails
                return defaultValue;
            }
        }

        public void RemoveGroup(string groupName)
        {
            throw new NotImplementedException();
        }

        public void RemoveSetting(string settingName)
        {
            _storage.Remove(settingName);
        }

        public void SaveSetting(string settingName, object settingValue)
        {
            _storage[settingName] = settingValue;

            Changed?.Invoke(this, new SettingsEventArgs(settingName));
        }
    }
}
