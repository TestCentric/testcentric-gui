// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Drawing;
using TestCentric.Gui.Elements;

namespace TestCentric.Gui.Views
{
    public interface ITextOutputView
    {
        bool WordWrap { get; set; }
        ISelection Labels { get; set; }
        //IChecked LabelsOn { get; }
        //IChecked LabelsOff { get; }
        //IChecked LabelsBefore { get; }
        //IChecked LabelsAfter { get; }
        //IChecked LabelsBeforeAndAfter { get; }
        void Clear();
        void Write(string text);
        void Write(string text, Color color);
    }
}
