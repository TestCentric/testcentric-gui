// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Drawing;
using System.Security.Principal;
using System.Windows.Forms;
using NUnit.Engine;
using NUnit.Framework;

namespace TestCentric.Gui.Model.Settings
{
    public abstract class SettingsTests<TSettings> where TSettings : SettingsGroup
    {
        protected const string APPLICATION_PREFIX = "TestCentric.";

        protected UserSettings _userSettings;

        protected SettingsEventArgs _changeEvent;

        protected abstract TSettings SettingsGroup { get; }

        [SetUp]
        public void SetUp()
        {
            ISettings settingsService = new SettingsStore();
            _userSettings = new UserSettings(settingsService, APPLICATION_PREFIX);
            settingsService.Changed += (object s, SettingsEventArgs e) => { _changeEvent = e; };
        }

        [TestCaseSource("TestCases")]
        public void CheckSetting<TValue>(string propertyName, TValue defaultValue, TValue testValue)
        {
            // Ensure that the property exists
            var propInfo = typeof(TSettings).GetProperty(propertyName);

            // Check the default value
            Assert.That(propInfo.GetValue(SettingsGroup), Is.EqualTo(defaultValue), $"Incorrect default value for {propertyName}");

            // Set the property and verify that it changed
            propInfo.SetValue(SettingsGroup, testValue);
            Assert.That(propInfo.GetValue(SettingsGroup), Is.EqualTo(testValue), $"Value did not change when {propertyName} was set");

            // Check that a Changed event was received with the correct storage key
            Assert.That(_changeEvent, Is.Not.Null, $"No event received when {propertyName} was set");
            Assert.That(_changeEvent.SettingName, Is.EqualTo(SettingsGroup.GroupPrefix + propertyName), $"Event has incorrect key for {propertyName}");
        }
    }

    public class EngineSettingsTests : SettingsTests<EngineSettings>
    {
        protected override EngineSettings SettingsGroup => _userSettings.Engine;

        [Test]
        public void GroupPrefixIsCorrect()
        {
            Assert.That(SettingsGroup.GroupPrefix, Is.EqualTo(APPLICATION_PREFIX + "Engine."));
        }

        public static TestCaseData[] TestCases = new TestCaseData[]
        {
            new TestCaseData("ShadowCopyFiles", true, false),
            new TestCaseData("Agents", 0, 8),
            new TestCaseData("ReloadOnChange", true, false),
            new TestCaseData("RerunOnChange", false, true),
            new TestCaseData("ReloadOnRun", false, true),
            new TestCaseData("SetPrincipalPolicy", false, true),
            new TestCaseData("PrincipalPolicy", nameof(PrincipalPolicy.UnauthenticatedPrincipal), nameof(PrincipalPolicy.WindowsPrincipal))
        };
    }

    public class ErrorDisplaySettingsTests : SettingsTests<ErrorDisplaySettings>
    {
        protected override ErrorDisplaySettings SettingsGroup => _userSettings.Gui.ErrorDisplay;

        [Test]
        public void GroupPrefixIsCorrect()
        {
            Assert.That(SettingsGroup.GroupPrefix, Is.EqualTo(APPLICATION_PREFIX + "Gui.ErrorDisplay."));
        }

        public static TestCaseData[] TestCases = new TestCaseData[]
        {
            new TestCaseData("SplitterPosition", 0, 12),
            new TestCaseData("WordWrapEnabled", true, false),
            new TestCaseData("ToolTipsEnabled", true, false),
            new TestCaseData("SourceCodeDisplay", false, true),
            new TestCaseData("SourceCodeSplitterOrientation", Orientation.Vertical, Orientation.Horizontal),
            new TestCaseData("SourceCodeVerticalSplitterPosition", 0.3f, 0.5f),
            new TestCaseData("SourceCodeHorizontalSplitterPosition", 0.3f, 0.5f)
        };
    }

    public class GuiSettingsTests : SettingsTests<GuiSettings>
    {
        private static readonly Font DEFAULT_FONT = new Font(FontFamily.GenericSansSerif, 8.25f);
        private static readonly Font DEFAULT_FIXED_FONT = new Font(FontFamily.GenericMonospace, 8.0f);

        private static readonly Font TEST_FONT = new Font(FontFamily.GenericSerif, 10.0f);
        private static readonly Font TEST_FIXED_FONT = new Font(FontFamily.GenericMonospace, 10.0f);

        protected override GuiSettings SettingsGroup => _userSettings.Gui;

        [Test]
        public void GroupPrefixIsCorrect()
        {
            Assert.That(SettingsGroup.GroupPrefix, Is.EqualTo(APPLICATION_PREFIX + "Gui."));
        }

