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
                testProgressBar.Display = (ProgressBarDisplay)_status;
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
                    testProgressBar.Display = (ProgressBarDisplay)_status;
                });
            }
        }
    }
}
