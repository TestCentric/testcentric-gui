// ***********************************************************************
// Copyright (c) 2018 Charlie Poole
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
        protected UserSettings _userSettings;
        protected SettingsEventArgs _changeEvent;
        protected TSettings _settingsGroup;

        protected abstract TSettings GetSettingsGroup();

        protected string _prefix;

        public SettingsTests(string prefix)
        {
            _prefix = prefix ?? string.Empty;

            if (_prefix != string.Empty && !prefix.EndsWith("."))
                _prefix += ".";
        }

        [SetUp]
        public void SetUp()
        {
            _userSettings = new TestModel(new MockTestEngine()).Services.UserSettings;
            _userSettings.Changed += (object s, SettingsEventArgs e) => { _changeEvent = e; };
            _settingsGroup = GetSettingsGroup();
        }

        [TestCaseSource("TestCases")]
        public void CheckSetting<TValue>(string propertyName, TValue defaultValue, TValue testValue)
        {
            // Ensure that the property exists
            var propInfo = typeof(TSettings).GetProperty(propertyName);

            // Check the default value
            Assert.That(propInfo.GetValue(_settingsGroup), Is.EqualTo(defaultValue), "Incorrect default value");

            // Set the property and verify that it changed
            propInfo.SetValue(_settingsGroup, testValue);
            Assert.That(propInfo.GetValue(_settingsGroup), Is.EqualTo(testValue), "Value did not change");

            // Check that a Changed event was received with the correct storage key
            Assert.That(_changeEvent, Is.Not.Null, "No event received");
            Assert.That(_changeEvent.SettingName, Is.EqualTo(_prefix + propertyName));
        }
    }

    public class EngineSettingsTests : SettingsTests<EngineSettings>
    {
        public EngineSettingsTests() : base("Engine.Options") { }

        protected override EngineSettings GetSettingsGroup()
        {
            return _userSettings.Engine;
        }

        public static TestCaseData[] TestCases = new TestCaseData[]
        {
            new TestCaseData("ShadowCopyFiles", true, false),
            new TestCaseData("ProcessModel", "Multiple", "InProcess"),
            new TestCaseData("DomainUsage", "Multiple", "Single"),
            new TestCaseData("Agents", 0, 8),
            new TestCaseData("SetPrincipalPolicy", false, true),
            new TestCaseData("PrincipalPolicy", nameof(PrincipalPolicy.UnauthenticatedPrincipal), nameof(PrincipalPolicy.WindowsPrincipal))
        };
    }

    public class ErrorDisplaySettingsTests : SettingsTests<ErrorDisplaySettings>
    {
        public ErrorDisplaySettingsTests() : base("Gui.ErrorDisplay") { }

        protected override ErrorDisplaySettings GetSettingsGroup()
        {
            return _userSettings.Gui.ErrorDisplay;
        }

        public static TestCaseData[] TestCases = new TestCaseData[]
        {
            new TestCaseData("SplitterPosition", 0, 12),
            new TestCaseData("WordWrapEnabled", true, false),
            new TestCaseData("SourceCodeDisplay", false, true),
            new TestCaseData("SplitterOrientation", Orientation.Vertical, Orientation.Horizontal),
            new TestCaseData("VerticalPosition", 0.3f, 0.5f),
            new TestCaseData("HorizontalPosition", 0.3f, 0.5f)
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
            return _userSettings.Gui;
        }

        public static TestCaseData[] TestCases = new TestCaseData[]
        {
            new TestCaseData("DisplayFormat", "Full", "Mini"),
            new TestCaseData("LoadLastProject", true, false),
            new TestCaseData("SelectedTab", 0, 1),
            new TestCaseData("InitialSettingsPage", null, "Some.Page"),
            new TestCaseData("ReloadOnChange", true, false),
            new TestCaseData("RerunOnChange", false, true),
            new TestCaseData("ReloadOnRun", false, true),
            new TestCaseData("ClearResultsOnReload", false, true),
            new TestCaseData("Font", DEFAULT_FONT, TEST_FONT),
            new TestCaseData("FixedFont", DEFAULT_FIXED_FONT, TEST_FIXED_FONT),
            new TestCaseData("ProjectEditorPath", "nunit-editor.exe", "/Some/Path/Editor.exe"),
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
            return _userSettings.Gui.MainForm;
        }

        public static TestCaseData[] TestCases = new TestCaseData[]
        {
            new TestCaseData("Left", 10, 0),
            new TestCaseData("Top", 10, 0),
            new TestCaseData("Width", 700, 900),
            new TestCaseData("Height", 400, 500),
            new TestCaseData("Maximized", false, true),
            new TestCaseData("SplitPosition", 0, 200)
        };
    }

    public class MiniFormSettingsTests : SettingsTests<MiniFormSettings>
    {
        public MiniFormSettingsTests() : base("Gui.MiniForm") { }

        protected override MiniFormSettings GetSettingsGroup()
        {
            return _userSettings.Gui.MiniForm;
        }

        public static TestCaseData[] TestCases = new TestCaseData[]
        {
            new TestCaseData("Left", 10, 0),
            new TestCaseData("Top", 10, 0),
            new TestCaseData("Width", 700, 900),
            new TestCaseData("Height", 400, 500),
            new TestCaseData("Maximized", false, true),
        };
    }

    public class RecentProjectsSettingsTests : SettingsTests<RecentProjectsSettings>
    {
        public RecentProjectsSettingsTests() : base("Gui.RecentProjects") { }

        protected override RecentProjectsSettings GetSettingsGroup()
        {
            return _userSettings.Gui.RecentProjects;
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
            return _userSettings.Gui.TestTree;
        }

        public static TestCaseData[] TestCases = new TestCaseData[]
        {
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
            return _userSettings.Gui.TextOutput;
        }

        public static TestCaseData[] TestCases = new TestCaseData[]
        {
            new TestCaseData("WordWrapEnabled", true, false),
            new TestCaseData("Labels", "ON", "AFTER")
        };
    }
}
