// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters.Main
{
    public class WhenTestRunBegins : MainPresenterTestBase
    {
        [SetUp]
        protected void SimulateTestRunStarting()
        {
            ClearAllReceivedCalls();

            _model.HasTests.Returns(true);
            _model.IsTestRunning.Returns(true);
            _model.TestPackage.Returns(new NUnit.Engine.TestPackage(new string[] { "dummy.dll" }));
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
        [TestCase("SaveResultsCommand", false)]
        [TestCase("RunAllButton", false)]
        [TestCase("RunSelectedButton", false)]
        [TestCase("RerunButton", false)]
        [TestCase("RunFailedButton", false)]
        [TestCase("DisplayFormatButton", false)]
        [TestCase("RunParametersButton", false)]
        [TestCase("StopRunButton", true)]
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

        [Test]
        public void RunSummaryIsHidden()
        {
            _view.RunSummaryButton.Received().Checked = false;
        }
    }
}
