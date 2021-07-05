// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Drawing;
using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// IControlElement is implemented by elements that wrap controls.
    /// </summary>
    public interface IControlElement : IViewElement
    {
        Point Location { get; set; }
        Size Size { get; set; }
        Size ClientSize { get; set; }

        ContextMenu ContextMenu { get; }
        ContextMenuStrip ContextMenuStrip { get; }
    }
}
