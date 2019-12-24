// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
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

        bool EnableToolTips { get; set; }
        Font Font { get; set; }
        int SplitterPosition { get; set; }
        float SourceCodeSplitterDistance { get; set; }
        Orientation SourceCodeSplitOrientation { get; set; }
        bool SourceCodeDisplay { get; set; }

        void Clear();
        void AddResult(string testName, string message, string stackTrace);
    }
}
