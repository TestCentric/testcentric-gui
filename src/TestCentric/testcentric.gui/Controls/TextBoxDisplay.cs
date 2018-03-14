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
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using NUnit.Engine;

namespace TestCentric.Gui.Controls
{
    using Model;
    using Model.Settings;

	/// <summary>
	/// TextBoxDisplay is an adapter that allows accessing a 
	/// System.Windows.Forms.TextBox using the TextDisplay interface.
	/// </summary>
	public class TextBoxDisplay : System.Windows.Forms.RichTextBox, IViewControl
	{
        static readonly Font DefaultFixedFont = new Font(FontFamily.GenericMonospace, 8.0F);

        static readonly Color WarningColor = Color.Yellow;
        static readonly Color ErrorColor = Color.Red;
        static readonly Color LabelColor = Color.Green;
        static readonly Color OutputColor = Color.Black;

        private MenuItem copyMenuItem;
		private MenuItem selectAllMenuItem;
		private MenuItem wordWrapMenuItem;
		private MenuItem fontMenuItem;
		private MenuItem increaseFontMenuItem;
		private MenuItem decreaseFontMenuItem;
		private MenuItem restoreFontMenuItem;

        private UserSettings _settings;

        private string _labels;
        private bool _displayBeforeOutput = true;
        private bool _displayBeforeTest = false;
        private bool _displayAfterTest = false;

        private string _currentLabel = null;
        private string _lastTestOutput = null;

        #region Constructor

        public TextBoxDisplay()
		{
			this.Multiline = true;
			this.ReadOnly = true;
			this.WordWrap = false;

			this.ContextMenu = new ContextMenu();
			this.copyMenuItem = new MenuItem( "&Copy", new EventHandler( copyMenuItem_Click ) );
			this.selectAllMenuItem = new MenuItem( "Select &All", new EventHandler( selectAllMenuItem_Click ) );
			this.wordWrapMenuItem = new MenuItem( "&Word Wrap", new EventHandler( wordWrapMenuItem_Click ) );
			this.fontMenuItem = new MenuItem( "Font" );
			this.increaseFontMenuItem = new MenuItem( "Increase", new EventHandler( increaseFontMenuItem_Click ) );
			this.decreaseFontMenuItem = new MenuItem( "Decrease", new EventHandler( decreaseFontMenuItem_Click ) );
			this.restoreFontMenuItem = new MenuItem( "Restore", new EventHandler( restoreFontMenuItem_Click ) );
			this.fontMenuItem.MenuItems.AddRange( new MenuItem[] { increaseFontMenuItem, decreaseFontMenuItem, new MenuItem("-"), restoreFontMenuItem } );
			this.ContextMenu.MenuItems.AddRange( new MenuItem[] { copyMenuItem, selectAllMenuItem, wordWrapMenuItem, fontMenuItem } );
			this.ContextMenu.Popup += new EventHandler(ContextMenu_Popup);
		}

        #endregion

        #region Event Handlers

        private void copyMenuItem_Click(object sender, EventArgs e)
		{
			this.Copy();
		}

		private void selectAllMenuItem_Click(object sender, EventArgs e)
		{
			this.SelectAll();
		}

		private void wordWrapMenuItem_Click(object sender, EventArgs e)
		{
			this.WordWrap = this.wordWrapMenuItem.Checked = !this.wordWrapMenuItem.Checked;
		}

		private void increaseFontMenuItem_Click(object sender, EventArgs e)
		{
			ApplyFont( new Font( this.Font.FontFamily, this.Font.SizeInPoints * 1.2f, this.Font.Style ) );
		}

		private void decreaseFontMenuItem_Click(object sender, EventArgs e)
		{
			ApplyFont( new Font( this.Font.FontFamily, this.Font.SizeInPoints / 1.2f, this.Font.Style ) );
		}

		private void restoreFontMenuItem_Click(object sender, EventArgs e)
		{
			ApplyFont( new Font( FontFamily.GenericMonospace, 8.0f ) );
		}

