// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// The IPopup interface represents a menu item, which
    /// displays a list of sub-items when clicked.
    /// </summary>
    public interface IPopup : IToolStripMenu
    {
        /// Popup event is raised to signal the presenter
        /// that the menu items under this element are
        /// about to be displayed.
        /// </summary>
        event CommandHandler Popup;
    }
}
