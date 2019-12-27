// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Model.Settings
{
    public class RecentProjectsSettings : SettingsGroup
    {
        public RecentProjectsSettings(ISettings settings, string prefix)
            : base(settings, prefix + "RecentProjects") { }

        public int MaxFiles
        {
            get { return GetSetting(nameof(MaxFiles), 24); }
            set { SaveSetting(nameof(MaxFiles), value); }
        }

        public bool CheckFilesExist
        {
            get { return GetSetting(nameof(CheckFilesExist), true); }
            set { SaveSetting(nameof(CheckFilesExist), value); }
        }
    }
}
