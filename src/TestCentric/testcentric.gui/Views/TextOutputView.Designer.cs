namespace TestCentric.Gui.Views
{
    partial class TextOutputView
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.wordWrapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.increaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decreaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.labelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelsOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelsOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelsBeforeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelsAfterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelsBeforeAndAfterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(150, 150);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.selectAllToolStripMenuItem,
            this.toolStripSeparator1,
            this.wordWrapToolStripMenuItem,
            this.fontToolStripMenuItem,
            this.toolStripSeparator2,
            this.labelsToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 148);
            this.contextMenuStrip1.Opened += new System.EventHandler(this.contextMenuStrip1_Opened);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            this.copyToolStripMenuItem.ToolTipText = "Copy selected text to the clipboard.";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.selectAllToolStripMenuItem.Text = "Select &All";
            this.selectAllToolStripMenuItem.ToolTipText = "Select all text displayed.";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // wordWrapToolStripMenuItem
            // 
            this.wordWrapToolStripMenuItem.Name = "wordWrapToolStripMenuItem";
            this.wordWrapToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.wordWrapToolStripMenuItem.Text = "&Word Wrap";
            this.wordWrapToolStripMenuItem.ToolTipText = "Toggle word-wrap.";
            this.wordWrapToolStripMenuItem.Click += new System.EventHandler(this.wordWrapToolStripMenuItem_Click);
            // 
            // fontToolStripMenuItem
            // 
            this.fontToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.increaseToolStripMenuItem,
            this.decreaseToolStripMenuItem,
            this.resetToolStripMenuItem});
            this.fontToolStripMenuItem.Name = "fontToolStripMenuItem";
            this.fontToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.fontToolStripMenuItem.Text = "Font";
            // 
            // increaseToolStripMenuItem
            // 
            this.increaseToolStripMenuItem.Name = "increaseToolStripMenuItem";
            this.increaseToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.increaseToolStripMenuItem.Text = "Increase";
            this.increaseToolStripMenuItem.Click += new System.EventHandler(this.increaseToolStripMenuItem_Click);
            // 
            // decreaseToolStripMenuItem
            // 
            this.decreaseToolStripMenuItem.Name = "decreaseToolStripMenuItem";
            this.decreaseToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.decreaseToolStripMenuItem.Text = "Decrease";
            this.decreaseToolStripMenuItem.Click += new System.EventHandler(this.decreaseToolStripMenuItem_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // labelsToolStripMenuItem
            // 
            this.labelsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelsOffToolStripMenuItem,
            this.labelsOnToolStripMenuItem,
            this.labelsBeforeToolStripMenuItem,
            this.labelsAfterToolStripMenuItem,
            this.labelsBeforeAndAfterToolStripMenuItem});
            this.labelsToolStripMenuItem.Name = "labelsToolStripMenuItem";
            this.labelsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.labelsToolStripMenuItem.Text = "Test Case Labels";
            // 
            // labelsOffToolStripMenuItem
            // 
            this.labelsOffToolStripMenuItem.Name = "labelsOffToolStripMenuItem";
            this.labelsOffToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.labelsOffToolStripMenuItem.Tag = "OFF";
            this.labelsOffToolStripMenuItem.Text = "Off";
            this.labelsOffToolStripMenuItem.ToolTipText = "Displays output without any labels.";
            // 
            // labelsOnToolStripMenuItem
            // 
            this.labelsOnToolStripMenuItem.Name = "labelsOnToolStripMenuItem";
            this.labelsOnToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.labelsOnToolStripMenuItem.Tag = "ON";
            this.labelsOnToolStripMenuItem.Text = "On";
            this.labelsOnToolStripMenuItem.ToolTipText = "Displays a label immediately before any text output.\r\n        Repeats the label i" +
    "f output is interleaved.";
            // 
            // labelsBeforeToolStripMenuItem
            // 
            this.labelsBeforeToolStripMenuItem.Name = "labelsBeforeToolStripMenuItem";
            this.labelsBeforeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.labelsBeforeToolStripMenuItem.Tag = "BEFORE";
            this.labelsBeforeToolStripMenuItem.Text = "Before";
            this.labelsBeforeToolStripMenuItem.ToolTipText = "Displays a label at the start of each  test, even if no output is produced.\r\n    " +
    "    Repeats the label if output is interleaved.";
            // 
            // labelsAfterToolStripMenuItem
            // 
            this.labelsAfterToolStripMenuItem.Name = "labelsAfterToolStripMenuItem";
            this.labelsAfterToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.labelsAfterToolStripMenuItem.Tag = "AFTER";
            this.labelsAfterToolStripMenuItem.Text = "After";
            this.labelsAfterToolStripMenuItem.ToolTipText = "Displays a label at the end of every test, including pass/fail status.\r\n        R" +
    "epeats the label if output is interleaved.";
            // 
            // labelsBeforeAndAfterToolStripMenuItem
            // 
            this.labelsBeforeAndAfterToolStripMenuItem.Name = "labelsBeforeAndAfterToolStripMenuItem";
            this.labelsBeforeAndAfterToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.labelsBeforeAndAfterToolStripMenuItem.Tag = "BEFOREANDAFTER";
            this.labelsBeforeAndAfterToolStripMenuItem.Text = "Before and After";
            this.labelsBeforeAndAfterToolStripMenuItem.ToolTipText = "Displays labels for both the start and the end of every test.\r\n        Repeats th" +
    "e label if output is interleaved.";
            // 
            // TextOutputView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.richTextBox1);
            this.Name = "TextOutputView";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wordWrapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem increaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decreaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem labelsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem labelsOffToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem labelsOnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem labelsBeforeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem labelsAfterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem labelsBeforeAndAfterToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}
