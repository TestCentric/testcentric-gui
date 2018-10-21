// ***********************************************************************
// Copyright (c) 2016-2018 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************


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

        public void DisplayTime(double time)
        {
            InvokeIfRequired(() =>
            {
                timePanel.Text = "Time : " + time.ToString("F3");
                timePanel.Visible = true;
            });
        }
    }
}
