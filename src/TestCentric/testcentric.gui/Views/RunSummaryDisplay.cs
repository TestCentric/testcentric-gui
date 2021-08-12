// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;

namespace TestCentric.Gui.Views
{
    public class RunSummaryDisplay : Form, IRunSummaryDisplay
    {
        private const int DEFAULT_TIMEOUT = 10000;

        private Label title;
        private Label closeButton;
        private TextBox runSummary;

        private IMainView _mainView;

        public RunSummaryDisplay(IMainView mainView)
        {
            InitializeComponent();

            _mainView = mainView;
        }

        private void InitializeComponent()
        {
            this.title = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Label();
            this.runSummary = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.Location = new System.Drawing.Point(6, 5);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(129, 17);
            this.title.TabIndex = 0;
            this.title.Text = "Test Run Summary";
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.AutoSize = true;
            this.closeButton.Font = new System.Drawing.Font("Wingdings", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.closeButton.Location = new System.Drawing.Point(451, 5);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(23, 17);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "á";
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // runSummary
            // 
            this.runSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.runSummary.BackColor = System.Drawing.Color.LightYellow;
            this.runSummary.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.runSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.runSummary.Location = new System.Drawing.Point(15, 36);
            this.runSummary.Multiline = true;
            this.runSummary.Name = "runSummary";
            this.runSummary.Size = new System.Drawing.Size(448, 106);
            this.runSummary.TabIndex = 2;
            // 
            // RunSummaryDisplay
            // 
            this.BackColor = System.Drawing.Color.LightYellow;
            this.ClientSize = new System.Drawing.Size(475, 154);
            this.ControlBox = false;
            this.Controls.Add(this.runSummary);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "RunSummaryDisplay";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        /// <summary>
        /// Display the Run Summary
        /// </summary>
        /// <param name="report">A string containing the run summary report</param>
        /// <param name="withTimeout">If true, close the display automatically after 10 seconds</param>
        public void Display(string report, bool withTimeout = false)
        {
            LayoutReport(report);

            if (withTimeout)
            {
                var timer = new Timer();
                timer.Tick += (s, e) => { timer.Stop(); Close(); };
                timer.Interval = DEFAULT_TIMEOUT;
                timer.Start();
            }

            ShowDialog();
        }

        private void LayoutReport(string report)
        {
            // Position display directly under the progress bar
            var progressView = _mainView.ProgressBarView;
            var lowerLeft = new Point(progressView.Left, progressView.Bottom);

            if (!IsHandleCreated)
                CreateHandle();

            Invoke(new MethodInvoker(() =>
            {
                Location = progressView.Parent.PointToScreen(lowerLeft);

                // Set the width so we can use ClientSize below
                Width = progressView.Width;
            }));

            runSummary.Text = report;
            runSummary.Select(0, 0);

            Graphics g = Graphics.FromHwnd(runSummary.Handle);
            SizeF sizeNeeded = g.MeasureString(
                report, runSummary.Font, ClientSize.Width - runSummary.Left - 8);

            Invoke(new MethodInvoker(() =>
            {
                runSummary.ClientSize = new Size(
                    (int)Math.Ceiling(sizeNeeded.Width),
                    (int)Math.Ceiling(sizeNeeded.Height));

                ClientSize = new Size(ClientSize.Width, runSummary.Bottom + 8);
            }));
        }

        private void closeButton_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}

