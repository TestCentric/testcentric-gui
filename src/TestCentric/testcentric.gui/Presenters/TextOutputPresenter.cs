// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Drawing;

namespace TestCentric.Gui.Presenters
{
    using System.Collections.Generic;
    using System.Drawing.Drawing2D;
    using Model;
    using Views;

    public class TextOutputPresenter
    {
        static readonly Color OutputColor = Color.Black;

        ITextOutputView _view;
        ITestModel _model;

        private bool _displayBeforeOutput = true;
        private bool _displayBeforeTest = false;
        private bool _displayAfterTest = false;

        private string _currentLabel = null;
        private string _lastTestOutput = null;

        private List<TestEventArgs> _events = new List<TestEventArgs>();

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
                Initialize(_model.Settings.Gui.ClearResultsOnReload);
            };

            _model.Events.RunStarting += (RunStartingEventArgs e) =>
            {
                Initialize(true);
            };

            _model.Events.TestStarting += (TestNodeEventArgs e) =>
            {
                _events.Add(e);

                OnTestStarting(e);
            };

            _model.Events.TestFinished += (TestResultEventArgs e) =>
            {
                _events.Add(e);

                OnTestFinished(e);
            };

            _model.Events.SuiteFinished += (TestResultEventArgs e) =>
            {
                _events.Add(e);

                OnSuiteFinished(e);
            };

            _model.Events.TestOutput += (TestOutputEventArgs e) =>
            {
                _events.Add(e);

                OnTestOutput(e);
            };

            //_model.Events.SelectedItemChanged += (e) =>
            //{
            //    var testNode = _model.ActiveTestItem as TestNode;
            //    _view.
            //}

            _view.LabelsOff.Execute += () => ChangeLabelSetting("OFF");
            _view.LabelsOn.Execute += () => ChangeLabelSetting("ON");
            _view.LabelsBefore.Execute += () => ChangeLabelSetting("BEFORE");
            _view.LabelsAfter.Execute += () => ChangeLabelSetting("AFTER");
            _view.LabelsBeforeAndAfter.Execute += () => ChangeLabelSetting("BEFOREANDAFTER");
        }

        private void OnTestStarting(TestNodeEventArgs e)
        {
            if (_displayBeforeTest)
                WriteLabelLine(e.Test.FullName, OutputColor);
        }

        private void OnTestFinished(TestResultEventArgs e)
        {
            if (e.Result.Output != null)
            {
                if (_displayBeforeOutput)
                    WriteLabelLine(e.Result.FullName, OutputColor);

                WriteOutputLine(e.Result.FullName, e.Result.Output, OutputColor);
            }

            if (_displayAfterTest)
                WriteLabelLineAfterTest(e.Result.FullName, e.Result.Outcome);
        }

        private void OnSuiteFinished(TestResultEventArgs e)
        {
            if (e.Result.Output != null)
            {
                if (_displayBeforeOutput)
                    WriteLabelLine(e.Result.FullName, OutputColor);

                FlushNewLineIfNeeded();
                WriteOutputLine(e.Result.FullName, e.Result.Output, OutputColor);
            }
        }

        private void OnTestOutput(TestOutputEventArgs e)
        {
            if (_displayBeforeOutput && e.TestName != null)
                WriteLabelLine(e.TestName, OutputColor);

            WriteOutputLine(e.TestName, e.Text,
                e.Stream == "Error" ? Color.Red : Color.Black);
        }

        private void ChangeLabelSetting(string labels)
        {
            _model.Settings.Gui.TextOutput.Labels = labels;
            Initialize(true);

            foreach (var ea in _events)
            {
                var testNodeEventArgs = ea as TestNodeEventArgs;
                var testResultEventArgs = ea as TestResultEventArgs;
                var testOutputEventArgs = ea as TestOutputEventArgs;

                if (testNodeEventArgs != null)
                    OnTestStarting(testNodeEventArgs);
                else if (testResultEventArgs != null)
                    if (testResultEventArgs.Result.IsSuite)
                        OnSuiteFinished(testResultEventArgs);
                    else OnTestFinished(testResultEventArgs);
                else if (testOutputEventArgs != null)
                    OnTestOutput(testOutputEventArgs);
            }
        }

        private void Initialize(bool clearDisplay = false)
        {
            var labels = _model.Settings.Gui.TextOutput.Labels;

            _displayBeforeTest = labels == "ALL" || labels == "BEFORE" || labels == "BEFOREANDAFTER";
            _displayAfterTest = labels == "AFTER" || labels == "BEFOREANDAFTER";
            _displayBeforeOutput = _displayBeforeTest || _displayAfterTest || labels == "ON";

            _currentLabel = _lastTestOutput = null;
            _wantNewLine = false;

            if (clearDisplay)
                _view.Clear();
        }

        private void WriteLabelLine(string label, Color color)
        {
            if (label != _currentLabel)
            {
                FlushNewLineIfNeeded();
                _lastTestOutput = label;

                WriteLine($"=> {label}", color);

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

        private void WriteLabelLineAfterTest(string label, ResultState outcome)
        {
            FlushNewLineIfNeeded();
            _lastTestOutput = label;

            var status = outcome.Label;
            if (string.IsNullOrEmpty(status))
                status = outcome.Status.ToString();

            var labelColor =
                outcome.Status == TestStatus.Passed ? Color.Green :
                outcome.Status == TestStatus.Failed ? Color.Red :
                outcome.Status == TestStatus.Warning ? Color.DarkOrange : Color.Purple;

            WriteLine($"{status} => {label}", labelColor);

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
