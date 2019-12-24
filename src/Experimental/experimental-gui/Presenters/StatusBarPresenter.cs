// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
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

        private Dictionary<string, TreeNode> _nodeIndex = new Dictionary<string, TreeNode>();

        public StatusBarPresenter(IStatusBarView view, ITestModel model)
        {
            _view = view;
            _model = model;

            WireUpEvents();
        }

        private void WireUpEvents()
        {
            _model.Events.TestLoaded += OnTestLoaded;
            _model.Events.TestReloaded += OnTestReloaded;
            _model.Events.TestUnloaded += OnTestUnloaded;
            _model.Events.RunStarting += OnRunStarting;
            _model.Events.RunFinished += OnRunFinished;
            _model.Events.TestStarting += OnTestStarting;
            _model.Events.TestFinished += OnTestFinished;
        }

        private void OnTestLoaded(TestNodeEventArgs ea)
        {
            _view.OnTestLoaded(ea.Test.TestCount);
        }

        private void OnTestReloaded(TestNodeEventArgs ea)
        {
            _view.OnTestLoaded(ea.Test.TestCount);
        }

        private void OnTestUnloaded(TestEventArgs ea)
        {
            _view.OnTestUnloaded();
        }

        private void OnRunStarting(RunStartingEventArgs ea)
        {
            _view.OnRunStarting(ea.TestCount);
        }

        private void OnRunFinished(TestResultEventArgs ea)
        {
            _view.OnRunFinished(ea.Result.Duration);
            var summary = ResultSummaryCreator.FromResultNode(ea.Result);
            _view.OnTestRunSummaryCompiled(ResultSummaryReporter.WriteSummaryReport(summary));
        }

        public void OnTestStarting(TestNodeEventArgs e)
        {
            _view.OnTestStarting(e.Test.Name);
        }

        private void OnTestFinished(TestResultEventArgs ea)
        {
            var result = ea.Result.Outcome;

            switch (result.Status)
            {
                case TestStatus.Passed:
                    _view.OnTestPassed();
                    break;
                case TestStatus.Failed:
                    _view.OnTestFailed();
                    break;
                case TestStatus.Warning:
                    _view.OnTestWarning();
                    break;
                case TestStatus.Inconclusive:
                    _view.OnTestInconclusive();
                    break;
            }
        }
    }
}
