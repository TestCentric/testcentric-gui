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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCentric.Gui.Presenters
{
    using Model;
    using Views;

    public class TextOutputPresenter
    {
        public static  Color WarningColor = Color.Yellow;
        static readonly Color ErrorColor = Color.Red;
        static readonly Color LabelColor = Color.Green;
        static readonly Color OutputColor = Color.Black;

        ITextOutputView _view;
        ITestModel _model;

        private string _labels;
        private bool _displayBeforeOutput = true;
        private bool _displayBeforeTest = false;
        private bool _displayAfterTest = false;

        private string _currentLabel = null;
        private string _lastTestOutput = null;

        bool _wantNewLine = false;

        public TextOutputPresenter(ITextOutputView view, ITestModel model)
        {
            _view = view;
            _model = model;

            Initialize();

            WireUpEvents();
        }

        private void WireUpEvents()
        {
            _model.Events.RunStarting += (RunStartingEventArgs e) =>
            {
                Initialize();
            };

            _model.Events.TestStarting += (TestNodeEventArgs e) =>
            {
                if (_displayBeforeTest)
                    WriteLabelLine(e.Test.FullName);
            };

            _model.Events.TestFinished += (TestResultEventArgs e) =>
            {
                if (e.Result.Output != null)
                {
                    if (_displayBeforeOutput)
                        WriteLabelLine(e.Result.FullName);

                    WriteOutputLine(e.Result.FullName, e.Result.Output, OutputColor);
                }

                if (_displayAfterTest)
                {
                    var status = e.Result.Outcome.Label;
                    if (string.IsNullOrEmpty(status))
                        status = e.Result.Outcome.Status.ToString();
                    WriteLabelLineAfterTest(e.Result.FullName, status);
                }
            };

            _model.Events.SuiteFinished += (TestResultEventArgs e) =>
            {
                if (e.Result.Output != null)
                {
                    if (_displayBeforeOutput)
                        WriteLabelLine(e.Result.FullName);

                    FlushNewLineIfNeeded();
                    WriteOutputLine(e.Result.FullName, e.Result.Output, OutputColor);
                }
            };

            _model.Events.TestOutput += (TestOutputEventArgs e) =>
            {
                if (_displayBeforeOutput && e.TestName != null)
                    WriteLabelLine(e.TestName);

                WriteOutputLine(e.TestName, e.Text,
                    e.Stream == "Error" ? ErrorColor : OutputColor);
            };
        }

        private void Initialize()
        {
            _labels = _model.Services.UserSettings.Gui.TextOutput.Labels;
            _displayBeforeTest = _labels == "ALL" || _labels == "BEFORE";
            _displayAfterTest = _labels == "AFTER";
            _displayBeforeOutput = _displayBeforeTest || _displayAfterTest || _labels == "ON";

            _currentLabel = _lastTestOutput = null;
            _wantNewLine = false;
        }

        private void WriteLabelLine(string label)
        {
            if (label != _currentLabel)
            {
                FlushNewLineIfNeeded();
                _lastTestOutput = label;

                WriteLine($"=> {label}", LabelColor);

                _currentLabel = label;
            }
        }

        private void WriteOutputLine(string testName, string text, Color color)
        {
            if (_lastTestOutput != testName)
            {
                FlushNewLineIfNeeded();
                _lastTestOutput = testName;
            }

            _view.Write(text, color);

            // If the text we just wrote did not have a new line, flag that we should eventually emit one.
            if (!text.EndsWith("\n"))
            {
                _wantNewLine = true;
            }
        }

        private void WriteLabelLineAfterTest(string label, string status)
        {
            FlushNewLineIfNeeded();
            _lastTestOutput = label;

            WriteLine($"{status} => {label}", LabelColor);

            _currentLabel = label;
        }

        public void WriteLine(string text)
        {
            _view.Write(text + Environment.NewLine);
        }

        public void WriteLine(string text, Color color)
        {
            _view.Write(text + Environment.NewLine, color);
        }

        private void FlushNewLineIfNeeded()
        {
            if (_wantNewLine)
            {
                _view.Write(Environment.NewLine);
                _wantNewLine = false;
            }
        }
    }
}
