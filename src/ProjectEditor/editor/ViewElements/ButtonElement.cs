// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Windows.Forms;

namespace NUnit.ProjectEditor.ViewElements
{
    /// <summary>
    /// ControlWrapper is a general wrapper for controls used
    /// by the view. It implements several different interfaces
    /// so that the view may choose which one to expose, based
    /// on the type of textBox and how it is used.
    /// </summary>
    public class ButtonElement : ControlElement, ICommand
    {
        public ButtonElement(Button button) : base(button)
        {
            button.Click += delegate
            {
                if (Execute != null)
                    Execute();
            };
        }

        public event CommandDelegate Execute;
    }
}
