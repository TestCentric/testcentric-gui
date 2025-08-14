// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Drawing;
using System.Windows.Forms;
using System;

namespace TestCentric.Gui.Controls
{
    /// <summary>
    /// This class is required to stretch a ToolStripTextBox control within a ToolStrip to fill the available space and to resize when the control resizes.
    /// The implementation is from the Microsoft Windows Forms documentation, but simplified to the current use case.
    /// "How to: Stretch a ToolStripTextBox to Fill the Remaining Width of a ToolStrip"
    /// https://learn.microsoft.com/en-us/dotnet/desktop/winforms/controls/stretch-a-toolstriptextbox-to-fill-the-remaining-width-of-a-toolstrip-wf?view=netframeworkdesktop-4.8
    /// </summary>
    internal class StretchToolStripTextBox : ToolStripTextBox
    {
        public override Size GetPreferredSize(Size constrainingSize)
        {
            // Get width of the owning ToolStrip
            int textBoxMargin = 2;
            Int32 width = Owner.DisplayRectangle.Width - textBoxMargin;

            // If the available width is less than the default width, use the default width
            if (width < DefaultSize.Width) width = DefaultSize.Width;

            // Retrieve the preferred size from the base class, but change the width to the calculated width.
            Size size = base.GetPreferredSize(constrainingSize);
            size.Width = width;
            return size;
        }
    }
}
