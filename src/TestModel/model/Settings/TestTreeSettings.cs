// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using TestCentric.Engine;

namespace TestCentric.Gui.Model.Settings
{
    public class TestTreeSettings : SettingsGroup
    {
        public TestTreeSettings(ISettings settings, string prefix)
             : base(settings, prefix + "TestTree") { }

        public FixtureListSettings FixtureList
        {
            get { return new FixtureListSettings(_settingsService, GroupPrefix); }
        }

        public TestListSettings TestList
        {
            get { return new TestListSettings(_settingsService, GroupPrefix); }
        }

        public string DisplayFormat
        {
            get { return GetSetting(nameof(DisplayFormat), "NUNIT_TREE"); }
            set { SaveSetting(nameof(DisplayFormat), value); }
        }

        public bool SaveVisualState
        {
            get { return GetSetting(nameof(SaveVisualState), true); }
            set { SaveSetting(nameof(SaveVisualState), value); }
        }

        public int InitialTreeDisplay
        {
            get { return GetSetting(nameof(InitialTreeDisplay), 0); }
            set { SaveSetting(nameof(InitialTreeDisplay), value); }
        }

        public string AlternateImageSet
        {
            get { return GetSetting(nameof(AlternateImageSet), "Default"); }
            set { SaveSetting(nameof(AlternateImageSet), value); }
        }

        public bool ShowCheckBoxes
        {
            get { return GetSetting(nameof(ShowCheckBoxes), false); }
            set { SaveSetting(nameof(ShowCheckBoxes), value); }
        }

        public class FixtureListSettings : SettingsGroup
        {
            public FixtureListSettings(ISettings settings, string prefix) : base(settings, prefix + "FixtureList") { }

            private string groupByKey = "GroupBy";
            public string GroupBy
            {
                get { return GetSetting(groupByKey, "OUTCOME"); }
                set { SaveSetting(groupByKey, value); }
            }
        }

        public class TestListSettings : SettingsGroup
        {
            public TestListSettings(ISettings settings, string prefix) : base(settings, prefix + "TestList") { }

            private string groupByKey = "GroupBy";
            public string GroupBy
            {
                get { return GetSetting(groupByKey, "OUTCOME"); }
                set { SaveSetting(groupByKey, value); }
            }
        }
    }
}

