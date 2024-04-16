// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;

namespace TestCentric.Gui.Controls
{
    public partial class TestResultDisplay : UserControl
    {
        public TestResultDisplay()
        {
            InitializeComponent();
        }

        public string Outcome
        {
            get { return outcome.Text; }
            set { this.InvokeIfRequired(() => { outcome.Text = value; }); }
        }

        public string ElapsedTime
        {
            get { return elapsedTime.Text; }
            set { this.InvokeIfRequired(() => { elapsedTime.Text = value; }); }
        }

        public string AssertCount
        {
            get { return assertCount.Text; }
            set { this.InvokeIfRequired(() => { assertCount.Text = value; }); }
        }

        public string Assertions
        {
            get { return assertions.Text; }
            set { this.InvokeIfRequired(() => { assertions.Text = value; }); }
        }

        public string Output
        {
            get { return output.Text; }
            set { this.InvokeIfRequired(() => { output.Text = value; }); }
        }
    }
}
