// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;

namespace TestCentric.Gui
{
    /// <summary>
    /// Interface implemented by objects, which know how to display a message
    /// </summary>
    public interface IMessageDisplay
    {
        void Error(string message);

        void Info(string message);

        bool YesNo(string message);

        MessageBoxResult YesNoCancel(string message);

        bool OkCancel(string message);
    }

    /// <summary>
    /// Enum representing the return value of a MessageBox
    /// It contains the identical values in same order as the DialogResult enum from Windows Forms
    /// It has the same intention as the interface <see cref="IMessageDisplay"/> to hide any implementation details to the caller.
    /// </summary>
    public enum MessageBoxResult
    { 
        None,
        OK,
        Cancel,
        Abort,
        Retry,
        Ignore,
        Yes,
        No,
    }
}
