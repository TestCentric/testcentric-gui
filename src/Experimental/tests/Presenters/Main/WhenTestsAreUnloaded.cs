// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters.Main
{
    using Model;

    public class WhenTestsAreUnloaded : MainPresenterTestBase
    {
        [SetUp]
        public void SimulateTestUnload()
        {
            Model.HasTests.Returns(false);
            Model.IsTestRunning.Returns(false);
            Model.Events.TestUnloaded += Raise.Event<TestEventHandler>(new TestEventArgs());
        }

#if NYI
        [Test]
        public void NewProject_IsEnabled()
        {
            View.NewProjectCommand.Received().Enabled = true;
        }
#endif

        [Test]
        public void OpenProject_IsEnabled()
        {
            View.OpenProjectCommand.Received().Enabled = true;
        }

        [Test]
        public void CloseCommand_IsDisabled()
        {
            View.CloseCommand.Received().Enabled = false;
        }

        [Test]
        public void SaveCommand_IsDisabled()
        {
            View.SaveCommand.Received().Enabled = false;
        }

        [Test]
        public void SaveAsCommand_IsDisabled()
        {
            View.SaveAsCommand.Received().Enabled = false;
        }

        [Test]
        public void SaveResults_IsEnabled()
        {
            View.SaveResultsCommand.Received().Enabled = false;
        }

        [Test]
        public void ReloadTests_IsDisabled()
        {
            View.ReloadTestsCommand.Received().Enabled = false;
        }

        [Test]
        public void RecentProjects_IsEnabled()
        {
            View.RecentProjectsMenu.Received().Enabled = true;
        }

        [Test]
        public void ExitCommand_IsEnabled()
        {
            View.ExitCommand.Received().Enabled = true;
        }

        [Test]
        public void ProjectMenu_IsInvisible()
        {
            View.ProjectMenu.Received().Visible = false;
        }
    }
}
