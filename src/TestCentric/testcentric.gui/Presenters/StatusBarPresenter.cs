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

using System.Collections.Generic;
using System.Windows.Forms;

namespace TestCentric.Gui.Presenters
{
    using Model;
    using Views;

    /// <summary>
    /// TreeViewAdapter provides a higher-level interface to
    /// a TreeView control used to display tests.
    /// </summary>
    public class StatusBarPresenter
    {
        private IStatusBarView _view;
        private ITestModel _model;

        // Counters are maintained presenter for display by the view.
        private int _testsRun;
        private int _passedCount;
        private int _failedCount;
        private int _warningCount;
        private int _inconclusiveCount;

        private Dictionary<string, TreeNode> _nodeIndex = new Dictionary<string, TreeNode>();

        public StatusBarPresenter(IStatusBarView view, ITestModel model)
        {
            _view = view;
            _model = model;

            WireUpEvents();
        }

        private void WireUpEvents()
        {
            _model.Events.TestLoaded += (ea) =>
            {
                ClearCounters();

                _view.Initialize(ea.Test.TestCount);
                _view.DisplayText(ea.Test.TestCount > 0 ? "Ready" : "");
            };

            _model.Events.TestReloaded += (ea) =>
            {
                ClearCounters();

                _view.Initialize(ea.Test.TestCount);
                _view.DisplayText("Reloaded");
            };

            _model.Events.TestUnloaded += (ea) =>
            {
                ClearCounters();

                _view.Initialize(0);
                _view.DisplayText("Unloaded");
            };

            _model.Events.RunStarting += (ea) =>
            {
                ClearCounters();

                _view.Initialize(ea.TestCount);
                _view.DisplayTestsRun(0);
                _view.DisplayPassed(0);
                _view.DisplayFailed(0);
                _view.DisplayWarnings(0);
                _view.DisplayInconclusive(0);
                _view.DisplayDuration(0.0);
            };

            _model.Events.RunFinished += (ea) =>
            {
                _view.DisplayText("Completed");
                _view.DisplayDuration(ea.Result.Duration);
            };

            _model.Events.TestStarting += (ea) =>
            {
                _view.DisplayText(ea.Test.FullName);
            };

            _model.Events.TestFinished += (ea) =>
            {
                _view.DisplayTestsRun(++_testsRun);

                switch (ea.Result.Outcome.Status)
                {
                    case TestStatus.Passed:
                        _view.DisplayPassed(++_passedCount);
                        break;
                    case TestStatus.Failed:
                        _view.DisplayFailed(++_failedCount);
                        break;
                    case TestStatus.Warning:
                        _view.DisplayWarnings(++_warningCount);
                        break;
                    case TestStatus.Inconclusive:
                        _view.DisplayInconclusive(++_inconclusiveCount);
                        break;
                }
            };
        }

        private void ClearCounters()
        {
            _testsRun = _passedCount = _failedCount = _warningCount = _inconclusiveCount = 0;
        }
    }
}
