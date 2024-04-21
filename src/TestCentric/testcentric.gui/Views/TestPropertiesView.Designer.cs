namespace TestCentric.Gui.Views
{
    partial class TestPropertiesView
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
            this.header = new System.Windows.Forms.Label();
            this.testResultDisplay = new TestCentric.Gui.Views.TestResultSubView();
            this.testPropertiesDisplay = new TestCentric.Gui.Views.TestPropertiesSubView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.packageSettingsDisplay = new TestCentric.Gui.Views.TestPackageSubView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // header
            // 
            this.header.BackColor = System.Drawing.SystemColors.Window;
            this.header.Dock = System.Windows.Forms.DockStyle.Top;
            this.header.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.header.Location = new System.Drawing.Point(0, 0);
            this.header.Margin = new System.Windows.Forms.Padding(0);
            this.header.Name = "header";
            this.header.Padding = new System.Windows.Forms.Padding(2);
            this.header.Size = new System.Drawing.Size(522, 18);
            this.header.TabIndex = 0;
            // 
            // testResultDisplay
            // 
            this.testResultDisplay.AssertCount = "";
            this.testResultDisplay.Assertions = "";
            this.testResultDisplay.BackColor = System.Drawing.SystemColors.Control;
            this.testResultDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.testResultDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testResultDisplay.ElapsedTime = "";
            this.testResultDisplay.Location = new System.Drawing.Point(0, 0);
            this.testResultDisplay.Name = "testResultDisplay";
            this.testResultDisplay.Outcome = "";
            this.testResultDisplay.Output = "";
            this.testResultDisplay.Size = new System.Drawing.Size(522, 228);
            this.testResultDisplay.TabIndex = 29;
            // 
            // testPropertiesDisplay
            // 
            this.testPropertiesDisplay.BackColor = System.Drawing.SystemColors.Control;
            this.testPropertiesDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.testPropertiesDisplay.Categories = "";
            this.testPropertiesDisplay.Description = "";
            this.testPropertiesDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testPropertiesDisplay.FullName = "";
            this.testPropertiesDisplay.Location = new System.Drawing.Point(0, 0);
            this.testPropertiesDisplay.Name = "testPropertiesDisplay";
            this.testPropertiesDisplay.Properties = "";
            this.testPropertiesDisplay.RunState = "";
            this.testPropertiesDisplay.Size = new System.Drawing.Size(522, 209);
            this.testPropertiesDisplay.SkipReason = "";
            this.testPropertiesDisplay.TabIndex = 30;
            this.testPropertiesDisplay.TestCount = "";
            this.testPropertiesDisplay.TestType = "";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 18);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1 (contains the package settings)
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.packageSettingsDisplay);
            this.splitContainer1.Panel1MinSize = 80;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2MinSize = 400;
            this.splitContainer1.Size = new System.Drawing.Size(522, 572);
            this.splitContainer1.SplitterDistance = 127;
            this.splitContainer1.TabIndex = 31;
            // 
            // packageSettingsDisplay
            // 
            this.packageSettingsDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.packageSettingsDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.packageSettingsDisplay.Location = new System.Drawing.Point(0, 0);
            this.packageSettingsDisplay.Name = "packageSettingsDisplay";
            this.packageSettingsDisplay.Size = new System.Drawing.Size(522, 127);
            this.packageSettingsDisplay.TabIndex = 30;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.Panel1.Controls.Add(this.testPropertiesDisplay);
            this.splitContainer2.Panel1MinSize = 200;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.Panel2.Controls.Add(this.testResultDisplay);
            this.splitContainer2.Size = new System.Drawing.Size(522, 441);
            this.splitContainer2.SplitterDistance = 209;
            this.splitContainer2.TabIndex = 31;
            // 
            // TestPropertiesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.header);
            this.Name = "TestPropertiesView";
            this.Size = new System.Drawing.Size(522, 590);
            this.MinimumSize = new System.Drawing.Size(522, 500);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label header;
        private TestCentric.Gui.Views.TestPackageSubView packageSettingsDisplay;
        private TestCentric.Gui.Views.TestPropertiesSubView testPropertiesDisplay;
        private TestCentric.Gui.Views.TestResultSubView testResultDisplay;

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
    }
}
