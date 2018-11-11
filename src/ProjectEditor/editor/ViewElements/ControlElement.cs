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

using System.Windows.Forms;

namespace NUnit.ProjectEditor.ViewElements
{
    /// <summary>
    /// ControlWrapper is a general wrapper for controls used
    /// by the view. It implements several different interfaces
    /// so that the view may choose which one to expose, based
    /// on the type of textBox and how it is used.
    /// </summary>
    public class ControlElement : IViewElement
    {
        private Control control;

        public ControlElement(Control control)
        {
            this.control = control;
        }

        public string Name
        {
            get { return control.Name; }
        }

        public bool Enabled
        {
            get { return control.Enabled; }
            set { control.Enabled = value; }
        }

        public bool Visible
        {
            get { return control.Visible; }
            set { control.Visible = value; }
        }

        public string Text
        {
            get { return control.Text; }
            set { control.Text = value; }
        }
    }
}
