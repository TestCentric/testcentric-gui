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
            this.outcomeLabel = new System.Windows.Forms.Label();
            this.outcome = new System.Windows.Forms.Label();
            this.elapsedTimeLabel = new System.Windows.Forms.Label();
            this.elapsedTime = new System.Windows.Forms.Label();
            this.assertCountLabel = new System.Windows.Forms.Label();
            this.assertCount = new System.Windows.Forms.Label();
            this.messageLabel = new System.Windows.Forms.Label();
            this.resultPanel = new System.Windows.Forms.Panel();
            this.output = new TestCentric.Gui.Controls.ExpandingLabel();
            this.outputLabel = new System.Windows.Forms.Label();
            this.assertions = new TestCentric.Gui.Controls.ExpandingLabel();
            this.testPropertiesDisplay = new Controls.TestPropertiesDisplay();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.packageSettingsDisplay = new TestCentric.Gui.Controls.PackageSettingsDisplay();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.resultPanel.SuspendLayout();
            this.testPropertiesDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.packageSettingsDisplay.SuspendLayout();
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
            // outcomeLabel
            // 
            this.outcomeLabel.Location = new System.Drawing.Point(3, 4);
            this.outcomeLabel.Name = "outcomeLabel";
            this.outcomeLabel.Size = new System.Drawing.Size(53, 13);
            this.outcomeLabel.TabIndex = 18;
            this.outcomeLabel.Text = "Outcome:";
            // 
            // outcome
            // 
            this.outcome.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outcome.Location = new System.Drawing.Point(84, 4);
            this.outcome.Name = "outcome";
            this.outcome.Size = new System.Drawing.Size(144, 13);
            this.outcome.TabIndex = 19;
            // 
            // elapsedTimeLabel
            // 
            this.elapsedTimeLabel.Location = new System.Drawing.Point(3, 21);
            this.elapsedTimeLabel.Name = "elapsedTimeLabel";
            this.elapsedTimeLabel.Size = new System.Drawing.Size(74, 13);
            this.elapsedTimeLabel.TabIndex = 21;
            this.elapsedTimeLabel.Text = "Elapsed Time:";
            // 
            // elapsedTime
            // 
            this.elapsedTime.Location = new System.Drawing.Point(81, 22);
            this.elapsedTime.Name = "elapsedTime";
            this.elapsedTime.Size = new System.Drawing.Size(67, 13);
            this.elapsedTime.TabIndex = 22;
            // 
            // assertCountLabel
            // 
            this.assertCountLabel.Location = new System.Drawing.Point(167, 21);
            this.assertCountLabel.Name = "assertCountLabel";
            this.assertCountLabel.Size = new System.Drawing.Size(44, 13);
            this.assertCountLabel.TabIndex = 23;
            this.assertCountLabel.Text = "Asserts:";
            // 
            // assertCount
            // 
            this.assertCount.Location = new System.Drawing.Point(231, 21);
            this.assertCount.Name = "assertCount";
            this.assertCount.Size = new System.Drawing.Size(49, 13);
            this.assertCount.TabIndex = 24;
            // 
            // messageLabel
            // 
            this.messageLabel.AutoSize = true;
            this.messageLabel.Location = new System.Drawing.Point(3, 39);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(58, 13);
            this.messageLabel.TabIndex = 25;
            this.messageLabel.Text = "Messages:";
            // 
            // resultPanel
            // 
            this.resultPanel.BackColor = System.Drawing.SystemColors.Control;
            this.resultPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.resultPanel.Controls.Add(this.output);
            this.resultPanel.Controls.Add(this.elapsedTime);
            this.resultPanel.Controls.Add(this.outputLabel);
            this.resultPanel.Controls.Add(this.outcomeLabel);
            this.resultPanel.Controls.Add(this.outcome);
            this.resultPanel.Controls.Add(this.elapsedTimeLabel);
            this.resultPanel.Controls.Add(this.assertCountLabel);
            this.resultPanel.Controls.Add(this.assertCount);
            this.resultPanel.Controls.Add(this.messageLabel);
            this.resultPanel.Controls.Add(this.assertions);
            this.resultPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultPanel.Location = new System.Drawing.Point(0, 0);
            this.resultPanel.Name = "resultPanel";
            this.resultPanel.Size = new System.Drawing.Size(522, 200);
            this.resultPanel.TabIndex = 29;
            // 
            // output
            // 
            this.output.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.output.BackColor = System.Drawing.Color.LightYellow;
            this.output.Expansion = TestCentric.Gui.Controls.TipWindow.ExpansionStyle.Both;
            this.output.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.output.Location = new System.Drawing.Point(6, 133);
            this.output.Name = "output";
            this.output.Size = new System.Drawing.Size(509, 56);
            this.output.TabIndex = 29;
            // 
            // outputLabel
            // 
            this.outputLabel.AutoSize = true;
            this.outputLabel.Location = new System.Drawing.Point(2, 114);
            this.outputLabel.Name = "outputLabel";
            this.outputLabel.Size = new System.Drawing.Size(66, 13);
            this.outputLabel.TabIndex = 0;
            this.outputLabel.Text = "Text Output:";
            // 
            // assertions
            // 
            this.assertions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assertions.BackColor = System.Drawing.Color.LightYellow;
            this.assertions.Expansion = TestCentric.Gui.Controls.TipWindow.ExpansionStyle.Both;
            this.assertions.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assertions.Location = new System.Drawing.Point(6, 57);
            this.assertions.Name = "assertions";
            this.assertions.Size = new System.Drawing.Size(509, 53);
            this.assertions.TabIndex = 26;
            // 
            // testPropertiesDisplay
            // 
            this.testPropertiesDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testPropertiesDisplay.Location = new System.Drawing.Point(0, 0);
            this.testPropertiesDisplay.TabIndex = 30;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 18);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.packageSettingsDisplay);
            this.splitContainer1.Panel1MinSize = 96;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(522, 572);
            this.splitContainer1.SplitterDistance = 140;
            this.splitContainer1.TabIndex = 31;
            // 
            // packageSettingsDisplay
            // 
            this.packageSettingsDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.packageSettingsDisplay.Location = new System.Drawing.Point(0, 0);
            this.packageSettingsDisplay.Name = "packageSettingsDisplay";
            this.packageSettingsDisplay.Size = new System.Drawing.Size(522, 140);
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
            this.splitContainer2.Panel2.Controls.Add(this.resultPanel);
            this.splitContainer2.Size = new System.Drawing.Size(522, 428);
            this.splitContainer2.SplitterDistance = 224;
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
            this.resultPanel.ResumeLayout(false);
            this.resultPanel.PerformLayout();
            this.testPropertiesDisplay.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.packageSettingsDisplay.ResumeLayout(false);
            this.packageSettingsDisplay.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label header;
        private TestCentric.Gui.Controls.PackageSettingsDisplay packageSettingsDisplay;
        private TestCentric.Gui.Controls.TestPropertiesDisplay testPropertiesDisplay;

        private System.Windows.Forms.Panel resultPanel;

        private System.Windows.Forms.Label outcomeLabel;
        private System.Windows.Forms.Label outcome;

        private System.Windows.Forms.Label elapsedTimeLabel;
        private System.Windows.Forms.Label elapsedTime;

        private System.Windows.Forms.Label assertCountLabel;
        private System.Windows.Forms.Label assertCount;

        private System.Windows.Forms.Label messageLabel;
        private TestCentric.Gui.Controls.ExpandingLabel assertions;

        private System.Windows.Forms.Label outputLabel;
        private TestCentric.Gui.Controls.ExpandingLabel output;

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
    }
}
