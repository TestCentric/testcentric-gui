// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Diagnostics;
using System.Windows.Forms;

namespace TestCentric.Gui.Views
{
    using Controls;

    public partial class ProgressBarView : UserControl, IProgressBarView
    {
        private int _maximum;

        public ProgressBarView()
        {
            InitializeComponent();
        }

        public void Initialize(int max)
        {
            Debug.Assert(max > 0, "Maximum value must be > 0");

            _maximum = max;
            _progress = 0;
            _status = ProgressBarStatus.Success;

            InvokeIfRequired(() =>
            {
                testProgressBar.Maximum = _maximum;
                testProgressBar.Value = _progress;
                testProgressBar.Status = _status;
            });
        }

        private int _progress;
        public int Progress
        {
            get { return _progress; }
            set
            {
                Debug.Assert(value <= _maximum, "Value must be <= maximum");

                _progress = value;
                InvokeIfRequired(() => { testProgressBar.Value = _progress; });
            }
        }

        private ProgressBarStatus _status;
        public ProgressBarStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                InvokeIfRequired(() => { testProgressBar.Status = _status; });
            }
        }

        private void InvokeIfRequired(MethodInvoker _delegate)
        {
            if (testProgressBar.InvokeRequired)
                testProgressBar.BeginInvoke(_delegate);
            else
                _delegate();
        }
    }
}
