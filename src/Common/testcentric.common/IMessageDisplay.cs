// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Windows.Forms;

namespace TestCentric.Gui
{
    public interface IMessageDisplay
    {
        DialogResult Error(string message);
        DialogResult Error(string message, Exception exception);

        DialogResult Info(string message);

        DialogResult Ask(string message);
    }
}
