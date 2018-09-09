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
using System.Windows.Forms;
using NUnit.Engine;

namespace TestCentric.Gui.Views
{
    using Model;
    using Model.Settings;

    public class RecentFileMenuHandler
    {
        private readonly Func<string, bool> fileExists;

        public RecentFileMenuHandler(MenuItem menu, ITestModel model, Func<string, bool> fileExists)
        {
            Menu = menu;
            UserSettings = model.Services.UserSettings;
            RecentFiles = model.Services.RecentFiles;
            this.fileExists = fileExists;
        }

        public MenuItem Menu { get; }

        public IRecentFiles RecentFiles { get; }

        public UserSettings UserSettings { get; }

        public string this[int index]
        {
            get { return Menu.MenuItems[index].Text.Substring(2); }
        }

        public void Load()
        {
            int maxNumberOfFilesToShow = UserSettings.Gui.RecentProjects.MaxFiles;
            bool checkFilesExist = UserSettings.Gui.RecentProjects.CheckFilesExist;

            var entries = RecentFiles.Entries;
            var entriesToShow = GetEntries(entries, maxNumberOfFilesToShow, checkFilesExist, fileExists);

            if (entriesToShow.Count == 0)
                Menu.Enabled = false;
            else
            {
                Menu.Enabled = true;
                Menu.MenuItems.Clear();
                for (int i = 0; i < entriesToShow.Count; i++)
                {
                    var entry = entriesToShow[i];
                    MenuItem item = new MenuItem(String.Format("{0} {1}", i+1, entry));
                    item.Click += new EventHandler(OnRecentFileClick);
                    Menu.MenuItems.Add(item);
                }
            }
        }

        internal static IList<string> GetEntries(
            IList<string> entries,
            int maxNumberOfFilesToShow,
            bool checkFilesExist,
            Func<string, bool> predicate)
        {
            var result = new List<string>(entries.Count);

            int maxEntriesToShow = Math.Min(entries.Count, maxNumberOfFilesToShow);
            int index = 1;
            foreach (var entry in entries)
            {
                if (!checkFilesExist || predicate(entry))
                {
                    result.Add(entry);
                    index++;
                }

                if (index > maxEntriesToShow)
                    break;
            }

            return result;
        }

        private void OnRecentFileClick(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string testFileName = item.Text.Substring(2);

            // TODO: Figure out a better way
            TestCentricMainView form = item.GetMainMenu().GetForm() as TestCentricMainView;
            if (form != null)
                form.Presenter.LoadTests(testFileName);
        }
    }
}
