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
using System.Drawing;
using NUnit.Framework;
using NSubstitute;

namespace TestCentric.Gui.Presenters
{
    using Views;

    public class TextOutputPresenterTests : PresenterTestBase<ITextOutputView>
    {
        static readonly string NL = Environment.NewLine;

        [TestCaseSource(nameof(EveryLabel))]
        public void TestStarting_DisplaysCorrectly(string labels)
        {
            CreatePresenter(labels);

            FireTestStartingEvent("SomeName");

            if (labels == "ALL" || labels == "BEFORE")
                VerifyLabelWasWritten("SomeName");
            else
                VerifyNothingWasWritten();
        }

        [TestCaseSource(nameof(EveryLabel))]
        public void SuiteStarting_DisplaysNothing(string labels)
        {
            CreatePresenter(labels);

            FireSuiteStartingEvent("SomeName");

            VerifyNothingWasWritten();
        }

        [TestCase("OFF", "Passed")]
        [TestCase("ON", "Passed")]
        [TestCase("ALL", "Passed")]
        [TestCase("BEFORE", "Passed")]
        [TestCase("AFTER", "Passed")]
        [TestCase("AFTER", "Failed")]
        [TestCase("AFTER", "Failed:Error")]
        [TestCase("AFTER", "Failed:Cancelled")]
        [TestCase("AFTER", "Failed:Invalid")]
        [TestCase("AFTER", "Warning")]
        [TestCase("AFTER", "Skipped")]
        [TestCase("AFTER", "Skipped:Ignored")]
        [TestCase("AFTER", "Inconclusive")]
        public void TestFinished_DisplaysCorrectly(string labels, string result)
        {
            CreatePresenter(labels);

            FireTestFinishedEvent("SomeName", result);

            if (labels == "AFTER")
                VerifyLabelWasWritten("SomeName", result);
            else
                VerifyNothingWasWritten();
        }

        [TestCase("OFF", "Passed")]
        [TestCase("ON", "Passed")]
        [TestCase("ALL", "Passed")]
        [TestCase("BEFORE", "Passed")]
        [TestCase("AFTER", "Passed")]
        [TestCase("AFTER", "Failed")]
        [TestCase("AFTER", "Failed:Error")]
        [TestCase("AFTER", "Failed:Cancelled")]
        [TestCase("AFTER", "Failed:Invalid")]
        [TestCase("AFTER", "Warning")]
        [TestCase("AFTER", "Skipped")]
        [TestCase("AFTER", "Skipped:Ignored")]
        [TestCase("AFTER", "Inconclusive")]
        public void TestFinishedWithOutput_DisplaysCorrectly(string labels, string result)
        {
            CreatePresenter(labels);

            FireTestFinishedEvent("SomeName", result, "OUTPUT");

            Received.InOrder(() =>
            {
                Label("SomeName", labels != "OFF");
                Output("OUTPUT");
                Label("SomeName", result, labels == "AFTER");
            });
        }

        [TestCase("OFF", "Passed")]
        [TestCase("ON", "Passed")]
        [TestCase("ALL", "Passed")]
        [TestCase("BEFORE", "Passed")]
        [TestCase("AFTER", "Passed")]
        [TestCase("AFTER", "Failed")]
        [TestCase("AFTER", "Failed:Error")]
        [TestCase("AFTER", "Failed:Cancelled")]
        [TestCase("AFTER", "Failed:Invalid")]
        [TestCase("AFTER", "Warning")]
        [TestCase("AFTER", "Skipped")]
        [TestCase("AFTER", "Skipped:Ignored")]
        [TestCase("AFTER", "Inconclusive")]
        public void SuiteFinishedWithOutput_DisplaysCorrectly(string labels, string result)
        {
            CreatePresenter(labels);

            FireSuiteFinishedEvent("SomeName", result, "OUTPUT");

            Received.InOrder(() =>
            {
                Label("SomeName", labels != "OFF");
                Output("OUTPUT");
            });
        }

        [TestCaseSource(nameof(EveryLabel))]
        public void SuiteFinished_DisplaysNothing(string labels)
        {
            CreatePresenter(labels);
            FireSuiteFinishedEvent("SomeName", "Passed");

            VerifyNothingWasWritten();
        }

        [TestCaseSource(nameof(EveryLabel))]
        public void TestOutput_DisplaysCorrectly(string labels)
        {
            CreatePresenter(labels);

            FireTestOutputEvent("SomeName", "Progress", "OUTPUT");

            Received.InOrder(() =>
            {
                Label("SomeName", labels != "OFF");
                Output("OUTPUT");
            });
        }

        [TestCaseSource(nameof(EveryLabel))]
        public void SingleTestStartAndFinish(string labels)
        {
            CreatePresenter(labels);

            FireTestStartingEvent("TestName");
            FireTestFinishedEvent("TestName", "Passed", "OUTPUT");

            Received.InOrder(() =>
            {
                Label("TestName", labels != "OFF");
                Output("OUTPUT");
                Label("TestName", "Passed", labels == "AFTER");
            });
        }

        [TestCaseSource(nameof(EveryLabel))]
        public void SingleTestImmediateOutput(string labels)
        {
            CreatePresenter(labels);

            FireTestStartingEvent("TEST1");
            FireTestOutputEvent("TEST1", "Progress", "IMMEDIATE");
            FireTestFinishedEvent("TEST1", "Passed", "OUTPUT");

            Received.InOrder(() =>
            {
                Label("TEST1", labels != "OFF");
                Output("IMMEDIATE");
                Output("OUTPUT");
                Label("TEST1", "Passed", labels == "AFTER");
            });
        }

