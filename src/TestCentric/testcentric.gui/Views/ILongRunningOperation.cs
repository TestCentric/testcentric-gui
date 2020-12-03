// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TestCentric.Gui
{
    public interface ILongRunningOperation
    {
        void Display(string text);
        void Hide();
    }
}
