// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace TestCentric.Gui.Model.Settings
{
    /// <summary>
    /// SettingsStore implements the loading and saving of settings
    /// from an XML file and allows access to them via a key
    /// through the ISettings interface.
    /// </summary>
    public class SettingsStore : ISettings
    {
        static Logger log = InternalTrace.GetLogger("SettingsStore");

        protected Dictionary<string, object> _settings = new Dictionary<string, object>();

        private string _settingsFile;
        private bool _writeable;

        /// <summary>
        /// Construct a SettingsStore without a backing file - used for testing.
        /// </summary>
        public SettingsStore() { }

        /// <summary>
        /// Construct a SettingsStore with a file name and indicate whether it is writeable
        /// </summary>
        /// <param name="settingsFile"></param>
        /// <param name="writeable"></param>
        public SettingsStore(string settingsFile, bool writeable)
        {
            _settingsFile = Path.GetFullPath(settingsFile);
            _writeable = writeable;
        }

        public void LoadSettings()
        {
            FileInfo info = new FileInfo(_settingsFile);
            if (!info.Exists || info.Length == 0)
                return;

            try
            {
                XmlDocument doc = new XmlDocument();
                using (var stream = new FileStream(_settingsFile, FileMode.Open, FileAccess.Read))
                {
                    doc.Load(stream);
                }

                foreach (XmlElement element in doc.DocumentElement["Settings"].ChildNodes)
                {
                    if (element.Name != "Setting")
                        throw new Exception("Unknown element in settings file: " + element.Name);

                    if (!element.HasAttribute("name"))
                        throw new Exception("Setting must have 'name' attribute");

                    if (!element.HasAttribute("value"))
                        throw new Exception("Setting must have 'value' attribute");

                    SaveSetting(element.GetAttribute("name"), element.GetAttribute("value"));
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("Error loading settings {0}. {1}", _settingsFile, ex.Message);
                throw new Exception(msg, ex);
            }
        }

        public void SaveSettings()
        {
            if (!_writeable || _settings.Keys.Count <= 0)
                return;

            try
            {
                string dirPath = Path.GetDirectoryName(_settingsFile);
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                var stream = new MemoryStream();
                using (var writer = new XmlTextWriter(stream, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;

                    writer.WriteProcessingInstruction("xml", "version=\"1.0\"");
                    writer.WriteStartElement("NUnitSettings");
                    writer.WriteStartElement("Settings");

                    List<string> keys = new List<string>(_settings.Keys);
                    keys.Sort();

                    foreach (string name in keys)
                    {
                        object val = GetSetting(name);
                        if (val != null)
                        {
                            writer.WriteStartElement("Setting");
                            writer.WriteAttributeString("name", name);
                            writer.WriteAttributeString("value",
                                TypeDescriptor.GetConverter(val).ConvertToInvariantString(val));
                            writer.WriteEndElement();
                        }
                    }

                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.Flush();

                    var reader = new StreamReader(stream, Encoding.UTF8, true);
                    stream.Seek(0, SeekOrigin.Begin);
                    var contents = reader.ReadToEnd();
                    File.WriteAllText(_settingsFile, contents, Encoding.UTF8);
                }
            }
            catch (Exception)
            {
                // So we won't try this again
                _writeable = false;
                throw;
            }
        }

        #region ISettings Implementation

        public event SettingsEventHandler Changed;

        /// <summary>
        /// Load the value of one of the group's settings
        /// </summary>
        /// <param name="settingName">The name of setting to load</param>
        /// <returns>The value of the setting</returns>
        public object GetSetting(string settingName)
        {
            return _settings.ContainsKey(settingName)
                ? _settings[settingName]
                : null;
        }

        /// <summary>
        /// Load the value of one of the group's settings or return a default value
        /// </summary>
        /// <param name="settingName">The name of setting to load</param>
        /// <param name="defaultValue">The value to return if the setting is not present</param>
        /// <returns>The value of the setting or the default</returns>
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

                return (T)converter.ConvertFrom(null, CultureInfo.InvariantCulture, result);
            }
            catch (Exception ex)
            {
                log.Error("Unable to convert setting {0} to {1}", settingName, typeof(T).Name);
                log.Error(ex.Message);
                return defaultValue;
            }
        }

        /// <summary>
        /// Remove a setting from the group
        /// </summary>
        /// <param name="settingName">The name of the setting to remove</param>
        public void RemoveSetting(string settingName)
        {
            _settings.Remove(settingName);

            if (Changed != null)
                Changed(this, new SettingsEventArgs(settingName));
        }

        /// <summary>
        /// Remove a group of settings
        /// </summary>
        /// <param name="groupName">The name of the group to remove</param>
        public void RemoveGroup(string groupName)
        {
            List<string> keysToRemove = new List<string>();

            string prefix = groupName;
            if (!prefix.EndsWith("."))
                prefix = prefix + ".";

            foreach (string key in _settings.Keys)
                if (key.StartsWith(prefix))
                    keysToRemove.Add(key);

            foreach (string key in keysToRemove)
                _settings.Remove(key);
        }

        /// <summary>
        /// Save the value of one of the group's settings
        /// </summary>
        /// <param name="settingName">The name of the setting to save</param>
        /// <param name="settingValue">The value to be saved</param>
        public void SaveSetting(string settingName, object settingValue)
        {
            object oldValue = GetSetting(settingName);

            // Avoid signaling "changes" when there is not really a change
            if (oldValue != null)
            {
                if (oldValue is string && settingValue is string && (string)oldValue == (string)settingValue ||
                    oldValue is int && settingValue is int && (int)oldValue == (int)settingValue ||
                    oldValue is bool && settingValue is bool && (bool)oldValue == (bool)settingValue ||
                    oldValue is Enum && settingValue is Enum && oldValue.Equals(settingValue))
                    return;
            }

            _settings[settingName] = settingValue;

            if (Changed != null)
                Changed(this, new SettingsEventArgs(settingName));
        }

        #endregion
    }
}
