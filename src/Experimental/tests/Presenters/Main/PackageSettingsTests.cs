// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
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
