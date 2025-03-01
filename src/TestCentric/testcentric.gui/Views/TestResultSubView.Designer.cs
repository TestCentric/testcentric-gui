using TestCentric.Gui.Controls;
using TestCentric.Gui.Properties;

namespace TestCentric.Gui.Views
{
    partial class TestResultSubView
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
            this.testCount = new System.Windows.Forms.Label();
            this.testCountLabel = new System.Windows.Forms.Label();
            this.detailSectionFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.passedPictureBox = new System.Windows.Forms.PictureBox();
            this.passedLabel = new System.Windows.Forms.Label();
            this.failedPictureBox = new System.Windows.Forms.PictureBox();
            this.failedLabel = new System.Windows.Forms.Label();
            this.warningsPictureBox = new System.Windows.Forms.PictureBox();
            this.warningLabel = new System.Windows.Forms.Label();
            this.inconclusivePictureBox = new System.Windows.Forms.PictureBox();
            this.inconclusiveLabel = new System.Windows.Forms.Label();
            this.ignoredPictureBox = new System.Windows.Forms.PictureBox();
            this.ignoredLabel = new System.Windows.Forms.Label();
            this.skippedPictureBox = new System.Windows.Forms.PictureBox();
            this.skippedLabel = new System.Windows.Forms.Label();
            this.notRunPictureBox = new System.Windows.Forms.PictureBox();
            this.notRunLabel = new System.Windows.Forms.Label();
            this.duration = new System.Windows.Forms.Label();
            this.assertCountLabel = new System.Windows.Forms.Label();
            this.outcomeLabel = new System.Windows.Forms.Label();
            this.assertCount = new System.Windows.Forms.Label();
            this.durationLabel = new System.Windows.Forms.Label();
            this.outcome = new System.Windows.Forms.Label();
            this.detailSectionBackgroundPanel = new System.Windows.Forms.Panel();
            this.detailSectionFlowLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.passedPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.failedPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.warningsPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inconclusivePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ignoredPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.skippedPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.notRunPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // testCount
            // 
            this.testCount.Location = new System.Drawing.Point(107, 14);
            this.testCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.testCount.Name = "testCount";
            this.testCount.Size = new System.Drawing.Size(62, 20);
            this.testCount.TabIndex = 56;
            // 
            // testCountLabel
            // 
            this.testCountLabel.Location = new System.Drawing.Point(4, 14);
            this.testCountLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.testCountLabel.Name = "testCountLabel";
            this.testCountLabel.Size = new System.Drawing.Size(95, 20);
            this.testCountLabel.TabIndex = 55;
            this.testCountLabel.Text = "Test Count:";
            // 
            // detailSectionFlowLayoutPanel
            // 
            this.detailSectionFlowLayoutPanel.BackColor = System.Drawing.Color.LightYellow;
            this.detailSectionFlowLayoutPanel.Controls.Add(this.passedPictureBox);
            this.detailSectionFlowLayoutPanel.Controls.Add(this.passedLabel);
            this.detailSectionFlowLayoutPanel.SetFlowBreak(this.passedLabel, true);
            this.detailSectionFlowLayoutPanel.Controls.Add(this.failedPictureBox);
            this.detailSectionFlowLayoutPanel.Controls.Add(this.failedLabel);
            this.detailSectionFlowLayoutPanel.SetFlowBreak(this.failedLabel, true);
            this.detailSectionFlowLayoutPanel.Controls.Add(this.warningsPictureBox);
            this.detailSectionFlowLayoutPanel.Controls.Add(this.warningLabel);
            this.detailSectionFlowLayoutPanel.SetFlowBreak(this.warningLabel, true);
            this.detailSectionFlowLayoutPanel.Controls.Add(this.inconclusivePictureBox);
            this.detailSectionFlowLayoutPanel.Controls.Add(this.inconclusiveLabel);
            this.detailSectionFlowLayoutPanel.SetFlowBreak(this.inconclusiveLabel, true);
            this.detailSectionFlowLayoutPanel.Controls.Add(this.ignoredPictureBox);
            this.detailSectionFlowLayoutPanel.Controls.Add(this.ignoredLabel);
            this.detailSectionFlowLayoutPanel.SetFlowBreak(this.ignoredLabel, true);
            this.detailSectionFlowLayoutPanel.Controls.Add(this.skippedPictureBox);
            this.detailSectionFlowLayoutPanel.Controls.Add(this.skippedLabel);
            this.detailSectionFlowLayoutPanel.SetFlowBreak(this.skippedLabel, true);
            this.detailSectionFlowLayoutPanel.Controls.Add(this.notRunPictureBox);
            this.detailSectionFlowLayoutPanel.Controls.Add(this.notRunLabel);
            this.detailSectionFlowLayoutPanel.Location = new System.Drawing.Point(50, 48);
            this.detailSectionFlowLayoutPanel.Name = "detailSectionFlowLayoutPanel";
            this.detailSectionFlowLayoutPanel.AutoSize = true;
            this.detailSectionFlowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.detailSectionFlowLayoutPanel.Size = new System.Drawing.Size(191, 184);
            this.detailSectionFlowLayoutPanel.TabIndex = 54;
            this.detailSectionFlowLayoutPanel.Visible = false;
            // 
            // passedPictureBox
            // 
            this.passedPictureBox.Location = new System.Drawing.Point(3, 2);
            this.passedPictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.passedPictureBox.Name = "passedPictureBox";
            this.passedPictureBox.Size = new System.Drawing.Size(16, 16);
            this.passedPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.passedPictureBox.TabIndex = 0;
            this.passedPictureBox.TabStop = false;
            // 
            // passedLabel
            // 
            this.passedLabel.Location = new System.Drawing.Point(25, 3);
            this.passedLabel.Margin = new System.Windows.Forms.Padding(3);
            this.passedLabel.Name = "passedLabel";
            this.passedLabel.Size = new System.Drawing.Size(155, 20);
            this.passedLabel.TabIndex = 46;
            // 
            // failedPictureBox
            // 
            this.failedPictureBox.Location = new System.Drawing.Point(3, 28);
            this.failedPictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.failedPictureBox.Name = "failedPictureBox";
            this.failedPictureBox.Size = new System.Drawing.Size(16, 16);
            this.failedPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.failedPictureBox.TabIndex = 47;
            this.failedPictureBox.TabStop = false;
            // 
            // failedLabel
            // 
            this.failedLabel.Location = new System.Drawing.Point(25, 29);
            this.failedLabel.Margin = new System.Windows.Forms.Padding(3);
            this.failedLabel.Name = "failedLabel";
            this.failedLabel.Size = new System.Drawing.Size(155, 20);
            this.failedLabel.TabIndex = 48;
            // 
            // warningsPictureBox
            // 
            this.warningsPictureBox.Location = new System.Drawing.Point(3, 54);
            this.warningsPictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.warningsPictureBox.Name = "warningsPictureBox";
            this.warningsPictureBox.Size = new System.Drawing.Size(16, 16);
            this.warningsPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.warningsPictureBox.TabIndex = 49;
            this.warningsPictureBox.TabStop = false;
            // 
            // warningLabel
            // 
            this.warningLabel.Location = new System.Drawing.Point(25, 55);
            this.warningLabel.Margin = new System.Windows.Forms.Padding(3);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(155, 20);
            this.warningLabel.TabIndex = 50;
            // 
            // inconclusivePictureBox
            // 
            this.inconclusivePictureBox.Location = new System.Drawing.Point(3, 80);
            this.inconclusivePictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.inconclusivePictureBox.Name = "inconclusivePictureBox";
            this.inconclusivePictureBox.Size = new System.Drawing.Size(16, 16);
            this.inconclusivePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.inconclusivePictureBox.TabIndex = 51;
            this.inconclusivePictureBox.TabStop = false;
            // 
            // inconclusiveLabel
            // 
            this.inconclusiveLabel.Location = new System.Drawing.Point(25, 81);
            this.inconclusiveLabel.Margin = new System.Windows.Forms.Padding(3);
            this.inconclusiveLabel.Name = "inconclusiveLabel";
            this.inconclusiveLabel.Size = new System.Drawing.Size(142, 20);
            this.inconclusiveLabel.TabIndex = 52;
            // 
            // ignoredPictureBox
            // 
            this.ignoredPictureBox.Location = new System.Drawing.Point(3, 106);
            this.ignoredPictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ignoredPictureBox.Name = "ignoredPictureBox";
            this.ignoredPictureBox.Size = new System.Drawing.Size(16, 16);
            this.ignoredPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.ignoredPictureBox.TabIndex = 53;
            this.ignoredPictureBox.TabStop = false;
            // 
            // ignoredLabel
            // 
            this.ignoredLabel.Location = new System.Drawing.Point(25, 107);
            this.ignoredLabel.Margin = new System.Windows.Forms.Padding(3);
            this.ignoredLabel.Name = "ignoredLabel";
            this.ignoredLabel.Size = new System.Drawing.Size(155, 20);
            this.ignoredLabel.TabIndex = 54;
            // 
            // skippedPictureBox
            // 
            this.skippedPictureBox.Location = new System.Drawing.Point(3, 132);
            this.skippedPictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.skippedPictureBox.Name = "skippedPictureBox";
            this.skippedPictureBox.Size = new System.Drawing.Size(16, 16);
            this.skippedPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.skippedPictureBox.TabIndex = 55;
            this.skippedPictureBox.TabStop = false;
            // 
            // skippedLabel
            // 
            this.skippedLabel.Location = new System.Drawing.Point(25, 133);
            this.skippedLabel.Margin = new System.Windows.Forms.Padding(3);
            this.skippedLabel.Name = "skippedLabel";
            this.skippedLabel.Size = new System.Drawing.Size(155, 20);
            this.skippedLabel.TabIndex = 56;
            // 
            // notRunPictureBox
            // 
            this.notRunPictureBox.Location = new System.Drawing.Point(3, 158);
            this.notRunPictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.notRunPictureBox.Name = "notRunPictureBox";
            this.notRunPictureBox.Size = new System.Drawing.Size(16, 16);
            this.notRunPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.notRunPictureBox.TabIndex = 57;
            this.notRunPictureBox.TabStop = false;
            // 
            // notRunLabel
            // 
            this.notRunLabel.Location = new System.Drawing.Point(25, 159);
            this.notRunLabel.Margin = new System.Windows.Forms.Padding(3);
            this.notRunLabel.Name = "notRunLabel";
            this.notRunLabel.Size = new System.Drawing.Size(155, 20);
            this.notRunLabel.TabIndex = 58;
            // 
            // duration
            // 
            this.duration.Location = new System.Drawing.Point(536, 14);
            this.duration.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.duration.Name = "duration";
            this.duration.Size = new System.Drawing.Size(100, 20);
            this.duration.TabIndex = 30;
            // 
            // assertCountLabel
            // 
            this.assertCountLabel.Location = new System.Drawing.Point(644, 14);
            this.assertCountLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.assertCountLabel.Name = "assertCountLabel";
            this.assertCountLabel.Size = new System.Drawing.Size(75, 20);
            this.assertCountLabel.TabIndex = 23;
            this.assertCountLabel.Text = "Asserts:";
            // 
            // outcomeLabel
            // 
            this.outcomeLabel.Location = new System.Drawing.Point(177, 14);
            this.outcomeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.outcomeLabel.Name = "outcomeLabel";
            this.outcomeLabel.Size = new System.Drawing.Size(80, 20);
            this.outcomeLabel.TabIndex = 18;
            this.outcomeLabel.Text = "Outcome:";
            // 
            // assertCount
            // 
            this.assertCount.Location = new System.Drawing.Point(727, 14);
            this.assertCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.assertCount.Name = "assertCount";
            this.assertCount.Size = new System.Drawing.Size(44, 20);
            this.assertCount.TabIndex = 24;
            // 
            // durationLabel
            // 
            this.durationLabel.Location = new System.Drawing.Point(448, 14);
            this.durationLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.durationLabel.Name = "durationLabel";
            this.durationLabel.Size = new System.Drawing.Size(80, 20);
            this.durationLabel.TabIndex = 21;
            this.durationLabel.Text = "Duration:";
            // 
            // outcome
            // 
            this.outcome.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outcome.Location = new System.Drawing.Point(265, 14);
            this.outcome.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.outcome.Name = "outcome";
            this.outcome.Size = new System.Drawing.Size(175, 20);
            this.outcome.TabIndex = 19;
            // 
            // detailSectionBackgroundPanel
            // 
            this.detailSectionBackgroundPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.detailSectionBackgroundPanel.BackColor = System.Drawing.Color.LightYellow;
            this.detailSectionBackgroundPanel.Location = new System.Drawing.Point(8, 45);
            this.detailSectionBackgroundPanel.Name = "detailSectionBackgroundPanel";
            this.detailSectionBackgroundPanel.Size = new System.Drawing.Size(762, 208);
            this.detailSectionBackgroundPanel.TabIndex = 57;
            // 
            // TestResultSubView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.testCount);
            this.Controls.Add(this.testCountLabel);
            this.Controls.Add(this.detailSectionFlowLayoutPanel);
            this.Controls.Add(this.duration);
            this.Controls.Add(this.assertCountLabel);
            this.Controls.Add(this.outcomeLabel);
            this.Controls.Add(this.assertCount);
            this.Controls.Add(this.durationLabel);
            this.Controls.Add(this.outcome);
            this.Controls.Add(this.detailSectionBackgroundPanel);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "TestResultSubView";
            this.Size = new System.Drawing.Size(782, 265);
            this.detailSectionFlowLayoutPanel.ResumeLayout(false);
            this.detailSectionFlowLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.passedPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.failedPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.warningsPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inconclusivePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ignoredPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.skippedPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.notRunPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label testCountLabel;
        private System.Windows.Forms.Label testCount;
        private System.Windows.Forms.Label outcomeLabel;
        private System.Windows.Forms.Label outcome;
        private System.Windows.Forms.Label durationLabel;
        private System.Windows.Forms.Label duration;
        private System.Windows.Forms.Label assertCountLabel;
        private System.Windows.Forms.Label assertCount;
        private System.Windows.Forms.Label passedLabel;
        private System.Windows.Forms.Label failedLabel;
        private System.Windows.Forms.Label warningLabel;
        private System.Windows.Forms.Label inconclusiveLabel;
        private System.Windows.Forms.Label ignoredLabel;
        private System.Windows.Forms.Label skippedLabel;
        private System.Windows.Forms.Label notRunLabel;
        private System.Windows.Forms.PictureBox passedPictureBox;
        private System.Windows.Forms.PictureBox failedPictureBox;
        private System.Windows.Forms.PictureBox warningsPictureBox;
        private System.Windows.Forms.PictureBox inconclusivePictureBox;
        private System.Windows.Forms.PictureBox ignoredPictureBox;
        private System.Windows.Forms.PictureBox skippedPictureBox;
        private System.Windows.Forms.PictureBox notRunPictureBox;
        private System.Windows.Forms.FlowLayoutPanel detailSectionFlowLayoutPanel;
        private System.Windows.Forms.Panel detailSectionBackgroundPanel;
    }
}
