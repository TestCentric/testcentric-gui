// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// Interface to handle keyboard commands
    /// </summary>
    public interface IKeyCommand
    {
        /// <summary>
        /// Triggered on key up event
        /// </summary>
        event CommandHandler KeyUp;

        /// <summary>
        /// Triggered on key down event
        /// </summary>
        event CommandHandler KeyDown;
    }
}
