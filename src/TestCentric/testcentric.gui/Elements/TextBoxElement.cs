// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************


using System;
using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// This class implements the IChanged interface for a TextBox control. It provides this additional functionality:
    /// - show a PlaceHoder text if there's no text input
    /// - Invoke the Changed event as soon as no further input is made within a short period of time.
    /// </summary>
    public class TextBoxElement : IChanged
    {
        private Timer _typingTimer;

        public event CommandHandler Changed;

        public TextBoxElement(Control textBox, string placeHolderText)
        {
            TextBox = textBox;
            PlaceHolderText = placeHolderText;
            TextBox.TextChanged += OnTextChanged;

            TextBox.LostFocus += OnTextBoxLostFocus;
            TextBox.GotFocus += OnTextBoxGotFocus;

            // Call LostFocus to set initial text and color
            OnTextBoxLostFocus(null, EventArgs.Empty);
        }

        public bool Enabled
        {
            get => TextBox.Enabled;
            set => TextBox.Enabled = value;
        }

        public bool Visible
        {
            get => TextBox.Visible;
            set => TextBox.Visible = value;
        }

        public string Text
        {
            get => TextBox.Text;
            set => TextBox.Text = value;
        }

        private string PlaceHolderText { get; set; }

        private Control TextBox { get; }

        private bool IsPlaceHolderTextShown { get; set; }

        public void InvokeIfRequired(MethodInvoker _delegate)
        {
            throw new NotImplementedException();
        }

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

        private void OnTextChanged(object sender, EventArgs e)
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
