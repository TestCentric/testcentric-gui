// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Views
{
    /// <summary>
    /// Common interface implemented by all modal dialog views used in
    /// the ProjectEditor application
    /// </summary>
    public interface IDialog : IView
    {
        void ShowDialog();
        void Close();
    }
}
