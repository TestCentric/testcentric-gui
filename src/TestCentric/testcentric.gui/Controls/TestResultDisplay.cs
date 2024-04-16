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
            set { InvokeIfRequired(() => { outcome.Text = value; }); }
        }

        public string ElapsedTime
        {
            get { return elapsedTime.Text; }
            set { InvokeIfRequired(() => { elapsedTime.Text = value; }); }
        }

        public string AssertCount
        {
            get { return assertCount.Text; }
            set { InvokeIfRequired(() => { assertCount.Text = value; }); }
        }

        public string Assertions
        {
            get { return assertions.Text; }
            set { InvokeIfRequired(() => { assertions.Text = value; }); }
        }

        public string Output
        {
            get { return output.Text; }
            set { InvokeIfRequired(() => { output.Text = value; }); }
        }

        #region Helper Methods

        private void InvokeIfRequired(MethodInvoker _delegate)
        {
            if (InvokeRequired)
                BeginInvoke(_delegate);
            else
                _delegate();
        }

        #endregion
    }
}
