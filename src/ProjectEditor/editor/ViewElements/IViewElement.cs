// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace NUnit.ProjectEditor
{
    /// <summary>
    /// The IViewElement interface is exposed by the view
    /// for an individual gui element. It is the base of
    /// other more specific interfaces.
    /// </summary>
    public interface IViewElement
    {
        /// <summary>
        /// Gets the name of the element in the view
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets the enabled status of the element
        /// </summary>
        bool Enabled { get; set; }
    }
}
