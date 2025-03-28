// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Windows.Forms;

namespace TestCentric.Gui.Views
{
    public partial class StatusBarView : UserControlView, IStatusBarView
    {
        public StatusBarView()
        {
            InitializeComponent();
        }

        public void Initialize(int testCount)
        {
            InvokeIfRequired(() =>
            {
                foreach (ToolStripStatusLabel panel in statusStrip1.Controls)
                    panel.Visible = false;
            });
        }

        public override string Text
        {
            set { StatusLabel.Text = value; }
        }

        public int Passed
        {
            set { UpdateCounter(passedPanel, value); }
        }

        public int Failed
        {
            set { UpdateCounter(failedPanel, value); }
        }

        public int Warnings
        {
            set { UpdateCounter(warningsPanel, value); }
        }

        public int Inconclusive
        {
            set { UpdateCounter(inconclusivePanel, value); }
        }

        public int Ignored
        {
            set { UpdateCounter(ignoredPanel, value); }
        }

        public int Skipped
        {
            set { UpdateCounter(skippedPanel, value); }
        }

        public double Duration
        {
            set 
            {
                InvokeIfRequired(() =>
                {
                    timePanel.Text = $"{value.ToString("F3")} sec";
                    timePanel.Visible = true;
                });
            }
        }

        private void UpdateCounter(ToolStripStatusLabel panel, int count)
        {
            InvokeIfRequired(() =>
            {
                panel.Text = count.ToString();
                panel.Visible = true;
            });
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            Height = Math.Max((int)Font.GetHeight() + 10, MinimumSize.Height);
            statusStrip1.Font = Font;
        }
    }
}
