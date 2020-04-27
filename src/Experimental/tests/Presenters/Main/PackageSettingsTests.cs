// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters.Main
{
    using Elements;

    public class PackageSettingsTests : MainPresenterTestBase
    {
        static readonly string[] _files = new string[] { "one", "two", "three" };

        [SetUp]
        public void SetUp()
        {
            Model.TestFiles.Returns(new List<string>(_files));
        }

        [TestCase("Single")]
        [TestCase("Multiple")]
        [TestCase("InProcess")]
        [TestCase("INVALID", Description = "Invalid Setting is passed on to the model")]
        public void ProcessModel_SettingChanged(string value)
        {
            View.ProcessModel.SelectedItem.Returns(value);

            View.ProcessModel.SelectionChanged += Raise.Event<CommandHandler>();

            Model.PackageOverrides.Received(1)["ProcessModel"] = value;
        }

        [Test]
        public void ProcessModel_SetToDefault()
        {
            View.ProcessModel.SelectedItem.Returns("DEFAULT");

            View.ProcessModel.SelectionChanged += Raise.Event<CommandHandler>();

            Model.PackageOverrides.Received(1).Remove("ProcessModel");
        }

        [TestCase("Single")]
        [TestCase("Multiple")]
        [TestCase("INVALID", Description = "Invalid Setting is passed on to the model")]
        public void DomainUsage_SettingChanged(string value)
        {
            View.DomainUsage.SelectedItem.Returns(value);

            View.DomainUsage.SelectionChanged += Raise.Event<CommandHandler>();

            Model.PackageOverrides.Received(1)["DomainUsage"] = value;
        }

        [Test]
        public void DomainUsage_SetToDefault()
        {
            View.DomainUsage.SelectedItem.Returns("DEFAULT");

            View.DomainUsage.SelectionChanged += Raise.Event<CommandHandler>();

            Model.PackageOverrides.Received(1).Remove("DomainUsage");
        }

        //[TestCase("net-2.0")]
        //[TestCase("net-4.5")]
        //[TestCase("INVALID", Description = "Invalid Setting is passed on to the model")]
        //[TestCase("net-2.0", "net-4.5")]
        //public void SelectedRuntime_SettingChanged(params string[] settings)
        //{
        //    foreach (var setting in settings)
        //    {
        //        View.SelectedRuntime.SelectedItem.Returns(setting);

        //        View.SelectedRuntime.SelectionChanged += Raise.Event<CommandHandler>();

        //        Model.PackageOverrides.Received(1)["RequestedRuntimeFramework"] = setting;
        //    }
        //}

        public void SelectedRuntime_MultipleChanges()
        {

        }

        //[Test]
        //public void SelectedRuntime_SetToDefault()
        //{
        //    View.SelectedRuntime.SelectedItem.Returns("DEFAULT");

        //    View.SelectedRuntime.SelectionChanged += Raise.Event<CommandHandler>();

        //    Model.PackageOverrides.Received(1).Remove("RequestedRuntimeFramework");
        //}
    }
}
