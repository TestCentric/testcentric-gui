// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Views
{
    // Interface is used by presenter and tests
    public interface IStatusBarView : IView
    {
        void OnTestLoaded(int testCount);
        void OnTestUnloaded();
        void OnRunStarting(int testCount);
        void OnRunFinished(double elapsedTime);
        void OnTestStarting(string name);
        void OnTestPassed();
        void OnTestFailed();
        void OnTestWarning();
        void OnTestInconclusive();
        void OnTestRunSummaryCompiled(string testRunSummary);
    }
}
