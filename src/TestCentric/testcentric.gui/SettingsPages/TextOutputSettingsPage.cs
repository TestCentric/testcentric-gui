// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;

namespace TestCentric.Gui.SettingsPages
{
    public class TextOutputSettingsPage : SettingsPage
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.HelpProvider helpProvider1;
        private GroupBox groupBox1;
        private RadioButton labelsOffRadioButton;
        private RadioButton labelsOnRadioButton;
        private RadioButton labelsBeforeRadioButton;
        private RadioButton labelsAfterRadioButton;
        private RadioButton labelsBeforeAndAfterRadioButtion;
        private Label label3;

        public TextOutputSettingsPage(string key) : base(key)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();
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
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelsOffRadioButton = new System.Windows.Forms.RadioButton();
            this.labelsAfterRadioButton = new System.Windows.Forms.RadioButton();
            this.labelsOnRadioButton = new System.Windows.Forms.RadioButton();
            this.labelsBeforeRadioButton = new System.Windows.Forms.RadioButton();
            this.labelsBeforeAndAfterRadioButtion = new System.Windows.Forms.RadioButton();
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
            this.labelsOffRadioButton.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.labelsOffRadioButton.Location = new System.Drawing.Point(40, 47);
            this.labelsOffRadioButton.Name = "labelsOffRadioButton";
            this.labelsOffRadioButton.Size = new System.Drawing.Size(185, 17);
            this.labelsOffRadioButton.TabIndex = 38;
            this.labelsOffRadioButton.TabStop = true;
            this.labelsOffRadioButton.Text = "Off - Display output without labels.";
            this.labelsOffRadioButton.UseVisualStyleBackColor = true;
            // 
            // labelsOnRadioButton
            // 
            this.labelsOnRadioButton.AutoSize = true;
            this.labelsOnRadioButton.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.labelsOnRadioButton.Location = new System.Drawing.Point(40, 75);
            this.labelsOnRadioButton.Name = "labelsOnRadioButton";
            this.labelsOnRadioButton.Size = new System.Drawing.Size(326, 43);
            this.labelsOnRadioButton.TabIndex = 39;
            this.labelsOnRadioButton.TabStop = true;
            this.labelsOnRadioButton.Text = "On - Label any output with the name of the test that produced it.\r\nRepeat the lab" +
    "el as necessary if output from multiple tests\r\nis interleaved.";
            this.labelsOnRadioButton.UseVisualStyleBackColor = true;
            // 
            // labelsBeforeRadioButton
            // 
            this.labelsBeforeRadioButton.AutoSize = true;
            this.labelsBeforeRadioButton.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.labelsBeforeRadioButton.Location = new System.Drawing.Point(40, 128);
            this.labelsBeforeRadioButton.Name = "labelsBeforeRadioButton";
            this.labelsBeforeRadioButton.Size = new System.Drawing.Size(343, 43);
            this.labelsBeforeRadioButton.TabIndex = 40;
            this.labelsBeforeRadioButton.TabStop = true;
            this.labelsBeforeRadioButton.Text = "Before - Label start of every test, whether output is produced or not.\r\nRepeat th" +
    "e label as necessary if output from multiple tests\r\nis interleaved.";
            this.labelsBeforeRadioButton.UseVisualStyleBackColor = true;
            // 
            // labelsAfterRadioButton
            // 
            this.labelsAfterRadioButton.AutoSize = true;
            this.labelsAfterRadioButton.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.labelsAfterRadioButton.Location = new System.Drawing.Point(40, 183);
            this.labelsAfterRadioButton.Name = "labelsAfterRadioButton";
            this.labelsAfterRadioButton.Size = new System.Drawing.Size(344, 30);
            this.labelsAfterRadioButton.TabIndex = 41;
            this.labelsAfterRadioButton.TabStop = true;
            this.labelsAfterRadioButton.Text = "After - Label end of every test, including pass/fail status. In addition,\r\nlabel any output as for the On setting.";
            this.labelsAfterRadioButton.UseVisualStyleBackColor = true;
            // 
            // labelsBeforeAndAfterRadioButtion
            // 
            this.labelsBeforeAndAfterRadioButtion.AutoSize = true;
            this.labelsBeforeAndAfterRadioButtion.Location = new System.Drawing.Point(40, 225);
            this.labelsBeforeAndAfterRadioButtion.Name = "labelsBeforeAndAfterRadioButtion";
            this.labelsBeforeAndAfterRadioButtion.Size = new System.Drawing.Size(540, 17);
            this.labelsBeforeAndAfterRadioButtion.TabIndex = 42;
            this.labelsBeforeAndAfterRadioButtion.TabStop = true;
            this.labelsBeforeAndAfterRadioButtion.Text = "Before and After - Label both start and end of every test.\r\nIn addition, label " +
    "any output as for the On setting.";
            this.labelsBeforeAndAfterRadioButtion.UseVisualStyleBackColor = true;
            // 
            // TextOutputSettingsPage
            // 
            this.Controls.Add(this.labelsBeforeAndAfterRadioButtion);
            this.Controls.Add(this.labelsAfterRadioButton);
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
            var labels = Settings.Gui.TextOutput.Labels;
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
                case "AFTER":
                    labelsAfterRadioButton.Checked = true;
                    break;
                case "BEFOREANDAFTER":
                    labelsBeforeAndAfterRadioButtion.Checked = true;
                    break;
                case "OFF": 
                    labelsOffRadioButton.Checked = true;
                    break;
            }
        }

        public override void ApplySettings()
        {
            var labels = 
                labelsOffRadioButton.Checked ? "OFF" :
                labelsBeforeRadioButton.Checked ? "BEFORE" :
                labelsAfterRadioButton.Checked ? "AFTER" :
                labelsBeforeAndAfterRadioButtion.Checked ? "BEFOREANDAFTER"
                    : "ON";

            Settings.Gui.TextOutput.Labels = labels;
        }
    }
}

