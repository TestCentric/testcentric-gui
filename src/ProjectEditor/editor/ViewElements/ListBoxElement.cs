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
using NUnit.ProjectEditor.ViewElements;

namespace NUnit.ProjectEditor
{
    public class ListBoxElement : ControlElement, ISelectionList
    {
        private ListBox listBox;

        public ListBoxElement(ListBox listBox)
            : base(listBox)
        {
            this.listBox = listBox;

            listBox.SelectedIndexChanged += delegate
            {
                if (SelectionChanged != null)
                    SelectionChanged();
            };
        }

        public int SelectedIndex
        {
            get { return listBox.SelectedIndex; }
            set { listBox.SelectedIndex = value; }
        }

        public string SelectedItem
        {
            get { return (string)listBox.SelectedItem; }
            set { listBox.SelectedItem = value; }
        }

        public string[] SelectionList
        {
            get
            {
                string[] list = new string[listBox.Items.Count];
                int index = 0;
                foreach (string item in listBox.Items)
                    list[index++] = item;

                return list;
            }
            set
            {
                listBox.Items.Clear();
                foreach (string item in value)
                    listBox.Items.Add(item);
            }
        }

        public event ActionDelegate SelectionChanged;
    }
}
