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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using NUnit.Engine;

namespace TestCentric.Gui.Controls
{
    using Model;

    /// <summary>
    /// ColorProgressBar provides a custom progress bar with the
    /// ability to control the color of the bar and to render itself
    /// in either solid or segmented style. The bar can be updated
    /// on the fly and has code to avoid repainting the entire bar
    /// when that occurs.
    /// </summary>
    public class ColorProgressBar : System.Windows.Forms.Control
    {
        #region Instance Variables
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// The current progress value
        /// </summary>
        private int val = 0;

        /// <summary>
        /// The minimum value allowed
        /// </summary>
        private int min = 0;

        /// <summary>
        /// The maximum value allowed
        /// </summary>
        private int max = 100;

        /// <summary>
        /// Amount to advance for each step
        /// </summary>
        private int step = 1;

        /// <summary>
        /// Last segment displayed when displaying asynchronously rather 
        /// than through OnPaint calls.
        /// </summary>
        private int lastSegmentCount=0;

        /// <summary>
        /// The brush to use in painting the progress bar
        /// </summary>
        private Brush foreBrush = null;

        /// <summary>
        /// The brush to use in painting the background of the bar
        /// </summary>
        private Brush backBrush = null;

        /// <summary>
        /// Indicates whether to draw the bar in segments or not
        /// </summary>
        private bool segmented = false;

        #endregion

