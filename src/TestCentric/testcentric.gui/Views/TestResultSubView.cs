// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using TestCentric.Gui.Model;
using TestCentric.Gui.Presenters;

namespace TestCentric.Gui.Views
{
    public partial class TestResultSubView : TestPropertiesView.SubView, ITestResultSubView
    {
        public TestResultSubView()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            InvokeIfRequired(() =>
            {
                testCount.Text = "";
                outcome.Text = "";
                duration.Text = "";
                assertCount.Text = "";

                detailSectionFlowLayoutPanel.Visible = false;
                Height = detailSectionBackgroundPanel.Top - 1;
            });
        }


        public void LoadImages(OutcomeImageSet imageSet)
        {
            passedPictureBox.Image = imageSet.LoadImage("Success");
            failedPictureBox.Image = imageSet.LoadImage("Failure");
            warningsPictureBox.Image = imageSet.LoadImage("Ignored");
            inconclusivePictureBox.Image = imageSet.LoadImage("Inconclusive");
            ignoredPictureBox.Image = imageSet.LoadImage("Ignored");
            skippedPictureBox.Image = imageSet.LoadImage("Skipped");
            notRunPictureBox.Image = imageSet.LoadImage("Skipped");
        }

        public void UpdateCaption(TestResultCounts testCounts, ResultNode result)
        {
            InvokeIfRequired(() =>
            {
                testCount.Text = testCounts.TestCount.ToString();
                outcome.Text = result?.Outcome.ToString();
                duration.Text = result?.Duration.ToString("f3");
                assertCount.Text = result?.AssertCount.ToString();
            });
        }

        public void UpdateDetailSectionVisibility(bool visible)
        {
            InvokeIfRequired(() => detailSectionFlowLayoutPanel.Visible = visible);
        }

        public void ShrinkToCaption()
        {
            // Adapt height of view to show only the upper part
            InvokeIfRequired(() => Height = detailSectionBackgroundPanel.Top - 1);
        }

        public void UpdateDetailSection(TestResultCounts summary)
        {
            InvokeIfRequired(() =>
            {
                // 1. Update all output labels with test count
                passedLabel.Text = $"{summary.PassedCount} Passed";
                failedLabel.Text = $"{summary.FailedCount} Failed";
                warningLabel.Text = $"{summary.WarningCount} Warnings";
                inconclusiveLabel.Text = $"{summary.InconclusiveCount} Inconclusive";
                ignoredLabel.Text = $"{summary.IgnoreCount} Ignored";
                skippedLabel.Text = $"{summary.ExplicitCount} Explicit";
                notRunLabel.Text = $"{summary.NotRunCount} Not Run";

                // 2. Hide all labels + icon with zero test count
                // FlowLayoutPanel will automatically rearrange rows
                // AutoSize = true for FlowLayoutPanel so Height is automatically adapted to content
                detailSectionFlowLayoutPanel.SuspendLayout();
                passedLabel.Visible = passedPictureBox.Visible = summary.PassedCount > 0;
                failedLabel.Visible = failedPictureBox.Visible = summary.FailedCount > 0;
                warningLabel.Visible = warningsPictureBox.Visible = summary.WarningCount > 0;
                inconclusiveLabel.Visible = inconclusivePictureBox.Visible = summary.InconclusiveCount > 0;
                ignoredLabel.Visible = ignoredPictureBox.Visible = summary.IgnoreCount > 0;
                skippedLabel.Visible = skippedPictureBox.Visible = summary.ExplicitCount > 0;
                notRunLabel.Visible = notRunPictureBox.Visible = summary.NotRunCount > 0;
                detailSectionFlowLayoutPanel.ResumeLayout();

                // 3. Expand height of view to show detail section
                Height = detailSectionFlowLayoutPanel.Bottom + 10;
            });
        }

        private Image LoadAlternateImage(string name, string imageDir)
        {
            string[] extensions = { ".png", ".jpg" };

            foreach (string ext in extensions)
            {
                string filePath = Path.Combine(imageDir, name + ext);
                if (File.Exists(filePath))
                {
                    return Image.FromFile(filePath);
                }
            }

            return null;
        }
    }
}
