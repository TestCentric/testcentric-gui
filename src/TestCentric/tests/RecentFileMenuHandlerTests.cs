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

using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NSubstitute;
using NUnit.Engine;
using NUnit.Framework;
using TestCentric.Gui.Model;
using TestCentric.Gui.Model.Settings;

namespace TestCentric.Gui.Tests
{
    [TestFixture]
    public class RecentFileMenuHandlerTests
    {
        private MenuItem menu;
        private IRecentFiles files;
        private RecentFileMenuHandler handler;
        private int maxFiles;
        private bool checkFilesExist;

        [SetUp]
        public void SetUp()
        {
            menu = new MenuItem();
            var testModel = Substitute.For<ITestModel>();
            var settings = Substitute.For<ISettings>();
            maxFiles = 24;
            settings.GetSetting("Gui.RecentProjects.MaxFiles", Arg.Any<int>()).Returns(_ => maxFiles);
            checkFilesExist = false;
            settings.GetSetting("Gui.RecentProjects.CheckFilesExist", Arg.Any<bool>()).Returns(_ => checkFilesExist);
            var userSettings = new UserSettings(settings);
            testModel.Services.UserSettings.Returns(userSettings);

            files = new FakeRecentFiles();
            testModel.Services.RecentFiles.Returns(files);

            handler = new RecentFileMenuHandler(menu, testModel, f => !f.StartsWith("_"));
        }

        [Test]
        public void DisableOnLoadWhenEmpty()
        {
            handler.Load();
            Assert.IsFalse(menu.Enabled);
        }

        [Test]
        public void EnableOnLoadWhenNotEmpty()
        {
            files.SetMostRecent("Test");
            handler.Load();
            Assert.IsTrue(menu.Enabled);
        }

        [Test]
        public void LoadMenuItems()
        {
            files.SetMostRecent("Third");
            files.SetMostRecent("Second");
            files.SetMostRecent("First");
            handler.Load();
            Assert.AreEqual(3, menu.MenuItems.Count);
            Assert.AreEqual("1 First", menu.MenuItems[0].Text);
        }

        [Test]
        public void LoadMoreMenuItemsThanMaxFiles([Values(1, 2, 3)] int numMaxFiles)
        {
            maxFiles = numMaxFiles;
            files.SetMostRecent("Fourth");
            files.SetMostRecent("Third");
            files.SetMostRecent("Second");
            files.SetMostRecent("First");
            handler.Load();
            Assert.That(menu.MenuItems.Count, Is.EqualTo(numMaxFiles));
        }

        [TestCase(true, new[] { "1 First", "2 Second", "3 Third" })]
        [TestCase(false, new[] { "1 First", "2 _ First filter", "3 Second", "4 _ Second filter", "5 Third" })]
        public void LoadMenuItemsFilteringWorks(bool checkFiles, string[] expectedItems)
        {
            checkFilesExist = checkFiles;
            files.SetMostRecent("Third");
            files.SetMostRecent("_ Second filter");
            files.SetMostRecent("Second");
            files.SetMostRecent("_ First filter");
            files.SetMostRecent("First");
            handler.Load();
            Assert.AreEqual(expectedItems.Count(), menu.MenuItems.Count);
            CollectionAssert.AreEqual(expectedItems, menu.MenuItems.Cast<MenuItem>().Select(m => m.Text));
        }

        [TestCase(true, new[] { "1 First", "2 Second" })]
        [TestCase(false, new[] { "1 First", "2 _ First filter" })]
        public void LoadMenuItemsFilteringWorksWithTooManyElements(bool checkFiles, string[] expectedItems)
        {
            maxFiles = 2;
            checkFilesExist = checkFiles;
            files.SetMostRecent("Third");
            files.SetMostRecent("_ Second filter");
            files.SetMostRecent("Second");
            files.SetMostRecent("_ First filter");
            files.SetMostRecent("First");
            handler.Load();
            Assert.AreEqual(expectedItems.Count(), menu.MenuItems.Count);
            CollectionAssert.AreEqual(expectedItems, menu.MenuItems.Cast<MenuItem>().Select(m => m.Text));
        }

        private class FakeRecentFiles : IRecentFiles
        {
            private IList<string> files = new List<string>();
            private int maxFiles = 24;

            public int Count
            {
                get { return files.Count; }
            }

            public int MaxFiles
            {
                get { return maxFiles; }
                set { maxFiles = value; }
            }

            public void SetMostRecent(string fileName)
            {
                files.Insert(0, fileName);
            }

            public IList<string> Entries
            {
                get { return files; }
            }

            public void Remove(string fileName)
            {
                files.Remove(fileName);
            }
        }

        // TODO: Need mock loader to test clicking
    }
}
