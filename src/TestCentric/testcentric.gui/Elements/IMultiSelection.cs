// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// The IMultiSelection interface represents a group of UI elements
    /// that allow the user to select a set of items.
    /// </summary>
    public interface IMultiSelection : IViewElement
    {
        /// <summary>
        /// Gets or sets the string values of the currently selected items
        /// </summary>
        IEnumerable<string> SelectedItems { get; set; }

        /// <summary>
        /// Event raised when the selection is changed by the user
        /// </summary>
        event CommandHandler SelectionChanged;
    }
}
