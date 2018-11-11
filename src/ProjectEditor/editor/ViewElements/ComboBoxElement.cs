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
using NUnit.ProjectEditor.ViewElements;

namespace NUnit.ProjectEditor
{
    /// <summary>
    /// ComboBoxElement is used to wrap a ComboBox. If the
    /// text is editable by the user, the view should expose
    /// the element using the IComboBox interface. Otherwise,
    /// the ISelectionInterface provides all the needed
    /// functionality.
    /// </summary>
    public class ComboBoxElement : ControlElement, ISelectionList, IComboBox
    {
        private ComboBox comboBox;

        public ComboBoxElement(ComboBox comboBox)
            : base(comboBox)
        {
            this.comboBox = comboBox;

            comboBox.SelectedIndexChanged += delegate
            {
                if (SelectionChanged != null)
                    SelectionChanged();
            };

            comboBox.Validated += delegate
            {
                if (TextValidated != null)
                    TextValidated();
            };
        }

        /// <summary>
        /// Gets or sets the SelectedIndex property of the associated ComboBox
        /// </summary>
        public int SelectedIndex
        {
            get { return comboBox.SelectedIndex; }
            set { comboBox.SelectedIndex = value; }
        }

        /// <summary>
        /// Gets or sets the SelectedItem property of the associated ComboBox
        /// </summary>
        public string SelectedItem
        {
            get { return (string)comboBox.SelectedItem; }
            set { comboBox.SelectedItem = value; }
        }

        /// <summary>
        /// Gets or sets the list of items displayed in the associated ComboBox
        /// </summary>
        public string[] SelectionList
        {
            get
            {
                string[] list = new string[comboBox.Items.Count];

                int index = 0;
                foreach (string item in comboBox.Items)
                    list[index++] = item;

                return list;
            }
            set
            {
                comboBox.Items.Clear();
                foreach (string item in value)
                    comboBox.Items.Add(item);
            }
        }

        /// <summary>
        /// Event raised when the selection in the associated ComboBox changes
        /// </summary>
        public event ActionDelegate SelectionChanged;

        /// <summary>
        /// Event raised when the Text of the associated ComboBox is validated
        /// </summary>
        public event ActionDelegate TextValidated;
    }
}
