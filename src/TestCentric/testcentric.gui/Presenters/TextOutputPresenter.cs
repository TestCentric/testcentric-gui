// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Drawing;

namespace TestCentric.Gui.Presenters
{
    using Model;
    using Views;

    public class TextOutputPresenter
    {
        public static Color WarningColor = Color.Yellow;
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
            _model.Events.TestLoaded += ((TestNodeEventArgs e) =>
            {
                Initialize(true);
            });

            _model.Events.TestUnloaded += ((TestEventArgs e) =>
            {
                Initialize(true);
            });

            _model.Events.TestReloaded += (TestNodeEventArgs e) =>
            {
                Initialize(_model.Services.UserSettings.Gui.ClearResultsOnReload);
            };

            _model.Events.RunStarting += (RunStartingEventArgs e) =>
            {
                Initialize(true);
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

        private void Initialize(bool clearDisplay = false)
        {
            _labels = _model.Services.UserSettings.Gui.TextOutput.Labels;
            _displayBeforeTest = _labels == "ALL" || _labels == "BEFORE" || _labels == "BEFOREANDAFTER";
            _displayAfterTest = _labels == "AFTER" || _labels == "BEFOREANDAFTER";
            _displayBeforeOutput = _displayBeforeTest || _displayAfterTest || _labels == "ON";

            _currentLabel = _lastTestOutput = null;
            _wantNewLine = false;

            if (clearDisplay)
                _view.Clear();
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
