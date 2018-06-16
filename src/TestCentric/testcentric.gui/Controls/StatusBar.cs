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
using System.Windows.Forms;

namespace TestCentric.Gui.Controls
{
    using Model;

    public class StatusBar : System.Windows.Forms.StatusBar, IViewControl
    {
        private StatusBarPanel statusPanel = new StatusBarPanel();
        private StatusBarPanel testCountPanel = new StatusBarPanel();
        private StatusBarPanel testsRunPanel = new StatusBarPanel();
        private StatusBarPanel errorsPanel = new StatusBarPanel();
        private StatusBarPanel failuresPanel = new StatusBarPanel();
        private StatusBarPanel timePanel = new StatusBarPanel();

        private int testCount = 0;
        private int testsRun = 0;
        private int errors = 0;
        private int failures = 0;
        private double time = 0.0;

        private bool displayProgress = false;

        public bool DisplayTestProgress
        {
            get { return displayProgress; }
            set { displayProgress = value; }
        }

        public StatusBar()
        {
            Panels.Add( statusPanel );
            Panels.Add( testCountPanel );
            Panels.Add( testsRunPanel );
            Panels.Add( errorsPanel );
            Panels.Add( failuresPanel );
            Panels.Add( timePanel );

            statusPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            statusPanel.BorderStyle = StatusBarPanelBorderStyle.None;
            statusPanel.Text = "Status";
            
            testCountPanel.AutoSize = StatusBarPanelAutoSize.Contents;
            testsRunPanel.AutoSize = StatusBarPanelAutoSize.Contents;
            errorsPanel.AutoSize = StatusBarPanelAutoSize.Contents;
            failuresPanel.AutoSize = StatusBarPanelAutoSize.Contents;
            timePanel.AutoSize = StatusBarPanelAutoSize.Contents;

            ShowPanels = true;
            InitPanels();
        }

        // Kluge to keep VS from generating code that sets the Panels for
        // the statusbar. Really, our status bar should be a user control
        // to avoid this and shouldn't allow the panels to be set except
        // according to specific protocols.
        [System.ComponentModel.DesignerSerializationVisibility( 
             System.ComponentModel.DesignerSerializationVisibility.Hidden )]
        public new System.Windows.Forms.StatusBar.StatusBarPanelCollection Panels
        {
            get 
            { 
                return base.Panels;
            }
        }

    
        public override string Text
        {
            get { return statusPanel.Text; }
            set { statusPanel.Text = value; }
        }

        public void Initialize( int testCount )
        {
            Initialize( testCount, testCount > 0 ? "Ready" : "" );
        }

        public void Initialize( int testCount, string text )
        {
            this.statusPanel.Text = text;

            this.testCount = testCount;
            this.testsRun = 0;
            this.errors = 0;
            this.failures = 0;
            this.time = 0.0;

            InitPanels();
        }

        private void InitPanels()
        {
            this.testCountPanel.MinWidth = 50;
            DisplayTestCount();

            this.testsRunPanel.MinWidth = 50;
            this.testsRunPanel.Text = "";

            this.errorsPanel.MinWidth = 50;
            this.errorsPanel.Text = "";
            
            this.failuresPanel.MinWidth = 50;
            this.failuresPanel.Text = "";
            
            this.timePanel.MinWidth = 50;
            this.timePanel.Text = "";
        }

        private void DisplayTestCount()
        {
            testCountPanel.Text = "Test Cases : " + testCount.ToString();
        }

        private void DisplayTestsRun()
        {
            testsRunPanel.Text = "Tests Run : " + testsRun.ToString();
        }

        private void DisplayErrors()
        {
            errorsPanel.Text = "Errors : " + errors.ToString();
        }

        private void DisplayFailures()
        {
            failuresPanel.Text = "Failures : " + failures.ToString();
        }

        private void DisplayTime()
        {
            timePanel.Text = "Time : " + time.ToString();
        }

        private void DisplayResult(ResultNode result)
        {
            ResultSummary summarizer = ResultSummaryCreator.FromResultNode(result);

            this.testCount = summarizer.TestCount;
            this.testsRun = summarizer.RunCount;
            this.errors = summarizer.ErrorCount;
            this.failures = summarizer.FailureCount;
            this.time = summarizer.Duration;
            DisplayTestCount();
            DisplayTestsRun();
            DisplayErrors();
            DisplayFailures();
            DisplayTime();
        }

        public void InitializeView(ITestModel model, TestCentricPresenter presenter)
        {
            model.Events.TestLoaded += (TestNodeEventArgs e) =>
            {
                Initialize(e.Test.TestCount);
            };

            model.Events.TestReloaded += (TestNodeEventArgs e) =>
            {
                Initialize(e.Test.TestCount, "Reloaded");
            };

            model.Events.TestUnloaded += (TestEventArgs e) =>
            {
                Initialize(0, "Unloaded");
            };

            model.Events.TestStarting += (TestNodeEventArgs e) =>
            {
                string fullText = "Running : " + e.Test.FullName;
                string shortText = "Running : " + e.Test.Name;

                Graphics g = Graphics.FromHwnd(Handle);
                SizeF sizeNeeded = g.MeasureString(fullText, Font);
                if (statusPanel.Width >= (int)sizeNeeded.Width)
                {
                    statusPanel.Text = fullText;
                    statusPanel.ToolTipText = "";
                }
                else
                {
                    sizeNeeded = g.MeasureString(shortText, Font);
                    statusPanel.Text = statusPanel.Width >= (int)sizeNeeded.Width
                        ? shortText : e.Test.Name;
                    statusPanel.ToolTipText = e.Test.FullName;
                }
            };

            model.Events.TestFinished += (TestResultEventArgs e) =>
            {
                if (DisplayTestProgress)
                {
                    ++testsRun;
                    DisplayTestsRun();
                    if (e.Result.Status == TestStatus.Failed)
                    {
                        if (e.Result.Label == "Error" || e.Result.Label == "Cancelled")
                        {
                            ++errors;
                            DisplayErrors();
                        }
                        else
                        {
                            ++failures;
                            DisplayFailures();
                        }
                    }
                }
            };

            model.Events.RunStarting += (RunStartingEventArgs e) =>
            {
                Initialize(e.TestCount, "Running");
                DisplayTestCount();
                DisplayTestsRun();
                DisplayErrors();
                DisplayFailures();
                DisplayTime();
            };

            model.Events.RunFinished += (TestResultEventArgs e) =>
            {
                statusPanel.Text = "Completed";
                DisplayResult(e.Result);
            };
        }

        //        protected override void OnFontChanged(EventArgs e)
        //        {
        //            base.OnFontChanged(e);
        //
        //            this.Height = (int)(this.Font.Height * 1.6);
        //        }

        #region TestObserver Members

        //public void Subscribe(ITestEvents events)
        //{
        //	events.TestLoaded	+= new TestEventHandler( OnTestLoaded );
        //	events.TestReloaded	+= new TestEventHandler( OnTestReloaded );
        //	events.TestUnloaded	+= new TestEventHandler( OnTestUnloaded );

        //	events.TestStarting	+= new TestEventHandler( OnTestStarting );
        //	events.TestFinished	+= new TestEventHandler( OnTestFinished );
        //	events.RunStarting	+= new TestEventHandler( OnRunStarting );
        //	events.RunFinished	+= new TestEventHandler( OnRunFinished );
        //}

        #endregion
    }
}
