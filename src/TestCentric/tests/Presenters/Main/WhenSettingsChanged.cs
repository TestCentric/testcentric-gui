// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Framework;

namespace TestCentric.Gui.Presenters.Main
{
    internal class WhenSettingsChanged : MainPresenterTestBase
    {
        [TestCase("NUNIT_TREE")]
        [TestCase("FIXTURE_LIST")]
        [TestCase("TEST_LIST")]
        public void DisplayFormat_SettingChanged_MenuItemIsUpdated(string displayFormat)
        {
            // 1. Act
            _settings.Gui.TestTree.DisplayFormat = displayFormat;

            // 2. Assert
            _view.DisplayFormat.SelectedItem = displayFormat;
        }

        [TestCase("ASSEMBLY")]
        [TestCase("CATEGORY")]
        [TestCase("OUTCOME")]
        public void FixtureListGroupBy_SettingChanged_MenuItemIsUpdated(string groupBy)
        {
            // 1. Act
            _settings.Gui.TestTree.FixtureList.GroupBy = groupBy;

            // 2. Assert
            _view.GroupBy.SelectedItem = groupBy;
        }

        [TestCase("ASSEMBLY")]
        [TestCase("CATEGORY")]
        [TestCase("OUTCOME")]
        public void TestListGroupBy_SettingChanged_MenuItemIsUpdated(string groupBy)
        {
            // 1. Act
            _settings.Gui.TestTree.TestList.GroupBy = groupBy;

            // 2. Assert
            _view.GroupBy.SelectedItem = groupBy;
        }

        [TestCase(true, 0)]
        [TestCase(false, 1)]
        public void ShowNamespace_SettingChanged_MenuItemIsUpdated(bool showNamespace, int expectedMenuIndex)
        {
            // 1. Act
            _settings.Gui.TestTree.ShowNamespace = showNamespace;

            // 2. Assert
            _view.ShowNamespace.SelectedIndex = expectedMenuIndex;
        }
    }
}
