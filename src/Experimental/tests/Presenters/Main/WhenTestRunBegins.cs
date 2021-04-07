// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters.Main
{
    using Model;

    public class WhenTestRunBegins : MainPresenterTestBase
    {
        [SetUp]
        protected void SimulateTestRunStarting()
        {
            Model.HasTests.Returns(true);
            Model.IsTestRunning.Returns(true);
            Model.Events.RunStarting += Raise.Event<RunStartingEventHandler>(new RunStartingEventArgs(1234));
        }

#if NYI
        [Test]
        public void NewProject_IsDisabled()
        {
            Assert.That(View.NewProjectCommand.Enabled == false);
        }
#endif

        [Test]
        public void OpenProject_IsDisabled()
        {
            Assert.That(View.OpenProjectCommand.Enabled == false);
        }

        [Test]
        public void CloseCommand_IsDisabled()
        {
            Assert.That(View.CloseCommand.Enabled == false);
        }

        [Test]
        public void SaveCommand_IsDisabled()
        {
            Assert.That(View.SaveCommand.Enabled == false);
        }

        [Test]
        public void SaveAsCommand_IsDisabled()
        {
            Assert.That(View.SaveAsCommand.Enabled == false);
        }

        [Test]
        public void SaveResults_IsDisabled()
        {
            Assert.That(View.SaveResultsCommand.Enabled == false);
        }

        [Test]
        public void ReloadTests_IsDisabled()
        {
            Assert.That(View.ReloadTestsCommand.Enabled == false);
        }

        [Test]
        public void RecentProjects_IsDisabled()
        {
            Assert.That(View.RecentProjectsMenu.Enabled == false);
        }

        [Test]
        public void ExitCommand_IsEnabled()
        {
            Assert.That(View.ExitCommand.Enabled == true);
        }

        [Test]
        public void ProjectMenu_IsVisible()
        {
            Assert.That(View.ProjectMenu.Visible == true);
        }
    }
}
