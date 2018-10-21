// ***********************************************************************
// Copyright (c) 2018 Charlie Poole
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
                if (_model.Services.UserSettings.Gui.ClearResultsOnReload)
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
