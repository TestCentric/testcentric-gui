// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Presenters
{
    using Model;
    using Views;

    public class TestsNotRunPresenter
    {
        const string CHILD_IGNORED_MESSAGE = "One or more child tests were ignored";

        private ITestsNotRunView _view;
        private ITestModel _model;

        public TestsNotRunPresenter(ITestsNotRunView view, ITestModel model)
        {
            _view = view;
            _model = model;

            WireUpEvents();
        }

        private void WireUpEvents()
        {
            _model.Events.TestLoaded += (TestNodeEventArgs e) =>
            {
                _view.Clear();
            };

            _model.Events.TestUnloaded += (TestEventArgs e) =>
            {
                _view.Clear();
            };

            _model.Events.TestReloaded += (TestNodeEventArgs e) =>
            {
                if (_model.Settings.Gui.ClearResultsOnReload)
                    _view.Clear();
            };

            _model.Events.RunStarting += (RunStartingEventArgs e) =>
            {
                _view.Clear();
            };

            _model.Events.TestFinished += (TestResultEventArgs e) =>
            {
                var result = e.Result;

                if (result.Status == TestStatus.Skipped &&
                    result.Site != FailureSite.Parent)
                {
                    AddResult(result);
                }
            };

            _model.Events.SuiteFinished += (TestResultEventArgs e) =>
            {
                var result = e.Result;

                // NOTE: Adhoc message test is needed due to an error in
                // some versions of the nunit framework, where the suite
                // FailureSite is not set correctly.
                if (result.Status == TestStatus.Skipped &&
                    result.Site != FailureSite.Parent &&
                    result.Site != FailureSite.Child &&
                    result.Message != CHILD_IGNORED_MESSAGE)
                {
                    AddResult(result);
                }
            };
        }

        private void AddResult(ResultNode result)
        {
            _view.AddResult(result.Name, result.Message);
        }
    }
}
