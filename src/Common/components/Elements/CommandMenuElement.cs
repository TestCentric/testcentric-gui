// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// CommandMenuElement represents a menu item, which
    /// invokes a command when clicked.
    /// </summary>
    public class CommandMenuElement : ToolStripMenuElement, ICommand
    {
        public event CommandHandler Execute;

        public CommandMenuElement(ToolStripMenuItem menuItem)
            : base(menuItem)
        {
            menuItem.Click += (s, e) => Execute?.Invoke();
        }
    }
}
