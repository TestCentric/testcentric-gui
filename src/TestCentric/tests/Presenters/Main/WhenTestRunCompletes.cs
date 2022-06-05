// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters.Main
{
    using Model;

    public class WhenTestRunCompletes : MainPresenterTestBase
    {
        private ResultSummary _summary;

        [SetUp]
        public void SimulateTestRunFinish()
        {
            ClearAllReceivedCalls();

            _model.HasTests.Returns(true);
            _model.HasResults.Returns(true);
            _model.ResultSummary.Returns(new ResultSummary() { FailureCount = 1 });
            _model.IsTestRunning.Returns(false);
            _model.StopRequested.Returns(false);
            _model.TestPackage.Returns(new NUnit.Engine.TestPackage("dummy.dll"));

            var resultNode = new ResultNode("<test-run id='XXX' result='Failed' />");
            FireRunFinishedEvent(resultNode);
        }

#if NYI // Add after implementation of project or package saving
        [TestCase("NewProjectCommand", true)]
        [TestCase("OpenProjectCommand", true)]
        [TestCase("SaveCommand", true)]
        [TestCase("SaveAsCommand", true)
#endif

        [TestCase("OpenCommand", true)]
        [TestCase("CloseCommand", true)]
        [TestCase("AddTestFilesCommand", true)]
        [TestCase("ReloadTestsCommand", true)]
        [TestCase("RecentFilesMenu", true)]
        [TestCase("ExitCommand", true)]
        [TestCase("SaveResultsCommand", true)]
        [TestCase("RunAllButton", true)]
        [TestCase("RunSelectedButton", true)]
        [TestCase("RerunButton", true)]
        [TestCase("RunFailedButton", true)]
        [TestCase("DisplayFormatButton", true)]
        [TestCase("RunParametersButton", true)]
        [TestCase("StopRunButton", false)]
        [TestCase("ForceStopButton", false)]
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
