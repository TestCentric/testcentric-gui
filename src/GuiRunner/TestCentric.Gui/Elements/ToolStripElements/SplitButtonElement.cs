// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// SplitButtonElement extends ToolStripElement for use with a SplitButton.
    /// </summary>
    [Obsolete("No longer used", true)]
    public class SplitButtonElement : ToolStripElement, ICommand
    {
        public SplitButtonElement(ToolStripSplitButton button) : base(button)
        {
            button.ButtonClick += delegate { if (Execute != null) Execute(); };
        }

        public event CommandHandler Execute;
    }
}
