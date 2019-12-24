// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Presenters
{
    using Model;
    using Views;

    public class ProgressBarPresenter
    {
        private IProgressBarView _view;
        private ITestModel _model;

        public ProgressBarPresenter(IProgressBarView view, ITestModel model)
        {
            _view = view;
            _model = model;

            WireUpEvents();
        }

        private void WireUpEvents()
        {
            _model.Events.TestLoaded += (ea) =>
            {
                _view.Initialize(100);
            };

            _model.Events.TestUnloaded += (ea) =>
            {
                _view.Initialize(100);
            };

            _model.Events.TestReloaded += (ea) =>
            {
                _view.Initialize(100);
            };

            _model.Events.RunStarting += (ea) =>
            {
                _view.Initialize(ea.TestCount);
            };

            _model.Events.TestFinished += (ea) =>
            {
                _view.Progress++;

                CheckStatus(ea.Result.Outcome);
            };

            _model.Events.SuiteFinished += (ea) =>
            {
                CheckStatus(ea.Result.Outcome);
            };
        }

        private void CheckStatus(ResultState result)
        {
            // Status can only get worse during a run, so if it's already
            // failing we don't need to do anything.
            if (_view.Status != ProgressBarStatus.Failure)
            {
                switch (result.Status)
                {
                    case TestStatus.Failed:
                        // A new failure always applies
                        _view.Status = ProgressBarStatus.Failure;
                        break;

                    case TestStatus.Warning:
                        // Make it a warning if it isn't already one
                        if (_view.Status != ProgressBarStatus.Warning)
                            _view.Status = ProgressBarStatus.Warning;
                        break;

                    case TestStatus.Skipped:
                        // Treat Skipped:Invalid and Skipped:Ignored specially
                        if (result.Label == "Invalid")
                            _view.Status = ProgressBarStatus.Failure;
                        else if (result.Label == "Ignored" && _view.Status != ProgressBarStatus.Warning)
                            _view.Status = ProgressBarStatus.Warning;
                        break;
                }
            }
        }
    }
}
