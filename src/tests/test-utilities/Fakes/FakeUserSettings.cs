// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TestCentric.TestUtilities.Fakes
{
    public class UserSettings : TestCentric.Gui.Model.Settings.UserSettings
    {
        public UserSettings() : base(new TestCentric.Gui.Model.Settings.SettingsStore()) { }
    }
}
