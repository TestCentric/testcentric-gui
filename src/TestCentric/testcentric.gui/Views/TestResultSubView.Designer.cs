using TestCentric.Gui.Controls;

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
            this.elapsedTime = new System.Windows.Forms.Label();
            this.assertions = new TestCentric.Gui.Controls.ExpandingLabel();
            this.messageLabel = new System.Windows.Forms.Label();
            this.assertCountLabel = new System.Windows.Forms.Label();
            this.outcomeLabel = new System.Windows.Forms.Label();
            this.assertCount = new System.Windows.Forms.Label();
            this.elapsedTimeLabel = new System.Windows.Forms.Label();
            this.outcome = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // elapsedTime
            // 
            this.elapsedTime.Location = new System.Drawing.Point(81, 26);
            this.elapsedTime.Name = "elapsedTime";
            this.elapsedTime.Size = new System.Drawing.Size(67, 13);
            this.elapsedTime.TabIndex = 30;
            // 
            // assertions
            // 
            this.assertions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assertions.BackColor = System.Drawing.Color.LightYellow;
            this.assertions.Expansion = TestCentric.Gui.Controls.TipWindow.ExpansionStyle.Both;
            this.assertions.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assertions.Location = new System.Drawing.Point(9, 62);
            this.assertions.Name = "assertions";
            this.assertions.Size = new System.Drawing.Size(457, 40);
            this.assertions.TabIndex = 26;
            // 
            // messageLabel
            // 
            this.messageLabel.AutoSize = true;
            this.messageLabel.Location = new System.Drawing.Point(6, 39);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(58, 13);
            this.messageLabel.TabIndex = 25;
            this.messageLabel.Text = "Messages:";
            // 
            // assertCountLabel
            // 
            this.assertCountLabel.Location = new System.Drawing.Point(165, 26);
            this.assertCountLabel.Name = "assertCountLabel";
            this.assertCountLabel.Size = new System.Drawing.Size(44, 13);
            this.assertCountLabel.TabIndex = 23;
            this.assertCountLabel.Text = "Asserts:";
            // 
            // outcomeLabel
            // 
            this.outcomeLabel.Location = new System.Drawing.Point(6, 9);
            this.outcomeLabel.Name = "outcomeLabel";
            this.outcomeLabel.Size = new System.Drawing.Size(53, 13);
            this.outcomeLabel.TabIndex = 18;
            this.outcomeLabel.Text = "Outcome:";
            // 
            // assertCount
            // 
            this.assertCount.Location = new System.Drawing.Point(215, 26);
            this.assertCount.Name = "assertCount";
            this.assertCount.Size = new System.Drawing.Size(49, 13);
            this.assertCount.TabIndex = 24;
            // 
            // elapsedTimeLabel
            // 
            this.elapsedTimeLabel.Location = new System.Drawing.Point(6, 26);
            this.elapsedTimeLabel.Name = "elapsedTimeLabel";
            this.elapsedTimeLabel.Size = new System.Drawing.Size(74, 13);
            this.elapsedTimeLabel.TabIndex = 21;
            this.elapsedTimeLabel.Text = "Elapsed Time:";
            // 
            // outcome
            // 
            this.outcome.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outcome.Location = new System.Drawing.Point(86, 9);
            this.outcome.Name = "outcome";
            this.outcome.Size = new System.Drawing.Size(144, 13);
            this.outcome.TabIndex = 19;
            // 
            // TestResultSubView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.elapsedTime);
            this.Controls.Add(this.assertions);
            this.Controls.Add(this.messageLabel);
            this.Controls.Add(this.assertCountLabel);
            this.Controls.Add(this.outcomeLabel);
            this.Controls.Add(this.assertCount);
            this.Controls.Add(this.elapsedTimeLabel);
            this.Controls.Add(this.outcome);
            this.Name = "TestResultSubView";
            this.Size = new System.Drawing.Size(473, 108);
            this.MinimumSize = new System.Drawing.Size(0, 108);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label outcomeLabel;
        private System.Windows.Forms.Label outcome;
        private System.Windows.Forms.Label elapsedTimeLabel;
        private System.Windows.Forms.Label elapsedTime;
        private System.Windows.Forms.Label assertCountLabel;
        private System.Windows.Forms.Label assertCount;
        private System.Windows.Forms.Label messageLabel;
        private ExpandingLabel assertions;
    }
}
