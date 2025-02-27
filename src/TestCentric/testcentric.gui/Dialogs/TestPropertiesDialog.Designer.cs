namespace TestCentric.Gui.Dialogs
{
    using Controls;

    partial class TestPropertiesDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestPropertiesDialog));
            this.testPropertiesView = new TestCentric.Gui.Views.TestPropertiesView();
            this.SuspendLayout();
            // 
            // testPropertiesView
            // 
            this.testPropertiesView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.testPropertiesView.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.testPropertiesView.Categories = "";
            this.testPropertiesView.Description = "";
            this.testPropertiesView.FullName = "";
            this.testPropertiesView.Header = "";
            this.testPropertiesView.Location = new System.Drawing.Point(6, 30);
            this.testPropertiesView.Name = "testPropertiesView";
            this.testPropertiesView.PackageSettings = "";
            this.testPropertiesView.Properties = "";
            this.testPropertiesView.RunState = "";
            this.testPropertiesView.Size = new System.Drawing.Size(324, 507);
            this.testPropertiesView.SkipReason = "";
            this.testPropertiesView.TabIndex = 6;
            this.testPropertiesView.TestCount = "";
            this.testPropertiesView.TestType = "";
            // 
            // TestPropertiesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(336, 536);
            this.Controls.Add(this.testPropertiesView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TestPropertiesDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Controls.SetChildIndex(this.testPropertiesView, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private TestCentric.Gui.Views.TestPropertiesView testPropertiesView;
    }
}
