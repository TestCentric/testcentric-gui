// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TestCentric.Gui
{
    public interface IMessageDisplay
    {
        void Error(string message);
        void Error(string message, Exception exception);

        void Info(string message);

        bool Ask(string message);
    }
}
