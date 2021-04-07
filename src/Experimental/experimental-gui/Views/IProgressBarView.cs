// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Views
{
    using Controls;

    public interface IProgressBarView
    {
        int Progress { get; set; }
        ProgressBarStatus Status { get; set; }

        void Initialize(int max);
    }
}
