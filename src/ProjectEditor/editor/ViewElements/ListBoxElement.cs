// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

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
