// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// This class is responsible to handle key events of a control
    /// </summary>
    internal class KeyCommand : IKeyCommand
    {

        public event CommandHandler KeyUp;
        public event CommandHandler KeyDown;

        private IEnumerable<Keys> _upKeys;
        private IEnumerable<Keys> _downKeys;

        public KeyCommand(Control control, IEnumerable<Keys> upkeys, IEnumerable<Keys> downkeys)
        {
            _upKeys = upkeys ?? new List<Keys>();
            _downKeys = downkeys ?? new List<Keys>();

            control.KeyUp += OnKeyUp;
            control.KeyUp += OnKeyDown;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (_upKeys.Contains(e.KeyCode))
                KeyUp?.Invoke();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (_downKeys.Contains(e.KeyCode))
                KeyDown?.Invoke();
        }
    }
}
