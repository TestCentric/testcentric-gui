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
