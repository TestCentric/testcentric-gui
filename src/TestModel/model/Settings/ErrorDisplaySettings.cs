// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;

namespace TestCentric.Gui.Model.Settings
{
    public class ErrorDisplaySettings : SettingsGroup
    {
        public ErrorDisplaySettings(ISettings settings, string prefix)
             : base(settings, prefix + "ErrorDisplay") { }

        public int SplitterPosition
        {
            get { return GetSetting(nameof(SplitterPosition), 0); }
            set { SaveSetting(nameof(SplitterPosition), value); }
        }

        public bool WordWrapEnabled
        {
            get { return GetSetting(nameof(WordWrapEnabled), true); }
            set { SaveSetting(nameof(WordWrapEnabled), value); }
        }

        public bool ToolTipsEnabled
        {
            get { return GetSetting(nameof(ToolTipsEnabled), true); }
            set { SaveSetting(nameof(ToolTipsEnabled), value); }
        }

        public bool SourceCodeDisplay
        {
            get { return GetSetting(nameof(SourceCodeDisplay), false); }
            set { SaveSetting(nameof(SourceCodeDisplay), value); }
        }

        public Orientation SourceCodeSplitterOrientation
        {
            get { return GetSetting(nameof(SourceCodeSplitterOrientation), Orientation.Vertical); }
            set { SaveSetting(nameof(SourceCodeSplitterOrientation), value); }
        }

        public float SourceCodeVerticalSplitterPosition
        {
            get { return GetSetting(nameof(SourceCodeVerticalSplitterPosition), 0.3f); }
            set { SaveSetting(nameof(SourceCodeVerticalSplitterPosition), value); }
        }

        public float SourceCodeHorizontalSplitterPosition
        {
            get { return GetSetting(nameof(SourceCodeHorizontalSplitterPosition), 0.3f); }
            set { SaveSetting(nameof(SourceCodeHorizontalSplitterPosition), value); }
        }
    }
}
