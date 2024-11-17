// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************


using System;
using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    internal class ToolStripTextBoxElement : ISelection
    {
        private Timer _typingTimer;

        public event CommandHandler SelectionChanged;

        internal ToolStripTextBoxElement(ToolStripTextBox toolStripTextBox) 
        { 
            ToolStripTextBox = toolStripTextBox;
            ToolStripTextBox.TextChanged += OnTextChanged;

            ToolStripTextBox.LostFocus += OnTextBoxLostFocus;
            ToolStripTextBox.GotFocus += OnTextBoxGotFocus;

            OnTextBoxLostFocus(null, EventArgs.Empty);
        }

        public string SelectedItem
        {
            get
            {
                return ToolStripTextBox.Text;
            }

            set
            {
                ToolStripTextBox.Text = value;
            }
        }

        public int  SelectedIndex
        {
            get => 0;
            set => throw new NotImplementedException();
        }

        public bool Enabled 
        {
            get => ToolStripTextBox.Enabled;
            set => ToolStripTextBox.Enabled = value;
        }

        public bool Visible
        {
            get => ToolStripTextBox.Visible;
            set => ToolStripTextBox.Visible = value;
        }

        public string Text
        {
            get => ToolStripTextBox.Text;
            set => ToolStripTextBox.Text = value;
        }

        private ToolStripTextBox ToolStripTextBox { get; }

        private bool PlaceHolderTextUsed { get; set; }

        public void InvokeIfRequired(MethodInvoker _delegate)
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        private void OnTextBoxGotFocus(object sender, EventArgs e)
        {
            if (PlaceHolderTextUsed)
            {
                ToolStripTextBox.Text = "";
                ToolStripTextBox.ForeColor = System.Drawing.Color.Black;
                PlaceHolderTextUsed = false;
            }
        }

        private void OnTextBoxLostFocus(object sender, EventArgs e)
        {
            string searchText = ToolStripTextBox.Text;
            if (string.IsNullOrEmpty(searchText))
            {
                PlaceHolderTextUsed = true;
                ToolStripTextBox.Text = "Search...";
                ToolStripTextBox.ForeColor = System.Drawing.Color.LightGray;
            }
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            if (PlaceHolderTextUsed)
                return;

            if (_typingTimer == null)
            {
                _typingTimer = new Timer();
                _typingTimer.Interval = 600;
                _typingTimer.Tick += TypingTimerTimeout;
            }

            _typingTimer.Stop();
            _typingTimer.Tag = (sender as ToolStripTextBoxControl).Text;
            _typingTimer.Start();
        }

        private void TypingTimerTimeout(object sender, EventArgs e)
        {
            var timer = sender as Timer;
            if (timer == null)
                return;

            // The timer must be stopped!
            timer.Stop();
            if (SelectionChanged != null)
                SelectionChanged();

            ToolStripTextBox.Focus();
        }
    }
}
