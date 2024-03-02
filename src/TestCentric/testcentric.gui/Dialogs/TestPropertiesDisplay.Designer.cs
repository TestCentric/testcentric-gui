namespace TestCentric.Gui.Dialogs
{
    using Controls;

    partial class TestPropertiesDisplay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestPropertiesDisplay));
            this.testPanel = new System.Windows.Forms.Panel();
            this.description = new TestCentric.Gui.Controls.ExpandingLabel();
            this.categories = new System.Windows.Forms.Label();
            this.properties = new TestCentric.Gui.Controls.ExpandingLabel();
            this.propertiesLabel = new System.Windows.Forms.Label();
            this.testCaseCount = new System.Windows.Forms.Label();
            this.skipReason = new TestCentric.Gui.Controls.ExpandingLabel();
            this.reasonLabel = new System.Windows.Forms.Label();
            this.testCountLabel = new System.Windows.Forms.Label();
            this.runState = new System.Windows.Forms.Label();
            this.runStateLabel = new System.Windows.Forms.Label();
            this.testType = new System.Windows.Forms.Label();
            this.testTypeLabel = new System.Windows.Forms.Label();
            this.categoriesLabel = new System.Windows.Forms.Label();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.fullName = new TestCentric.Gui.Controls.ExpandingLabel();
            this.fullNameLabel = new System.Windows.Forms.Label();
            this.displayHiddenProperties = new System.Windows.Forms.CheckBox();
            this.resultPanel = new System.Windows.Forms.Panel();
            this.assertCount = new System.Windows.Forms.Label();
            this.elapsedTime = new System.Windows.Forms.Label();
            this.outcomeLabel = new System.Windows.Forms.Label();
            this.outcome = new System.Windows.Forms.Label();
            this.assertCountLabel = new System.Windows.Forms.Label();
            this.messageLabel = new System.Windows.Forms.Label();
            this.elapsedTimeLabel = new System.Windows.Forms.Label();
            this.stackTraceLabel = new System.Windows.Forms.Label();
            this.message = new TestCentric.Gui.Controls.ExpandingLabel();
            this.stackTrace = new TestCentric.Gui.Controls.ExpandingLabel();
            this.packagePanel = new System.Windows.Forms.Panel();
            this.packageSettingsLabel = new System.Windows.Forms.Label();
            this.packageSettings = new TestCentric.Gui.Controls.ExpandingLabel();
            this.testPanel.SuspendLayout();
            this.resultPanel.SuspendLayout();
            this.packagePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // testPanel
            // 
            this.testPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.testPanel.BackColor = System.Drawing.SystemColors.Control;
            this.testPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.testPanel.Controls.Add(this.description);
            this.testPanel.Controls.Add(this.categories);
            this.testPanel.Controls.Add(this.properties);
            this.testPanel.Controls.Add(this.propertiesLabel);
            this.testPanel.Controls.Add(this.testCaseCount);
            this.testPanel.Controls.Add(this.skipReason);
            this.testPanel.Controls.Add(this.reasonLabel);
            this.testPanel.Controls.Add(this.testCountLabel);
            this.testPanel.Controls.Add(this.runState);
            this.testPanel.Controls.Add(this.runStateLabel);
            this.testPanel.Controls.Add(this.testType);
            this.testPanel.Controls.Add(this.testTypeLabel);
            this.testPanel.Controls.Add(this.categoriesLabel);
            this.testPanel.Controls.Add(this.descriptionLabel);
            this.testPanel.Controls.Add(this.fullName);
            this.testPanel.Controls.Add(this.fullNameLabel);
            this.testPanel.Controls.Add(this.displayHiddenProperties);
            this.testPanel.Location = new System.Drawing.Point(6, 134);
            this.testPanel.Name = "testPanel";
            this.testPanel.Size = new System.Drawing.Size(324, 207);
            this.testPanel.TabIndex = 5;
            this.testPanel.Text = "Test Details";
            // 
            // description
            // 
            this.description.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.description.CopySupported = true;
            this.description.Expansion = TestCentric.Gui.Controls.TipWindow.ExpansionStyle.Both;
            this.description.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.description.Location = new System.Drawing.Point(101, 38);
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(212, 13);
            this.description.TabIndex = 5;
            // 
            // categories
            // 
            this.categories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.categories.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.categories.Location = new System.Drawing.Point(101, 56);
            this.categories.Name = "categories";
            this.categories.Size = new System.Drawing.Size(212, 13);
            this.categories.TabIndex = 7;
            // 
            // properties
            // 
            this.properties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.properties.BackColor = System.Drawing.Color.LightYellow;
            this.properties.Expansion = TestCentric.Gui.Controls.TipWindow.ExpansionStyle.Both;
            this.properties.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.properties.Location = new System.Drawing.Point(6, 128);
            this.properties.Name = "properties";
            this.properties.Size = new System.Drawing.Size(310, 69);
            this.properties.TabIndex = 15;
            // 
            // propertiesLabel
            // 
            this.propertiesLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.propertiesLabel.Location = new System.Drawing.Point(5, 107);
            this.propertiesLabel.Name = "propertiesLabel";
            this.propertiesLabel.Size = new System.Drawing.Size(57, 14);
            this.propertiesLabel.TabIndex = 14;
            this.propertiesLabel.Text = "Properties:";
            // 
            // testCaseCount
            // 
            this.testCaseCount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.testCaseCount.Location = new System.Drawing.Point(74, 73);
            this.testCaseCount.Name = "testCaseCount";
            this.testCaseCount.Size = new System.Drawing.Size(71, 13);
            this.testCaseCount.TabIndex = 9;
            // 
            // skipReason
            // 
            this.skipReason.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skipReason.CopySupported = true;
            this.skipReason.Expansion = TestCentric.Gui.Controls.TipWindow.ExpansionStyle.Vertical;
            this.skipReason.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.skipReason.Location = new System.Drawing.Point(101, 90);
            this.skipReason.Name = "skipReason";
            this.skipReason.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.skipReason.Size = new System.Drawing.Size(212, 13);
            this.skipReason.TabIndex = 13;
            // 
            // reasonLabel
            // 
            this.reasonLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.reasonLabel.Location = new System.Drawing.Point(5, 90);
            this.reasonLabel.Name = "reasonLabel";
            this.reasonLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.reasonLabel.Size = new System.Drawing.Size(80, 16);
            this.reasonLabel.TabIndex = 12;
            this.reasonLabel.Text = "Reason:";
            // 
            // testCountLabel
            // 
            this.testCountLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.testCountLabel.Location = new System.Drawing.Point(5, 73);
            this.testCountLabel.Name = "testCountLabel";
            this.testCountLabel.Size = new System.Drawing.Size(80, 15);
            this.testCountLabel.TabIndex = 8;
            this.testCountLabel.Text = "Test Count:";
            // 
            // runState
            // 
            this.runState.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.runState.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.runState.Location = new System.Drawing.Point(228, 73);
            this.runState.Name = "runState";
            this.runState.Size = new System.Drawing.Size(86, 13);
            this.runState.TabIndex = 11;
            this.runState.Text = "NonRunnable";
            // 
            // runStateLabel
            // 
            this.runStateLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.runStateLabel.Location = new System.Drawing.Point(169, 73);
            this.runStateLabel.Name = "runStateLabel";
            this.runStateLabel.Size = new System.Drawing.Size(61, 13);
            this.runStateLabel.TabIndex = 10;
            this.runStateLabel.Text = "Run State:";
            // 
            // testType
            // 
            this.testType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.testType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.testType.Location = new System.Drawing.Point(101, 4);
            this.testType.Name = "testType";
            this.testType.Size = new System.Drawing.Size(212, 13);
            this.testType.TabIndex = 1;
            // 
            // testTypeLabel
            // 
            this.testTypeLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.testTypeLabel.Location = new System.Drawing.Point(5, 4);
            this.testTypeLabel.Name = "testTypeLabel";
            this.testTypeLabel.Size = new System.Drawing.Size(80, 16);
            this.testTypeLabel.TabIndex = 0;
            this.testTypeLabel.Text = "Test Type:";
            // 
            // categoriesLabel
            // 
            this.categoriesLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.categoriesLabel.Location = new System.Drawing.Point(5, 56);
            this.categoriesLabel.Name = "categoriesLabel";
            this.categoriesLabel.Size = new System.Drawing.Size(80, 16);
            this.categoriesLabel.TabIndex = 6;
            this.categoriesLabel.Text = "Categories:";
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.descriptionLabel.Location = new System.Drawing.Point(5, 38);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(80, 17);
            this.descriptionLabel.TabIndex = 4;
            this.descriptionLabel.Text = "Description:";
            // 
            // fullName
            // 
            this.fullName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fullName.CopySupported = true;
            this.fullName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.fullName.Location = new System.Drawing.Point(101, 21);
            this.fullName.Name = "fullName";
            this.fullName.Size = new System.Drawing.Size(212, 13);
            this.fullName.TabIndex = 3;
            // 
            // fullNameLabel
            // 
            this.fullNameLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.fullNameLabel.Location = new System.Drawing.Point(5, 21);
            this.fullNameLabel.Name = "fullNameLabel";
            this.fullNameLabel.Size = new System.Drawing.Size(80, 17);
            this.fullNameLabel.TabIndex = 2;
            this.fullNameLabel.Text = "Full Name:";
            // 
            // displayHiddenProperties
            // 
            this.displayHiddenProperties.AutoSize = true;
            this.displayHiddenProperties.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.displayHiddenProperties.Location = new System.Drawing.Point(74, 106);
            this.displayHiddenProperties.Name = "displayHiddenProperties";
            this.displayHiddenProperties.Size = new System.Drawing.Size(144, 17);
            this.displayHiddenProperties.TabIndex = 16;
            this.displayHiddenProperties.Text = "Display hidden properties";
            this.displayHiddenProperties.UseVisualStyleBackColor = true;
            this.displayHiddenProperties.CheckedChanged += new System.EventHandler(this.hiddenProperties_CheckedChanged);
            // 
            // resultPanel
            // 
            this.resultPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultPanel.BackColor = System.Drawing.SystemColors.Control;
            this.resultPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.resultPanel.Controls.Add(this.assertCount);
            this.resultPanel.Controls.Add(this.elapsedTime);
            this.resultPanel.Controls.Add(this.outcomeLabel);
            this.resultPanel.Controls.Add(this.outcome);
            this.resultPanel.Controls.Add(this.assertCountLabel);
            this.resultPanel.Controls.Add(this.messageLabel);
            this.resultPanel.Controls.Add(this.elapsedTimeLabel);
            this.resultPanel.Controls.Add(this.stackTraceLabel);
            this.resultPanel.Controls.Add(this.message);
            this.resultPanel.Controls.Add(this.stackTrace);
            this.resultPanel.Location = new System.Drawing.Point(6, 349);
            this.resultPanel.Name = "resultPanel";
            this.resultPanel.Size = new System.Drawing.Size(324, 145);
            this.resultPanel.TabIndex = 0;
            this.resultPanel.Text = "Result";
            // 
            // assertCount
            // 
            this.assertCount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.assertCount.Location = new System.Drawing.Point(231, 21);
            this.assertCount.Name = "assertCount";
            this.assertCount.Size = new System.Drawing.Size(49, 13);
            this.assertCount.TabIndex = 5;
            // 
            // elapsedTime
            // 
            this.elapsedTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.elapsedTime.Location = new System.Drawing.Point(84, 22);
            this.elapsedTime.Name = "elapsedTime";
            this.elapsedTime.Size = new System.Drawing.Size(67, 13);
            this.elapsedTime.TabIndex = 3;
            // 
            // outcomeLabel
            // 
            this.outcomeLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.outcomeLabel.Location = new System.Drawing.Point(5, 4);
            this.outcomeLabel.Name = "outcomeLabel";
            this.outcomeLabel.Size = new System.Drawing.Size(53, 13);
            this.outcomeLabel.TabIndex = 0;
            this.outcomeLabel.Text = "Outcome:";
            // 
            // outcome
            // 
            this.outcome.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.outcome.Location = new System.Drawing.Point(84, 4);
            this.outcome.Name = "outcome";
            this.outcome.Size = new System.Drawing.Size(110, 16);
            this.outcome.TabIndex = 1;
            this.outcome.Text = "Inconclusive";
            // 
            // assertCountLabel
            // 
            this.assertCountLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.assertCountLabel.Location = new System.Drawing.Point(167, 21);
            this.assertCountLabel.Name = "assertCountLabel";
            this.assertCountLabel.Size = new System.Drawing.Size(44, 13);
            this.assertCountLabel.TabIndex = 4;
            this.assertCountLabel.Text = "Asserts:";
            // 
            // messageLabel
            // 
            this.messageLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.messageLabel.Location = new System.Drawing.Point(5, 39);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(80, 17);
            this.messageLabel.TabIndex = 6;
            this.messageLabel.Text = "Message:";
            // 
            // elapsedTimeLabel
            // 
            this.elapsedTimeLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.elapsedTimeLabel.Location = new System.Drawing.Point(5, 22);
            this.elapsedTimeLabel.Name = "elapsedTimeLabel";
            this.elapsedTimeLabel.Size = new System.Drawing.Size(74, 13);
            this.elapsedTimeLabel.TabIndex = 2;
            this.elapsedTimeLabel.Text = "Elapsed Time:";
            // 
            // stackTraceLabel
            // 
            this.stackTraceLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.stackTraceLabel.Location = new System.Drawing.Point(5, 56);
            this.stackTraceLabel.Name = "stackTraceLabel";
            this.stackTraceLabel.Size = new System.Drawing.Size(72, 14);
            this.stackTraceLabel.TabIndex = 8;
            this.stackTraceLabel.Text = "Stack:";
            // 
            // message
            // 
            this.message.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.message.CopySupported = true;
            this.message.Expansion = TestCentric.Gui.Controls.TipWindow.ExpansionStyle.Both;
            this.message.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.message.Location = new System.Drawing.Point(84, 39);
            this.message.Name = "message";
            this.message.Size = new System.Drawing.Size(232, 13);
            this.message.TabIndex = 7;
            // 
            // stackTrace
            // 
            this.stackTrace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stackTrace.BackColor = System.Drawing.Color.LightYellow;
            this.stackTrace.CopySupported = true;
            this.stackTrace.Expansion = TestCentric.Gui.Controls.TipWindow.ExpansionStyle.Both;
            this.stackTrace.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.stackTrace.Location = new System.Drawing.Point(6, 73);
            this.stackTrace.Name = "stackTrace";
            this.stackTrace.Size = new System.Drawing.Size(310, 64);
            this.stackTrace.TabIndex = 9;
            // 
            // packagePanel
            // 
            this.packagePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.packagePanel.BackColor = System.Drawing.SystemColors.Control;
            this.packagePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.packagePanel.Controls.Add(this.packageSettingsLabel);
            this.packagePanel.Controls.Add(this.packageSettings);
            this.packagePanel.Location = new System.Drawing.Point(6, 30);
            this.packagePanel.Name = "packagePanel";
            this.packagePanel.Size = new System.Drawing.Size(324, 96);
            this.packagePanel.TabIndex = 4;
            this.packagePanel.Text = "Package Settings";
            // 
            // packageSettingsLabel
            // 
            this.packageSettingsLabel.AutoSize = true;
            this.packageSettingsLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.packageSettingsLabel.Location = new System.Drawing.Point(5, 4);
            this.packageSettingsLabel.Name = "packageSettingsLabel";
            this.packageSettingsLabel.Size = new System.Drawing.Size(94, 13);
            this.packageSettingsLabel.TabIndex = 0;
            this.packageSettingsLabel.Text = "Package Settings:";
            // 
            // packageSettings
            // 
            this.packageSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.packageSettings.BackColor = System.Drawing.Color.LightYellow;
            this.packageSettings.Expansion = TestCentric.Gui.Controls.TipWindow.ExpansionStyle.Both;
            this.packageSettings.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.packageSettings.Location = new System.Drawing.Point(6, 22);
            this.packageSettings.Name = "packageSettings";
            this.packageSettings.Size = new System.Drawing.Size(310, 64);
            this.packageSettings.TabIndex = 1;
            // 
            // TestPropertiesDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(336, 500);
            this.Controls.Add(this.packagePanel);
            this.Controls.Add(this.resultPanel);
            this.Controls.Add(this.testPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TestPropertiesDisplay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.ResizeEnd += new System.EventHandler(this.TestPropertiesDialog_ResizeEnd);
            this.Controls.SetChildIndex(this.testPanel, 0);
            this.Controls.SetChildIndex(this.resultPanel, 0);
            this.Controls.SetChildIndex(this.packagePanel, 0);
            this.testPanel.ResumeLayout(false);
            this.testPanel.PerformLayout();
            this.resultPanel.ResumeLayout(false);
            this.packagePanel.ResumeLayout(false);
            this.packagePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel testPanel;
        private ExpandingLabel description;
        private System.Windows.Forms.Label categories;
        private TestCentric.Gui.Controls.ExpandingLabel properties;
        private System.Windows.Forms.Label propertiesLabel;
        private System.Windows.Forms.Label testCaseCount;
        private ExpandingLabel skipReason;
        private System.Windows.Forms.Label reasonLabel;
        private System.Windows.Forms.Label testCountLabel;
        private System.Windows.Forms.Label runState;
        private System.Windows.Forms.Label runStateLabel;
        private System.Windows.Forms.Label testType;
        private System.Windows.Forms.Label testTypeLabel;
        private System.Windows.Forms.Label categoriesLabel;
        private System.Windows.Forms.Label descriptionLabel;
        private ExpandingLabel fullName;
        private System.Windows.Forms.Label fullNameLabel;
        private System.Windows.Forms.Panel resultPanel;
        private System.Windows.Forms.Label assertCountLabel;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.Label elapsedTimeLabel;
        private System.Windows.Forms.Label stackTraceLabel;
        private ExpandingLabel message;
        private ExpandingLabel stackTrace;
        private System.Windows.Forms.CheckBox displayHiddenProperties;
        private System.Windows.Forms.Panel packagePanel;
        private TestCentric.Gui.Controls.ExpandingLabel packageSettings;
        private System.Windows.Forms.Label outcome;
        private System.Windows.Forms.Label packageSettingsLabel;
        private System.Windows.Forms.Label outcomeLabel;
        private System.Windows.Forms.Label elapsedTime;
        private System.Windows.Forms.Label assertCount;
    }
}
