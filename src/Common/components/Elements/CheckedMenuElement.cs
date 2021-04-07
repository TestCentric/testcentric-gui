// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// Implements a MenuElement, which may be  checked or unchecked.
    /// </summary>
    public class CheckedMenuElement : ToolStripMenuElement, IChecked
    {
        public event CommandHandler CheckedChanged;

        public CheckedMenuElement(ToolStripMenuItem menuItem)
            : base(menuItem)
        {
            menuItem.CheckOnClick = true;
            menuItem.CheckedChanged += (s, e) => CheckedChanged?.Invoke();
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
    }
}
