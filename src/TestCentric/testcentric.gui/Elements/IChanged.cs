// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// The IChanged interface represents a IViewElement.
    /// If the IViewElement changes, it will raise the Changed event.
    /// </summary>
    public interface IChanged : IViewElement
    {
        /// <summary>
        /// Event raised when the element is changed by the user
        /// </summary>
        event CommandHandler Changed;
    }
}
