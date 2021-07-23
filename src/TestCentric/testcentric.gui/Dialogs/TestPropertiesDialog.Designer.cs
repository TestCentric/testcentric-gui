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
            this.testResult = new System.Windows.Forms.Label();
            this.pinButton = new System.Windows.Forms.CheckBox();
            this.testName = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.description = new TestCentric.Gui.Controls.ExpandingLabel();
            this.categories = new System.Windows.Forms.Label();
            this.properties = new System.Windows.Forms.ListBox();
            this.propertiesLabel = new System.Windows.Forms.Label();
            this.testCaseCount = new System.Windows.Forms.Label();
            this.ignoreReason = new TestCentric.Gui.Controls.ExpandingLabel();
            this.ignoreReasonLabel = new System.Windows.Forms.Label();
            this.testCaseCountLabel = new System.Windows.Forms.Label();
            this.shouldRun = new System.Windows.Forms.Label();
            this.shouldRunLabel = new System.Windows.Forms.Label();
            this.testType = new System.Windows.Forms.Label();
            this.testTypeLabel = new System.Windows.Forms.Label();
            this.categoriesLabel = new System.Windows.Forms.Label();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.fullName = new TestCentric.Gui.Controls.ExpandingLabel();
            this.fullNameLabel = new System.Windows.Forms.Label();
            this.hiddenProperties = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.assertCount = new System.Windows.Forms.Label();
            this.messageLabel = new System.Windows.Forms.Label();
            this.elapsedTime = new System.Windows.Forms.Label();
            this.stackTraceLabel = new System.Windows.Forms.Label();
            this.message = new TestCentric.Gui.Controls.ExpandingLabel();
            this.stackTrace = new TestCentric.Gui.Controls.ExpandingLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.packageSettings = new System.Windows.Forms.ListBox();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // testResult
            // 
            this.testResult.Location = new System.Drawing.Point(9, 11);
            this.testResult.Name = "testResult";
            this.testResult.Size = new System.Drawing.Size(110, 16);
            this.testResult.TabIndex = 0;
            this.testResult.Text = "Inconclusive";
            // 
            // pinButton
            // 
            this.pinButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pinButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.pinButton.Location = new System.Drawing.Point(332, 10);
            this.pinButton.Name = "pinButton";
            this.pinButton.Size = new System.Drawing.Size(20, 20);
            this.pinButton.TabIndex = 2;
            this.pinButton.Click += new System.EventHandler(this.pinButton_Click);
            // 
            // testName
            // 
            this.testName.Location = new System.Drawing.Point(135, 12);
            this.testName.Name = "testName";
            this.testName.Size = new System.Drawing.Size(176, 14);
            this.testName.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.description);
            this.groupBox2.Controls.Add(this.categories);
            this.groupBox2.Controls.Add(this.properties);
            this.groupBox2.Controls.Add(this.propertiesLabel);
            this.groupBox2.Controls.Add(this.testCaseCount);
            this.groupBox2.Controls.Add(this.ignoreReason);
            this.groupBox2.Controls.Add(this.ignoreReasonLabel);
            this.groupBox2.Controls.Add(this.testCaseCountLabel);
            this.groupBox2.Controls.Add(this.shouldRun);
            this.groupBox2.Controls.Add(this.shouldRunLabel);
            this.groupBox2.Controls.Add(this.testType);
            this.groupBox2.Controls.Add(this.testTypeLabel);
            this.groupBox2.Controls.Add(this.categoriesLabel);
            this.groupBox2.Controls.Add(this.descriptionLabel);
            this.groupBox2.Controls.Add(this.fullName);
            this.groupBox2.Controls.Add(this.fullNameLabel);
            this.groupBox2.Controls.Add(this.hiddenProperties);
            this.groupBox2.Location = new System.Drawing.Point(12, 144);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(340, 246);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Test Details";
            // 
            // description
            // 
            this.description.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.description.CopySupported = true;
            this.description.Expansion = TestCentric.Gui.Controls.TipWindow.ExpansionStyle.Both;
            this.description.Location = new System.Drawing.Point(101, 64);
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(230, 17);
            this.description.TabIndex = 5;
            // 
            // categories
            // 
            this.categories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.categories.Location = new System.Drawing.Point(101, 86);
            this.categories.Name = "categories";
            this.categories.Size = new System.Drawing.Size(230, 16);
            this.categories.TabIndex = 7;
            // 
            // properties
            // 
            this.properties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.properties.Location = new System.Drawing.Point(16, 162);
            this.properties.Name = "properties";
            this.properties.Size = new System.Drawing.Size(312, 69);
            this.properties.TabIndex = 15;
            // 
            // propertiesLabel
            // 
            this.propertiesLabel.Location = new System.Drawing.Point(13, 143);
            this.propertiesLabel.Name = "propertiesLabel";
            this.propertiesLabel.Size = new System.Drawing.Size(80, 16);
            this.propertiesLabel.TabIndex = 14;
            this.propertiesLabel.Text = "Properties:";
            // 
            // testCaseCount
            // 
            this.testCaseCount.Location = new System.Drawing.Point(101, 108);
            this.testCaseCount.Name = "testCaseCount";
            this.testCaseCount.Size = new System.Drawing.Size(48, 15);
            this.testCaseCount.TabIndex = 9;
            // 
            // ignoreReason
            // 
            this.ignoreReason.CopySupported = true;
            this.ignoreReason.Expansion = TestCentric.Gui.Controls.TipWindow.ExpansionStyle.Vertical;
            this.ignoreReason.Location = new System.Drawing.Point(101, 125);
            this.ignoreReason.Name = "ignoreReason";
            this.ignoreReason.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ignoreReason.Size = new System.Drawing.Size(311, 16);
            this.ignoreReason.TabIndex = 13;
            // 
            // ignoreReasonLabel
            // 
            this.ignoreReasonLabel.Location = new System.Drawing.Point(13, 125);
            this.ignoreReasonLabel.Name = "ignoreReasonLabel";
            this.ignoreReasonLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ignoreReasonLabel.Size = new System.Drawing.Size(80, 16);
            this.ignoreReasonLabel.TabIndex = 12;
            this.ignoreReasonLabel.Text = "Reason:";
            // 
            // testCaseCountLabel
            // 
            this.testCaseCountLabel.Location = new System.Drawing.Point(13, 108);
            this.testCaseCountLabel.Name = "testCaseCountLabel";
            this.testCaseCountLabel.Size = new System.Drawing.Size(80, 15);
            this.testCaseCountLabel.TabIndex = 8;
            this.testCaseCountLabel.Text = "Test Count:";
            // 
            // shouldRun
            // 
            this.shouldRun.Location = new System.Drawing.Point(299, 108);
            this.shouldRun.Name = "shouldRun";
            this.shouldRun.Size = new System.Drawing.Size(29, 15);
            this.shouldRun.TabIndex = 11;
            this.shouldRun.Text = "Yes";
            // 
            // shouldRunLabel
            // 
            this.shouldRunLabel.Location = new System.Drawing.Point(183, 108);
            this.shouldRunLabel.Name = "shouldRunLabel";
            this.shouldRunLabel.Size = new System.Drawing.Size(84, 15);
            this.shouldRunLabel.TabIndex = 10;
            this.shouldRunLabel.Text = "Should Run?";
            // 
            // testType
            // 
            this.testType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.testType.Location = new System.Drawing.Point(101, 24);
            this.testType.Name = "testType";
            this.testType.Size = new System.Drawing.Size(230, 16);
            this.testType.TabIndex = 1;
            // 
            // testTypeLabel
            // 
            this.testTypeLabel.Location = new System.Drawing.Point(13, 24);
            this.testTypeLabel.Name = "testTypeLabel";
            this.testTypeLabel.Size = new System.Drawing.Size(80, 16);
            this.testTypeLabel.TabIndex = 0;
            this.testTypeLabel.Text = "Test Type:";
            // 
            // categoriesLabel
            // 
            this.categoriesLabel.Location = new System.Drawing.Point(13, 86);
            this.categoriesLabel.Name = "categoriesLabel";
            this.categoriesLabel.Size = new System.Drawing.Size(80, 16);
            this.categoriesLabel.TabIndex = 6;
            this.categoriesLabel.Text = "Categories:";
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.Location = new System.Drawing.Point(13, 64);
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
            this.fullName.Location = new System.Drawing.Point(101, 43);
            this.fullName.Name = "fullName";
            this.fullName.Size = new System.Drawing.Size(230, 15);
            this.fullName.TabIndex = 3;
            // 
            // fullNameLabel
            // 
            this.fullNameLabel.Location = new System.Drawing.Point(13, 43);
            this.fullNameLabel.Name = "fullNameLabel";
            this.fullNameLabel.Size = new System.Drawing.Size(80, 17);
            this.fullNameLabel.TabIndex = 2;
            this.fullNameLabel.Text = "Full Name:";
            // 
            // hiddenProperties
            // 
            this.hiddenProperties.AutoSize = true;
            this.hiddenProperties.Location = new System.Drawing.Point(186, 143);
            this.hiddenProperties.Name = "hiddenProperties";
            this.hiddenProperties.Size = new System.Drawing.Size(144, 17);
            this.hiddenProperties.TabIndex = 16;
            this.hiddenProperties.Text = "Display hidden properties";
            this.hiddenProperties.UseVisualStyleBackColor = true;
            this.hiddenProperties.CheckedChanged += new System.EventHandler(this.hiddenProperties_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.assertCount);
            this.groupBox3.Controls.Add(this.messageLabel);
            this.groupBox3.Controls.Add(this.elapsedTime);
            this.groupBox3.Controls.Add(this.stackTraceLabel);
            this.groupBox3.Controls.Add(this.message);
            this.groupBox3.Controls.Add(this.stackTrace);
            this.groupBox3.Location = new System.Drawing.Point(12, 396);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(340, 170);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Result";
            // 
            // assertCount
            // 
            this.assertCount.Location = new System.Drawing.Point(196, 26);
            this.assertCount.Name = "assertCount";
            this.assertCount.Size = new System.Drawing.Size(131, 16);
            this.assertCount.TabIndex = 1;
            this.assertCount.Text = "Assert Count:";
            // 
            // messageLabel
            // 
            this.messageLabel.Location = new System.Drawing.Point(10, 46);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(80, 17);
            this.messageLabel.TabIndex = 2;
            this.messageLabel.Text = "Message:";
            // 
            // elapsedTime
            // 
            this.elapsedTime.Location = new System.Drawing.Point(10, 26);
            this.elapsedTime.Name = "elapsedTime";
            this.elapsedTime.Size = new System.Drawing.Size(136, 16);
            this.elapsedTime.TabIndex = 0;
            this.elapsedTime.Text = "Execution Time:";
            // 
            // stackTraceLabel
            // 
            this.stackTraceLabel.Location = new System.Drawing.Point(10, 67);
            this.stackTraceLabel.Name = "stackTraceLabel";
            this.stackTraceLabel.Size = new System.Drawing.Size(72, 14);
            this.stackTraceLabel.TabIndex = 4;
            this.stackTraceLabel.Text = "Stack:";
            // 
            // message
            // 
            this.message.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.message.CopySupported = true;
            this.message.Expansion = TestCentric.Gui.Controls.TipWindow.ExpansionStyle.Both;
            this.message.Location = new System.Drawing.Point(106, 47);
            this.message.Name = "message";
            this.message.Size = new System.Drawing.Size(222, 17);
            this.message.TabIndex = 3;
            // 
            // stackTrace
            // 
            this.stackTrace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stackTrace.CopySupported = true;
            this.stackTrace.Expansion = TestCentric.Gui.Controls.TipWindow.ExpansionStyle.Both;
            this.stackTrace.Location = new System.Drawing.Point(10, 85);
            this.stackTrace.Name = "stackTrace";
            this.stackTrace.Size = new System.Drawing.Size(315, 70);
            this.stackTrace.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.packageSettings);
            this.groupBox1.Location = new System.Drawing.Point(12, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(340, 105);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Package Settings";
            // 
            // packageSettings
            // 
            this.packageSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.packageSettings.FormattingEnabled = true;
            this.packageSettings.Location = new System.Drawing.Point(9, 22);
            this.packageSettings.Name = "packageSettings";
            this.packageSettings.Size = new System.Drawing.Size(322, 69);
            this.packageSettings.TabIndex = 18;
            // 
            // TestPropertiesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(364, 571);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.testName);
            this.Controls.Add(this.pinButton);
            this.Controls.Add(this.testResult);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(380, 39);
            this.Name = "TestPropertiesDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Test Properties";
            this.ResizeEnd += new System.EventHandler(this.TestPropertiesDialog_ResizeEnd);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label testResult;
        private System.Windows.Forms.CheckBox pinButton;
        private System.Windows.Forms.Label testName;
        private System.Windows.Forms.GroupBox groupBox2;
        private ExpandingLabel description;
        private System.Windows.Forms.Label categories;
        private System.Windows.Forms.ListBox properties;
        private System.Windows.Forms.Label propertiesLabel;
        private System.Windows.Forms.Label testCaseCount;
        private ExpandingLabel ignoreReason;
        private System.Windows.Forms.Label ignoreReasonLabel;
        private System.Windows.Forms.Label testCaseCountLabel;
        private System.Windows.Forms.Label shouldRun;
        private System.Windows.Forms.Label shouldRunLabel;
        private System.Windows.Forms.Label testType;
        private System.Windows.Forms.Label testTypeLabel;
        private System.Windows.Forms.Label categoriesLabel;
        private System.Windows.Forms.Label descriptionLabel;
        private ExpandingLabel fullName;
        private System.Windows.Forms.Label fullNameLabel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label assertCount;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.Label elapsedTime;
        private System.Windows.Forms.Label stackTraceLabel;
        private ExpandingLabel message;
        private ExpandingLabel stackTrace;
        private System.Windows.Forms.CheckBox hiddenProperties;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox packageSettings;
    }
}
