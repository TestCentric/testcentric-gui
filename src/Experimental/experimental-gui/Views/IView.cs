// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TestCentric.Gui.Views
{
    public interface IView
    {
        event EventHandler Load;

        void SuspendLayout();
        void ResumeLayout();
    }
}