		private void ContextMenu_Popup(object sender, EventArgs e)
		{
			this.copyMenuItem.Enabled = this.SelectedText != "";
			this.selectAllMenuItem.Enabled = this.TextLength > 0;
		}

		protected override void OnFontChanged(EventArgs e)
		{
			// Do nothing - this control uses it's own font
		}

        #endregion

        #region TextDisplay Members

        public void Write( string text, bool bold = false )
		{
            if (bold)
            {
                SelectionStart = TextLength;
                SelectionLength = 0;
                SelectionFont = new Font(Font, FontStyle.Bold);
            }

			AppendText( text );

            if (bold)
                SelectionFont = Font;
		}

        public void Write(string text, Color color)
        {
            SelectionStart = TextLength;
            SelectionLength = 0;
            SelectionColor = color;

            AppendText(text);

            SelectionColor = ForeColor;
        }

        public void WriteLine( string text, bool bold = false )
		{
			Write( text + Environment.NewLine, bold );
		}

        public void WriteLine( string text, Color color)
        {
            Write(text + Environment.NewLine, color);
        }

        #endregion

        #region IViewControl Implementation

        public void InitializeView(ITestModel model, TestCentricPresenter presenter)
        {
            _settings = model.Services.UserSettings;
            WordWrap = _settings.Gui.ResultTabs.TextOutput.WordWrapEnabled;
            Font = _settings.Gui.FixedFont;

            model.Events.RunStarting += (RunStartingEventArgs e) =>
            {
                _labels = _settings.Gui.ResultTabs.TextOutput.Labels;
                _displayBeforeTest = _labels == "ALL" || _labels == "BEFORE";
                _displayAfterTest = _labels == "AFTER";
                _displayBeforeOutput = _displayBeforeTest || _displayAfterTest || _labels == "ON";

                _currentLabel = _lastTestOutput = null;
                _wantNewLine = false;
            };

            model.Events.TestStarting += (TestNodeEventArgs e) =>
            {
                if (_displayBeforeTest)
                    WriteLabelLine(e.Test.FullName);
            };

            model.Events.TestFinished += (TestResultEventArgs e) =>
            {
                if (e.Result.Output != null)
                {
                    if (_displayBeforeOutput)
                        WriteLabelLine(e.Result.FullName);

                    WriteOutputLine(e.Result.Name, e.Result.Output, OutputColor);
                }
            };

            model.Events.SuiteFinished += (TestResultEventArgs e) =>
            {
                if (e.Result.Output != null)
                {
                    if (_displayBeforeOutput)
                        WriteLabelLine(e.Result.FullName);

                    FlushNewLineIfNeeded();
                    WriteOutputLine(e.Result.Name, e.Result.Output, OutputColor);
                }
            };

            model.Events.TestOutput += (TestOutputEventArgs e) =>
            {
                if (_displayBeforeOutput && e.TestName != null)
                    WriteLabelLine(e.TestName);

                WriteOutputLine(e.TestName, e.Text, 
                    e.Stream == "Error" ? ErrorColor : OutputColor);
            };
        }

        #endregion

        #region Helper Methods

        private void ApplyFont(Font font)
        {
            _settings.Gui.FixedFont = Font = font;
        }

        private void WriteLabelLine(string label)
        {
            if (label != _currentLabel)
            {
                FlushNewLineIfNeeded();
                _lastTestOutput = label;

                WriteLine($"=> {label}", LabelColor);

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

            Write(text, color);

            // If the text we just wrote did not have a new line, flag that we should eventually emit one.
            if (!text.EndsWith("\n"))
            {
                _wantNewLine = true;
            }
        }

        private bool _wantNewLine;

        private void FlushNewLineIfNeeded()
        {
            if (_wantNewLine)
            {
                Write(Environment.NewLine);
                _wantNewLine = false;
            }
        }

        #endregion
    }
}
