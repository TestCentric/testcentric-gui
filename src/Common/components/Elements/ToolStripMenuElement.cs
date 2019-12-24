// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// MenuElement is the implemented here using a ToolStripItem 
    /// but the view exposes each element using one of the three 
    /// key interfaces (IMenu, ICommand or IChecked) which should
    /// not contain any control-specific logic.
    /// </summary>
    public class ToolStripMenuElement : ToolStripElement, IToolStripMenu, ICommand, IChecked
    {
        public event CommandHandler Execute;
        public event CommandHandler Popup;
        public event CommandHandler CheckedChanged;

        private ToolStripMenuItem _menuItem;

        public ToolStripMenuElement(ToolStripMenuItem menuItem)
            : base(menuItem)
        {
            _menuItem = menuItem;

            menuItem.Click += (s, e) => Execute?.Invoke();
            menuItem.DropDownOpening += (s, e) => Popup?.Invoke();
            menuItem.CheckedChanged += (s, e) => CheckedChanged?.Invoke();
        }

        public ToolStripMenuElement(string text) : this(new ToolStripMenuItem(text)) { }

        public ToolStripMenuElement(string text, CommandHandler execute) : this(text)
        {
            this.Execute = execute;
        }

        public bool Checked
        {
            get { return _menuItem.Checked; }
            set
            {
                if (_menuItem.Checked != value)
                {
                    InvokeIfRequired(() =>
                    {
                        _menuItem.Checked = value;
                    });
                }
            }
        }

        public ToolStripItemCollection MenuItems
        {
            get { return _menuItem.DropDown.Items; }
        }
    }
}