        #region Constructors & Disposer
        public ColorProgressBar()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
                this.ReleaseBrushes();
            }
            base.Dispose( disposing );
        }
        #endregion

        #region Properties
        
        [Category("Behavior")]
        public int Minimum
        {
            get { return this.min; }
            set
            {
                if (value <= Maximum) 
                {
                    if (this.min != value) 
                    {
                        this.min = value;
                        this.Invalidate();
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Minimum", value
                        ,"Minimum must be <= Maximum.");
                }
            }
        }

        [Category("Behavior")]
        public int Maximum 
        {
            get	{ return this.max; }
            set
            {
                if (value >= Minimum) 
                {
                    if (this.max != value) 
                    {
                        this.max = value;
                        this.Invalidate();
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Maximum", value
                        ,"Maximum must be >= Minimum.");
                }
            }
        }

        [Category("Behavior")]
        public int Step
        {
            get	{ return this.step; }
            set
            {
                if (value <= Maximum && value >= Minimum) 
                {
                    this.step = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Step", value
                        ,"Must fall between Minimum and Maximum inclusive.");
                }
            }
        }
        
        [Browsable(false)]
        private float PercentValue
        {
            get
            {
                if (0 != Maximum - Minimum) // NRG 05/28/03: Prevent divide by zero
                    return((float)this.val / ((float)Maximum - (float)Minimum));
                else
                    return(0);
            }
        }	

        [Category("Behavior")]
        public int Value 
        {
            get { return this.val; }
            set 
            {
                if(value == this.val)
                    return;
                else if(value <= Maximum && value >= Minimum)
                {
                    this.val = value;
                    this.Invalidate();
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Value", value
                        ,"Must fall between Minimum and Maximum inclusive.");
                }
            }
        }

        [Category("Appearance")]
        public bool Segmented
        {
            get { return segmented; }
            set { segmented = value; }
        }

        #endregion

        #region Methods

        protected override void OnCreateControl()
        {
        }

        public void PerformStep()
        {
            int newValue = Value + Step;

            if( newValue > Maximum )
                newValue = Maximum;

            Value = newValue;
        }

        protected override void OnBackColorChanged(System.EventArgs e)
        {
            base.OnBackColorChanged(e);
            this.Refresh();
        }
        protected override void OnForeColorChanged(System.EventArgs e)
        {
            base.OnForeColorChanged(e);
            this.Refresh();
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            this.lastSegmentCount=0;
            this.ReleaseBrushes();
            PaintBar(e.Graphics);
            ControlPaint.DrawBorder3D(
                e.Graphics
                ,this.ClientRectangle
                ,Border3DStyle.SunkenOuter);
            //e.Graphics.Flush();
        }

        private void ReleaseBrushes()
        {
            if(foreBrush != null)
            {
                foreBrush.Dispose();
                backBrush.Dispose();
                foreBrush=null;
                backBrush=null;
            }
        }

        private void AcquireBrushes()
        {
            if(foreBrush == null)
            {
                foreBrush = new SolidBrush(this.ForeColor);
                backBrush = new SolidBrush(this.BackColor);
            }
        }

        private void PaintBar(Graphics g)
        {
            Rectangle theBar = Rectangle.Inflate(ClientRectangle, -2, -2);
            int maxRight = theBar.Right-1;
            this.AcquireBrushes();

            if ( segmented )
            {
                int segmentWidth = (int)((float)ClientRectangle.Height * 0.66f);
                int maxSegmentCount = ( theBar.Width + segmentWidth ) / segmentWidth;

                //int maxRight = Bar.Right;
                int newSegmentCount = (int)System.Math.Ceiling(PercentValue*maxSegmentCount);
                if(newSegmentCount > lastSegmentCount)
                {
                    theBar.X += lastSegmentCount*segmentWidth;
                    while (lastSegmentCount < newSegmentCount )
                    {
                        theBar.Width = System.Math.Min( maxRight - theBar.X, segmentWidth - 2 );
                        g.FillRectangle( foreBrush, theBar );
                        theBar.X += segmentWidth;
                        lastSegmentCount++;
                    }
                }
                else if(newSegmentCount < lastSegmentCount)
                {
                    theBar.X += newSegmentCount * segmentWidth;
                    theBar.Width = maxRight - theBar.X;
                    g.FillRectangle(backBrush, theBar);
                    lastSegmentCount = newSegmentCount;
                }
            }
            else
            {
                //g.FillRectangle( backBrush, theBar );
                theBar.Width = theBar.Width * val / max;
                g.FillRectangle( foreBrush, theBar );
            }

            if(Value == Minimum || Value == Maximum)
                this.ReleaseBrushes();
        }

        #endregion

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // ProgressBar
            // 
            this.CausesValidation = false;
            this.Enabled = false;
            this.ForeColor = System.Drawing.SystemColors.Highlight;
            this.Name = "ProgressBar";
            this.Size = new System.Drawing.Size(432, 24);
        }
        #endregion
    }

    public class TestProgressBar : ColorProgressBar, IViewControl
    {
        private readonly static Color SuccessColor = Color.Lime;
        private readonly static Color FailureColor = Color.Red;
        private readonly static Color IgnoredColor = Color.Yellow;
        private readonly static Color WarningColor = Color.Yellow;

        private bool ClearResultsOnReload = true;

        public TestProgressBar()
        {
            Initialize( 100 );
        }

        private void Initialize( int testCount )
        {
            Value = 0;
            Maximum = testCount;
            ForeColor = SuccessColor;
        }

        #region ITestObserver Members

        public void InitializeView(ITestModel model, TestCentricPresenter presenter)
        {
            model.Events.TestLoaded += (TestNodeEventArgs e) =>
            {
                Initialize(e.Test.TestCount);
            };

            model.Events.TestReloaded += (TestNodeEventArgs e) =>
            {
                if (model.Services.UserSettings.Gui.ClearResultsOnReload)
                    Initialize(e.Test.TestCount);
                else
                    Value = Maximum = e.Test.TestCount;
            };

            model.Events.TestUnloaded += (TestEventArgs e) =>
            {
                Initialize(100);
            };

            model.Events.RunStarting += (RunStartingEventArgs e) =>
            {
                Initialize(e.TestCount);
            };

            model.Events.TestFinished += (TestResultEventArgs e) =>
            {
                PerformStep();

                switch (e.Result.Outcome.Status)
                {
                    case TestStatus.Failed:
                        ForeColor = FailureColor;
                        break;
                    case TestStatus.Skipped:
                        if (ForeColor == SuccessColor && e.Result.Outcome.Label == "Ignored")
                            ForeColor = IgnoredColor;
                        break;
                    case TestStatus.Warning:
                        if (ForeColor == SuccessColor)
                            ForeColor = WarningColor;
                        break;
                    default:
                        break;
                }
            };

            model.Events.SuiteFinished += (TestResultEventArgs e) =>
            {
                var outcome = e.Result.Outcome;
                if (outcome.Site == FailureSite.TearDown && outcome.Status == TestStatus.Failed)
                    ForeColor = FailureColor;
            };

            //model.TestException += (TestEventArgs e) =>
            //{
            //    ForeColor = FailureColor;
            //};
        }

        #endregion
    }
}
