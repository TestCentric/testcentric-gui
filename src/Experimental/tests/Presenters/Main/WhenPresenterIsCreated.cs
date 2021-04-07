// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters.Main
{
    public class WhenPresenterIsCreated : MainPresenterTestBase
    {
#if NYI
        [Test]
        public void NewProject_IsEnabled()
        {
            View.NewProjectCommand.Received(1).Enabled = true;
        }
#endif

        [Test]
        public void OpenProject_IsEnabled()
        {
            View.OpenProjectCommand.Received(1).Enabled = true;
        }

        [Test]
        public void CloseCommand_IsDisabled()
        {
            View.CloseCommand.Received(1).Enabled = false;
        }

        [Test]
        public void SaveCommand_IsDisabled()
        {
            View.SaveCommand.Received(1).Enabled = false;
        }

        [Test]
        public void SaveAsCommand_IsDisabled()
        {
            View.SaveAsCommand.Received(1).Enabled = false;
        }

        [Test]
        public void SaveResults_IsDisabled()
        {
            View.SaveResultsCommand.Received(1).Enabled = false;
        }

        [Test]
        public void ReloadTests_IsDisabled()
        {
            View.ReloadTestsCommand.Received(1).Enabled = false;
        }

        [Test]
        public void RecentProjects_IsEnabled()
        {
            View.RecentProjectsMenu.Received(1).Enabled = true;
        }

        [Test]
        public void ExitCommand_IsEnabled()
        {
            View.ExitCommand.Received(1).Enabled = true;
        }

        [Test]
        public void ProjectMenu_IsDisabled()
        {
            View.ProjectMenu.Received(1).Enabled = false;
        }

        [Test]
        public void ProjectMenu_IsInvisible()
        {
            View.ProjectMenu.Received(1).Visible = false;
        }

    }
}
