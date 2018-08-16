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
