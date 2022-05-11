// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;

namespace TestCentric.Gui.Controls
{
    public class RunSummaryDisplay : UserControl, IRunSummaryDisplay
    {
        private Label title;
        private TextBox runSummary;

        public RunSummaryDisplay()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.title = new System.Windows.Forms.Label();
            this.runSummary = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.Location = new System.Drawing.Point(0, 0);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(137, 16);
            this.title.TabIndex = 0;
            this.title.Text = "Test Run Summary";
            // 
            // runSummary
            // 
            this.runSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.runSummary.BackColor = System.Drawing.Color.LightYellow;
            this.runSummary.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.runSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.runSummary.Location = new System.Drawing.Point(3, 19);
            this.runSummary.Multiline = true;
            this.runSummary.Name = "runSummary";
            this.runSummary.Size = new System.Drawing.Size(469, 120);
            this.runSummary.TabIndex = 2;
            // 
            // RunSummaryDisplay
            // 
            this.BackColor = System.Drawing.Color.LightYellow;
            this.Controls.Add(this.runSummary);
            this.Controls.Add(this.title);
            this.Name = "RunSummaryDisplay";
            this.Size = new System.Drawing.Size(475, 139);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        /// <summary>
        /// Display the Run Summary
        /// </summary>
        /// <param name="report">A string containing the run summary report</param>
        /// <param name="withTimeout">If true, close the display automatically after 10 seconds</param>
        public void Display(string report)
        {
            LayoutReport(report);
            Visible = true;
        }

        private void LayoutReport(string report)
        {
            runSummary.Text = report;
            runSummary.Select(0, 0);

            Graphics g = Graphics.FromHwnd(runSummary.Handle);
            SizeF sizeNeeded = g.MeasureString(
                report, runSummary.Font, ClientSize.Width - runSummary.Left - 8);

            runSummary.ClientSize = new Size(
                (int)Math.Ceiling(sizeNeeded.Width),
                (int)Math.Ceiling(sizeNeeded.Height));

            ClientSize = new Size(ClientSize.Width, runSummary.Bottom + 8);
        }
    }
}

