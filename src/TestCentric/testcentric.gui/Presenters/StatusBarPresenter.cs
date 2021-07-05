// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
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
                var summary = ResultSummaryCreator.FromResultNode(ea.Result);
                _view.OnTestRunSummaryCompiled(ResultSummaryReporter.WriteSummaryReport(summary));
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
