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

using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using NUnit.UiException.Controls;

namespace TestCentric.Gui.Views
{
    using Controls;

    /// <summary>
    /// Summary description for ErrorDisplay.
    /// </summary>
    public class ErrorsAndFailuresView : UserControlView, IErrorsAndFailuresView
    {
        int hoverIndex = -1;
        private System.Windows.Forms.Timer hoverTimer;
        TipWindow tipWindow;
        private bool wordWrap = false;

        private System.Windows.Forms.ListBox detailList;
        public StackTraceDisplay stackTraceDisplay;
        public ErrorBrowser errorBrowser;
        private SourceCodeDisplay sourceCode;
        public System.Windows.Forms.Splitter tabSplitter;
        private System.Windows.Forms.ContextMenu detailListContextMenu;
        private System.Windows.Forms.MenuItem copyDetailMenuItem;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        #region Construction and Disposal

        public ErrorsAndFailuresView()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            sourceCode.SplitOrientationChanged += (s, e) =>
            {
                SourceCodeSplitOrientationChanged?.Invoke(this, new EventArgs());
            };

            sourceCode.SplitterDistanceChanged += (s, e) =>
            {
                SourceCodeSplitterDistanceChanged?.Invoke(this, new EventArgs());
            };

            errorBrowser.StackTraceDisplayChanged += (s, e) =>
            {
                SourceCodeDisplayChanged?.Invoke(this, new EventArgs());
            };
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
            }
            base.Dispose( disposing );
        }

        #endregion

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.detailList = new System.Windows.Forms.ListBox();
            this.tabSplitter = new System.Windows.Forms.Splitter();

            this.errorBrowser = new NUnit.UiException.Controls.ErrorBrowser();
            this.sourceCode = new SourceCodeDisplay();
            this.stackTraceDisplay = new StackTraceDisplay();
            this.detailListContextMenu = new System.Windows.Forms.ContextMenu();
            this.copyDetailMenuItem = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
			// 
			// detailList
			// 
			//this.detailList.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.detailList.Dock = DockStyle.Top;
            this.detailList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.detailList.Font = new Font(FontFamily.GenericMonospace, 8.0F);
            this.detailList.HorizontalExtent = 2000;
            this.detailList.HorizontalScrollbar = true;
            this.detailList.ItemHeight = 16;
            this.detailList.Location = new System.Drawing.Point(0, 123);
            this.detailList.Name = "detailList";
            this.detailList.ScrollAlwaysVisible = true;
            this.detailList.Size = new System.Drawing.Size(496, 128);
            this.detailList.TabIndex = 1;
            this.detailList.Resize += new System.EventHandler(this.detailList_Resize);
            this.detailList.MouseHover += new System.EventHandler(this.OnMouseHover);
            this.detailList.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.detailList_MeasureItem);
            this.detailList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.detailList_MouseMove);
            this.detailList.MouseLeave += new System.EventHandler(this.detailList_MouseLeave);
            this.detailList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.detailList_DrawItem);
            this.detailList.SelectedIndexChanged += new System.EventHandler(this.detailList_SelectedIndexChanged);
			// 
			// tabSplitter
			// 
            this.tabSplitter.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabSplitter.Location = new System.Drawing.Point(0, 251);
            this.tabSplitter.MinSize = 100;
            this.tabSplitter.Name = "tabSplitter";
            this.tabSplitter.Size = new System.Drawing.Size(496, 9);
            this.tabSplitter.TabIndex = 3;
            this.tabSplitter.TabStop = false;
			this.tabSplitter.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.tabSplitter_SplitterMoved);
            // 
            // errorBrowser
            // 
            this.errorBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorBrowser.Name = "errorBrowser";
            this.errorBrowser.Size = new System.Drawing.Size(496, 151);
            this.errorBrowser.StackTraceSource = null;
            this.errorBrowser.TabIndex = 4;
            //
            // configure and register SourceCodeDisplay
            //
            this.sourceCode.AutoSelectFirstItem = true;
            this.sourceCode.ListOrderPolicy = ErrorListOrderPolicy.ReverseOrder;
            this.sourceCode.SplitOrientation = Orientation.Vertical;
            this.sourceCode.SplitterDistance = 0.3f;
            this.stackTraceDisplay.Font = new Font(FontFamily.GenericMonospace, 8.0F);
            this.errorBrowser.RegisterDisplay(sourceCode);
            this.errorBrowser.RegisterDisplay(stackTraceDisplay);
            //
            // detailListContextMenu
            // 
            this.detailListContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                this.copyDetailMenuItem});
            // 
            // copyDetailMenuItem
            // 
            this.copyDetailMenuItem.Index = 0;
            this.copyDetailMenuItem.Text = "Copy";
            this.copyDetailMenuItem.Click += new System.EventHandler(this.copyDetailMenuItem_Click);
            // 
            // ErrorDisplay
            // 
			this.Controls.Add(this.errorBrowser);
			this.Controls.Add(this.tabSplitter);
			this.Controls.Add(this.detailList);
            this.Name = "ErrorDisplay";
            this.Size = new System.Drawing.Size(496, 288);
            this.ResumeLayout(false);

        }
        #endregion

        #region IErrorsAndFailuresView Members

        public event EventHandler SplitterPositionChanged;
        public event EventHandler SourceCodeSplitterDistanceChanged;
        public event EventHandler SourceCodeSplitOrientationChanged;
        public event EventHandler SourceCodeDisplayChanged;

        public bool WordWrap
        {
            get { return wordWrap; }
            set
            {
                if (value != wordWrap)
                    InvokeIfRequired(() =>
                    {
                        wordWrap = value;
                        RefillDetailList();
                    });
            }
        }

        public bool EnableToolTips { get; set; }

        public override Font Font
        {
            get { return base.Font; }
            set
            {
                if (value != base.Font)
                    InvokeIfRequired(() =>
                    {
                        base.Font = value;
                        detailList.Font = value;
                        stackTraceDisplay.Font = value;
                        sourceCode.CodeDisplayFont = value;
                        RefillDetailList();
                    });
            }
        }

        public int SplitterPosition
        {
            get { return tabSplitter.SplitPosition; }
            set
            {
                if (value >= tabSplitter.MinSize && value < ClientSize.Height)
                    InvokeIfRequired(() =>
                    {
                        tabSplitter.SplitPosition = value;
                    });
            }
        }

        public float SourceCodeSplitterDistance
        {
            get { return sourceCode.SplitterDistance; }
            set
            {
                InvokeIfRequired(() =>
                {
                    sourceCode.SplitterDistance = value;
                });
            }
        }

        public Orientation SourceCodeSplitOrientation
        {
            get { return sourceCode.SplitOrientation; }
            set
            {
                InvokeIfRequired(() =>
                {
                    sourceCode.SplitOrientation = value;
                });
            }

        }

        public bool SourceCodeDisplay
        {
            get { return errorBrowser.SelectedDisplay == sourceCode; }
            set
            {
                InvokeIfRequired(() =>
                {
                    if (value)
                        errorBrowser.SelectedDisplay = sourceCode;
                    else
                        errorBrowser.SelectedDisplay = stackTraceDisplay;
                });
            }
        }

        public void Clear()
        {
            InvokeIfRequired(() =>
            {
                detailList.Items.Clear();
                detailList.ContextMenu = null;
                errorBrowser.StackTraceSource = "";
            });
        }

        public void AddResult(string testName, string message, string stackTrace)
        {
            InvokeIfRequired(() =>
            {
                InsertTestResultItem(new TestResultItem(testName, message, stackTrace));
            });
        }

        #endregion

        #region DetailList Events
        /// <summary>
        /// When one of the detail failure items is selected, display
        /// the stack trace and set up the tool tip for that item.
        /// </summary>
        private void detailList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            TestResultItem resultItem = (TestResultItem)detailList.SelectedItem;
            errorBrowser.StackTraceSource = resultItem.StackTrace;
            detailList.ContextMenu = detailListContextMenu;
        }

        private void detailList_MeasureItem(object sender, System.Windows.Forms.MeasureItemEventArgs e)
        {
            TestResultItem item = (TestResultItem)detailList.Items[e.Index];
            string s = item.ToString();
            SizeF size = this.WordWrap
                ? e.Graphics.MeasureString(item.ToString(), detailList.Font, detailList.ClientSize.Width)
                : e.Graphics.MeasureString(item.ToString(), detailList.Font);
            e.ItemHeight = (int)size.Height;
            e.ItemWidth = (int)size.Width;
        }

        private void detailList_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                e.DrawBackground();
                TestResultItem item = (TestResultItem)detailList.Items[e.Index];
                bool selected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected) ? true : false;
                Brush brush = selected ? SystemBrushes.HighlightText : SystemBrushes.WindowText;
                RectangleF layoutRect = e.Bounds;
                if (this.WordWrap && layoutRect.Width > detailList.ClientSize.Width)
                    layoutRect.Width = detailList.ClientSize.Width;
                e.Graphics.DrawString(item.ToString(), detailList.Font, brush, layoutRect);
            }
        }

        private void detailList_Resize(object sender, System.EventArgs e)
        {
            if ( this.WordWrap ) RefillDetailList();
        }

        private void RefillDetailList()
        {
            if ( this.detailList.Items.Count > 0 )
            {
                this.detailList.BeginUpdate();
                ArrayList copiedItems = new ArrayList( detailList.Items );
                this.detailList.Items.Clear();
                foreach( object item in copiedItems )
                    this.detailList.Items.Add( item );
                this.detailList.EndUpdate();
            }
        }

        private void copyDetailMenuItem_Click(object sender, System.EventArgs e)
        {
            if ( detailList.SelectedItem != null )
                Clipboard.SetDataObject( detailList.SelectedItem.ToString() );
        }

        private void OnMouseHover(object sender, System.EventArgs e)
        {
            if (tipWindow != null) tipWindow.Close();

            if (hoverIndex >= 0 && hoverIndex < detailList.Items.Count)
            {
                Graphics g = Graphics.FromHwnd(detailList.Handle);

                Rectangle itemRect = detailList.GetItemRectangle(hoverIndex);
                string text = detailList.Items[hoverIndex].ToString();

                SizeF sizeNeeded = g.MeasureString(text, detailList.Font);
                bool expansionNeeded =
                    itemRect.Width < (int)sizeNeeded.Width ||
                    itemRect.Height < (int)sizeNeeded.Height;

                if (expansionNeeded)
                {
                    tipWindow = new TipWindow(detailList, hoverIndex);
                    tipWindow.ItemBounds = itemRect;
                    tipWindow.TipText = text;
                    tipWindow.Expansion = TipWindow.ExpansionStyle.Both;
                    tipWindow.Overlay = true;
                    tipWindow.WantClicks = true;
                    tipWindow.Closed += new EventHandler(tipWindow_Closed);
                    tipWindow.Show();
                }
            }
        }

        private void tipWindow_Closed( object sender, System.EventArgs e )
        {
            tipWindow = null;
            hoverIndex = -1;
            ClearTimer();
        }

        private void detailList_MouseLeave(object sender, System.EventArgs e)
        {
            hoverIndex = -1;
            ClearTimer();
        }

        private void detailList_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ClearTimer();

            hoverIndex = detailList.IndexFromPoint( e.X, e.Y );	

            if ( hoverIndex >= 0 && hoverIndex < detailList.Items.Count )
            {
                // Workaround problem of IndexFromPoint returning an
                // index when mouse is over bottom part of list.
                Rectangle r = detailList.GetItemRectangle( hoverIndex );
                if ( e.Y > r.Bottom )
                    hoverIndex = -1;
                else
                {
                    hoverTimer = new System.Windows.Forms.Timer();
                    hoverTimer.Interval = 800;
                    hoverTimer.Tick += new EventHandler( OnMouseHover );
                    hoverTimer.Start();
                }
            }
        }

        private void ClearTimer()
        {
            if ( hoverTimer != null )
            {
                hoverTimer.Stop();
                hoverTimer.Dispose();
            }
        }

        private void tabSplitter_SplitterMoved( object sender, SplitterEventArgs e )
        {
            SplitterPositionChanged?.Invoke(this, new EventArgs());
        }

        #endregion

        #region Helper Methods

        private void InsertTestResultItem(TestResultItem item)
        {
            detailList.BeginUpdate();
            detailList.Items.Insert(detailList.Items.Count, item);
            detailList.SelectedIndex = 0;
            detailList.EndUpdate();
        }

        #endregion
    }
}
