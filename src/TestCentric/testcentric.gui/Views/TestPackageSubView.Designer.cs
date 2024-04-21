namespace TestCentric.Gui.Views
{
    partial class TestPackageSubView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.packageSettingsLabel = new System.Windows.Forms.Label();
            this.packageSettings = new TestCentric.Gui.Controls.ExpandingLabel();
            this.SuspendLayout();
            // 
            // packageSettingsLabel
            // 
            this.packageSettingsLabel.AutoSize = true;
            this.packageSettingsLabel.Location = new System.Drawing.Point(5, 4);
            this.packageSettingsLabel.Name = "packageSettingsLabel";
            this.packageSettingsLabel.Size = new System.Drawing.Size(94, 13);
            this.packageSettingsLabel.TabIndex = 0;
            this.packageSettingsLabel.Text = "Package Settings:";
            // 
            // packageSettings
            // 
            this.packageSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.packageSettings.BackColor = System.Drawing.Color.LightYellow;
            this.packageSettings.Expansion = TestCentric.Gui.Controls.TipWindow.ExpansionStyle.Both;
            this.packageSettings.Location = new System.Drawing.Point(6, 22);
            this.packageSettings.Name = "packageSettings";
            this.packageSettings.Size = new System.Drawing.Size(509, 45);
            this.packageSettings.TabIndex = 1;
            // 
            // PackageSettingsDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.packageSettings);
            this.Controls.Add(this.packageSettingsLabel);
            this.Name = "PackageSettingsDisplay";
            this.Size = new System.Drawing.Size(522, 73);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label packageSettingsLabel;
        private TestCentric.Gui.Controls.ExpandingLabel packageSettings;
    }
}
