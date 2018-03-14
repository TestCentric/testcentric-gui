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
	public class GuiSettingsPage : SettingsPage
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox loadLastProjectCheckBox;
		private System.Windows.Forms.RadioButton fullGuiRadioButton;
		private System.Windows.Forms.RadioButton miniGuiRadioButton;
		private System.Windows.Forms.HelpProvider helpProvider1;
        private CheckBox checkFilesExistCheckBox;
        private Label label3;
        private TextBox recentFilesCountTextBox;
        private Label label4;
        private System.ComponentModel.IContainer components = null;

        private const int MIN_RECENT_FILES = 0;
        private const int MAX_RECENT_FILES = 24;

		public GuiSettingsPage(string key) : base(key)
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
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.loadLastProjectCheckBox = new System.Windows.Forms.CheckBox();
            this.fullGuiRadioButton = new System.Windows.Forms.RadioButton();
            this.miniGuiRadioButton = new System.Windows.Forms.RadioButton();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.recentFilesCountTextBox = new System.Windows.Forms.TextBox();
            this.checkFilesExistCheckBox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Gui Display";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(135, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(313, 8);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Recent Files";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Location = new System.Drawing.Point(135, 96);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(313, 8);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            // 
            // loadLastProjectCheckBox
            // 
            this.loadLastProjectCheckBox.AutoSize = true;
            this.helpProvider1.SetHelpString(this.loadLastProjectCheckBox, "If checked, most recent project is loaded at startup.");
            this.loadLastProjectCheckBox.Location = new System.Drawing.Point(32, 198);
            this.loadLastProjectCheckBox.Name = "loadLastProjectCheckBox";
            this.helpProvider1.SetShowHelp(this.loadLastProjectCheckBox, true);
            this.loadLastProjectCheckBox.Size = new System.Drawing.Size(193, 17);
            this.loadLastProjectCheckBox.TabIndex = 31;
            this.loadLastProjectCheckBox.Text = "Load most recent project at startup.";
            // 
            // fullGuiRadioButton
            // 
            this.fullGuiRadioButton.AutoSize = true;
            this.helpProvider1.SetHelpString(this.fullGuiRadioButton, "If selected, the full Gui is displayed, including the progress bar and output tab" +
        "s.");
            this.fullGuiRadioButton.Location = new System.Drawing.Point(32, 24);
            this.fullGuiRadioButton.Name = "fullGuiRadioButton";
            this.helpProvider1.SetShowHelp(this.fullGuiRadioButton, true);
            this.fullGuiRadioButton.Size = new System.Drawing.Size(215, 17);
            this.fullGuiRadioButton.TabIndex = 32;
            this.fullGuiRadioButton.Text = "Full Gui with progress bar and result tabs";
            // 
            // miniGuiRadioButton
            // 
            this.miniGuiRadioButton.AutoSize = true;
            this.helpProvider1.SetHelpString(this.miniGuiRadioButton, "If selected, the mini-Gui, consisting of only the tree of tests, is displayed.");
            this.miniGuiRadioButton.Location = new System.Drawing.Point(32, 56);
            this.miniGuiRadioButton.Name = "miniGuiRadioButton";
            this.helpProvider1.SetShowHelp(this.miniGuiRadioButton, true);
            this.miniGuiRadioButton.Size = new System.Drawing.Size(148, 17);
            this.miniGuiRadioButton.TabIndex = 33;
            this.miniGuiRadioButton.Text = "Mini Gui showing tree only";
            // 
            // recentFilesCountTextBox
            // 
            this.helpProvider1.SetHelpString(this.recentFilesCountTextBox, "The maximum number of files to display in the Recent Files list.");
            this.recentFilesCountTextBox.Location = new System.Drawing.Point(69, 120);
            this.recentFilesCountTextBox.Name = "recentFilesCountTextBox";
            this.helpProvider1.SetShowHelp(this.recentFilesCountTextBox, true);
            this.recentFilesCountTextBox.Size = new System.Drawing.Size(40, 20);
            this.recentFilesCountTextBox.TabIndex = 29;
            this.recentFilesCountTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.recentFilesCountTextBox_Validating);
            this.recentFilesCountTextBox.Validated += new System.EventHandler(this.recentFilesCountTextBox_Validated);
            // 
            // checkFilesExistCheckBox
            // 
            this.checkFilesExistCheckBox.AutoSize = true;
            this.checkFilesExistCheckBox.Location = new System.Drawing.Point(32, 159);
            this.checkFilesExistCheckBox.Name = "checkFilesExistCheckBox";
            this.checkFilesExistCheckBox.Size = new System.Drawing.Size(185, 17);
            this.checkFilesExistCheckBox.TabIndex = 34;
            this.checkFilesExistCheckBox.Text = "Check that files exist before listing";
            this.checkFilesExistCheckBox.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(125, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "files in menu";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "List";
            // 
            // GuiSettingsPage
            // 
            this.Controls.Add(this.checkFilesExistCheckBox);
            this.Controls.Add(this.miniGuiRadioButton);
            this.Controls.Add(this.fullGuiRadioButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.recentFilesCountTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.loadLastProjectCheckBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Name = "GuiSettingsPage";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		public override void LoadSettings()
		{
            switch (Settings.Gui.DisplayFormat)
            {
                case "Full":
                    fullGuiRadioButton.Checked = true;
                    break;
                case "Mini":
                    miniGuiRadioButton.Checked = true;
                    break;
            }

            recentFilesCountTextBox.Text = Settings.Gui.RecentProjects.MaxFiles.ToString();
            checkFilesExistCheckBox.Checked = Settings.Gui.RecentProjects.CheckFilesExist;
            loadLastProjectCheckBox.Checked = Settings.Options.LoadLastProject;
        }

		public override void ApplySettings()
		{
			Settings.Gui.DisplayFormat = fullGuiRadioButton.Checked ? "Full" : "Mini";
            Settings.Gui.RecentProjects.CheckFilesExist = checkFilesExistCheckBox.Checked;
			Settings.Options.LoadLastProject = loadLastProjectCheckBox.Checked;
		}

		private void recentFilesCountTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if ( recentFilesCountTextBox.Text.Length == 0 )
			{
				recentFilesCountTextBox.Text = MAX_RECENT_FILES.ToString();
				recentFilesCountTextBox.SelectAll();
				e.Cancel = true;
			}
			else
			{
				string errmsg = null;

				try
				{
					int count = int.Parse( recentFilesCountTextBox.Text );

                    if (count < MIN_RECENT_FILES ||
                        count > MAX_RECENT_FILES)
                    {
                        errmsg = string.Format("Number of files must be from {0} to {1}",
                            MIN_RECENT_FILES, MAX_RECENT_FILES);
                    }
                }
				catch
				{
					errmsg = "Number of files must be numeric";
				}

				if ( errmsg != null )
				{
					recentFilesCountTextBox.SelectAll();
                    MessageDisplay.Error(errmsg);
					e.Cancel = true;
				}
			}
		}

		private void recentFilesCountTextBox_Validated(object sender, System.EventArgs e)
		{
			int count = int.Parse( recentFilesCountTextBox.Text );
			Settings.Gui.RecentProjects.MaxFiles = count;
            if (count == 0)
                loadLastProjectCheckBox.Checked = false;
		}
	}
}

