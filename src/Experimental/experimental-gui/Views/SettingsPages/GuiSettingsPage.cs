// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using TestCentric.Gui.Model.Settings;

namespace TestCentric.Gui.Views.SettingsPages
{
    public partial class GuiSettingsPage : SettingsPage
    {
        public GuiSettingsPage(UserSettings settings) : base("Gui.General", settings)
        {
            InitializeComponent();
        }

        public override void LoadSettings()
        {
            recentFilesCountTextBox.Text = Settings.Gui.RecentProjects.MaxFiles.ToString();
            initialDisplayComboBox.SelectedIndex = (int)Settings.Gui.TestTree.InitialTreeDisplay;
            saveVisualStateCheckBox.Checked = Settings.Gui.TestTree.SaveVisualState;
        }

        public override void ApplySettings()
        {
            Settings.Gui.TestTree.InitialTreeDisplay = initialDisplayComboBox.SelectedIndex;
            Settings.Gui.TestTree.SaveVisualState = saveVisualStateCheckBox.Checked;
        }

        private void recentFilesCountTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (recentFilesCountTextBox.Text.Length == 0)
            {
                recentFilesCountTextBox.Text = Settings.Gui.RecentProjects.MaxFiles.ToString();
                recentFilesCountTextBox.SelectAll();
                e.Cancel = true;
            }
            else
            {
                string errmsg = null;

                try
                {
                    int count = int.Parse(recentFilesCountTextBox.Text);

                    if (count < 0 || count > 24)
                    {
                        errmsg = string.Format("Number of files must be from 0 to 24");
                    }
                }
                catch
                {
                    errmsg = "Number of files must be numeric";
                }

                if (errmsg != null)
                {
                    recentFilesCountTextBox.SelectAll();
                    MessageDisplay.Error(errmsg);
                    e.Cancel = true;
                }
            }
        }

        private void recentFilesCountTextBox_Validated(object sender, System.EventArgs e)
        {
            int count = int.Parse(recentFilesCountTextBox.Text);
            Settings.Gui.RecentProjects.MaxFiles = count;
        }
    }
}
