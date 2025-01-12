// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NSubstitute;
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
            Assert.That(_view.DisplayFormat.SelectedItem, Is.EqualTo(displayFormat));
        }

        [TestCase("ASSEMBLY")]
        [TestCase("CATEGORY")]
        [TestCase("OUTCOME")]
        public void FixtureListGroupBy_SettingChanged_MenuItemIsUpdated(string groupBy)
        {
            // 1. Act
            _settings.Gui.TestTree.FixtureList.GroupBy = groupBy;

            // 2. Assert
            Assert.That(_view.GroupBy.SelectedItem, Is.EqualTo(groupBy));
        }

        [TestCase("ASSEMBLY")]
        [TestCase("CATEGORY")]
        [TestCase("OUTCOME")]
        public void TestListGroupBy_SettingChanged_MenuItemIsUpdated(string groupBy)
        {
            // 1. Act
            _settings.Gui.TestTree.TestList.GroupBy = groupBy;

            // 2. Assert
            Assert.That(_view.GroupBy.SelectedItem, Is.EqualTo(groupBy));
        }

        [TestCase(true, 0)]
        [TestCase(false, 1)]
        public void ShowNamespace_SettingChanged_MenuItemIsUpdated(bool showNamespace, int expectedMenuIndex)
        {
            // 1. Act
            _settings.Gui.TestTree.ShowNamespace = showNamespace;

            // 2. Assert
            Assert.That(_view.ShowNamespace.SelectedIndex, Is.EqualTo(expectedMenuIndex));
        }

        [TestCase("NUNIT_TREE", true)]
        [TestCase("FIXTURE_LIST", false)]
        [TestCase("TEST_LIST", false)]
        public void DisplayFormat_SettingChanged_ShowHideFilterButton_IsUpdated(string displayFormat, bool expectedState)
        {
            // 1. Arrange
            _model.HasTests.Returns(true);

            // 2. Act
            _settings.Gui.TestTree.DisplayFormat = displayFormat;

            // 3. Assert
            Assert.That(_view.ShowHideFilterButton.Visible, Is.EqualTo(expectedState));
            Assert.That(_view.ShowHideFilterButton.Enabled, Is.EqualTo(expectedState));
        }
    }
}
