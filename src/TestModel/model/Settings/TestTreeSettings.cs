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

using NUnit.Engine;

namespace TestCentric.Gui.Model.Settings
{
    public class TestTreeSettings : SettingsGroup
    {
        public TestTreeSettings(ISettings settings, string prefix)
             : base(settings, prefix + "Gui.TestTree") { }

        public FixtureListSettings FixtureList
        {
            get { return new FixtureListSettings(_settingsService, _prefix); }
        }

        public TestListSettings TestList
        {
            get { return new TestListSettings(_settingsService, _prefix); }
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
            public FixtureListSettings(ISettings settings, string prefix) : base(settings, prefix + "Gui.TestTree.FixtureList") { }

            private string groupByKey = "GroupBy";
            public string GroupBy
            {
                get { return GetSetting(groupByKey, "OUTCOME"); }
                set { SaveSetting(groupByKey, value); }
            }
        }

        public class TestListSettings : SettingsGroup
        {
            public TestListSettings(ISettings settings, string prefix) : base(settings, prefix + "Gui.TestTree.TestList") { }

            private string groupByKey = "GroupBy";
            public string GroupBy
            {
                get { return GetSetting(groupByKey, "OUTCOME"); }
                set { SaveSetting(groupByKey, value); }
            }
        }
    }
}

