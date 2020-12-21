// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
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
    }
}
