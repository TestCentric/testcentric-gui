// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// ToolStripMenuElement is the abstract base for all our menu
    /// elements. It wraps a single ToolStripMenuItem. Use the appropriate
    /// derived class depending on whether the element is a popup
    /// menu, a checked menu item or simply invokes a command.
    /// </summary>
    public abstract class ToolStripMenuElement : ToolStripElement
    {
        protected ToolStripMenuItem _menuItem;

        public ToolStripMenuElement(ToolStripMenuItem menuItem)
            : base(menuItem)
        {
            _menuItem = menuItem;
        }

        public ToolStripMenuElement(string text) : this(new ToolStripMenuItem(text)) { }

        public ToolStripItemCollection MenuItems
        {
            get { return _menuItem.DropDown.Items; }
        }
    }
}
