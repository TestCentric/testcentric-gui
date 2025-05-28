// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

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

        public int InitialTreeDisplay
        {
            get { return GetSetting(nameof(InitialTreeDisplay), 0); }
            set { SaveSetting(nameof(InitialTreeDisplay), value); }
        }

        public string AlternateImageSet
        {
            get 
            {
                // Image set 'Default' is removed:
                // If Image set 'Default' is still present in settings, use the new default image set 'Classic' instead
                string imageSet = GetSetting(nameof(AlternateImageSet), "Classic");
                if (imageSet == "Default")
                {
                    imageSet = "Classic";
                    AlternateImageSet = imageSet;
                }

                return imageSet;
            }
            set { SaveSetting(nameof(AlternateImageSet), value); }
        }

        public bool ShowCheckBoxes
        {
            get { return GetSetting(nameof(ShowCheckBoxes), false); }
            set { SaveSetting(nameof(ShowCheckBoxes), value); }
        }

        public bool ShowTestDuration
        {
            get { return GetSetting(nameof(ShowTestDuration), false); }
            set { SaveSetting(nameof(ShowTestDuration), value); }
        }

        public string DisplayFormat
        {
            get { return GetSetting(nameof(DisplayFormat), "NUNIT_TREE"); }
            set { SaveSetting(nameof(DisplayFormat), value); }
        }

        public bool ShowNamespace
        {
            get { return GetSetting(nameof(ShowNamespace), true); }
            set { SaveSetting(nameof(ShowNamespace), value); }
        }

        public bool ShowFilter
        {
            get { return GetSetting(nameof(ShowFilter), true); }
            set { SaveSetting(nameof(ShowFilter), value); }
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

