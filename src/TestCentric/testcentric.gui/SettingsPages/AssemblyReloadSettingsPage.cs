// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;

namespace TestCentric.Gui.SettingsPages
{
    public class AssemblyReloadSettingsPage : SettingsPage
    {
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox rerunOnChangeCheckBox;
        private System.Windows.Forms.RadioButton reloadOnRunRadioButton;
        private System.Windows.Forms.RadioButton reloadOnChangeRadioButton;
        private System.Windows.Forms.RadioButton noAutoReloadRadioButton;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private CheckBox clearResultsCheckBox;
        private Label label2;
        private System.ComponentModel.IContainer components = null;

        public AssemblyReloadSettingsPage(string key) : base(key)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rerunOnChangeCheckBox = new System.Windows.Forms.CheckBox();
            this.reloadOnRunRadioButton = new System.Windows.Forms.RadioButton();
            this.reloadOnChangeRadioButton = new System.Windows.Forms.RadioButton();
            this.noAutoReloadRadioButton = new System.Windows.Forms.RadioButton();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.clearResultsCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Automatic Reload";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(181, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(259, 8);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // rerunOnChangeCheckBox
            // 
            this.rerunOnChangeCheckBox.AutoSize = true;
            this.rerunOnChangeCheckBox.Enabled = false;
            this.helpProvider1.SetHelpString(this.rerunOnChangeCheckBox, "If checked, the last tests run will be re-run automatically whenever the assembly" +
        " changes.");
            this.rerunOnChangeCheckBox.Location = new System.Drawing.Point(48, 84);
            this.rerunOnChangeCheckBox.Name = "rerunOnChangeCheckBox";
            this.helpProvider1.SetShowHelp(this.rerunOnChangeCheckBox, true);
            this.rerunOnChangeCheckBox.Size = new System.Drawing.Size(120, 17);
            this.rerunOnChangeCheckBox.TabIndex = 13;
            this.rerunOnChangeCheckBox.Text = "Re-run last tests run";
            // 
            // reloadOnRunRadioButton
            // 
            this.reloadOnRunRadioButton.AutoSize = true;
            this.helpProvider1.SetHelpString(this.reloadOnRunRadioButton, "If checked, the assembly is reloaded before each run");
            this.reloadOnRunRadioButton.Location = new System.Drawing.Point(24, 32);
            this.reloadOnRunRadioButton.Name = "reloadOnRunRadioButton";
            this.helpProvider1.SetShowHelp(this.reloadOnRunRadioButton, true);
            this.reloadOnRunRadioButton.Size = new System.Drawing.Size(157, 17);
            this.reloadOnRunRadioButton.TabIndex = 11;
            this.reloadOnRunRadioButton.Text = "Reload before each test run";
            // 
            // reloadOnChangeRadioButton
            // 
            this.reloadOnChangeRadioButton.AutoSize = true;
            this.helpProvider1.SetHelpString(this.reloadOnChangeRadioButton, "If checked, the assembly is reloaded whenever it changes. Changes to this setting" +
        " do not take effect until the next time an assembly is loaded.");
            this.reloadOnChangeRadioButton.Location = new System.Drawing.Point(24, 60);
            this.reloadOnChangeRadioButton.Name = "reloadOnChangeRadioButton";
            this.helpProvider1.SetShowHelp(this.reloadOnChangeRadioButton, true);
            this.reloadOnChangeRadioButton.Size = new System.Drawing.Size(198, 17);
            this.reloadOnChangeRadioButton.TabIndex = 12;
            this.reloadOnChangeRadioButton.Text = "Reload when test assembly changes";
            this.reloadOnChangeRadioButton.CheckedChanged += new System.EventHandler(this.reloadOnChangeRadioButton_CheckedChanged);
            // 
            // noAutoReloadRadioButton
            // 
            this.noAutoReloadRadioButton.AutoSize = true;
            this.helpProvider1.SetHelpString(this.noAutoReloadRadioButton, "If checked, the assembly is never reloaded automatically");
            this.noAutoReloadRadioButton.Location = new System.Drawing.Point(24, 107);
            this.noAutoReloadRadioButton.Name = "noAutoReloadRadioButton";
            this.helpProvider1.SetShowHelp(this.noAutoReloadRadioButton, true);
            this.noAutoReloadRadioButton.Size = new System.Drawing.Size(120, 17);
            this.noAutoReloadRadioButton.TabIndex = 13;
            this.noAutoReloadRadioButton.Text = "No automatic reload";
            // 
            // clearResultsCheckBox
            // 
            this.clearResultsCheckBox.AutoSize = true;
            this.clearResultsCheckBox.Enabled = false;
            this.helpProvider1.SetHelpString(this.clearResultsCheckBox, "If checked, any prior results are cleared when reloading");
            this.clearResultsCheckBox.Location = new System.Drawing.Point(24, 151);
            this.clearResultsCheckBox.Name = "clearResultsCheckBox";
            this.helpProvider1.SetShowHelp(this.clearResultsCheckBox, true);
            this.clearResultsCheckBox.Size = new System.Drawing.Size(161, 17);
            this.clearResultsCheckBox.TabIndex = 35;
            this.clearResultsCheckBox.Text = "Clear results when reloading.";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(21, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(336, 33);
            this.label2.TabIndex = 36;
            this.label2.Text = "This setting is currently disabled. Results are aloways cleared due to  a problem" +
    " in the engine Reload function.";
            // 
            // AssemblyReloadSettingsPage
            // 
            this.Controls.Add(this.label2);
            this.Controls.Add(this.noAutoReloadRadioButton);
            this.Controls.Add(this.clearResultsCheckBox);
            this.Controls.Add(this.rerunOnChangeCheckBox);
            this.Controls.Add(this.reloadOnRunRadioButton);
            this.Controls.Add(this.reloadOnChangeRadioButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Name = "AssemblyReloadSettingsPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        public override void LoadSettings()
        {
            if (Settings.Engine.ReloadOnChange)
                reloadOnChangeRadioButton.Checked = true;
            else if (Settings.Engine.ReloadOnRun)
                reloadOnRunRadioButton.Checked = true;
            else
                noAutoReloadRadioButton.Checked = true;

            rerunOnChangeCheckBox.Checked = Settings.Engine.RerunOnChange;
            clearResultsCheckBox.Checked = Settings.Gui.ClearResultsOnReload;
        }

        public override void ApplySettings()
        {
            Settings.Engine.ReloadOnChange = reloadOnChangeRadioButton.Checked;
            Settings.Engine.ReloadOnRun = reloadOnRunRadioButton.Checked;

            Settings.Engine.RerunOnChange = rerunOnChangeCheckBox.Checked;
            Settings.Gui.ClearResultsOnReload = clearResultsCheckBox.Checked;
        }



        private void reloadOnChangeRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            rerunOnChangeCheckBox.Enabled = reloadOnChangeRadioButton.Checked;
        }

        protected override void OnHelpRequested(HelpEventArgs hevent)
        {
            System.Diagnostics.Process.Start("http://nunit.com/?p=optionsDialog&r=2.4.5");
        }

    }
}

