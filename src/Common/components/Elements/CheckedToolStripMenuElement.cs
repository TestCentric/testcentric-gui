// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// MenuElement is the implemented here using a ToolStripItem 
    /// but the view exposes each element using one of the three 
    /// key interfaces (IMenu, ICommand or IChecked) which should
    /// not contain any control-specific logic.
    /// </summary>
    public class CheckedToolStripMenuElement : ToolStripElement, IChecked
    {
        public event CommandHandler CheckedChanged;

        private ToolStripMenuItem _menuItem;

        public CheckedToolStripMenuElement(ToolStripMenuItem menuItem)
            : base(menuItem)
        {
            _menuItem = menuItem;

            menuItem.CheckOnClick = true;
            menuItem.CheckedChanged += (s, e) => CheckedChanged?.Invoke();
        }

        public CheckedToolStripMenuElement(string text) : this(new ToolStripMenuItem(text)) { }

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
    }
}
