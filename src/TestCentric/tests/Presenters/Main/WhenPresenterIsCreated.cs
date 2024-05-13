// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters.Main
{
    public class WhenPresenterIsCreated : MainPresenterTestBase
    {
#if NYI // Add after implementation of project or package saving
        [TestCase("NewProjectCommand", true)]
        [TestCase("OpenProjectCommand", true)]
		[TestCase("SaveCommand", false)]
        [TestCase("SaveAsCommand", false)]
#endif

        [TestCase("NewProjectCommand", true)]
        [TestCase("OpenProjectCommand", true)]
        [TestCase("SaveProjectCommand", false)]
        [TestCase("CloseCommand", false)]
        [TestCase("AddTestFilesCommand", false)]
        [TestCase("ReloadTestsCommand", false)]
        [TestCase("RecentFilesMenu", true)]
        [TestCase("ExitCommand", true)]
        [TestCase("RunAllButton", false)]
        [TestCase("RunFailedButton", false)]
        [TestCase("RerunButton", false)]
        [TestCase("RunSelectedButton", false)]
        [TestCase("DisplayFormatButton", false)]
        [TestCase("RunParametersButton", false)]
        [TestCase("StopRunButton", false)]
        [TestCase("ForceStopButton", false)]
        [TestCase("SaveResultsCommand", false)]
        public void CheckCommandEnabled(string propName, bool enabled)
        {
            ViewElement(propName).Received().Enabled = enabled;
        }

        [TestCase("StopRunButton", true)]
        [TestCase("ForceStopButton", false)]
        public void CheckCommandVisible(string propName, bool visible)
        {
            ViewElement(propName).Received().Visible = visible;
        }
    }
}
