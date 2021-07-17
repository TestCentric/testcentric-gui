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
#if NYI // Add after implementation of project or package saving
        [TestCase("NewProjectCommand", true)]
        [TestCase("OpenProjectCommand", true)]
		[TestCase("SaveCommand", false)]
        [TestCase("SaveAsCommand", false)]
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
        [TestCase("RunAllToolbarCommand", false)]
        [TestCase("RunSelectedToolbarCommand", false)]
        [TestCase("DebugAllToolbarCommand", false)]
        [TestCase("DebugSelectedToolbarCommand", false)]
        [TestCase("TestParametersToolbarCommand", false)]
        [TestCase("StopRunButton", false)]
        [TestCase("ForceStopButton", false)]

        [TestCase("SaveResultsCommand", false)]
        public void CheckCommandEnabled(string propName, bool enabled)
        {
            ViewElement(propName).Received().Enabled = enabled;
        }

        [TestCase("StopRunMenuCommand", true)]
        [TestCase("ForceStopMenuCommand", false)]
        [TestCase("StopRunButton", true)]
        [TestCase("ForceStopButton", false)]
        [TestCase("RunSummaryButton", false)]
        public void CheckCommandVisible(string propName, bool visible)
        {
            ViewElement(propName).Received().Visible = visible;
        }

        [Test]
        public void DisplayFormatIsSet()
        {
            var expectedFormat = _settings.Gui.TestTree.DisplayFormat;
            _view.DisplayFormat.Received().SelectedItem = expectedFormat;
        }

        [Test]
        public void GroupByIsSet()
        {
            var displayFormat = _settings.Gui.TestTree.DisplayFormat;
            if (displayFormat == "TEST_LIST")
                _view.GroupBy.Received().SelectedItem =
                    _settings.Gui.TestTree.TestList.GroupBy;
            else if (displayFormat == "FIXTURE_LIST")
                _view.GroupBy.Received().SelectedItem =
                    _settings.Gui.TestTree.FixtureList.GroupBy;
        }
    }
}
