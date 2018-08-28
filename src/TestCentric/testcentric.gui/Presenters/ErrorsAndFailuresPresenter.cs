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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCentric.Gui.Presenters
{
    using Model;
    using Views;

    public class ErrorsAndFailuresPresenter
    {
        private IErrorsAndFailuresView _view;
        private ITestModel _model;

        public ErrorsAndFailuresPresenter(IErrorsAndFailuresView view, ITestModel model)
        {
            _view = view;
            _model = model;

            WireUpEvents();
        }

        private void WireUpEvents()
        {
            _model.Events.TestLoaded += ((TestNodeEventArgs e) =>
            {
                _view.Clear();
            });

            _model.Events.TestUnloaded += ((TestEventArgs e) =>
            {
                _view.Clear();
            });

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
                if (e.Result.Status == TestStatus.Failed || e.Result.Status == TestStatus.Warning)
                    if (e.Result.Site != FailureSite.Parent)
                        AddResult(e.Result);
            };

            _model.Events.SuiteFinished += (TestResultEventArgs e) =>
            {
                if (e.Result.Status == TestStatus.Failed || e.Result.Status == TestStatus.Warning)
                    if (e.Result.Site != FailureSite.Parent && e.Result.Site != FailureSite.Child)
                        AddResult(e.Result);
            };
        }

        private void AddResult(ResultNode result)
        {
            var testName = result.FullName;
            var message = result.Message;
            var stackTrace = result.StackTrace;

            if (result.IsSuite && result.Site == FailureSite.SetUp)
                testName += " (TestFixtureSetUp)";

            _view.AddResult(testName, message, stackTrace);
        }
    }
}
