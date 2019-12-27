// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Drawing;

namespace TestCentric.Gui.Model.Settings
{
    /// <summary>
    /// Settings specific to TestCentric. Because we store settings in the
    /// NUnit 3 settings file, we use our own unique prefix to avoid conflicts.
    /// </summary>
    public class GuiSettings : SettingsGroup
    {
        public GuiSettings(ISettings settings, string prefix)
             : base(settings, prefix + "Gui") { }

        public TestTreeSettings TestTree
        {
            get { return new TestTreeSettings(_settingsService, GroupPrefix); }
        }

        public RecentProjectsSettings RecentProjects
        {
            get { return new RecentProjectsSettings(_settingsService, GroupPrefix); }
        }

        public MiniFormSettings MiniForm
        {
            get { return new MiniFormSettings(_settingsService, GroupPrefix); }
        }

        public MainFormSettings MainForm
        {
            get { return new MainFormSettings(_settingsService, GroupPrefix); }
        }

        public ErrorDisplaySettings ErrorDisplay
        {
            get { return new ErrorDisplaySettings(_settingsService, GroupPrefix); }
        }

        public TextOutputSettings TextOutput
        {
            get { return new TextOutputSettings(_settingsService, GroupPrefix); }
        }

        public string DisplayFormat
        {
            get { return GetSetting(nameof(DisplayFormat), "Full"); }
            set { SaveSetting(nameof(DisplayFormat), value); }
        }

        public bool LoadLastProject
        {
            get { return GetSetting(nameof(LoadLastProject), true); }
            set { SaveSetting(nameof(LoadLastProject), value); }
        }

        public int SelectedTab
        {
            get { return GetSetting(nameof(SelectedTab), 0); }
            set { SaveSetting(nameof(SelectedTab), value); }
        }

        public string InitialSettingsPage
        {
            get { return (string)GetSetting(nameof(InitialSettingsPage)); }
            set { SaveSetting(nameof(InitialSettingsPage), value); }
        }

        public bool ClearResultsOnReload
        {
            get { return GetSetting(nameof(ClearResultsOnReload), false); }
            set { SaveSetting(nameof(ClearResultsOnReload), value); }
        }

        private static readonly Font DefaultFont = new Font(FontFamily.GenericSansSerif, 8.25f);
        public Font Font
        {
            get { return GetSetting(nameof(Font), DefaultFont); }
            set { SaveSetting(nameof(Font), value); }
        }

        private static readonly Font DefaultFixedFont = new Font(FontFamily.GenericMonospace, 8.0F);
        public Font FixedFont
        {
            get { return GetSetting(nameof(FixedFont), DefaultFixedFont); }
            set { SaveSetting(nameof(FixedFont), value); }
        }

        public Engine.InternalTraceLevel InternalTraceLevel
        {
            get { return GetSetting(nameof(InternalTraceLevel), Engine.InternalTraceLevel.Off); }
            set { SaveSetting(nameof(InternalTraceLevel), value); }
        }
    }
}
