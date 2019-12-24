// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace NUnit.ProjectEditor.ViewElements
{
    public interface ICommand : IViewElement
    {
        /// <summary>
        /// Execute event is raised to signal the presenter
        /// to execute the command with which this menu
        /// item is associated.
        /// </summary>
        event CommandDelegate Execute;
    }
}
