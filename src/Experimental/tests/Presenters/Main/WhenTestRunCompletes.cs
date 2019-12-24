// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters.Main
{
    using Model;
    using Views;

    public class WhenTestRunCompletes : MainPresenterTestBase
    {
        [SetUp]
        public void SimulateTestRunFinish()
        {
            Model.HasTests.Returns(true);
            Model.IsTestRunning.Returns(false);

            var resultNode = new ResultNode("<test-suite/>");
            Model.Events.RunFinished += Raise.Event<TestResultEventHandler>(new TestResultEventArgs(resultNode));
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
        public void CloseCommand_IsEnabled()
        {
            View.CloseCommand.Received().Enabled = true;
        }

        [Test]
        public void SaveCommand_IsEnabled()
        {
            View.SaveCommand.Received().Enabled = true;
        }

        [Test]
        public void SaveAsCommand_IsEnabled()
        {
            View.SaveAsCommand.Received().Enabled = true;
        }

        [Test]
        public void SaveResults_IsEnabled()
        {
            View.SaveResultsCommand.Received().Enabled = false;
        }

        [Test]
        public void ReloadTests_IsEnabled()
        {
            View.ReloadTestsCommand.Received().Enabled = true;
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
        public void ProjectMenu_IsVisible()
        {
            View.ProjectMenu.Received().Visible = true;
        }
    }
}
