// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// ListBoxElement wraps a ListBox that contains string items
    /// or items that implement ToString() in a useful way.
    /// </summary>
    public class ListBoxElement : ControlElement, IListBox
    {
        private ListBox _listBox;

        public ListBoxElement(ListBox listBox) : base(listBox)
        {
            _listBox = listBox;
            listBox.DoubleClick += (s, e) => DoubleClick?.Invoke();
        }

        #region IListBox Implementation

        public ListBox.ObjectCollection Items => _listBox.Items;

        public ListBox.SelectedObjectCollection SelectedItems => _listBox.SelectedItems;

        public event CommandHandler DoubleClick;

        public void Add(string item)
        {
            InvokeIfRequired(() => _listBox.Items.Add(item));
        }

        public void Remove(string item)
        {
            InvokeIfRequired(() => _listBox.Items.Remove(item));
        }

        #endregion
    }
}
