// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace NUnit.ProjectEditor.ViewElements
{
    /// <summary>
    /// The ISelectionList interface represents
    /// a ui element that allows the user to select one of
    /// a set of items.
    /// </summary>
    public interface ISelectionList : ISelection
    {
        /// <summary>
        /// Gets or sets the currently selected item
        /// </summary>
        string SelectedItem { get; set; }

        /// <summary>
        /// Gets or sets the contents of the selection list
        /// </summary>
        string[] SelectionList { get; set; }
    }
}
