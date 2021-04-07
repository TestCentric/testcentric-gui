// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Model.Settings
{
    public class SettingsGroup : ISettings
    {
        protected ISettings _settingsService;

        public readonly string GroupPrefix;

        public SettingsGroup(ISettings settingsService, string groupPrefix)
        {
            _settingsService = settingsService;

            GroupPrefix = groupPrefix ?? string.Empty;

            if (GroupPrefix != string.Empty && !groupPrefix.EndsWith("."))
                GroupPrefix += ".";

            // Forward any changes from the engine
            _settingsService.Changed += (object s, SettingsEventArgs args) =>
            {
                Changed?.Invoke(s, args);
            };
        }

        #region ISettings Implementation

        public event SettingsEventHandler Changed;

        public object GetSetting(string settingName)
        {
            return _settingsService.GetSetting(GroupPrefix + settingName);
        }

        public T GetSetting<T>(string settingName, T defaultValue)
        {
            return _settingsService.GetSetting<T>(GroupPrefix + settingName, defaultValue);
        }

        public void RemoveGroup(string groupName)
        {
            _settingsService.RemoveGroup(GroupPrefix + groupName);
        }

        public void RemoveSetting(string settingName)
        {
            _settingsService.RemoveSetting(GroupPrefix + settingName);
        }

        public void SaveSetting(string settingName, object settingValue)
        {
            if (settingValue != null)
                _settingsService.SaveSetting(GroupPrefix + settingName, settingValue);
            else
                RemoveSetting(settingName);
        }

        #endregion
    }
}
