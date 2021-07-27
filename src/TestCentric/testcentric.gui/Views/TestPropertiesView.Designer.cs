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
            this.testTypeLabel = new System.Windows.Forms.Label();
            this.testType = new System.Windows.Forms.Label();
            this.fullNameLabel = new System.Windows.Forms.Label();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.categoriesLabel = new System.Windows.Forms.Label();
            this.categories = new System.Windows.Forms.Label();
            this.propertiesLabel = new System.Windows.Forms.Label();
            this.reasonLabel = new System.Windows.Forms.Label();
            this.skipReason = new System.Windows.Forms.Label();
            this.displayHiddenProperties = new System.Windows.Forms.CheckBox();
            this.outcomeLabel = new System.Windows.Forms.Label();
            this.outcome = new System.Windows.Forms.Label();
            this.elapsedTimeLabel = new System.Windows.Forms.Label();
            this.elapsedTime = new System.Windows.Forms.Label();
            this.assertCountLabel = new System.Windows.Forms.Label();
            this.assertCount = new System.Windows.Forms.Label();
            this.messageLabel = new System.Windows.Forms.Label();
            this.testCount = new System.Windows.Forms.Label();
            this.runStateLabel = new System.Windows.Forms.Label();
            this.testCountLabel = new System.Windows.Forms.Label();
            this.runState = new System.Windows.Forms.Label();
            this.resultPanel = new System.Windows.Forms.Panel();
            this.output = new TestCentric.Gui.Controls.ExpandingLabel();
            this.outputLabel = new System.Windows.Forms.Label();
            this.assertions = new TestCentric.Gui.Controls.ExpandingLabel();
            this.testPanel = new System.Windows.Forms.Panel();
            this.fullName = new TestCentric.Gui.Controls.ExpandingLabel();
            this.properties = new TestCentric.Gui.Controls.ExpandingLabel();
            this.description = new TestCentric.Gui.Controls.ExpandingLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.packagePanel = new System.Windows.Forms.Panel();
            this.packageSettings = new TestCentric.Gui.Controls.ExpandingLabel();
            this.packageSettingsLabel = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.resultPanel.SuspendLayout();
            this.testPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.packagePanel.SuspendLayout();
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
            this.header.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.header.Size = new System.Drawing.Size(522, 18);
            this.header.TabIndex = 0;
            // 
            // testTypeLabel
            // 
            this.testTypeLabel.Location = new System.Drawing.Point(5, 4);
            this.testTypeLabel.Name = "testTypeLabel";
            this.testTypeLabel.Size = new System.Drawing.Size(58, 13);
            this.testTypeLabel.TabIndex = 1;
            this.testTypeLabel.Text = "Test Type:";
            // 
            // testType
            // 
            this.testType.Location = new System.Drawing.Point(74, 4);
            this.testType.Name = "testType";
            this.testType.Size = new System.Drawing.Size(117, 13);
            this.testType.TabIndex = 2;
            // 
            // fullNameLabel
            // 
            this.fullNameLabel.Location = new System.Drawing.Point(5, 21);
            this.fullNameLabel.Name = "fullNameLabel";
            this.fullNameLabel.Size = new System.Drawing.Size(57, 13);
            this.fullNameLabel.TabIndex = 3;
            this.fullNameLabel.Text = "Full Name:";
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.Location = new System.Drawing.Point(5, 38);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(63, 13);
            this.descriptionLabel.TabIndex = 5;
            this.descriptionLabel.Text = "Description:";
            // 
            // categoriesLabel
            // 
            this.categoriesLabel.Location = new System.Drawing.Point(5, 56);
            this.categoriesLabel.Name = "categoriesLabel";
            this.categoriesLabel.Size = new System.Drawing.Size(60, 13);
            this.categoriesLabel.TabIndex = 7;
            this.categoriesLabel.Text = "Categories:";
            // 
            // categories
            // 
            this.categories.Location = new System.Drawing.Point(74, 56);
            this.categories.Name = "categories";
            this.categories.Size = new System.Drawing.Size(449, 17);
            this.categories.TabIndex = 8;
            // 
            // propertiesLabel
            // 
            this.propertiesLabel.Location = new System.Drawing.Point(5, 107);
            this.propertiesLabel.Name = "propertiesLabel";
            this.propertiesLabel.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.propertiesLabel.Size = new System.Drawing.Size(57, 14);
            this.propertiesLabel.TabIndex = 15;
            this.propertiesLabel.Text = "Properties:";
            // 
            // reasonLabel
            // 
            this.reasonLabel.Location = new System.Drawing.Point(5, 90);
            this.reasonLabel.Name = "reasonLabel";
            this.reasonLabel.Size = new System.Drawing.Size(47, 13);
            this.reasonLabel.TabIndex = 13;
            this.reasonLabel.Text = "Reason:";
            // 
            // skipReason
            // 
            this.skipReason.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skipReason.Location = new System.Drawing.Point(74, 90);
            this.skipReason.Name = "skipReason";
            this.skipReason.Size = new System.Drawing.Size(434, 13);
            this.skipReason.TabIndex = 14;
            // 
            // displayHiddenProperties
            // 
            this.displayHiddenProperties.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.displayHiddenProperties.Location = new System.Drawing.Point(74, 106);
            this.displayHiddenProperties.Name = "displayHiddenProperties";
            this.displayHiddenProperties.Size = new System.Drawing.Size(157, 19);
            this.displayHiddenProperties.TabIndex = 16;
            this.displayHiddenProperties.Text = "Display hidden properties";
            this.displayHiddenProperties.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.displayHiddenProperties.UseVisualStyleBackColor = true;
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
            // testCount
            // 
            this.testCount.Location = new System.Drawing.Point(74, 73);
            this.testCount.Name = "testCount";
            this.testCount.Size = new System.Drawing.Size(71, 13);
            this.testCount.TabIndex = 10;
            // 
            // runStateLabel
            // 
            this.runStateLabel.Location = new System.Drawing.Point(169, 73);
            this.runStateLabel.Name = "runStateLabel";
            this.runStateLabel.Size = new System.Drawing.Size(61, 13);
            this.runStateLabel.TabIndex = 11;
            this.runStateLabel.Text = "Run State: ";
            // 
            // testCountLabel
            // 
            this.testCountLabel.Location = new System.Drawing.Point(5, 73);
            this.testCountLabel.Name = "testCountLabel";
            this.testCountLabel.Size = new System.Drawing.Size(62, 13);
            this.testCountLabel.TabIndex = 9;
            this.testCountLabel.Text = "Test Count:";
            // 
            // runState
            // 
            this.runState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.runState.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.runState.Location = new System.Drawing.Point(236, 73);
            this.runState.Name = "runState";
            this.runState.Size = new System.Drawing.Size(279, 17);
            this.runState.TabIndex = 12;
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
            this.resultPanel.Location = new System.Drawing.Point(0, 0);
            this.resultPanel.Name = "resultPanel";
            this.resultPanel.Size = new System.Drawing.Size(522, 218);
            this.resultPanel.TabIndex = 29;
            // 
            // output
            // 
            this.output.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.output.BackColor = System.Drawing.Color.LightYellow;
            this.output.Expansion = TestCentric.Gui.Controls.TipWindow.ExpansionStyle.Both;
            this.output.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.output.Location = new System.Drawing.Point(5, 146);
            this.output.Name = "output";
            this.output.Size = new System.Drawing.Size(510, 64);
            this.output.TabIndex = 29;
            // 
            // outputLabel
            // 
            this.outputLabel.AutoSize = true;
            this.outputLabel.Location = new System.Drawing.Point(2, 127);
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
            this.assertions.Location = new System.Drawing.Point(3, 57);
            this.assertions.Name = "assertions";
            this.assertions.Size = new System.Drawing.Size(510, 64);
            this.assertions.TabIndex = 26;
            // 
            // testPanel
            // 
            this.testPanel.BackColor = System.Drawing.SystemColors.Control;
            this.testPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.testPanel.Controls.Add(this.testTypeLabel);
            this.testPanel.Controls.Add(this.runState);
            this.testPanel.Controls.Add(this.testCountLabel);
            this.testPanel.Controls.Add(this.testCount);
            this.testPanel.Controls.Add(this.testType);
            this.testPanel.Controls.Add(this.runStateLabel);
            this.testPanel.Controls.Add(this.fullNameLabel);
            this.testPanel.Controls.Add(this.propertiesLabel);
            this.testPanel.Controls.Add(this.fullName);
            this.testPanel.Controls.Add(this.properties);
            this.testPanel.Controls.Add(this.descriptionLabel);
            this.testPanel.Controls.Add(this.reasonLabel);
            this.testPanel.Controls.Add(this.description);
            this.testPanel.Controls.Add(this.skipReason);
            this.testPanel.Controls.Add(this.categoriesLabel);
            this.testPanel.Controls.Add(this.displayHiddenProperties);
            this.testPanel.Controls.Add(this.categories);
            this.testPanel.Location = new System.Drawing.Point(0, 0);
            this.testPanel.Name = "testPanel";
            this.testPanel.Size = new System.Drawing.Size(522, 202);
            this.testPanel.TabIndex = 30;
            // 
            // fullName
            // 
            this.fullName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fullName.Location = new System.Drawing.Point(74, 21);
            this.fullName.Name = "fullName";
            this.fullName.Size = new System.Drawing.Size(441, 13);
            this.fullName.TabIndex = 4;
            // 
            // properties
            // 
            this.properties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.properties.BackColor = System.Drawing.Color.LightYellow;
            this.properties.Expansion = TestCentric.Gui.Controls.TipWindow.ExpansionStyle.Both;
            this.properties.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.properties.Location = new System.Drawing.Point(6, 128);
            this.properties.Name = "properties";
            this.properties.Size = new System.Drawing.Size(509, 64);
            this.properties.TabIndex = 17;
            // 
            // description
            // 
            this.description.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.description.AutoEllipsis = true;
            this.description.Location = new System.Drawing.Point(88, 38);
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(427, 18);
            this.description.TabIndex = 6;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 18);
            this.splitContainer1.MaximumSize = new System.Drawing.Size(522, 516);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.packagePanel);
            this.splitContainer1.Panel1MinSize = 96;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(522, 504);
            this.splitContainer1.SplitterDistance = 96;
            this.splitContainer1.TabIndex = 31;
            // 
            // packagePanel
            // 
            this.packagePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.packagePanel.Controls.Add(this.packageSettings);
            this.packagePanel.Controls.Add(this.packageSettingsLabel);
            this.packagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.packagePanel.Location = new System.Drawing.Point(0, 0);
            this.packagePanel.Name = "packagePanel";
            this.packagePanel.Size = new System.Drawing.Size(522, 96);
            this.packagePanel.TabIndex = 30;
            // 
            // packageSettings
            // 
            this.packageSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.packageSettings.BackColor = System.Drawing.Color.LightYellow;
            this.packageSettings.Expansion = TestCentric.Gui.Controls.TipWindow.ExpansionStyle.Both;
            this.packageSettings.Location = new System.Drawing.Point(6, 22);
            this.packageSettings.Name = "packageSettings";
            this.packageSettings.Size = new System.Drawing.Size(520, 64);
            this.packageSettings.TabIndex = 1;
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
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.Panel1.Controls.Add(this.testPanel);
            this.splitContainer2.Panel1MinSize = 200;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.Panel2.Controls.Add(this.resultPanel);
            this.splitContainer2.Size = new System.Drawing.Size(522, 426);
            this.splitContainer2.SplitterDistance = 202;
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
            this.Size = new System.Drawing.Size(522, 522);
            this.resultPanel.ResumeLayout(false);
            this.resultPanel.PerformLayout();
            this.testPanel.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.packagePanel.ResumeLayout(false);
            this.packagePanel.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label header;
        private System.Windows.Forms.Label testTypeLabel;
        private System.Windows.Forms.Label testType;
        private System.Windows.Forms.Label fullNameLabel;
        private TestCentric.Gui.Controls.ExpandingLabel fullName;
        private System.Windows.Forms.Label descriptionLabel;
        private TestCentric.Gui.Controls.ExpandingLabel description;
        private System.Windows.Forms.Label categoriesLabel;
        private System.Windows.Forms.Label categories;
        private System.Windows.Forms.Label propertiesLabel;
        private TestCentric.Gui.Controls.ExpandingLabel properties;
        private System.Windows.Forms.Label reasonLabel;
        private System.Windows.Forms.Label skipReason;
        private System.Windows.Forms.CheckBox displayHiddenProperties;
        private System.Windows.Forms.Label outcomeLabel;
        private System.Windows.Forms.Label outcome;
        private System.Windows.Forms.Label elapsedTimeLabel;
        private System.Windows.Forms.Label elapsedTime;
        private System.Windows.Forms.Label assertCountLabel;
        private System.Windows.Forms.Label assertCount;
        private System.Windows.Forms.Label messageLabel;
        private TestCentric.Gui.Controls.ExpandingLabel assertions;
        private System.Windows.Forms.Label testCount;
        private System.Windows.Forms.Label runStateLabel;
        private System.Windows.Forms.Label testCountLabel;
        private System.Windows.Forms.Label runState;
        private System.Windows.Forms.Panel resultPanel;
        private System.Windows.Forms.Panel testPanel;
        private System.Windows.Forms.Panel packagePanel;
        private TestCentric.Gui.Controls.ExpandingLabel output;
        private System.Windows.Forms.Label outputLabel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label packageSettingsLabel;
        private TestCentric.Gui.Controls.ExpandingLabel packageSettings;
    }
}
