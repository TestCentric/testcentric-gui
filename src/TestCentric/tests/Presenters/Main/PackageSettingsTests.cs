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
        //[TestCase("net-2.0")]
        //[TestCase("net-4.5")]
        //[TestCase("INVALID", Description = "Invalid Setting is passed on to the model")]
        //[TestCase("net-2.0", "net-4.5")]
        //public void SelectedRuntime_SettingChanged(params string[] settings)
        //{
        //    foreach (var setting in settings)
        //    {
        //        _view.SelectedRuntime.SelectedItem.Returns(setting);

        //        _view.SelectedRuntime.SelectionChanged += Raise.Event<CommandHandler>();

        //        _model.PackageOverrides.Received(1)["RequestedRuntimeFramework"] = setting;
        //    }
        //}

        public void SelectedRuntime_MultipleChanges()
        {

        }

        //[Test]
        //public void SelectedRuntime_SetToDefault()
        //{
        //    _view.SelectedRuntime.SelectedItem.Returns("DEFAULT");

        //    _view.SelectedRuntime.SelectionChanged += Raise.Event<CommandHandler>();

        //    _model.PackageOverrides.Received(1).Remove("RequestedRuntimeFramework");
        //}
    }
}
