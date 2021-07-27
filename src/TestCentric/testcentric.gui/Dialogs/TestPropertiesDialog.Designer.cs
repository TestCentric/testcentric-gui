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
            this.pinButton = new System.Windows.Forms.CheckBox();
            this.testName = new System.Windows.Forms.Label();
            this.testGroupBox = new System.Windows.Forms.Panel();
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
            this.resultGroupBox = new System.Windows.Forms.Panel();
            this.outcomeLabel = new System.Windows.Forms.Label();
            this.testResult = new System.Windows.Forms.Label();
            this.assertCount = new System.Windows.Forms.Label();
            this.messageLabel = new System.Windows.Forms.Label();
            this.elapsedTime = new System.Windows.Forms.Label();
            this.stackTraceLabel = new System.Windows.Forms.Label();
            this.message = new TestCentric.Gui.Controls.ExpandingLabel();
            this.stackTrace = new TestCentric.Gui.Controls.ExpandingLabel();
            this.packageGroupBox = new System.Windows.Forms.Panel();
            this.packageSettingsLabel = new System.Windows.Forms.Label();
            this.packageSettings = new TestCentric.Gui.Controls.ExpandingLabel();
            this.testGroupBox.SuspendLayout();
            this.resultGroupBox.SuspendLayout();
            this.packageGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // pinButton
            // 
            resources.ApplyResources(this.pinButton, "pinButton");
            this.pinButton.Name = "pinButton";
            this.pinButton.Click += new System.EventHandler(this.pinButton_Click);
            // 
            // testName
            // 
            resources.ApplyResources(this.testName, "testName");
            this.testName.Name = "testName";
            // 
            // testGroupBox
            // 
            resources.ApplyResources(this.testGroupBox, "testGroupBox");
            this.testGroupBox.BackColor = System.Drawing.SystemColors.Control;
            this.testGroupBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.testGroupBox.Controls.Add(this.description);
            this.testGroupBox.Controls.Add(this.categories);
            this.testGroupBox.Controls.Add(this.properties);
            this.testGroupBox.Controls.Add(this.propertiesLabel);
            this.testGroupBox.Controls.Add(this.testCaseCount);
            this.testGroupBox.Controls.Add(this.ignoreReason);
            this.testGroupBox.Controls.Add(this.ignoreReasonLabel);
            this.testGroupBox.Controls.Add(this.testCaseCountLabel);
            this.testGroupBox.Controls.Add(this.shouldRun);
            this.testGroupBox.Controls.Add(this.shouldRunLabel);
            this.testGroupBox.Controls.Add(this.testType);
            this.testGroupBox.Controls.Add(this.testTypeLabel);
            this.testGroupBox.Controls.Add(this.categoriesLabel);
            this.testGroupBox.Controls.Add(this.descriptionLabel);
            this.testGroupBox.Controls.Add(this.fullName);
            this.testGroupBox.Controls.Add(this.fullNameLabel);
            this.testGroupBox.Controls.Add(this.hiddenProperties);
            this.testGroupBox.Name = "testGroupBox";
            // 
            // description
            // 
            resources.ApplyResources(this.description, "description");
            this.description.CopySupported = true;
            this.description.Expansion = TestCentric.Gui.Controls.TipWindow.ExpansionStyle.Both;
            this.description.Name = "description";
            // 
            // categories
            // 
            resources.ApplyResources(this.categories, "categories");
            this.categories.Name = "categories";
            // 
            // properties
            // 
            resources.ApplyResources(this.properties, "properties");
            this.properties.Name = "properties";
            // 
            // propertiesLabel
            // 
            resources.ApplyResources(this.propertiesLabel, "propertiesLabel");
            this.propertiesLabel.Name = "propertiesLabel";
            // 
            // testCaseCount
            // 
            resources.ApplyResources(this.testCaseCount, "testCaseCount");
            this.testCaseCount.Name = "testCaseCount";
            // 
            // ignoreReason
            // 
            resources.ApplyResources(this.ignoreReason, "ignoreReason");
            this.ignoreReason.CopySupported = true;
            this.ignoreReason.Expansion = TestCentric.Gui.Controls.TipWindow.ExpansionStyle.Vertical;
            this.ignoreReason.Name = "ignoreReason";
            // 
            // ignoreReasonLabel
            // 
            resources.ApplyResources(this.ignoreReasonLabel, "ignoreReasonLabel");
            this.ignoreReasonLabel.Name = "ignoreReasonLabel";
            // 
            // testCaseCountLabel
            // 
            resources.ApplyResources(this.testCaseCountLabel, "testCaseCountLabel");
            this.testCaseCountLabel.Name = "testCaseCountLabel";
            // 
            // shouldRun
            // 
            resources.ApplyResources(this.shouldRun, "shouldRun");
            this.shouldRun.Name = "shouldRun";
            // 
            // shouldRunLabel
            // 
            resources.ApplyResources(this.shouldRunLabel, "shouldRunLabel");
            this.shouldRunLabel.Name = "shouldRunLabel";
            // 
            // testType
            // 
            resources.ApplyResources(this.testType, "testType");
            this.testType.Name = "testType";
            // 
            // testTypeLabel
            // 
            resources.ApplyResources(this.testTypeLabel, "testTypeLabel");
            this.testTypeLabel.Name = "testTypeLabel";
            // 
            // categoriesLabel
            // 
            resources.ApplyResources(this.categoriesLabel, "categoriesLabel");
            this.categoriesLabel.Name = "categoriesLabel";
            // 
            // descriptionLabel
            // 
            resources.ApplyResources(this.descriptionLabel, "descriptionLabel");
            this.descriptionLabel.Name = "descriptionLabel";
            // 
            // fullName
            // 
            resources.ApplyResources(this.fullName, "fullName");
            this.fullName.CopySupported = true;
            this.fullName.Name = "fullName";
            // 
            // fullNameLabel
            // 
            resources.ApplyResources(this.fullNameLabel, "fullNameLabel");
            this.fullNameLabel.Name = "fullNameLabel";
            // 
            // hiddenProperties
            // 
            resources.ApplyResources(this.hiddenProperties, "hiddenProperties");
            this.hiddenProperties.Name = "hiddenProperties";
            this.hiddenProperties.UseVisualStyleBackColor = true;
            this.hiddenProperties.CheckedChanged += new System.EventHandler(this.hiddenProperties_CheckedChanged);
            // 
            // resultGroupBox
            // 
            resources.ApplyResources(this.resultGroupBox, "resultGroupBox");
            this.resultGroupBox.BackColor = System.Drawing.SystemColors.Control;
            this.resultGroupBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.resultGroupBox.Controls.Add(this.outcomeLabel);
            this.resultGroupBox.Controls.Add(this.testResult);
            this.resultGroupBox.Controls.Add(this.assertCount);
            this.resultGroupBox.Controls.Add(this.messageLabel);
            this.resultGroupBox.Controls.Add(this.elapsedTime);
            this.resultGroupBox.Controls.Add(this.stackTraceLabel);
            this.resultGroupBox.Controls.Add(this.message);
            this.resultGroupBox.Controls.Add(this.stackTrace);
            this.resultGroupBox.Name = "resultGroupBox";
            // 
            // outcomeLabel
            // 
            resources.ApplyResources(this.outcomeLabel, "outcomeLabel");
            this.outcomeLabel.Name = "outcomeLabel";
            // 
            // testResult
            // 
            resources.ApplyResources(this.testResult, "testResult");
            this.testResult.Name = "testResult";
            // 
            // assertCount
            // 
            resources.ApplyResources(this.assertCount, "assertCount");
            this.assertCount.Name = "assertCount";
            // 
            // messageLabel
            // 
            resources.ApplyResources(this.messageLabel, "messageLabel");
            this.messageLabel.Name = "messageLabel";
            // 
            // elapsedTime
            // 
            resources.ApplyResources(this.elapsedTime, "elapsedTime");
            this.elapsedTime.Name = "elapsedTime";
            // 
            // stackTraceLabel
            // 
            resources.ApplyResources(this.stackTraceLabel, "stackTraceLabel");
            this.stackTraceLabel.Name = "stackTraceLabel";
            // 
            // message
            // 
            resources.ApplyResources(this.message, "message");
            this.message.CopySupported = true;
            this.message.Expansion = TestCentric.Gui.Controls.TipWindow.ExpansionStyle.Both;
            this.message.Name = "message";
            // 
            // stackTrace
            // 
            resources.ApplyResources(this.stackTrace, "stackTrace");
            this.stackTrace.CopySupported = true;
            this.stackTrace.Expansion = TestCentric.Gui.Controls.TipWindow.ExpansionStyle.Both;
            this.stackTrace.Name = "stackTrace";
            // 
            // packageGroupBox
            // 
            resources.ApplyResources(this.packageGroupBox, "packageGroupBox");
            this.packageGroupBox.BackColor = System.Drawing.SystemColors.Control;
            this.packageGroupBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.packageGroupBox.Controls.Add(this.packageSettingsLabel);
            this.packageGroupBox.Controls.Add(this.packageSettings);
            this.packageGroupBox.Name = "packageGroupBox";
            // 
            // packageSettingsLabel
            // 
            resources.ApplyResources(this.packageSettingsLabel, "packageSettingsLabel");
            this.packageSettingsLabel.Name = "packageSettingsLabel";
            // 
            // packageSettings
            // 
            resources.ApplyResources(this.packageSettings, "packageSettings");
            this.packageSettings.BackColor = System.Drawing.Color.LightYellow;
            this.packageSettings.Expansion = TestCentric.Gui.Controls.TipWindow.ExpansionStyle.Both;
            this.packageSettings.Name = "packageSettings";
            // 
            // TestPropertiesDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.packageGroupBox);
            this.Controls.Add(this.resultGroupBox);
            this.Controls.Add(this.testGroupBox);
            this.Controls.Add(this.testName);
            this.Controls.Add(this.pinButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TestPropertiesDialog";
            this.ResizeEnd += new System.EventHandler(this.TestPropertiesDialog_ResizeEnd);
            this.testGroupBox.ResumeLayout(false);
            this.testGroupBox.PerformLayout();
            this.resultGroupBox.ResumeLayout(false);
            this.packageGroupBox.ResumeLayout(false);
            this.packageGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox pinButton;
        private System.Windows.Forms.Label testName;
        private System.Windows.Forms.Panel testGroupBox;
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
        private System.Windows.Forms.Panel resultGroupBox;
        private System.Windows.Forms.Label assertCount;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.Label elapsedTime;
        private System.Windows.Forms.Label stackTraceLabel;
        private ExpandingLabel message;
        private ExpandingLabel stackTrace;
        private System.Windows.Forms.CheckBox hiddenProperties;
        private System.Windows.Forms.Panel packageGroupBox;
        private TestCentric.Gui.Controls.ExpandingLabel packageSettings;
        private System.Windows.Forms.Label testResult;
        private System.Windows.Forms.Label packageSettingsLabel;
        private System.Windows.Forms.Label outcomeLabel;
    }
}
