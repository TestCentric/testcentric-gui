// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    public class CheckedMenuItem : MenuElement, IChecked
    {
        public event CommandHandler CheckedChanged;

        public CheckedMenuItem(MenuItem menuItem) : base(menuItem)
        {
            menuItem.Click += (s, e) =>
            {
                menuItem.Checked = !menuItem.Checked;
                CheckedChanged?.Invoke();
            };
        }

        public bool Checked
        {
            get { return _menuItem.Checked; }
            set
            {
                if (_menuItem.Checked != value)
                    InvokeIfRequired(() => _menuItem.Checked = value);
            }
        }
    }
}
