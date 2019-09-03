// ***********************************************************************
// Copyright (c) 2018-2019 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Security.Principal;
using System.Windows.Forms;
using NUnit.Framework;
using NUnit.Engine;
using NUnit.TestUtilities.Fakes;

namespace TestCentric.Gui.Model.Settings
{
    public abstract class SettingsTests<TSettings> where TSettings : SettingsGroup
    {
        protected const string APPLICATION_PREFIX = "TestCentric.";
        protected string _groupPrefix;

        protected ISettings _settingsService;
        protected SettingsEventArgs _changeEvent;
        protected TSettings _settingsGroup;

        protected abstract TSettings GetSettingsGroup();

        public SettingsTests(string groupPrefix)
        {
            _groupPrefix = groupPrefix ?? string.Empty;

            if (_groupPrefix != string.Empty && !groupPrefix.EndsWith("."))
                _groupPrefix += ".";
        }

        [SetUp]
        public void SetUp()
        {
            _settingsService = new TestModel(new MockTestEngine()).Services.UserSettings;
            _settingsService.Changed += (object s, SettingsEventArgs e) => { _changeEvent = e; };
            _settingsGroup = GetSettingsGroup();
        }

        [TestCaseSource("TestCases")]
        public void CheckSetting<TValue>(string propertyName, TValue defaultValue, TValue testValue)
        {
            // Ensure that the property exists
            var propInfo = typeof(TSettings).GetProperty(propertyName);

            // Check the default value
            Assert.That(propInfo.GetValue(_settingsGroup), Is.EqualTo(defaultValue), $"Incorrect default value for {propertyName}");

            // Set the property and verify that it changed
            propInfo.SetValue(_settingsGroup, testValue);
            Assert.That(propInfo.GetValue(_settingsGroup), Is.EqualTo(testValue), $"Value did not change when {propertyName} was set");

            // Check that a Changed event was received with the correct storage key
            Assert.That(_changeEvent, Is.Not.Null, $"No event received when {propertyName} was set");
            Assert.That(_changeEvent.SettingName, Is.EqualTo(APPLICATION_PREFIX + _groupPrefix + propertyName), $"Event has incorrect key for {propertyName}");
        }
    }

    public class EngineSettingsTests : SettingsTests<EngineSettings>
    {
        public EngineSettingsTests() : base("Engine.Options") { }

        protected override EngineSettings GetSettingsGroup()
        {
            return new EngineSettings(_settingsService, APPLICATION_PREFIX);
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
        public ErrorDisplaySettingsTests() : base("Gui.ErrorDisplay") { }

        protected override ErrorDisplaySettings GetSettingsGroup()
        {
            return new ErrorDisplaySettings(_settingsService, APPLICATION_PREFIX);
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
        public GuiSettingsTests() : base("Gui.Options") { }

        private static readonly Font DEFAULT_FONT = new Font(FontFamily.GenericSansSerif, 8.25f);
        private static readonly Font DEFAULT_FIXED_FONT = new Font(FontFamily.GenericMonospace, 8.0f);

        private static readonly Font TEST_FONT = new Font(FontFamily.GenericSerif, 10.0f);
        private static readonly Font TEST_FIXED_FONT = new Font(FontFamily.GenericMonospace, 10.0f);

        protected override GuiSettings GetSettingsGroup()
        {
            return new GuiSettings(_settingsService, APPLICATION_PREFIX);
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
        public MainFormSettingsTests() : base("Gui.MainForm") { }

        protected override MainFormSettings GetSettingsGroup()
        {
            return new MainFormSettings(_settingsService, APPLICATION_PREFIX);
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
        public MiniFormSettingsTests() : base("Gui.MiniForm") { }

        protected override MiniFormSettings GetSettingsGroup()
        {
            return new MiniFormSettings(_settingsService, APPLICATION_PREFIX);
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
        public RecentProjectsSettingsTests() : base("Gui.RecentProjects") { }

        protected override RecentProjectsSettings GetSettingsGroup()
        {
            return new RecentProjectsSettings(_settingsService, APPLICATION_PREFIX);
        }

        public static TestCaseData[] TestCases = new TestCaseData[]
        {
            new TestCaseData("MaxFiles", 24, 12),
            new TestCaseData("CheckFilesExist", true, false)
        };
    }

    public class TestTreeSettingsTests : SettingsTests<TestTreeSettings>
    {
        public TestTreeSettingsTests() : base("Gui.TestTree") { }

        protected override TestTreeSettings GetSettingsGroup()
        {
            return new TestTreeSettings(_settingsService, APPLICATION_PREFIX);
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
        public TextOutputSettingsTests() : base("Gui.ResultTabs.TextOutput") { }

        protected override TextOutputSettings GetSettingsGroup()
        {
            return new TextOutputSettings(_settingsService, APPLICATION_PREFIX);
        }

        public static TestCaseData[] TestCases = new TestCaseData[]
        {
            new TestCaseData("WordWrapEnabled", true, false),
            new TestCaseData("Labels", "ON", "AFTER")
        };
    }
}
