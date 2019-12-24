// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Views
{
    public enum ProgressBarStatus
    {
        Success = 0,
        Warning = 1,
        Failure = 2
    }

    public interface IProgressBarView
    {
        int Progress { get; set; }
        ProgressBarStatus Status { get; set; }

        void Initialize(int max);
    }
}
