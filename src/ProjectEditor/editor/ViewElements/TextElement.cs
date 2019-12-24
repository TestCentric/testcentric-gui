// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Windows.Forms;

namespace NUnit.ProjectEditor.ViewElements
{
    public class TextElement : ControlElement, ITextElement
    {
        private TextBoxBase textBox;

        public TextElement(Label label) : base(label) { }

        public TextElement(TextBoxBase textBox) : base(textBox)
        {
            this.textBox = textBox;

            textBox.TextChanged += delegate
            {
                if (Changed != null)
                    Changed();
            };

            textBox.Validated += delegate
            {
                if (Validated != null)
                    Validated();
            };
        }

        public void Select(int offset, int length)
        {
            if (textBox == null)
                throw new InvalidOperationException("Cannot select text in a label");

            textBox.Select(offset, length);
        }

        public event ActionDelegate Changed;

        public event ActionDelegate Validated;
    }
}
