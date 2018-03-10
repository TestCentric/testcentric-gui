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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using NUnit.UiException.Properties;

namespace NUnit.UiException.Controls
{
    /// <summary>
    /// Implements IErrorDisplay to show the actual stack trace in a TextBox control.
    /// </summary>
    public class StackTraceDisplay :
        UserControl,
        IErrorDisplay
    {
        private TextBox _textContent;
        private ToolStripButton _btnPlugin;
        private ToolStripButton _btnCopy;

        /// <summary>
        /// Builds a new instance of StackTraceDisplay.
        /// </summary>
        public StackTraceDisplay()
        {
            _btnPlugin = ErrorToolbar.NewStripButton(true, "Display actual stack trace", Resources.ImageStackTraceDisplay, null);
            _btnCopy = ErrorToolbar.NewStripButton(false, "Copy stack trace to clipboard", Resources.ImageCopyToClipboard, OnClick);

            _textContent = new TextBox();
            _textContent.ReadOnly = true;
            _textContent.Multiline = true;
            _textContent.ScrollBars = ScrollBars.Both;

           return;
        }

        protected override void OnFontChanged(EventArgs e)
        {
            _textContent.Font = this.Font;

            base.OnFontChanged(e);
        }

        /// <summary>
        /// Copies the actual stack trace to the clipboard.
        /// </summary>
        public void CopyToClipBoard()
        {
            if (String.IsNullOrEmpty(_textContent.Text))
            {
                Clipboard.Clear();
                return;
            }

            Clipboard.SetText(_textContent.Text);

            return;
        }

        #region IErrorDisplay Membres

        public ToolStripButton PluginItem
        {
            get { return (_btnPlugin); }
        }

        public ToolStripItem[] OptionItems
        {
            get { return (new ToolStripItem[] { _btnCopy }); }
        }
        
        public Control Content
        {
            get { return (_textContent); }
        }

        public void OnStackTraceChanged(string stackTrace)
        {
            _textContent.Text = stackTrace;
        }

        #endregion

        private void OnClick(object sender, EventArgs args)
        {
            CopyToClipBoard();
        }
    }
}
