// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;
using TestCentric.Gui.Model.Settings;

namespace TestCentric.Gui.Views.SettingsPages
{
    public partial class AssemblyReloadSettingsPage : SettingsPage
    {
        public AssemblyReloadSettingsPage(UserSettings settings) : base("Engine.Assembly Reload", settings)
        {
            InitializeComponent();
        }

        public override void LoadSettings()
        {
            reloadOnChangeCheckBox.Checked = Settings.Engine.ReloadOnChange;
            rerunOnChangeCheckBox.Checked = Settings.Engine.RerunOnChange;
            reloadOnRunCheckBox.Checked = Settings.Engine.ReloadOnRun;
        }

        public override void ApplySettings()
        {
            Settings.Engine.ReloadOnChange = reloadOnChangeCheckBox.Checked;
            Settings.Engine.RerunOnChange = rerunOnChangeCheckBox.Checked;
            Settings.Engine.ReloadOnRun = reloadOnRunCheckBox.Checked;
        }



        private void reloadOnChangeCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // TODO: Waiting for issue #233
            //rerunOnChangeCheckBox.Enabled = reloadOnChangeCheckBox.Checked;
        }

        protected override void OnHelpRequested(HelpEventArgs hevent)
        {
            System.Diagnostics.Process.Start("http://nunit.com/?p=optionsDialog&r=2.4.5");
        }
    }
}
