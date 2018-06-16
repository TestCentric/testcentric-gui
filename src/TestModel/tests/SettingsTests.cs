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

namespace TestCentric.Gui.Model.Settings
{
    public abstract class SettingsTests<TSettings> : ISettings where TSettings : SettingsGroup
    {
        protected object _settings;
        protected Dictionary<string, object> _storage;

        private readonly string _prefix;

        public SettingsTests(string prefix)
        {
            _prefix = prefix ?? string.Empty;

            if (_prefix != string.Empty && !prefix.EndsWith("."))
                _prefix += ".";
        }

        [SetUp]
        public void SetUp()
        {
            _storage = new Dictionary<string, object>();
            _settings = Activator.CreateInstance(typeof(TSettings), this);
        }

        [TestCaseSource("TestCases")]
        public void CheckSetting<TValue>(string propertyName, TValue defaultValue, TValue testValue)
        {
            // Check that the property exists
            var propInfo = GetProperty(propertyName);

            // Check the default value
            Assert.That(propInfo.GetValue(_settings), Is.EqualTo(defaultValue), "Incorrect default value");

            // Set the property and verify that it changed
            propInfo.SetValue(_settings, testValue);
            Assert.That(propInfo.GetValue(_settings), Is.EqualTo(testValue), "Value did not change");

            // Check that correct storage key was used
            Assert.That(_storage[_prefix + propertyName], Is.EqualTo(testValue), "Incorrect storage key");
        }

        protected PropertyInfo GetProperty(string propertyName)
        {
            var propInfo = _settings.GetType().GetProperty(propertyName);
            Assert.NotNull(propInfo, $"Property {propertyName} not found.");

            return propInfo;
        }

        #region ISettings Implementation

        public event SettingsEventHandler Changed;

        public object GetSetting(string settingName)
        {
            return _storage.ContainsKey(settingName)
                ? _storage[settingName]
                : null;
        }

        public T GetSetting<T>(string settingName, T defaultValue)
        {
            return _storage.ContainsKey(settingName)
                ? (T)_storage[settingName]
                : defaultValue;
        }

        public void RemoveGroup(string groupName)
        {
            throw new NotImplementedException();
        }

        public void RemoveSetting(string settingName)
        {
            throw new NotImplementedException();
        }

        public void SaveSetting(string settingName, object settingValue)
        {
            _storage[settingName] = settingValue;
        }

        #endregion
    }

    public class EngineSettingsTests : SettingsTests<EngineSettings>
    {
        public EngineSettingsTests() : base("Engine.Options") { }

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
            var propInfo = GetProperty(propertyName);
            Assert.That(propInfo.PropertyType, Is.EqualTo(expectedType));
        }
    }

    public class MainFormSettingsTests : SettingsTests<MainFormSettings>
    {
        public MainFormSettingsTests() : base("Gui.MainForm") { }

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

        public static TestCaseData[] TestCases = new TestCaseData[]
        {
            new TestCaseData("MaxFiles", 24, 12),
            new TestCaseData("CheckFilesExist", true, false)
        };
    }

    public class TestTreeSettingsTests : SettingsTests<TestTreeSettings>
    {
        public TestTreeSettingsTests() : base("Gui.TestTree") { }

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

        public static TestCaseData[] TestCases = new TestCaseData[]
        {
            new TestCaseData("WordWrapEnabled", true, false),
            new TestCaseData("Labels", "ON", "AFTER")
        };
    }
}
