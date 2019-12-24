// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Presenters
{
    using Controls;
    using Model;
    using Views;

    public class ProgressBarPresenter
    {
        private IProgressBarView _progressBar;
        private ITestModel _model;

        public ProgressBarPresenter(IProgressBarView progressBar, ITestModel model)
        {
            _progressBar = progressBar;
            _model = model;

            WireUpEvents();
        }

        private void WireUpEvents()
        {
            _model.Events.TestLoaded += (ea) => { Initialize(100); };
            _model.Events.TestUnloaded += (ea) => { Initialize(100); };
            _model.Events.TestReloaded += (ea) => { Initialize(100); };
            _model.Events.RunStarting += (ea) => { Initialize(ea.TestCount); };
            _model.Events.TestFinished += (ea) => { ReportTestOutcome(ea.Result); };
            _model.Events.SuiteFinished += (ea) => { ReportTestOutcome(ea.Result); };
        }

        public void Initialize(int max)
        {
            _progressBar.Initialize(max);
        }

        public void ReportTestOutcome(ResultNode result)
        {
            UpdateProgress(result);
            UpdateStatus(result.Outcome);
        }

        private void UpdateStatus(ResultState result)
        {
            // Status can only get worse during a run, so we can avoid
            // unnecessary calls to Invoke by checking current status.
            var currentStatus = _progressBar.Status;
            if (currentStatus != ProgressBarStatus.Failure)
                switch (result.Status)
                {
                    case TestStatus.Failed:
                        _progressBar.Status = ProgressBarStatus.Failure;
                        break;
                    case TestStatus.Warning:
                        _progressBar.Status = ProgressBarStatus.Warning;
                        break;
                    case TestStatus.Skipped:
                        if (result.Label == "Invalid")
                            _progressBar.Status = ProgressBarStatus.Failure;
                        else if (result.Label == "Ignored" && currentStatus != ProgressBarStatus.Warning)
                            _progressBar.Status = ProgressBarStatus.Warning;
                        break;
                }
        }

        private void UpdateProgress(TestNode result)
        {
            if (!result.IsSuite)
                _progressBar.Progress++;
        }
    }
}
