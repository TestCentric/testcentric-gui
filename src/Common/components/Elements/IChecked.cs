// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// The IChecked interface is implemented by either a CheckBox
    /// or a MenuItem, which supports being checked or unchecked.
    /// </summary>
    public interface IChecked : IViewElement
    {
        bool Checked { get; set; }

        event CommandHandler CheckedChanged;
    }
}
