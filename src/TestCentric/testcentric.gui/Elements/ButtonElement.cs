// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// ButtonElement wraps a Button as an ICommand.
    /// </summary>
    public class ButtonElement : ToolStripElement, ICommand
    {
        public event CommandHandler Execute;

        public ButtonElement(ToolStripButton button) : base(button)
        {
            button.Click += (s, e) => Execute?.Invoke();
        }
    }
}
