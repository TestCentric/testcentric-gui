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

using System;
using System.Windows.Forms;

namespace TestCentric.Gui.SettingsPages
{
    public class TestLoaderSettingsPage : SettingsPage
    {
        private System.Windows.Forms.RadioButton singleDomainRadioButton;
        private System.Windows.Forms.RadioButton multiDomainRadioButton;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private Label label3;
        private GroupBox groupBox3;
        private RadioButton multiProcessRadioButton;
        private RadioButton separateProcessRadioButton;
        private RadioButton sameProcessRadioButton;
        private Label label2;
        private GroupBox groupBox2;
        private NumericUpDown numberOfAgentsUpDown;
        private CheckBox numberOfAgentsCheckBox;
        private RadioButton defaultDomainRadioButton;
        private RadioButton defaultProcessRadioButton;
        private System.ComponentModel.IContainer components = null;

        public TestLoaderSettingsPage(string key) : base(key)
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
            this.singleDomainRadioButton = new System.Windows.Forms.RadioButton();
            this.multiDomainRadioButton = new System.Windows.Forms.RadioButton();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.multiProcessRadioButton = new System.Windows.Forms.RadioButton();
            this.separateProcessRadioButton = new System.Windows.Forms.RadioButton();
            this.sameProcessRadioButton = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numberOfAgentsUpDown = new System.Windows.Forms.NumericUpDown();
            this.numberOfAgentsCheckBox = new System.Windows.Forms.CheckBox();
            this.defaultDomainRadioButton = new System.Windows.Forms.RadioButton();
            this.defaultProcessRadioButton = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfAgentsUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // singleDomainRadioButton
            // 
            this.singleDomainRadioButton.AutoCheck = false;
            this.singleDomainRadioButton.AutoSize = true;
            this.helpProvider1.SetHelpString(this.singleDomainRadioButton, "If selected, all test assemblies will be loaded in the same AppDomain.");
            this.singleDomainRadioButton.Location = new System.Drawing.Point(32, 280);
            this.singleDomainRadioButton.Name = "singleDomainRadioButton";
            this.helpProvider1.SetShowHelp(this.singleDomainRadioButton, true);
            this.singleDomainRadioButton.Size = new System.Drawing.Size(194, 17);
            this.singleDomainRadioButton.TabIndex = 9;
            this.singleDomainRadioButton.TabStop = true;
            this.singleDomainRadioButton.Text = "Use a single AppDomain for all tests";
            this.singleDomainRadioButton.Click += new System.EventHandler(this.DomainUsage_Click);
            // 
            // multiDomainRadioButton
            // 
            this.multiDomainRadioButton.AutoCheck = false;
            this.multiDomainRadioButton.AutoSize = true;
            this.helpProvider1.SetHelpString(this.multiDomainRadioButton, "If selected, each test assembly will be loaded in a separate AppDomain.");
            this.multiDomainRadioButton.Location = new System.Drawing.Point(32, 250);
            this.multiDomainRadioButton.Name = "multiDomainRadioButton";
            this.helpProvider1.SetShowHelp(this.multiDomainRadioButton, true);
            this.multiDomainRadioButton.Size = new System.Drawing.Size(220, 17);
            this.multiDomainRadioButton.TabIndex = 8;
            this.multiDomainRadioButton.Text = "Use a separate AppDomain per Assembly";
            this.multiDomainRadioButton.Click += new System.EventHandler(this.DomainUsage_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 35;
            this.label3.Text = "Process Model";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Location = new System.Drawing.Point(199, 8);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(253, 8);
            this.groupBox3.TabIndex = 34;
            this.groupBox3.TabStop = false;
            // 
            // multiProcessRadioButton
            // 
            this.multiProcessRadioButton.AutoSize = true;
            this.multiProcessRadioButton.Location = new System.Drawing.Point(32, 70);
            this.multiProcessRadioButton.Name = "multiProcessRadioButton";
            this.multiProcessRadioButton.Size = new System.Drawing.Size(298, 17);
            this.multiProcessRadioButton.TabIndex = 36;
            this.multiProcessRadioButton.Text = "Run tests in separate agent processes, one per Assembly.";
            this.multiProcessRadioButton.CheckedChanged += new System.EventHandler(this.ProcessModel_CheckedChanged);
            // 
            // separateProcessRadioButton
            // 
            this.separateProcessRadioButton.AutoSize = true;
            this.separateProcessRadioButton.Location = new System.Drawing.Point(32, 126);
            this.separateProcessRadioButton.Name = "separateProcessRadioButton";
            this.separateProcessRadioButton.Size = new System.Drawing.Size(204, 17);
            this.separateProcessRadioButton.TabIndex = 37;
            this.separateProcessRadioButton.Text = "Run tests in a single separate process";
            this.separateProcessRadioButton.CheckedChanged += new System.EventHandler(this.ProcessModel_CheckedChanged);
            // 
            // sameProcessRadioButton
            // 
            this.sameProcessRadioButton.AutoSize = true;
            this.sameProcessRadioButton.Location = new System.Drawing.Point(32, 160);
            this.sameProcessRadioButton.Name = "sameProcessRadioButton";
            this.sameProcessRadioButton.Size = new System.Drawing.Size(232, 17);
            this.sameProcessRadioButton.TabIndex = 38;
            this.sameProcessRadioButton.Text = "Run tests directly in the TestCentric process";
            this.sameProcessRadioButton.CheckedChanged += new System.EventHandler(this.ProcessModel_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 197);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 40;
            this.label2.Text = "Domain Usage";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Location = new System.Drawing.Point(199, 197);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(253, 8);
            this.groupBox2.TabIndex = 39;
            this.groupBox2.TabStop = false;
            // 
            // numberOfAgentsUpDown
            // 
            this.numberOfAgentsUpDown.Enabled = false;
            this.numberOfAgentsUpDown.Location = new System.Drawing.Point(348, 94);
            this.numberOfAgentsUpDown.Name = "numberOfAgentsUpDown";
            this.numberOfAgentsUpDown.Size = new System.Drawing.Size(66, 20);
            this.numberOfAgentsUpDown.TabIndex = 41;
            // 
            // numberOfAgentsCheckBox
            // 
            this.numberOfAgentsCheckBox.AutoSize = true;
            this.numberOfAgentsCheckBox.Location = new System.Drawing.Point(48, 97);
            this.numberOfAgentsCheckBox.Name = "numberOfAgentsCheckBox";
            this.numberOfAgentsCheckBox.Size = new System.Drawing.Size(175, 17);
            this.numberOfAgentsCheckBox.TabIndex = 42;
            this.numberOfAgentsCheckBox.Text = "LImit simultaneous processes to";
            this.numberOfAgentsCheckBox.UseVisualStyleBackColor = true;
            this.numberOfAgentsCheckBox.CheckedChanged += new System.EventHandler(this.numberOfAgentsCheckBox_CheckedChanged);
            // 
            // defaultDomainRadioButton
            // 
            this.defaultDomainRadioButton.AutoCheck = false;
            this.defaultDomainRadioButton.AutoSize = true;
            this.defaultDomainRadioButton.Checked = true;
            this.defaultDomainRadioButton.Location = new System.Drawing.Point(33, 222);
            this.defaultDomainRadioButton.Name = "defaultDomainRadioButton";
            this.defaultDomainRadioButton.Size = new System.Drawing.Size(115, 17);
            this.defaultDomainRadioButton.TabIndex = 43;
            this.defaultDomainRadioButton.Text = "Use Default setting";
            this.defaultDomainRadioButton.UseVisualStyleBackColor = true;
            this.defaultDomainRadioButton.Click += new System.EventHandler(this.DomainUsage_Click);
            // 
            // defaultProcessRadioButton
            // 
            this.defaultProcessRadioButton.AutoSize = true;
            this.defaultProcessRadioButton.Checked = true;
            this.defaultProcessRadioButton.Location = new System.Drawing.Point(32, 36);
            this.defaultProcessRadioButton.Name = "defaultProcessRadioButton";
            this.defaultProcessRadioButton.Size = new System.Drawing.Size(115, 17);
            this.defaultProcessRadioButton.TabIndex = 44;
            this.defaultProcessRadioButton.TabStop = true;
            this.defaultProcessRadioButton.Text = "Use Default setting";
            this.defaultProcessRadioButton.UseVisualStyleBackColor = true;
            this.defaultProcessRadioButton.CheckedChanged += new System.EventHandler(this.ProcessModel_CheckedChanged);
            // 
            // TestLoaderSettingsPage
            // 
            this.Controls.Add(this.defaultProcessRadioButton);
            this.Controls.Add(this.defaultDomainRadioButton);
            this.Controls.Add(this.numberOfAgentsCheckBox);
            this.Controls.Add(this.numberOfAgentsUpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.sameProcessRadioButton);
            this.Controls.Add(this.separateProcessRadioButton);
            this.Controls.Add(this.multiProcessRadioButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.singleDomainRadioButton);
            this.Controls.Add(this.multiDomainRadioButton);
            this.Name = "TestLoaderSettingsPage";
            this.Size = new System.Drawing.Size(456, 341);
            ((System.ComponentModel.ISupportInitialize)(this.numberOfAgentsUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        public override void LoadSettings()
        {
            string processModel = Settings.Engine.ProcessModel;
            string domainUsage = Settings.Engine.DomainUsage;
            int agents = Settings.Engine.Agents;

            if (processModel == "Multiple")
                SelectProcessModelButton(multiProcessRadioButton);
            else if (processModel == "Single")
                SelectProcessModelButton(sameProcessRadioButton);
            else if (processModel == "Separate")
                SelectProcessModelButton(separateProcessRadioButton);
            else
                SelectProcessModelButton(defaultProcessRadioButton);

            numberOfAgentsCheckBox.Enabled = multiProcessRadioButton.Checked;
            numberOfAgentsCheckBox.Checked = agents > 0;
            numberOfAgentsUpDown.Value = agents;

            if (domainUsage == "Multiple")
                SelectDomainUsageButton(multiDomainRadioButton);
            else if (domainUsage == "Single")
                SelectDomainUsageButton(singleDomainRadioButton);
            else
                SelectDomainUsageButton(defaultDomainRadioButton);

            defaultDomainRadioButton.Enabled = multiDomainRadioButton.Enabled = singleDomainRadioButton.Enabled = processModel != "Multiple";
        }

        public override void ApplySettings()
        {
            Settings.Engine.ProcessModel = sameProcessRadioButton.Checked
                ? "Single"
                : separateProcessRadioButton.Checked
                    ? "Separate"
                    : multiProcessRadioButton.Checked
                        ? "Multiple"
                        : "Default";

            Settings.Engine.Agents = numberOfAgentsCheckBox.Checked
                ? (int)numberOfAgentsUpDown.Value
                : 0;

            Settings.Engine.DomainUsage = singleDomainRadioButton.Checked
                ? "Single"
                : multiDomainRadioButton.Checked
                    ? "Multiple"
                    : "Default";
        }

        public override bool HasChangesRequiringReload
        {
            get
            {
                return
                    Settings.Engine.ProcessModel != SelectedProcessModel() ||
                    Settings.Engine.DomainUsage != SelectedDomainUsage() ||
                    Settings.Engine.Agents != numberOfAgentsUpDown.Value;
            }
        }

        private void ProcessModel_CheckedChanged(object sender, EventArgs e)
        {
            bool multiProcess = multiProcessRadioButton.Checked;
            numberOfAgentsCheckBox.Enabled = multiProcess;
            numberOfAgentsUpDown.Enabled = multiProcess && numberOfAgentsCheckBox.Checked;

            singleDomainRadioButton.Enabled = multiDomainRadioButton.Enabled = defaultDomainRadioButton.Enabled = !multiProcess;
        }

        private void numberOfAgentsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            numberOfAgentsUpDown.Enabled = numberOfAgentsCheckBox.Checked;
        }

        private void DomainUsage_Click(object sender, EventArgs e)
        {
            SelectDomainUsageButton(sender as RadioButton);
        }

        private string SelectedProcessModel()
        {
            return sameProcessRadioButton.Checked
                ? "Single"
                : separateProcessRadioButton.Checked
                    ? "Separate"
                    : "Multiple";
        }

        private void SelectProcessModelButton(RadioButton selected)
        {
            defaultProcessRadioButton.Checked = selected == defaultProcessRadioButton;
            sameProcessRadioButton.Checked = selected == sameProcessRadioButton;
            separateProcessRadioButton.Checked = selected == separateProcessRadioButton;
            multiProcessRadioButton.Checked = selected == multiProcessRadioButton;
        }

        private string SelectedDomainUsage()
        {
            return multiDomainRadioButton.Checked
                ? "Multiple"
                : singleDomainRadioButton.Checked
                    ? "Single"
                    : "Default";
        }

        private void SelectDomainUsageButton(RadioButton selected)
        {
            defaultDomainRadioButton.Checked = selected == defaultDomainRadioButton;
            singleDomainRadioButton.Checked = selected == singleDomainRadioButton;
            multiDomainRadioButton.Checked = selected == multiDomainRadioButton;
        }
    }
}

