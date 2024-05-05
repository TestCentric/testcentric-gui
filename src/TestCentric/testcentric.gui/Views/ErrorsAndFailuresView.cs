// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
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
        static Logger log = InternalTrace.GetLogger("ErrorsAndFailureView");

        private const int ITEM_MARGIN = 2;

        int hoverIndex = -1;
        private System.Windows.Forms.Timer hoverTimer;
        TipWindow tipWindow;

        private System.Windows.Forms.ListBox detailList;
        public StackTraceDisplay stackTraceDisplay;
        public ErrorBrowser errorBrowser;
        private SourceCodeDisplay sourceCode;
        public System.Windows.Forms.Splitter tabSplitter;
        private System.Windows.Forms.ContextMenuStrip detailListContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem copyDetailMenuItem;
        private Label noErrorsMessage;
        private System.ComponentModel.IContainer components;

        #region Construction and Disposal

        public ErrorsAndFailuresView()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            sourceCode = new SourceCodeDisplay();

            errorBrowser.RegisterDisplay(sourceCode);
            errorBrowser.RegisterDisplay(stackTraceDisplay);

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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.noErrorsMessage = new System.Windows.Forms.Label();
            this.detailList = new System.Windows.Forms.ListBox();
            this.tabSplitter = new System.Windows.Forms.Splitter();
            this.errorBrowser = new NUnit.UiException.Controls.ErrorBrowser();
            this.stackTraceDisplay = new NUnit.UiException.Controls.StackTraceDisplay();
            this.detailListContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyDetailMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detailListContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // noErrorsMessage
            // 
            this.noErrorsMessage.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.noErrorsMessage.AutoSize = true;
            this.noErrorsMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noErrorsMessage.Location = new System.Drawing.Point(120, 45);
            this.noErrorsMessage.Name = "noErrorsMessage";
            this.noErrorsMessage.Size = new System.Drawing.Size(229, 20);
            this.noErrorsMessage.TabIndex = 1;
            this.noErrorsMessage.Text = "No Errors, Failures or Warnings";
            // 
            // detailList
            // 
            this.detailList.Dock = System.Windows.Forms.DockStyle.Top;
            this.detailList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.detailList.Font = new System.Drawing.Font("Courier New", 8F);
            this.detailList.HorizontalScrollbar = true;
            this.detailList.Location = new System.Drawing.Point(0, 0);
            this.detailList.Name = "detailList";
            this.detailList.ScrollAlwaysVisible = true;
            this.detailList.Size = new System.Drawing.Size(496, 128);
            this.detailList.TabIndex = 1;
            this.detailList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.detailList_DrawItem);
            this.detailList.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.detailList_MeasureItem);
            this.detailList.SelectedIndexChanged += new System.EventHandler(this.detailList_SelectedIndexChanged);
            this.detailList.MouseLeave += new System.EventHandler(this.detailList_MouseLeave);
            this.detailList.MouseHover += new System.EventHandler(this.OnMouseHover);
            this.detailList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.detailList_MouseMove);
            // 
            // tabSplitter
            // 
            this.tabSplitter.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabSplitter.Location = new System.Drawing.Point(0, 128);
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
            this.errorBrowser.Location = new System.Drawing.Point(0, 137);
            this.errorBrowser.Name = "errorBrowser";
            this.errorBrowser.SelectedDisplay = null;
            this.errorBrowser.Size = new System.Drawing.Size(496, 151);
            this.errorBrowser.StackTraceSource = null;
            this.errorBrowser.TabIndex = 4;
            // 
            // stackTraceDisplay
            // 
            this.stackTraceDisplay.Font = new System.Drawing.Font("Courier New", 8F);
            this.stackTraceDisplay.Location = new System.Drawing.Point(0, 0);
            this.stackTraceDisplay.Name = "stackTraceDisplay";
            this.stackTraceDisplay.Size = new System.Drawing.Size(150, 150);
            this.stackTraceDisplay.TabIndex = 0;
            // 
            // detailListContextMenuStrip
            // 
            this.detailListContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyDetailMenuItem});
            this.detailListContextMenuStrip.Name = "detailListContextMenuStrip";
            this.detailListContextMenuStrip.Size = new System.Drawing.Size(103, 26);
            // 
            // copyDetailMenuItem
            // 
            this.copyDetailMenuItem.Name = "copyDetailMenuItem";
            this.copyDetailMenuItem.Size = new System.Drawing.Size(102, 22);
            this.copyDetailMenuItem.Text = "Copy";
            this.copyDetailMenuItem.Click += new System.EventHandler(this.copyDetailMenuItem_Click);
            // 
            // ErrorsAndFailuresView
            // 
            this.Controls.Add(this.noErrorsMessage);
            this.Controls.Add(this.errorBrowser);
            this.Controls.Add(this.tabSplitter);
            this.Controls.Add(this.detailList);
            this.Name = "ErrorsAndFailuresView";
            this.Size = new System.Drawing.Size(496, 288);
            this.detailListContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region IErrorsAndFailuresView Members

        public event EventHandler SplitterPositionChanged;
        public event EventHandler SourceCodeSplitterDistanceChanged;
        public event EventHandler SourceCodeSplitOrientationChanged;
        public event EventHandler SourceCodeDisplayChanged;

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
                detailList.ContextMenuStrip = null;
                errorBrowser.StackTraceSource = "";
                noErrorsMessage.Show();
            });
        }

        public void AddResult(string testName, string message, string stackTrace)
        {
            InvokeIfRequired(() =>
            {
                noErrorsMessage.Hide();
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
            errorBrowser.StackTraceSource = resultItem.FilteredStackTrace;
            detailList.ContextMenuStrip = detailListContextMenuStrip;
        }

        private void detailList_MeasureItem(object sender, System.Windows.Forms.MeasureItemEventArgs e)
        {
            TestResultItem item = (TestResultItem)detailList.Items[e.Index];
            SizeF size = e.Graphics.MeasureString(item.ToString(), detailList.Font);
            e.ItemHeight = (int)size.Height + 2 * ITEM_MARGIN;
            e.ItemWidth = (int)size.Width;
            if (e.ItemWidth > detailList.HorizontalExtent)
                detailList.HorizontalExtent = e.ItemWidth;
        }

        private void detailList_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                e.DrawBackground();
                TestResultItem item = (TestResultItem)detailList.Items[e.Index];
                Brush brush = (e.State & DrawItemState.Selected) == DrawItemState.Selected
                    ? SystemBrushes.HighlightText
                    : SystemBrushes.WindowText;
                e.Graphics.DrawString(item.ToString(), detailList.Font, brush, e.Bounds.Left, e.Bounds.Top + ITEM_MARGIN);
            }
        }

        private void RefillDetailList()
        {
            if (this.detailList.Items.Count > 0)
            {
                this.detailList.BeginUpdate();
                ArrayList copiedItems = new ArrayList(detailList.Items);
                this.detailList.Items.Clear();
                foreach (object item in copiedItems)
                    this.detailList.Items.Add(item);
                this.detailList.EndUpdate();
                this.detailList.Invalidate();
            }
        }

        private void copyDetailMenuItem_Click(object sender, System.EventArgs e)
        {
            if (detailList.SelectedItem != null)
                Clipboard.SetDataObject(detailList.SelectedItem.ToString());
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
                    tipWindow.MouseLeaveDelay = 500;
                    tipWindow.Closed += new EventHandler(tipWindow_Closed);
                    tipWindow.Show();
                }
            }
        }

        private void tipWindow_Closed(object sender, System.EventArgs e)
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

            hoverIndex = detailList.IndexFromPoint(e.X, e.Y);

            if (hoverIndex >= 0 && hoverIndex < detailList.Items.Count)
            {
                // Workaround problem of IndexFromPoint returning an
                // index when mouse is over bottom part of list.
                Rectangle r = detailList.GetItemRectangle(hoverIndex);
                if (e.Y > r.Bottom)
                    hoverIndex = -1;
                else
                {
                    hoverTimer = new System.Windows.Forms.Timer();
                    hoverTimer.Interval = 800;
                    hoverTimer.Tick += new EventHandler(OnMouseHover);
                    hoverTimer.Start();
                }
            }
        }

        private void ClearTimer()
        {
            if (hoverTimer != null)
            {
                hoverTimer.Stop();
                hoverTimer.Dispose();
            }
        }

        private void tabSplitter_SplitterMoved(object sender, SplitterEventArgs e)
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
