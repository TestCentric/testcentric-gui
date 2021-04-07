// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Drawing;

namespace TestCentric.Gui.Views
{
    public partial class TextOutputView : UserControlView, ITextOutputView
    {
        public TextOutputView()
        {
            InitializeComponent();

            richTextBox1.ContextMenuStrip = contextMenuStrip1;
        }

        public bool WordWrap
        {
            get { return richTextBox1.WordWrap; }
            set { richTextBox1.WordWrap = value; }
        }

        public void Clear()
        {
            InvokeIfRequired(() =>
            {
                richTextBox1.Clear();
            });
        }

        public void Write(string text)
        {
            InvokeIfRequired(() =>
            {
                richTextBox1.AppendText(text);
            });
        }

        public void Write(string text, Color color)
        {
            InvokeIfRequired(() =>
            {
                richTextBox1.SelectionStart = richTextBox1.TextLength;
                richTextBox1.SelectionLength = 0;
                richTextBox1.SelectionColor = color;

                richTextBox1.AppendText(text);

                richTextBox1.SelectionColor = ForeColor;
            });
        }

        #region Context Menu Actions

        // NOTE: The TextOutputView teakes care of its own context menu since
        // the effects of all the actions are entirely within the view.

        private void copyToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void selectAllToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        private void wordWrapToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            wordWrapToolStripMenuItem.Checked = richTextBox1.WordWrap = !richTextBox1.WordWrap;
        }

        private void increaseToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Font = new Font(Font.FontFamily, Font.SizeInPoints * 1.2f, Font.Style);
        }

        private void decreaseToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Font = new Font(Font.FontFamily, Font.SizeInPoints / 1.2f, Font.Style);
        }

        private void resetToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Font = new Font(FontFamily.GenericMonospace, 8.0f);
        }

        private void contextMenuStrip1_Opened(object sender, System.EventArgs e)
        {
            wordWrapToolStripMenuItem.Checked = richTextBox1.WordWrap;
            copyToolStripMenuItem.Enabled = richTextBox1.SelectedText != "";
            selectAllToolStripMenuItem.Enabled = richTextBox1.TextLength > 0;
        }

        #endregion
    }
}
