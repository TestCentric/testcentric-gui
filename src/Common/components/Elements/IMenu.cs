// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// IMenu is implemented by a menu item that displays subitems.
    /// </summary>
    public interface IMenu : IViewElement
    {
        /// <summary>
        /// Popup event is raised to signal the presenter
        /// that the menu items under this element are
        /// about to be displayed.
        /// </summary>
        event CommandHandler Popup;

        Menu.MenuItemCollection MenuItems { get; }
    }
}
