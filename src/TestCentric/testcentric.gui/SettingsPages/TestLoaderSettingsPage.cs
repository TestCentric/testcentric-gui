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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
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
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
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
            ((System.ComponentModel.ISupportInitialize)(this.numberOfAgentsUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // singleDomainRadioButton
            // 
            this.singleDomainRadioButton.AutoCheck = false;
            this.singleDomainRadioButton.AutoSize = true;
            this.singleDomainRadioButton.Checked = true;
            this.helpProvider1.SetHelpString(this.singleDomainRadioButton, "If selected, all test assemblies will be loaded in the same AppDomain.");
            this.singleDomainRadioButton.Location = new System.Drawing.Point(32, 212);
            this.singleDomainRadioButton.Name = "singleDomainRadioButton";
            this.helpProvider1.SetShowHelp(this.singleDomainRadioButton, true);
            this.singleDomainRadioButton.Size = new System.Drawing.Size(194, 17);
            this.singleDomainRadioButton.TabIndex = 9;
            this.singleDomainRadioButton.TabStop = true;
            this.singleDomainRadioButton.Text = "Use a single AppDomain for all tests";
            this.singleDomainRadioButton.Click += new System.EventHandler(this.toggleMultiDomain);
            // 
            // multiDomainRadioButton
            // 
            this.multiDomainRadioButton.AutoCheck = false;
            this.multiDomainRadioButton.AutoSize = true;
            this.helpProvider1.SetHelpString(this.multiDomainRadioButton, "If selected, each test assembly will be loaded in a separate AppDomain.");
            this.multiDomainRadioButton.Location = new System.Drawing.Point(32, 182);
            this.multiDomainRadioButton.Name = "multiDomainRadioButton";
            this.helpProvider1.SetShowHelp(this.multiDomainRadioButton, true);
            this.multiDomainRadioButton.Size = new System.Drawing.Size(220, 17);
            this.multiDomainRadioButton.TabIndex = 8;
            this.multiDomainRadioButton.Text = "Use a separate AppDomain per Assembly";
            this.multiDomainRadioButton.Click += new System.EventHandler(this.toggleMultiDomain);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 13);
            this.label3.TabIndex = 35;
            this.label3.Text = "Default Process Model";
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
            this.multiProcessRadioButton.Checked = true;
            this.multiProcessRadioButton.Location = new System.Drawing.Point(32, 31);
            this.multiProcessRadioButton.Name = "multiProcessRadioButton";
            this.multiProcessRadioButton.Size = new System.Drawing.Size(298, 17);
            this.multiProcessRadioButton.TabIndex = 36;
            this.multiProcessRadioButton.TabStop = true;
            this.multiProcessRadioButton.Text = "Run tests in separate agent processes, one per Assembly.";
            this.multiProcessRadioButton.CheckedChanged += new System.EventHandler(this.toggleProcessUsage);
            // 
            // separateProcessRadioButton
            // 
            this.separateProcessRadioButton.AutoSize = true;
            this.separateProcessRadioButton.Location = new System.Drawing.Point(32, 87);
            this.separateProcessRadioButton.Name = "separateProcessRadioButton";
            this.separateProcessRadioButton.Size = new System.Drawing.Size(204, 17);
            this.separateProcessRadioButton.TabIndex = 37;
            this.separateProcessRadioButton.Text = "Run tests in a single separate process";
            this.separateProcessRadioButton.CheckedChanged += new System.EventHandler(this.toggleProcessUsage);
            // 
            // sameProcessRadioButton
            // 
            this.sameProcessRadioButton.AutoSize = true;
            this.sameProcessRadioButton.Location = new System.Drawing.Point(32, 121);
            this.sameProcessRadioButton.Name = "sameProcessRadioButton";
            this.sameProcessRadioButton.Size = new System.Drawing.Size(232, 17);
            this.sameProcessRadioButton.TabIndex = 38;
            this.sameProcessRadioButton.Text = "Run tests directly in the TestCentric process";
            this.sameProcessRadioButton.CheckedChanged += new System.EventHandler(this.toggleProcessUsage);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 158);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 13);
            this.label2.TabIndex = 40;
            this.label2.Text = "Default Domain Usage";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Location = new System.Drawing.Point(199, 158);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(253, 8);
            this.groupBox2.TabIndex = 39;
            this.groupBox2.TabStop = false;
            // 
            // numberOfAgentsUpDown
            // 
            this.numberOfAgentsUpDown.Enabled = false;
            this.numberOfAgentsUpDown.Location = new System.Drawing.Point(348, 55);
            this.numberOfAgentsUpDown.Name = "numberOfAgentsUpDown";
            this.numberOfAgentsUpDown.Size = new System.Drawing.Size(66, 20);
            this.numberOfAgentsUpDown.TabIndex = 41;
            // 
            // numberOfAgentsCheckBox
            // 
            this.numberOfAgentsCheckBox.AutoSize = true;
            this.numberOfAgentsCheckBox.Location = new System.Drawing.Point(48, 58);
            this.numberOfAgentsCheckBox.Name = "numberOfAgentsCheckBox";
            this.numberOfAgentsCheckBox.Size = new System.Drawing.Size(175, 17);
            this.numberOfAgentsCheckBox.TabIndex = 42;
            this.numberOfAgentsCheckBox.Text = "LImit simultaneous processes to";
            this.numberOfAgentsCheckBox.UseVisualStyleBackColor = true;
            this.numberOfAgentsCheckBox.CheckedChanged += new System.EventHandler(this.numberOfAgentsCheckBox_CheckedChanged);
            // 
            // TestLoaderSettingsPage
            // 
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
            switch( Settings.Options.TestLoader.ProcessModel )
            {
                case "Separate":
                    separateProcessRadioButton.Checked = true;
                    sameProcessRadioButton.Checked = false;
                    multiProcessRadioButton.Checked = false;
                    break;
                case "Single":
                    sameProcessRadioButton.Checked = true;
                    multiProcessRadioButton.Checked = false;
                    separateProcessRadioButton.Checked = false;
                    break;
                default:
                    multiProcessRadioButton.Checked = true;
                    sameProcessRadioButton.Checked = false;
                    separateProcessRadioButton.Checked = false;
                    break;
            }

            var agents = Settings.Options.TestLoader.Agents;
            numberOfAgentsCheckBox.Enabled = multiProcessRadioButton.Checked;
            numberOfAgentsCheckBox.Checked = agents > 0;
            numberOfAgentsUpDown.Value = agents;

			bool singleDomain = Settings.Options.TestLoader.DomainUsage == "Single";
			multiDomainRadioButton.Checked = !singleDomain;
			singleDomainRadioButton.Checked = singleDomain;
		}

        public override void ApplySettings()
        {
            Settings.Options.TestLoader.ProcessModel = sameProcessRadioButton.Checked
                ? "Single"
                : separateProcessRadioButton.Checked
                    ? "Separate"
                    : "Multiple";

            Settings.Options.TestLoader.Agents = numberOfAgentsCheckBox.Checked
                ? (int)numberOfAgentsUpDown.Value
                : 0;

            Settings.Options.TestLoader.DomainUsage = singleDomainRadioButton.Checked
                ? "Single"
                : "Multiple";
        }

        private void toggleProcessUsage(object sender, EventArgs e)
        {
            bool multiProcess = multiProcessRadioButton.Checked;
            numberOfAgentsCheckBox.Enabled = multiProcess;
            numberOfAgentsUpDown.Enabled = multiProcess && numberOfAgentsCheckBox.Checked;

            singleDomainRadioButton.Enabled = multiDomainRadioButton.Enabled = !multiProcess;
        }

        private void numberOfAgentsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            numberOfAgentsUpDown.Enabled = numberOfAgentsCheckBox.Checked;
        }

        private void toggleMultiDomain(object sender, System.EventArgs e)
		{
			bool multiDomain = multiDomainRadioButton.Checked = ! multiDomainRadioButton.Checked;
			singleDomainRadioButton.Checked = !multiDomain;
		}

		public override bool HasChangesRequiringReload
		{
			get 
			{
                return
                    Settings.Options.TestLoader.ProcessModel != SelectedProcessModel() ||
                    Settings.Options.TestLoader.DomainUsage != SelectedDomainUsage();
			}
		}

        private string SelectedProcessModel()
        {
            return sameProcessRadioButton.Checked
                ? "Single"
                : separateProcessRadioButton.Checked
                    ? "Separate"
                    : "Multiple";
        }

        private string SelectedDomainUsage()
        {
            return multiDomainRadioButton.Checked
                ? "Multiple"
                : "Single";
        }
    }
}

