// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Views
{
    // Interface is used by presenter and tests
    public interface IStatusBarView
    {
        void Initialize(int count);

        void DisplayText(string text);
        void DisplayTestsRun(int count);
        void DisplayPassed(int count);
        void DisplayFailed(int count);
        void DisplayWarnings(int count);
        void DisplayInconclusive(int count);
        void DisplayDuration(double time);
    }
}