        public static TestCaseData[] TestCases = new TestCaseData[]
        {
            new TestCaseData("DisplayFormat", "Full", "Mini"),
            new TestCaseData("LoadLastProject", true, false),
            new TestCaseData("SelectedTab", 0, 1),
            new TestCaseData("InitialSettingsPage", null, "Some.Page"),
            new TestCaseData("ClearResultsOnReload", false, true),
            new TestCaseData("Font", DEFAULT_FONT, TEST_FONT),
            new TestCaseData("FixedFont", DEFAULT_FIXED_FONT, TEST_FIXED_FONT),
            new TestCaseData("InternalTraceLevel", InternalTraceLevel.Off, InternalTraceLevel.Verbose)
        };

        [TestCase("TestTree", typeof(TestTreeSettings))]
        [TestCase("RecentProjects", typeof(RecentProjectsSettings))]
        [TestCase("MainForm", typeof(MainFormSettings))]
        [TestCase("MiniForm", typeof(MiniFormSettings))]
        [TestCase("ErrorDisplay", typeof(ErrorDisplaySettings))]
        [TestCase("TextOutput", typeof(TextOutputSettings))]
        public void CheckNestedSettingGroups(string propertyName, Type expectedType)
        {
            // Check that the property exists and returns the expected type
            var propInfo = typeof(GuiSettings).GetProperty(propertyName);
            Assert.That(propInfo.PropertyType, Is.EqualTo(expectedType));
        }
    }

    public class MainFormSettingsTests : SettingsTests<MainFormSettings>
    {
        protected override MainFormSettings SettingsGroup => _userSettings.Gui.MainForm;

        [Test]
        public void GroupPrefixIsCorrect()
        {
            Assert.That(SettingsGroup.GroupPrefix, Is.EqualTo(APPLICATION_PREFIX + "Gui.MainForm."));
        }

        public static TestCaseData[] TestCases()
        {
            return new TestCaseData[]
            {
                new TestCaseData("Location", new Point(10, 10), new Point(50, 50)),
                new TestCaseData("Size", new Size(700, 400), new Size(900, 500)),
                new TestCaseData("Maximized", false, true),
                new TestCaseData("SplitPosition", 0, 200)
            };
        }
    }

    public class MiniFormSettingsTests : SettingsTests<MiniFormSettings>
    {
        protected override MiniFormSettings SettingsGroup => _userSettings.Gui.MiniForm;

        [Test]
        public void GroupPrefixIsCorrect()
        {
            Assert.That(SettingsGroup.GroupPrefix, Is.EqualTo(APPLICATION_PREFIX + "Gui.MiniForm."));
        }

        public static TestCaseData[] TestCases()
        {
            return new TestCaseData[]
            {
                new TestCaseData("Location", new Point(10, 10), new Point(0, 0)),
                new TestCaseData("Size", new Size(700, 400), new Size(900, 500)),
                new TestCaseData("Maximized", false, true)
            };
        }
    }

    public class RecentProjectsSettingsTests : SettingsTests<RecentProjectsSettings>
    {
        protected override RecentProjectsSettings SettingsGroup => _userSettings.Gui.RecentProjects;

        [Test]
        public void GroupPrefixIsCorrect()
        {
            Assert.That(SettingsGroup.GroupPrefix, Is.EqualTo(APPLICATION_PREFIX + "Gui.RecentProjects."));
        }

        public static TestCaseData[] TestCases = new TestCaseData[]
        {
            new TestCaseData("MaxFiles", 24, 12),
            new TestCaseData("CheckFilesExist", true, false)
        };
    }

    public class TestTreeSettingsTests : SettingsTests<TestTreeSettings>
    {
        protected override TestTreeSettings SettingsGroup => _userSettings.Gui.TestTree;

        [Test]
        public void GroupPrefixIsCorrect()
        {
            Assert.That(SettingsGroup.GroupPrefix, Is.EqualTo(APPLICATION_PREFIX + "Gui.TestTree."));
        }

        public static TestCaseData[] TestCases = new TestCaseData[]
        {
            new TestCaseData("DisplayFormat", "NUNIT_TREE", "TEST_LIST"),
            new TestCaseData("SaveVisualState", true, false),
            new TestCaseData("InitialTreeDisplay", 0, 2),
            new TestCaseData("AlternateImageSet", "Default", "Custom"),
            new TestCaseData("ShowCheckBoxes", false, true)
        };
    }

    public class TextOutputSettingsTests : SettingsTests<TextOutputSettings>
    {
        protected override TextOutputSettings SettingsGroup => _userSettings.Gui.TextOutput;

        [Test]
        public void GroupPrefixIsCorrect()
        {
            Assert.That(SettingsGroup.GroupPrefix, Is.EqualTo(APPLICATION_PREFIX + "Gui.TextOutput."));
        }

        public static TestCaseData[] TestCases = new TestCaseData[]
        {
            new TestCaseData("WordWrapEnabled", true, false),
            new TestCaseData("Labels", "ON", "AFTER")
        };
    }
}
