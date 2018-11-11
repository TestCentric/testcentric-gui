// ***********************************************************************
// Copyright (c) 2018 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using NUnit.Engine;

namespace TestCentric.Gui.Model.Settings
{
    public class SettingsGroup : ISettings
    {
        protected ISettings _settings;
        protected string _prefix;

        public SettingsGroup(ISettings engineSettings, string prefix)
        {
            _settings = engineSettings;
            _prefix = prefix ?? string.Empty;

            if (_prefix != string.Empty && !prefix.EndsWith("."))
                _prefix += ".";

            // Forward any changes from the engine
            _settings.Changed += (object s, SettingsEventArgs args) =>
            {
                Changed?.Invoke(s, args);
            };
        }

        #region ISettings Implementation

        public event SettingsEventHandler Changed;

        public object GetSetting(string settingName)
        {
            return _settings.GetSetting(_prefix + settingName);
        }

        public T GetSetting<T>(string settingName, T defaultValue)
        {
            return _settings.GetSetting<T>(_prefix + settingName, defaultValue);
        }

        public void RemoveGroup(string groupName)
        {
            _settings.RemoveGroup(_prefix + groupName);
        }

        public void RemoveSetting(string settingName)
        {
            _settings.RemoveSetting(_prefix + settingName);
        }

        public void SaveSetting(string settingName, object settingValue)
        {
            if (settingValue != null)
                _settings.SaveSetting(_prefix + settingName, settingValue);
            else
                RemoveSetting(settingName);
        }

        #endregion
    }
}