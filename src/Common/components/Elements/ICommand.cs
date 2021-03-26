// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// CommandHandler is used to request an action
    /// </summary>
    public delegate void CommandHandler();

    /// <summary>
    /// The ICommand interface represents any GUI item, which
    /// executes a command, e.g. a button or a menu item.
    /// </summary>
    public interface ICommand : IViewElement
    {
        /// <summary>
        /// Execute event is raised to signal the presenter
        /// to execute the associated command.
        /// </summary>
        event CommandHandler Execute;
    }
}
