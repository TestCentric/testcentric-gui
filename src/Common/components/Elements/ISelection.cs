// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// The ISelection interface represents a single UI element
    /// or a group of elements that allow the user to select one
    /// of a set of items.
    /// </summary>
    public interface ISelection : IViewElement
    {
        /// <summary>
        /// Gets or sets the index of the currently selected item
        /// </summary>
        int SelectedIndex { get; set; }

        /// <summary>
        /// Gets or sets the string value of the currently selected item
        /// </summary>
        string SelectedItem { get; set; }

        /// <summary>
        /// Refresh selection if possible, otherwise noop
        /// </summary>
        void Refresh();

        /// <summary>
        /// Event raised when the selection is changed by the user
        /// </summary>
        event CommandHandler SelectionChanged;
    }
}
