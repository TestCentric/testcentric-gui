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
using NUnit.TestUtilities.Fakes;
using TestCentric.Gui.Model;
using TestCentric.Gui.Model.Settings;

namespace TestCentric.Gui.Tests
{
    [TestFixture]
    public class RecentFileMenuHandlerTests
    {
        private ITestModel _model;
        private RecentProjectsSettings _settings;
        private IRecentFiles _recentFiles;

        private MenuItem _menu;
        private RecentFileMenuHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _model = new TestModel(new MockTestEngine());
            _settings = _model.Services.UserSettings.Gui.RecentProjects;
            _recentFiles = _model.Services.RecentFiles;

            _menu = new MenuItem();
            _handler = new RecentFileMenuHandler(_menu, _model, f => !f.StartsWith("_"));
        }

        [Test]
        public void DisableOnLoadWhenEmpty()
        {
            _handler.Load();
            Assert.IsFalse(_menu.Enabled);
        }

        [Test]
        public void EnableOnLoadWhenNotEmpty()
        {
            _recentFiles.SetMostRecent("Test");
            _handler.Load();
            Assert.IsTrue(_menu.Enabled);
        }

        [Test]
        public void LoadMenuItems()
        {
            _recentFiles.SetMostRecent("Third");
            _recentFiles.SetMostRecent("Second");
            _recentFiles.SetMostRecent("First");
            _handler.Load();
            Assert.AreEqual(3, _menu.MenuItems.Count);
            Assert.AreEqual("1 First", _menu.MenuItems[0].Text);
        }

        [Test]
        public void LoadMoreMenuItemsThanMaxFiles([Values(1, 2, 3)] int numMaxFiles)
        {
            _settings.MaxFiles = numMaxFiles;

            _recentFiles.SetMostRecent("Fourth");
            _recentFiles.SetMostRecent("Third");
            _recentFiles.SetMostRecent("Second");
            _recentFiles.SetMostRecent("First");
            _handler.Load();
            Assert.That(_menu.MenuItems.Count, Is.EqualTo(numMaxFiles));
        }

        [TestCase(true, new[] { "1 First", "2 Second", "3 Third" })]
        [TestCase(false, new[] { "1 First", "2 _ First filter", "3 Second", "4 _ Second filter", "5 Third" })]
        public void LoadMenuItemsFilteringWorks(bool checkFiles, string[] expectedItems)
        {
            _settings.MaxFiles = 24;
            _settings.CheckFilesExist = checkFiles;

            _recentFiles.SetMostRecent("Third");
            _recentFiles.SetMostRecent("_ Second filter");
            _recentFiles.SetMostRecent("Second");
            _recentFiles.SetMostRecent("_ First filter");
            _recentFiles.SetMostRecent("First");
            _handler.Load();
            Assert.AreEqual(expectedItems.Count(), _menu.MenuItems.Count);
            CollectionAssert.AreEqual(expectedItems, _menu.MenuItems.Cast<MenuItem>().Select(m => m.Text));
        }

        [TestCase(true, new[] { "1 First", "2 Second" })]
        [TestCase(false, new[] { "1 First", "2 _ First filter" })]
        public void LoadMenuItemsFilteringWorksWithTooManyElements(bool checkFiles, string[] expectedItems)
        {
            _settings.MaxFiles = 2;
            _settings.CheckFilesExist = checkFiles;

            _recentFiles.SetMostRecent("Third");
            _recentFiles.SetMostRecent("_ Second filter");
            _recentFiles.SetMostRecent("Second");
            _recentFiles.SetMostRecent("_ First filter");
            _recentFiles.SetMostRecent("First");
            _handler.Load();
            Assert.AreEqual(expectedItems.Count(), _menu.MenuItems.Count);
            CollectionAssert.AreEqual(expectedItems, _menu.MenuItems.Cast<MenuItem>().Select(m => m.Text));
        }

        // TODO: Need mock loader to test clicking
    }
}
