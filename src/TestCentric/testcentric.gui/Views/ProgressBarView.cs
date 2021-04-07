// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.ComponentModel;
using System.Diagnostics;

namespace TestCentric.Gui.Views
{
    using Controls;

    public partial class ProgressBarView : UserControlView, IProgressBarView
    {
        private int _maximum;
        private int _progress;
        private ProgressBarStatus _status;

        public ProgressBarView()
        {
            InitializeComponent();
        }

        public void Initialize(int max)
        {
            //Debug.Assert(max > 0, "Maximum value must be > 0");

            _maximum = max;
            _progress = 0;
            _status = ProgressBarStatus.Success;

            InvokeIfRequired(() =>
            {
                testProgressBar.Maximum = _maximum;
                testProgressBar.Value = _progress;
                testProgressBar.Status = (Controls.ProgressBarStatus)_status;
            });
        }

        [Browsable(false)]
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

        [Browsable(false)]
        public ProgressBarStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;

                InvokeIfRequired(() =>
                {
                    testProgressBar.Status = (Controls.ProgressBarStatus)_status;
                });
            }
        }
    }
}
