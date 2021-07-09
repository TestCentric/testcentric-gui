// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Windows.Forms;

namespace TestCentric.Gui.Views
{
    public partial class StatusBarView : UserControlView, IStatusBarView
    {
        private ToolTip _resultSummaryToolTip;

        public StatusBarView()
        {
            InitializeComponent();
            _resultSummaryToolTip = new ToolTip()
            {
                IsBalloon = true,
                UseAnimation = true,
                UseFading = true,
            };
        }

        public void Initialize(int testCount)
        {
            InvokeIfRequired(() =>
            {
                testCountPanel.Text = "Tests : " + testCount.ToString();
                testCountPanel.Visible = true;
                testsRunPanel.Visible = false;
                passedPanel.Visible = false;
                failedPanel.Visible = false;
                warningsPanel.Visible = false;
                inconclusivePanel.Visible = false;
                timePanel.Visible = false;
            });
        }

        public void DisplayText(string text)
        {
            InvokeIfRequired(() =>
            {
                StatusLabel.Text = text;
            });
        }

        public void DisplayTestsRun(int count)
        {
            InvokeIfRequired(() =>
            {
                testsRunPanel.Text = "Run : " + count.ToString();
                testsRunPanel.Visible = true;
            });
        }

        public void DisplayPassed(int count)
        {
            InvokeIfRequired(() =>
            {
                passedPanel.Text = "Passed : " + count.ToString();
                passedPanel.Visible = true;
            });
        }

        public void DisplayFailed(int count)
        {
            InvokeIfRequired(() =>
            {
                failedPanel.Text = "Failed : " + count.ToString();
                failedPanel.Visible = true;
            });
        }

        public void DisplayWarnings(int count)
        {
            InvokeIfRequired(() =>
            {
                warningsPanel.Text = "Warnings : " + count.ToString();
                warningsPanel.Visible = true;
            });
        }

        public void DisplayInconclusive(int count)
        {
            InvokeIfRequired(() =>
            {
                inconclusivePanel.Text = "Inconclusive : " + count.ToString();
                inconclusivePanel.Visible = true;
            });
        }

        public void DisplayDuration(double duration)
        {
            InvokeIfRequired(() =>
            {
                timePanel.Text = $"Duration : {duration.ToString("F3")}s";
                timePanel.Visible = true;
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
