// ****************************************************************
// Copyright 2002-2018, Charlie Poole
// This is free software licensed under the NUnit license, a copy
// of which should be included with this software. If not, you may
// obtain a copy at https://github.com/nunit-legacy/nunitv2.
// ****************************************************************

using System;
using System.Windows.Forms;

namespace TestCentric.Gui.SettingsPages
{
    public partial class ProjectEditorSettingsPage : SettingsPage
    {
        public ProjectEditorSettingsPage(string key) : base(key)
        {
            InitializeComponent();
        }

        public override void LoadSettings()
        {
            string editorPath = (string)Settings.Gui.ProjectEditorPath;

            if (editorPath != null)
            {
                useOtherEditorRadioButton.Checked = true;
                editorPathTextBox.Text = editorPath;
            }
            else
            {
                useNUnitEditorRadioButton.Checked = true;
                editorPathTextBox.Text = "";
            }
        }

        public override void ApplySettings()
        {
            if (useNUnitEditorRadioButton.Checked)
                Settings.Gui.ProjectEditorPath = null;
            else
                Settings.Gui.ProjectEditorPath = editorPathTextBox.Text;
        }

        private void editorPathTextBox_TextChanged(object sender, EventArgs e)
        {
            if (editorPathTextBox.TextLength == 0)
                useNUnitEditorRadioButton.Checked = true;
            else
                useOtherEditorRadioButton.Checked = true;
        }

        private void editorPathBrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if ( Site != null ) dlg.Site = Site;
            dlg.Title = "Select Project Editor";

            dlg.Filter = "Executable Files (*.exe)|*.exe";
            dlg.FilterIndex = 1;
            dlg.FileName = "";

            if ( dlg.ShowDialog( this ) == DialogResult.OK ) 
                editorPathTextBox.Text = dlg.FileName;
        }
    }
}
