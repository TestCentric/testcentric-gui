// ***********************************************************************
// Copyright (c) 2019 Charlie Poole
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

using System.Drawing;
using System.ComponentModel;
using NUnit.Framework;
using TestCentric.Engine;
using NSubstitute;

namespace TestCentric.Gui.Model
{
    [TestFixture]
    public class SettingsGroupTests
    {
        private const string PREFIX = "Testing.";

        private SettingsGroup _settings;
        private ISettings _settingsService;

        [SetUp]
        public void BeforeEachTest()
        {
            _settingsService = Substitute.For<ISettings>();
            _settings = new SettingsGroup(_settingsService, PREFIX);
        }

        [Test]
        public void PrefixIsSetCorrectly()
        {
            Assert.That(_settings.GroupPrefix, Is.EqualTo(PREFIX));
        }
        

        [Test]
        public void GetSetting_WhenNotPresent_ReturnsNull()
        {
            Assert.IsNull(_settings.GetSetting("X"));
        }

        [TestCaseSource(nameof(TestCases))]
        public void GetSetting_WhenPresent_ReturnsValue<T>(string name, T expected)
        {
            _settingsService.GetSetting(_settings.GroupPrefix + name).Returns(expected);

            object actual = _settings.GetSetting(name);

            _settingsService.Received().GetSetting(_settings.GroupPrefix + name);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCaseSource(nameof(TestCases))]
        public void GetSettingWithDefault_WhenNotPresent_ReturnsDefault<T>(string name, T expected)
        {
            _settingsService.GetSetting(_settings.GroupPrefix + name, expected).Returns(expected);

            T actual = _settings.GetSetting(name, expected);

            _settingsService.Received().GetSetting(_settings.GroupPrefix + name, expected);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCaseSource(nameof(TestCases))]
        public void GetSettingWithDefault_WhenPresent_ReturnsValue<T>(string name, T expected)
        {
            _settingsService.GetSetting(_settings.GroupPrefix + name, expected).Returns(expected);

            object actual = _settings.GetSetting(name, expected);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCaseSource(nameof(TestCases))]
        public void SaveSetting_CallsSettingsServiceCorrectly<T>(string name, T expected)
        {
            _settings.SaveSetting(name, expected);

            _settingsService.Received().SaveSetting(_settings.GroupPrefix + name, expected);
        }

        [Test]
        public void RemoveSetting_CallsSettingsServiceCorrectly()
        {
            _settings.RemoveSetting("JUNK");

            _settingsService.Received().RemoveSetting(_settings.GroupPrefix + "JUNK");
        }

        [Test]
        public void RemoveGroup_CallsSettingsServiceCorrectly()
        {
            _settings.RemoveGroup("SUBGROUP");

            _settingsService.Received().RemoveGroup(_settings.GroupPrefix + "SUBGROUP");
        }

        private static TestCaseData[] TestCases = new TestCaseData[] {
            new TestCaseData("X", 5),
            new TestCaseData("X", 5),
            new TestCaseData("Y", 2.5),
            new TestCaseData("NAME", "Charlie"),
            new TestCaseData("Flag", true),
            new TestCaseData("Priority", PriorityValue.A)
        };

        private enum PriorityValue
        {
            A,
            B,
            C
        };
    }
}
