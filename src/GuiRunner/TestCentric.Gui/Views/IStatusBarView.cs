// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Views
{
    // Interface is used by presenter and tests
    public interface IStatusBarView
    {
        void Initialize();

        bool Visible { get; set; }
        string Text { get; set; }
        int Passed { set; }
        int Failed { set; }
        int Warnings { set; }
        int Inconclusive { set; }
        int Ignored { set; }
        int Skipped { set; }
        double Duration { set; }

        void LoadImages(OutcomeImageSet imageSet);
    }
}
