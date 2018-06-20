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
using System.IO;
using System.Windows.Forms;
using NUnit.Engine;

namespace TestCentric.Gui
{
    using Model;
    using Model.Settings;

    public class RecentFileMenuHandler
    {
        public RecentFileMenuHandler(MenuItem menu, ITestModel model)
        {
            Menu = menu;
            UserSettings = model.Services.UserSettings;
            RecentFiles = model.Services.RecentFiles;
            CheckFilesExist = UserSettings.Gui.RecentProjects.CheckFilesExist;
            ShowNonRunnableFiles = false;
        }

        public bool CheckFilesExist { get; }

        public bool ShowNonRunnableFiles { get; }

        public MenuItem Menu { get; }

        public IRecentFiles RecentFiles { get; }

        public UserSettings UserSettings { get; }

        public string this[int index]
        {
            get { return Menu.MenuItems[index].Text.Substring(2); }
        }

        public void Load()
        {
            const int MAX_RECENT_FILES = 24;
            int maxNumberOfFilesToShow =
                UserSettings.GetSetting("Gui.RecentProjects.MaxFiles", MAX_RECENT_FILES);
            bool checkFilesExist =
                UserSettings.GetSetting("Gui.RecentProjects.CheckFilesExist", true);

            var entries = RecentFiles.Entries;

            if (entries.Count == 0)
                Menu.Enabled = false;
            else
            {
                Menu.Enabled = true;
                Menu.MenuItems.Clear();
                int maxEntriesToShow = Math.Min(entries.Count, maxNumberOfFilesToShow);
                int index = 1;
                for (int i = 0; index <= maxEntriesToShow && i < entries.Count; i++)
                {
                    string entry = entries[i];
                    if (!checkFilesExist || File.Exists(entry))
                    {
                        MenuItem item = new MenuItem(String.Format("{0} {1}", index++, entry));
                        item.Click += new EventHandler(OnRecentFileClick);
                        Menu.MenuItems.Add(item);
                    }
                }
            }
        }

        private void OnRecentFileClick(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string testFileName = item.Text.Substring(2);

            // TODO: Figure out a better way
            TestCentricMainForm form = item.GetMainMenu().GetForm() as TestCentricMainForm;
            if (form != null)
                form.Presenter.OpenProject(testFileName);
        }
    }
}
