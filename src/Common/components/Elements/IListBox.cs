// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// The IListBox interface is implemented by an element
    /// representing a ListBox containing string items or
    /// items that implement ToString() in a useful way.
    /// </summary>
    public interface IListBox : IControlElement
    {
        ListBox.ObjectCollection Items { get; }
        ListBox.SelectedObjectCollection SelectedItems { get; }

        event CommandHandler DoubleClick;

        void Add(string item);
        void Remove(string item);
    }
}
