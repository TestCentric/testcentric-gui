// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;

namespace TestCentric.Gui.Views
{
    public interface IErrorsAndFailuresView
    {
        event EventHandler SplitterPositionChanged;
        event EventHandler SourceCodeSplitterDistanceChanged;
        event EventHandler SourceCodeSplitOrientationChanged;
        event EventHandler SourceCodeDisplayChanged;

        string Header { get; set; }

        bool EnableToolTips { get; set; }
        int SplitterPosition { get; set; }
        float SourceCodeSplitterDistance { get; set; }
        Orientation SourceCodeSplitOrientation { get; set; }
        bool SourceCodeDisplay { get; set; }

        TestResultSubView TestResultSubView { get; }
        TestOutputSubView TestOutputSubView { get; }

        string Outcome { get; set; }
        string ElapsedTime { get; set; }
        string AssertCount { get; set; }
        string Assertions { get; set; }
        string Output { get; set; }

        void Clear();
        void AddResult(string status, string testName, string message, string stackTrace);
        void SetFixedFont(Font font);
    }
}
