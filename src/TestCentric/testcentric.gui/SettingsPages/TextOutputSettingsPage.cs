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
using System.Diagnostics;

namespace TestCentric.Gui.SettingsPages
{
	public class TextOutputSettingsPage : SettingsPage
	{
		private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.HelpProvider helpProvider1;
        private RadioButton labelsOnRadioButton;
        private RadioButton labelsBeforeRadioButton;
        private GroupBox groupBox1;
        private RadioButton labelsOffRadioButton;
        private Label label3;

		public TextOutputSettingsPage(string key) : base(key)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
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
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.label3 = new System.Windows.Forms.Label();
            this.labelsOnRadioButton = new System.Windows.Forms.RadioButton();
            this.labelsBeforeRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelsOffRadioButton = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 37;
            this.label3.Text = "Test Case Labels";
            // 
            // labelsOnRadioButton
            // 
            this.labelsOnRadioButton.AutoSize = true;
            this.labelsOnRadioButton.Location = new System.Drawing.Point(40, 53);
            this.labelsOnRadioButton.Name = "labelsOnRadioButton";
            this.labelsOnRadioButton.Size = new System.Drawing.Size(303, 43);
            this.labelsOnRadioButton.TabIndex = 38;
            this.labelsOnRadioButton.TabStop = true;
            this.labelsOnRadioButton.Text = "Label any output with the name of the test that produced it.\r\nRepeat the label as" +
    " necessary if output from multiple tests\r\nis interleaved.";
            this.labelsOnRadioButton.UseVisualStyleBackColor = true;
            // 
            // labelsBeforeRadioButton
            // 
            this.labelsBeforeRadioButton.AutoSize = true;
            this.labelsBeforeRadioButton.Location = new System.Drawing.Point(40, 106);
            this.labelsBeforeRadioButton.Name = "labelsBeforeRadioButton";
            this.labelsBeforeRadioButton.Size = new System.Drawing.Size(303, 43);
            this.labelsBeforeRadioButton.TabIndex = 39;
            this.labelsBeforeRadioButton.TabStop = true;
            this.labelsBeforeRadioButton.Text = "Label start of every test, whether output is produced or not.\r\nRepeat the label a" +
    "s necessary if output from multiple tests\r\nis interleaved.";
            this.labelsBeforeRadioButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(131, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(325, 8);
            this.groupBox1.TabIndex = 40;
            this.groupBox1.TabStop = false;
            // 
            // labelsOffRadioButton
            // 
            this.labelsOffRadioButton.AutoSize = true;
            this.labelsOffRadioButton.Location = new System.Drawing.Point(40, 159);
            this.labelsOffRadioButton.Name = "labelsOffRadioButton";
            this.labelsOffRadioButton.Size = new System.Drawing.Size(162, 17);
            this.labelsOffRadioButton.TabIndex = 41;
            this.labelsOffRadioButton.TabStop = true;
            this.labelsOffRadioButton.Text = "Display output without labels.";
            this.labelsOffRadioButton.UseVisualStyleBackColor = true;
            // 
            // TextOutputSettingsPage
            // 
            this.Controls.Add(this.labelsOffRadioButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelsBeforeRadioButton);
            this.Controls.Add(this.labelsOnRadioButton);
            this.Controls.Add(this.label3);
            this.Name = "TextOutputSettingsPage";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		public override void LoadSettings()
		{
            var labels = Settings.GetSetting("Gui.ResultTabs.TextOutput.Labels", "ON").ToUpper();
            switch (labels)
            {
                case "ON":
                default:
                    labelsOnRadioButton.Checked = true;
                    break;
                case "ALL":
                case "BEFORE":
                    labelsBeforeRadioButton.Checked = true;
                    break;
                case "OFF":
                    labelsOffRadioButton.Checked = true;
                    break;
            }
		}

		public override void ApplySettings()
		{
            var labels = labelsOffRadioButton.Checked
                ? "OFF"
                : labelsBeforeRadioButton.Checked
                    ? "BEFORE"
                    : "ON";

            Settings.SaveSetting("Gui.ResultTabs.TextOutput.Labels", labels);
		}
    }
}

