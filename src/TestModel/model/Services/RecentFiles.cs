// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using TestCentric.Engine;

namespace TestCentric.Gui.Model.Services
{
    public class RecentFiles
    {
        private IList<string> _fileEntries = new List<string>();
        private ISettings _userSettings;
        private string _prefix;

        private const int MAX_FILES = 24;

        public RecentFiles(ISettings userSettings, string applicationPrefix)
        {
            _userSettings = userSettings;
            _prefix = applicationPrefix ?? string.Empty;

            if (_prefix != string.Empty && !_prefix.EndsWith("."))
                _prefix += ".";

            _prefix += "Gui.RecentProjects";

            LoadEntries();
        }

        public int MaxFiles { get; } = MAX_FILES;

        public string Latest
        {
            get { return _fileEntries.Count == 0 ? null : _fileEntries[0]; }
            set
            {
                if (Latest != value)
                {
                    _fileEntries.Remove(value);

                    _fileEntries.Insert(0, value);
                    if (_fileEntries.Count > MaxFiles)
                        _fileEntries.RemoveAt(MaxFiles);

                    SaveEntries();
                }
            }
        }

        public IList<string> Entries
        {
            get { return _fileEntries; }
        }

        public void Remove(string fileName)
        {
            _fileEntries.Remove(fileName);
        }

        private void LoadEntries()
        {
            _fileEntries.Clear();

            for (int index = 1; index < MaxFiles; index++)
            {
                if (_fileEntries.Count >= MaxFiles) break;

                string fileSpec = _userSettings.GetSetting(GetRecentFileKey(index)) as string;
                if (fileSpec != null) _fileEntries.Add(fileSpec);
            }
        }

        private void SaveEntries()
        {
            while (_fileEntries.Count > MaxFiles)
                _fileEntries.RemoveAt(_fileEntries.Count - 1);

            for (int index = 0; index < MaxFiles; index++)
            {
                string keyName = GetRecentFileKey(index + 1);
                if (index < _fileEntries.Count)
                    _userSettings.SaveSetting(keyName, _fileEntries[index]);
                else
                    _userSettings.RemoveSetting(keyName);
            }
        }

        private string GetRecentFileKey(int index)
        {
            return string.Format("{0}.File{1}", _prefix, index);
        }
    }
}
