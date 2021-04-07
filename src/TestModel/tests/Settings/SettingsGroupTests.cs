// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Framework;
using NSubstitute;

namespace TestCentric.Gui.Model.Settings
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
