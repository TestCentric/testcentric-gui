// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Windows.Forms;

namespace TestCentric.Gui.Presenters
{
    using Model;
    using Model.Settings;
    using Views;

    /// <summary>
    /// TreeViewAdapter provides a higher-level interface to
    /// a TreeView control used to display tests.
    /// </summary>
    public class StatusBarPresenter
    {
        private IStatusBarView _view;
        private ITestModel _model;

        // Counters are maintained by presenter and passed to the view for display.
        private int _passedCount;
        private int _failedCount;
        private int _warningCount;
        private int _inconclusiveCount;
        private int _skippedCount;
        private int _ignoredCount;

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

                _view.Initialize();
                _view.Text = ea.Test.TestCount > 0 ? "Ready" : "";
            };

            _model.Events.TestReloaded += (ea) =>
            {
                ClearCounters();

                _view.Initialize();
                _view.Text = "Reloaded";
            };

            _model.Events.TestUnloaded += (ea) =>
            {
                ClearCounters();

                _view.Initialize();
                _view.Text = "Unloaded";
            };

            _model.Events.RunStarting += (ea) =>
            {
                ClearCounters();

                _view.Initialize();
            };

            _model.Events.RunFinished += (ea) =>
            {
                _view.Text = "Completed";
                _view.Duration = ea.Result.Duration;
            };

            _model.Events.TestStarting += (ea) =>
            {
                _view.Text = ea.Test.FullName;
            };

            _model.Events.TestFinished += (ea) =>
            {
                var outcome = ea.Result.Outcome;
                switch (outcome.Status)
                {
                    case TestStatus.Passed:
                        _view.Passed = ++_passedCount;
                        break;
                    case TestStatus.Failed:
                        _view.Failed = ++_failedCount;
                        break;
                    case TestStatus.Warning:
                        _view.Warnings = ++_warningCount;
                        break;
                    case TestStatus.Inconclusive:
                        _view.Inconclusive = ++_inconclusiveCount;
                        break;
                    case TestStatus.Skipped:
                        if (outcome.Label == "Ignored")
                            _view.Ignored = ++_ignoredCount;
                        else
                            _view.Skipped = ++_skippedCount;
                        break;
                }
            };
        }

        private void ClearCounters()
        {
            _passedCount =
            _failedCount =
            _warningCount =
            _inconclusiveCount =
            _skippedCount =
            _ignoredCount = 0;
        }
    }
}
