// ***********************************************************************
// Copyright (c) 2018 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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
