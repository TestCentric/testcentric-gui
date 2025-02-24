// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Views
{
    public interface ITestResultSubView
    {
        string Outcome { get; set; }
        string ElapsedTime { get; set; }
        string AssertCount { get; set; }
        string Assertions { get; set; }
    }
}
