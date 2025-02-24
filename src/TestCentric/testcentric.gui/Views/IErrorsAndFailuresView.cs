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

        ITestResultSubView TestResultSubView { get; }
        ITestOutputSubView TestOutputSubView { get; }

        void Clear();
        void AddResult(string status, string testName, string message, string stackTrace);
        void SetFixedFont(Font font);
    }
}
