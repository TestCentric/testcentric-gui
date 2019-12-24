// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace NUnit.ProjectEditor.ViewElements
{
    public interface ISelection : IViewElement
    {
        /// <summary>
        /// Gets or sets the index of the currently selected item
        /// </summary>
        int SelectedIndex { get; set; }

        /// <summary>
        /// Event raised when the selection is changed by the user
        /// </summary>
        event ActionDelegate SelectionChanged;
    }
}
