// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Drawing;

namespace TestCentric.Gui.Views
{
    public interface ITextOutputView
    {
        bool WordWrap { get; set; }
        void Clear();
        void Write(string text);
        void Write(string text, Color color);
    }
}
