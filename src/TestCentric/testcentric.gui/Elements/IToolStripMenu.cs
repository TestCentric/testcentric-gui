// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// IMenu is implemented by a menu item that displays subitems.
    /// </summary>
    public interface IToolStripMenu : IViewElement
    {
        ToolStripItemCollection MenuItems { get; }
    }
}
