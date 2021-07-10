// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters.Main
{
    public class WhenTestRunBegins : TestTreePresenterTestBase
    {
        [SetUp]
        protected void SimulateTestRunStarting()
        {
            ClearAllReceivedCalls();

            _model.HasTests.Returns(true);
            _model.IsTestRunning.Returns(true);
            _model.TestPackage.Returns(new NUnit.Engine.TestPackage("dummy.dll"));
            FireRunStartingEvent(1234);
        }

#if NYI // Add after implementation of project or package saving
        [TestCase("NewProjectCommand", false)]
        [TestCase("OpenProjectCommand", false)]
        [TestCase("SaveCommand", false)]
        [TestCase("SaveAsCommand", false)
#endif

        [TestCase("OpenCommand", false)]
        [TestCase("CloseCommand", false)]
        [TestCase("AddTestFilesCommand", false)]
        [TestCase("ReloadTestsCommand", false)]
        [TestCase("RecentFilesMenu", false)]
        [TestCase("ExitCommand", true)]
        [TestCase("RunAllCommand", false)]
        [TestCase("RunSelectedCommand", false)]
        [TestCase("RunFailedCommand", false)]
        [TestCase("TestParametersCommand", false)]
        [TestCase("SaveResultsCommand", false)]
        public void CheckCommandEnabled(string propName, bool enabled)
        {
            ViewElement(propName).Received().Enabled = enabled;
        }
    }
}
