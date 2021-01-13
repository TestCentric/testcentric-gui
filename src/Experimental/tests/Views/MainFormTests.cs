// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Linq;
using NUnit.Framework;

namespace TestCentric.Gui.Views
{
    using Elements;

    [Platform(Exclude = "Linux", Reason = "Uninitialized form causes an error in Travis-CI")]
    public class MainFormTests
    {
        private IMainView _view;

        [SetUp]
        public void CreateView()
        {
            _view = new MainForm();
        }

        //[Test]
        //public void SelectedRuntimeTest()
        //{
        //    var selectedRuntime = _view.SelectedRuntime as CheckedToolStripMenuGroup;

        //    Assert.NotNull(selectedRuntime, "SelectedRuntime not set properly");
        //    Assert.NotNull(selectedRuntime.TopMenu, "TopMenu is not set");
        //    Assert.That(selectedRuntime.MenuItems.Select((p) => p.Text), Is.EqualTo(new string[] { "Default" }));
        //    Assert.That(selectedRuntime.MenuItems.Select((p) => p.Tag), Is.EqualTo(new string[] { "DEFAULT" }));
        //}

        [Test]
        public void ProcessModelTest()
        {
            var processModel = _view.ProcessModel as CheckedToolStripMenuGroup;

            Assert.NotNull(processModel, "ProcessModel not set properly");
            Assert.That(processModel.MenuItems.Select((p) => p.Text),
                Is.EqualTo(new string[] { "Default", "InProcess (DEPRECATED)", "Separate", "Multiple" }));
            Assert.That(processModel.MenuItems.Select((p) => p.Tag),
                Is.EqualTo(new string[] { "DEFAULT", "InProcess", "Separate", "Multiple" }));
        }
    }
}
