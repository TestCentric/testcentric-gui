// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TestCentric.Engine
{
    [Serializable]
    public class SettingsDictionary : IDictionary<string, object>
    {
        private Dictionary<string, object> _settings = new Dictionary<string, object>();

        public event EventHandler<EventArgs> Changed;

        public object this[string key]
        {
            get => _settings[key];
            set { _settings[key] = value; OnChanged(); }
        }

        public ICollection<string> Keys => _settings.Keys;

        public ICollection<object> Values => _settings.Values;

        public int Count => _settings.Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<string, object>>)_settings).IsReadOnly;

        public void Add(string key, object value)
        {
            _settings.Add(key, value);
            OnChanged();
        }

        public void Add(KeyValuePair<string, object> item)
        {
            ((ICollection<KeyValuePair<string, object>>)_settings).Add(item);
            OnChanged();
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<string, object>>)_settings).Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return ((ICollection<KeyValuePair<string, object>>)_settings).Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return _settings.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, object>>)_settings).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, object>>)_settings).GetEnumerator();
        }

        public bool Remove(string key)
        {
            var changed = _settings.Remove(key);
            if (changed) OnChanged();
            return changed;
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            var changed = ((ICollection<KeyValuePair<string, object>>)_settings).Remove(item);
            if (changed) OnChanged();
            return changed;
        }

        public bool TryGetValue(string key, out object value)
        {
            return ((IDictionary<string, object>)_settings).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_settings).GetEnumerator();
        }

        private void OnChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }
}
