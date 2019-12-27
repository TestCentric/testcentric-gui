// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Model.Settings
{
    public class TextOutputSettings : SettingsGroup
    {
        public TextOutputSettings(ISettings settings, string prefix)
             : base(settings, prefix + "TextOutput") { }

        public bool WordWrapEnabled
        {
            get { return GetSetting(nameof(WordWrapEnabled), true); }
            set { SaveSetting(nameof(WordWrapEnabled), value); }
        }

        public string Labels
        {
            get { return GetSetting(nameof(Labels), "ON").ToUpper(); }
            set { SaveSetting(nameof(Labels), value); }
        }
    }
}
