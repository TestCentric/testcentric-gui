// ***********************************************************************
// Copyright (c) 2019 Charlie Poole
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

namespace TestCentric.Gui.Model.Services
{
    public class RecentFiles
    {
        private IList<string> _fileEntries = new List<string>();
        private ISettings _userSettings;

        private const int MAX_FILES = 24;

        public RecentFiles(ISettings userSettings)
        {
            _userSettings = userSettings;
            LoadEntriesFromSettings();
        }

        public int MaxFiles
        {
            get { return MAX_FILES; }
        }

        public IList<string> Entries
        {
            get { return _fileEntries; }
        }

        public void Remove(string fileName)
        {
            _fileEntries.Remove(fileName);
        }

        public void SetMostRecent(string filePath)
        {
            _fileEntries.Remove(filePath);

            _fileEntries.Insert(0, filePath);
            if (_fileEntries.Count > MAX_FILES)
                _fileEntries.RemoveAt(MAX_FILES);
        }

        private void LoadEntriesFromSettings()
        {
            _fileEntries.Clear();

            // TODO: Prefix should be provided by caller
            AddEntriesForPrefix("Gui.RecentProjects");
        }

        private void AddEntriesForPrefix(string prefix)
        {
            for (int index = 1; index < MAX_FILES; index++)
            {
                if (_fileEntries.Count >= MAX_FILES) break;

                string fileSpec = _userSettings.GetSetting(GetRecentFileKey(prefix, index)) as string;
                if (fileSpec != null) _fileEntries.Add(fileSpec);
            }
        }

        private void SaveEntriesToSettings()
        {
            string prefix = "Gui.RecentProjects";

            while (_fileEntries.Count > MAX_FILES)
                _fileEntries.RemoveAt(_fileEntries.Count - 1);

            for (int index = 0; index < MAX_FILES; index++)
            {
                string keyName = GetRecentFileKey(prefix, index + 1);
                if (index < _fileEntries.Count)
                    _userSettings.SaveSetting(keyName, _fileEntries[index]);
                else
                    _userSettings.RemoveSetting(keyName);
            }

            // Remove legacy entries here
            _userSettings.RemoveGroup("RecentProjects");
        }

        private string GetRecentFileKey(string prefix, int index)
        {
            return string.Format("{0}.File{1}", prefix, index);
        }
    }
}
