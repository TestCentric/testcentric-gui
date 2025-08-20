// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// ButtonElement wraps a Button as an ICommand.
    /// </summary>
    [Obsolete("No longer used", true)]
    public class ButtonElement : ControlElement, ICommand
    {
        private Button _button;

        public event CommandHandler Execute;
        //public event CommandHandler CheckedChanged;

        //public bool Checked
        //{
        //    get { return _button.Checked; }
        //    set { InvokeIfRequired(() => _button.Checked = value); }
        //}

        public ButtonElement(Button button) : base(button)
        {
            _button = button;

            button.Click += (s, e) => Execute?.Invoke();
        }
    }
}
