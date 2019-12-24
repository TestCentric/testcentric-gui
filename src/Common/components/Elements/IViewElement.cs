// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// The IViewElement interface wraps an individual gui
    /// item like a control or toolstrip item. It is generally
    /// exposed by views and is the base of other interfaces
    /// in the TestCentric.Gui.Elements namespace.
    /// </summary>
    public interface IViewElement
    {
        /// <summary>
        /// Gets or sets the Enabled status of the element
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the Visible status of the element
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the text of an element.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Invoke a delegate if necessary, otherwise just call it
        /// </summary>
        void InvokeIfRequired(MethodInvoker _delegate);
    }
}
