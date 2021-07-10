// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// IToolTip is implemented by elements, which are able to
    /// get and set their own tool tip text. It is a single-
    /// capability interface and is generally used in conjunction
    /// with IViewElement or a derived interface.
    /// </summary>
    public interface IToolTip
    {
        string ToolTipText { get; set; }
    }
}
