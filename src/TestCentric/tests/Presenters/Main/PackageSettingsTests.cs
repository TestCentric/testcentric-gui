// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters.Main
{
    using Elements;

    public class PackageSettingsTests : MainPresenterTestBase
    {
        [TestCase("Separate")]
        [TestCase("Multiple")]
        [TestCase("InProcess")]
        [TestCase("INVALID", Description = "Invalid Setting is passed on to the model")]
        public void ProcessModel_SettingChanged(string value)
        {
            _view.ProcessModel.SelectedItem.Returns(value);

            _view.ProcessModel.SelectionChanged += Raise.Event<CommandHandler>();

            _model.PackageOverrides.Received(1)["ProcessModel"] = value;
        }

        [Test]
        public void ProcessModel_SetToDefault()
        {
            _view.ProcessModel.SelectedItem.Returns("DEFAULT");

            _view.ProcessModel.SelectionChanged += Raise.Event<CommandHandler>();

            _model.PackageOverrides.Received(1).Remove("ProcessModel");
        }

        [TestCase("Single")]
        [TestCase("Multiple")]
        [TestCase("INVALID", Description = "Invalid Setting is passed on to the model")]
        public void DomainUsage_SettingChanged(string value)
        {
            _view.DomainUsage.SelectedItem.Returns(value);

            _view.DomainUsage.SelectionChanged += Raise.Event<CommandHandler>();

            _model.PackageOverrides.Received(1)["DomainUsage"] = value;
        }

        [Test]
        public void DomainUsage_SetToDefault()
        {
            _view.DomainUsage.SelectedItem.Returns("DEFAULT");

            _view.DomainUsage.SelectionChanged += Raise.Event<CommandHandler>();

            _model.PackageOverrides.Received(1).Remove("DomainUsage");
        }

        [TestCase("net-2.0")]
        [TestCase("net-4.5")]
        [TestCase("INVALID", Description = "Invalid Setting is passed on to the model")]
        [TestCase("net-2.0", "net-4.5")]
        public void SelectedRuntime_SettingChanged(params string[] settings)
        {
            foreach (var setting in settings)
            {
                _view.SelectedRuntime.SelectedItem.Returns(setting);

                _view.SelectedRuntime.SelectionChanged += Raise.Event<CommandHandler>();

                _model.PackageOverrides.Received(1)["RuntimeFramework"] = setting;
            }
        }

        public void SelectedRuntime_MultipleChanges()
        {

        }

        [Test]
        public void SelectedRuntime_SetToDefault()
        {
            _view.SelectedRuntime.SelectedItem.Returns("DEFAULT");

            _view.SelectedRuntime.SelectionChanged += Raise.Event<CommandHandler>();

            _model.PackageOverrides.Received(1).Remove("RuntimeFramework");
        }
    }
}
