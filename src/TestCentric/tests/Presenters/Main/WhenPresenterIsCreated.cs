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
        [TestCase("CloseProjectCommand", false)]
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

        [TestCase("NUNIT_TREE")]
        [TestCase("FIXTURE_LIST")]
        [TestCase("TEST_LIST")]
        public void CheckMenu_DisplayFormat_SelectedItem_IsInitialzedFromSettings(string displayFormat)
        {
            // 1. Arrange
            _settings.Gui.TestTree.DisplayFormat = displayFormat;

            // 2. Act
            _presenter = new TestCentricPresenter(_view, _model, new CommandLineOptions());

            // 3. Assert
            _view.DisplayFormat.Received().SelectedItem = displayFormat;
        }

        [TestCase("ASSEMBLY")]
        [TestCase("CATEGORY")]
        [TestCase("OUTCOME")]
        public void CheckMenu_GroupBy_SelectedItem_FixtureList_IsInitialzedFromSettings(string groupBy)
        {
            // 1. Arrange
            _settings.Gui.TestTree.DisplayFormat = "FIXTURE_LIST";
            _settings.Gui.TestTree.FixtureList.GroupBy = groupBy;

            // 2. Act
            _presenter = new TestCentricPresenter(_view, _model, new CommandLineOptions());

            // 3. Assert
            _view.GroupBy.Received().SelectedItem = groupBy;
        }

        [TestCase("ASSEMBLY")]
        [TestCase("CATEGORY")]
        [TestCase("OUTCOME")]
        public void CheckMenu_GroupBy_SelectedItem_TestList_IsInitialzedFromSettings(string groupBy)
        {
            // 1. Arrange
            _settings.Gui.TestTree.DisplayFormat = "TEST_LIST";
            _settings.Gui.TestTree.TestList.GroupBy = groupBy;

            // 2. Act
            _presenter = new TestCentricPresenter(_view, _model, new CommandLineOptions());

            // 3. Assert
            _view.GroupBy.Received().SelectedItem = groupBy;
        }
    }
}
