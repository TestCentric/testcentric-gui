// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters.Main
{
    public class WhenTestsAreUnloaded : MainPresenterTestBase
    {
        [SetUp]
        public void SimulateTestUnload()
        {
            ClearAllReceivedCalls();

            _model.HasTests.Returns(false);
            _model.IsTestRunning.Returns(false);
            FireTestUnloadedEvent();
        }

#if NYI // Add after implementation of project or package saving
        [TestCase("NewProjectCommand", true)]
        [TestCase("OpenProjectCommand", true)]
        [TestCase("SaveCommand", false)]
        [TestCase("SaveAsCommand", false)
#endif

        [TestCase("OpenCommand", true)]
        [TestCase("CloseCommand", false)]
        [TestCase("AddTestFilesCommand", false)]
        [TestCase("ReloadTestsCommand", false)]
        [TestCase("RecentFilesMenu", true)]
        [TestCase("ExitCommand", true)]
        [TestCase("RunAllMenuCommand", false)]
        [TestCase("RunSelectedMenuCommand", false)]
        [TestCase("RunFailedMenuCommand", false)]
        [TestCase("StopRunMenuCommand", false)]
        [TestCase("ForceStopMenuCommand", false)]
        [TestCase("TestParametersMenuCommand", false)]
        [TestCase("SaveResultsCommand", false)]
        [TestCase("RunAllToolbarCommand", false)]
        [TestCase("RunSelectedToolbarCommand", false)]
        [TestCase("DebugAllToolbarCommand", false)]
        [TestCase("DebugSelectedToolbarCommand", false)]
        [TestCase("TestParametersToolbarCommand", false)]
        [TestCase("StopRunButton", false)]
        [TestCase("ForceStopButton", false)]
        public void CheckCommandEnabled(string propName, bool enabled)
        {
            ViewElement(propName).Received().Enabled = enabled;
        }

        [TestCase("StopRunButton", true)]
        [TestCase("ForceStopButton", false)]
        public void CheckElementVisibility(string propName, bool visible)
        {
            ViewElement(propName).Received().Visible = visible;
        }
    }
}
