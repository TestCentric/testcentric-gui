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

using System;
using System.Collections.Generic;
using NUnit.Engine;

namespace NUnit.TestUtilities.Fakes
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
            return _storage.ContainsKey(settingName)
                ? (T)_storage[settingName]
                : defaultValue;
        }

        public void RemoveGroup(string groupName)
        {
            throw new NotImplementedException();
        }

        public void RemoveSetting(string settingName)
        {
            throw new NotImplementedException();
        }

        public void SaveSetting(string settingName, object settingValue)
        {
            _storage[settingName] = settingValue;

            Changed?.Invoke(this, new SettingsEventArgs(settingName));
        }
    }
}
