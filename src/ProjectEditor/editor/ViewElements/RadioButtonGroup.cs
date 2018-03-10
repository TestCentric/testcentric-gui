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
    public class RadioButtonGroup : ISelection
    {
        private string name;
        private bool enabled;
        private RadioButton[] buttons;

        public RadioButtonGroup(string name, params RadioButton[] buttons)
        {
            this.name = name;
            this.enabled = buttons.Length > 0 ? buttons[0].Enabled : false;
            this.buttons = buttons;

            foreach (RadioButton button in buttons)
                button.CheckedChanged += delegate
                {
                    if (SelectionChanged != null)
                        SelectionChanged();
                };
        }

        public string Name
        {
            get { return name; }
        }

        public bool Enabled
        {
            get 
            {
                return enabled;
            }
            set
            {
                enabled = value;

                foreach (RadioButton button in buttons)
                    button.Enabled = enabled;
            }
        }

        public int SelectedIndex
        {
            get
            {
                for (int index = 0; index < buttons.Length; index++)
                    if (buttons[index].Checked)
                        return index;

                return -1;
            }
            set 
            { 
                if (value >= 0 && value < buttons.Length)
                    buttons[value].Checked = true; 
            }
        }

        public event ActionDelegate SelectionChanged;
    }
}
