// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using NUnit.ProjectEditor.ViewElements;

namespace NUnit.ProjectEditor
{
    /// <summary>
    /// Common interface implemented by all views used in
    /// the ProjectEditor application
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Object that knows how to display various messages
        /// in a MessageBox.
        /// </summary>
        IMessageDisplay MessageDisplay { get; }

        /// <summary>
        /// Gets or sets the visibility of the view
        /// </summary>
        bool Visible { get; set; }
    }
}
