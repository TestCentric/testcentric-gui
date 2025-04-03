namespace TestCentric.Gui.Views
{
    partial class StatusBarView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatusBarView));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.passedPanel = new System.Windows.Forms.ToolStripStatusLabel();
            this.failedPanel = new System.Windows.Forms.ToolStripStatusLabel();
            this.warningsPanel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ignoredPanel = new System.Windows.Forms.ToolStripStatusLabel();
            this.inconclusivePanel = new System.Windows.Forms.ToolStripStatusLabel();
            this.skippedPanel = new System.Windows.Forms.ToolStripStatusLabel();
            this.timePanel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel,
            this.passedPanel,
            this.failedPanel,
            this.warningsPanel,
            this.ignoredPanel,
            this.inconclusivePanel,
            this.skippedPanel,
            this.timePanel});
            this.statusStrip1.Location = new System.Drawing.Point(0, -1);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(579, 25);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(258, 20);
            this.StatusLabel.Spring = true;
            this.StatusLabel.Text = "Ready";
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // passedPanel
            // 
            this.passedPanel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.passedPanel.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.passedPanel.Image = ((System.Drawing.Image)(resources.GetObject("passedPanel.Image")));
            this.passedPanel.Name = "passedPanel";
            this.passedPanel.Size = new System.Drawing.Size(33, 20);
            this.passedPanel.Text = "0";
            this.passedPanel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.passedPanel.Visible = false;
            // 
            // failedPanel
            // 
            this.failedPanel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.failedPanel.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.failedPanel.Image = ((System.Drawing.Image)(resources.GetObject("failedPanel.Image")));
            this.failedPanel.Name = "failedPanel";
            this.failedPanel.Size = new System.Drawing.Size(33, 20);
            this.failedPanel.Text = "0";
            this.failedPanel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.failedPanel.Visible = false;
            // 
            // warningsPanel
            // 
            this.warningsPanel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.warningsPanel.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.warningsPanel.Image = ((System.Drawing.Image)(resources.GetObject("warningsPanel.Image")));
            this.warningsPanel.Name = "warningsPanel";
            this.warningsPanel.Size = new System.Drawing.Size(33, 20);
            this.warningsPanel.Text = "0";
            this.warningsPanel.Visible = false;
            // 
            // ignoredPanel
            // 
            this.ignoredPanel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.ignoredPanel.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.ignoredPanel.Image = ((System.Drawing.Image)(resources.GetObject("ignoredPanel.Image")));
            this.ignoredPanel.Name = "ignoredPanel";
            this.ignoredPanel.Size = new System.Drawing.Size(33, 20);
            this.ignoredPanel.Text = "0";
            this.ignoredPanel.Visible = false;
            // 
            // inconclusivePanel
            // 
            this.inconclusivePanel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.inconclusivePanel.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.inconclusivePanel.Image = ((System.Drawing.Image)(resources.GetObject("inconclusivePanel.Image")));
            this.inconclusivePanel.Name = "inconclusivePanel";
            this.inconclusivePanel.Size = new System.Drawing.Size(33, 20);
            this.inconclusivePanel.Text = "0";
            this.inconclusivePanel.Visible = false;
            // 
            // skippedPanel
            // 
            this.skippedPanel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.skippedPanel.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.skippedPanel.Image = ((System.Drawing.Image)(resources.GetObject("skippedPanel.Image")));
            this.skippedPanel.Name = "skippedPanel";
            this.skippedPanel.Size = new System.Drawing.Size(33, 20);
            this.skippedPanel.Text = "0";
            this.skippedPanel.Visible = false;
            // 
            // timePanel
            // 
            this.timePanel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.timePanel.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.timePanel.Image = ((System.Drawing.Image)(resources.GetObject("timePanel.Image")));
            this.timePanel.Name = "timePanel";
            this.timePanel.Size = new System.Drawing.Size(77, 20);
            this.timePanel.Text = "Time : 0.0";
            this.timePanel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.timePanel.Visible = false;
            // 
            // StatusBarView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusStrip1);
            this.MinimumSize = new System.Drawing.Size(0, 24);
            this.Name = "StatusBarView";
            this.Size = new System.Drawing.Size(579, 24);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        public System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        public System.Windows.Forms.ToolStripStatusLabel passedPanel;
        public System.Windows.Forms.ToolStripStatusLabel failedPanel;
        private System.Windows.Forms.ToolStripStatusLabel timePanel;
        private System.Windows.Forms.ToolStripStatusLabel warningsPanel;
        private System.Windows.Forms.ToolStripStatusLabel inconclusivePanel;
        private System.Windows.Forms.ToolStripStatusLabel skippedPanel;
        private System.Windows.Forms.ToolStripStatusLabel ignoredPanel;
    }
}
