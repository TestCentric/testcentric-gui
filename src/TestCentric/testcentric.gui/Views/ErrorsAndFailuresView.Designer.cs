using NUnit.UiException.Controls;

namespace TestCentric.Gui.Views
{
    partial class ErrorsAndFailuresView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.header = new System.Windows.Forms.Label();
            this.noErrorsMessage = new System.Windows.Forms.Label();
            this.detailList = new System.Windows.Forms.ListBox();
            this.tabSplitter = new System.Windows.Forms.Splitter();
            this.errorBrowser = new NUnit.UiException.Controls.ErrorBrowser();
            this.panel1 = new System.Windows.Forms.Panel();
            this.stackTraceDisplay = new NUnit.UiException.Controls.StackTraceDisplay();
            this.detailListContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyDetailMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.detailListContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // header
            // 
            this.header.BackColor = System.Drawing.SystemColors.Window;
            this.header.Dock = System.Windows.Forms.DockStyle.Top;
            this.header.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.header.Location = new System.Drawing.Point(0, 0);
            this.header.Margin = new System.Windows.Forms.Padding(0);
            this.header.Name = "header";
            this.header.Padding = new System.Windows.Forms.Padding(2);
            this.header.Size = new System.Drawing.Size(496, 18);
            this.header.TabIndex = 0;
            this.header.Text = "Test name goes here";
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
            this.errorBrowser.Location = new System.Drawing.Point(0, 0);
            this.errorBrowser.Name = "errorBrowser";
            this.errorBrowser.SelectedDisplay = null;
            this.errorBrowser.Size = new System.Drawing.Size(496, 270);
            this.errorBrowser.StackTraceSource = null;
            this.errorBrowser.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.errorBrowser);
            this.panel1.Controls.Add(this.tabSplitter);
            this.panel1.Controls.Add(this.detailList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(496, 270);
            this.panel1.TabIndex = 2;
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
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.header);
            this.Name = "ErrorsAndFailuresView";
            this.Size = new System.Drawing.Size(496, 288);
            this.panel1.ResumeLayout(false);
            this.detailListContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label header;
        private System.Windows.Forms.ListBox detailList;
        private System.Windows.Forms.Panel panel1;
        public ErrorBrowser errorBrowser;
        public StackTraceDisplay stackTraceDisplay;
        private SourceCodeDisplay sourceCode;
        public System.Windows.Forms.Splitter tabSplitter;
        private System.Windows.Forms.ContextMenuStrip detailListContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem copyDetailMenuItem;
        private System.Windows.Forms.Label noErrorsMessage;
    }
}
