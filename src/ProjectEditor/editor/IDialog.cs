// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Windows.Forms;

namespace NUnit.ProjectEditor
{
    /// <summary>
    /// Common interface implemented by all modal dialog views used in
    /// the ProjectEditor application
    /// </summary>
    public interface IDialog : IView
    {
        DialogResult ShowDialog();
        void Close();
    }
}