        [TestCaseSource(nameof(EveryLabel))]
        public void TwoTests_Sequential(string labels)
        {
            CreatePresenter(labels);

            FireTestStartingEvent("TEST1");
            FireTestFinishedEvent("TEST1", "Failed", "OUTPUT1");
            FireTestStartingEvent("TEST2");
            FireTestFinishedEvent("TEST2", "Passed", "OUTPUT2");

            Received.InOrder(() =>
            {
                Label("TEST1", labels != "OFF");
                Output("OUTPUT1");
                Label("TEST1", "Failed", labels == "AFTER");
                Label("TEST2", labels != "OFF");
                Output("OUTPUT2");
                Label("TEST2", "Passed", labels == "AFTER");
            });
        }

        [TestCaseSource(nameof(EveryLabel))]
        public void TwoTests_Interleaved(string labels)
        {
            CreatePresenter(labels);

            FireTestStartingEvent("TEST1");
            FireTestOutputEvent("TEST1", "Progress", "IMMEDIATE1A");
            FireTestStartingEvent("TEST2");
            FireTestOutputEvent("TEST1", "Progress", "IMMEDIATE1B");
            FireTestOutputEvent("TEST2", "Progress", "IMMEDIATE2");
            FireTestFinishedEvent("TEST1", "Failed", "OUTPUT1");
            FireTestFinishedEvent("TEST2", "Passed", "OUTPUT2");

            Received.InOrder(() =>
            {
                Label("TEST1", labels != "OFF");
                Output("IMMEDIATE1A");
                Label("TEST2", labels == "BEFORE" || labels == "ALL");
                Label("TEST1", labels == "BEFORE" || labels == "ALL");
                Output("IMMEDIATE1B");
                Label("TEST2", labels != "OFF");
                Output("IMMEDIATE2");
                Label("TEST1", labels != "OFF");
                Output("OUTPUT1");
                Label("TEST1", "Failed", labels == "AFTER");
                Label("TEST2", labels != "OFF");
                Output("OUTPUT2");
                Label("TEST2", "Passed", labels == "AFTER");
            });
        }

        [TestCaseSource(nameof(EveryLabel))]
        public void TwoTests_Nested(string labels)
        {
            CreatePresenter(labels);

            FireTestStartingEvent("TEST1");
            FireTestOutputEvent("TEST1", "Progress", "IMMEDIATE1");
            FireTestStartingEvent("TEST2");
            FireTestOutputEvent("TEST2", "Progress", "IMMEDIATE2");
            FireTestFinishedEvent("TEST2", "Passed", "OUTPUT2");
            FireTestFinishedEvent("TEST1", "Failed", "OUTPUT1");

            Received.InOrder(() =>
            {
                Label("TEST1", labels != "OFF");
                Output("IMMEDIATE1");
                Label("TEST2", labels != "OFF");
                Output("IMMEDIATE2");
                Output("OUTPUT2");
                Label("TEST2", "Passed", labels == "AFTER");
                Label("TEST1", labels != "OFF");
                Output("OUTPUT1");
                Label("TEST1", "Failed", labels == "AFTER");
            });
        }

        private static string[] EveryLabel = { "OFF", "ON", "ALL", "BEFORE", "AFTER" };

        #region Helper Methods

        private void CreatePresenter(string labels)
        {
            _settings.Gui.TextOutput.Labels = labels;
            new TextOutputPresenter(_view, _model);
        }

        /// <summary>
        /// Verify a label was written as part of a Received.InOrder block.
        /// </summary>
        private void Label(string testName, bool condition = true)
        {
            if (condition)
                _view.Write($"=> {testName}{NL}", Color.Green);
        }

        /// <summary>
        /// Verify a label was written as part of a Received.InOrder block.
        /// </summary>
        private void Label(string testName, string result, bool condition = true)
        {
            if (condition)
                _view.Write($"{Displayed(result)} => {testName}{NL}", Color.Green);
        }

        /// <summary>
        /// Verify output was written as part of a Received.InOrder block.
        /// </summary>
        private void Output(string output, bool condition = true)
        {
            if (condition)
                _view.Write(output, Color.Black);
        }

        /// <summary>
        /// Verify label was written outside of a Received.InOrder block.
        /// </summary>
        private void VerifyLabelWasWritten(string testName)
        {
            _view.Received().Write($"=> {testName}{NL}", Color.Green);
        }

        /// <summary>
        /// Verify label was written outside of a Received.InOrder block.
        /// </summary>
        private void VerifyLabelWasWritten(string testName, string result)
        {
            _view.Received().Write($"{Displayed(result)} => {testName}{NL}", Color.Green);
        }

        private static string Displayed(string result)
        {
            int colon = result.IndexOf(':');
            return colon >= 0
                ? result.Substring(colon + 1)
                : result;
        }

        /// <summary>
        /// Verify output was written outside of of a Received.InOrder block.
        /// </summary>
        private void VerifyOutputWasWritten(string output, bool condition = true)
        {
            if (condition)
                _view.Received().Write(output, Color.Black);
        }

        /// <summary>
        /// Verify that nothing was written to the view
        /// </summary>
        private void VerifyNothingWasWritten()
        {
            _view.DidNotReceiveWithAnyArgs().Write(Arg.Any<string>(), Arg.Any<Color>());
        }

        #endregion
    }
}
