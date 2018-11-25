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

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// ListBoxElement wraps a ListBox that contains string items
    /// or items that implement ToString() in a useful way.
    /// </summary>
    public class ListBoxElement : ControlElement<ListBox>, IListBox
    {
        public ListBoxElement(ListBox listBox) : base(listBox)
        {
            listBox.DoubleClick += (s, e) => DoubleClick?.Invoke();
        }

        #region IListBox Implementation

        public ListBox.ObjectCollection Items => Control.Items;

        public ListBox.SelectedObjectCollection SelectedItems => Control.SelectedItems;

        public event CommandHandler DoubleClick;

        public void Add(string item)
        {
            InvokeIfRequired(() => Control.Items.Add(item));
        }

        public void Remove(string item)
        {
            InvokeIfRequired(() => Control.Items.Remove(item));
        }

        #endregion
    }
}
