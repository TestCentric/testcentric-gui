// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************


using System;
using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// This class implements the IChanged interface for a ToolStripTextBox control. It provides this additional functionality:
    /// - show a PlaceHoder text if there's no text input
    /// - Invoke the Changed event as soon as no further input is made within a short period of time.
    /// </summary>
    public class ToolStripTextBoxElement : ToolStripElement, IChanged
    {
        private Timer _typingTimer;

        public event CommandHandler Changed;

        public ToolStripTextBoxElement(ToolStripTextBox textBox, string placeHolderText)
            : base(textBox)
        {
            TextBox = textBox;
            PlaceHolderText = placeHolderText;
            TextBox.KeyUp += OnKeyUp;

            TextBox.LostFocus += OnTextBoxLostFocus;
            TextBox.GotFocus += OnTextBoxGotFocus;

            // Call LostFocus to set initial text and color
            OnTextBoxLostFocus(null, EventArgs.Empty);
        }

        private string PlaceHolderText { get; set; }

        private ToolStripTextBox TextBox { get; }

        private bool IsPlaceHolderTextShown { get; set; }

        private void OnTextBoxGotFocus(object sender, EventArgs e)
        {
            // If the PlaceHolderText is shown, replace it with an empty text
            if (IsPlaceHolderTextShown)
            {
                TextBox.Text = "";
                TextBox.ForeColor = System.Drawing.Color.Black;
                IsPlaceHolderTextShown = false;
            }
        }

        private void OnTextBoxLostFocus(object sender, EventArgs e)
        {
            // If there's no text input, show the PlaceHolderText instead
            string searchText = TextBox.Text;
            if (string.IsNullOrEmpty(searchText) && !string.IsNullOrEmpty(PlaceHolderText))
            {
                IsPlaceHolderTextShown = true;
                TextBox.Text = PlaceHolderText;
                TextBox.ForeColor = System.Drawing.Color.LightGray;
            }
        }

        /// <summary>
        /// Use KeyUp event instead of TextChanged event
        /// If the Text property is set from client programmatically, a TextChanged event would be triggered => that's not intended
        /// </summary>
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (IsPlaceHolderTextShown)
                return;

            if (_typingTimer == null)
            {
                _typingTimer = new Timer();
                _typingTimer.Interval = 600;
                _typingTimer.Tick += TypingTimerTimeout;
            }

            _typingTimer.Stop();
            _typingTimer.Start();
        }

        private void TypingTimerTimeout(object sender, EventArgs e)
        {
            var timer = sender as Timer;
            if (timer == null)
                return;

            // The timer must be stopped!
            timer.Stop();
            if (Changed != null)
                Changed();

            TextBox.Focus();
        }
    }
}
