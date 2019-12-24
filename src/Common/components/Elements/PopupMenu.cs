// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    public class PopupMenu : MenuElement, IMenu
    {
        public event CommandHandler Popup;

        public PopupMenu(MenuItem menuItem) : base(menuItem)
        {
            menuItem.Popup += (s, e) => Popup?.Invoke();
        }

        public Menu.MenuItemCollection MenuItems
        {
            get { return _menuItem.MenuItems; }
        }
    }
}
