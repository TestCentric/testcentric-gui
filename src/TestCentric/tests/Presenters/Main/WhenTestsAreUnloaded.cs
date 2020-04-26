// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
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

        [TestCase("RunButton", false)]
        [TestCase("StopButton", false)]
        [TestCase("OpenCommand", true)]
        [TestCase("CloseCommand", false)]
        [TestCase("AddTestFilesCommand", false)]
        [TestCase("ReloadTestsCommand", false)]
        [TestCase("RuntimeMenu", false)]
        [TestCase("RecentFilesMenu", true)]
        [TestCase("ExitCommand", true)]
        [TestCase("RunAllCommand", false)]
        [TestCase("RunSelectedCommand", false)]
        [TestCase("RunFailedCommand", false)]
        [TestCase("TestParametersCommand", false)]
        [TestCase("StopRunCommand", false)]
        [TestCase("SaveResultsCommand", false)]
        public void CheckCommandEnabled(string propName, bool enabled)
        {
            ViewElement(propName).Received().Enabled = enabled;
        }
    }
}
