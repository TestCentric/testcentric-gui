// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// A TabSelector represents which tab of a particular TabControl in
    /// the view is selected.
    /// </summary>
    public class TabSelector : ControlElement, ISelection
    {
        public event CommandHandler SelectionChanged;

        private TabControl _tabControl;

        public TabSelector(TabControl tabControl) : base(tabControl)
        {
            _tabControl = tabControl;

            tabControl.SelectedIndexChanged += (s, e) =>
            {
                int index = tabControl.SelectedIndex;
                if (index >= 0 && index < tabControl.TabCount)
                    SelectionChanged?.Invoke();
            };
        }

        public int SelectedIndex
        {
            get { return _tabControl.SelectedIndex; }
            set { _tabControl.SelectedIndex = value; }
        }

        public string SelectedItem
        {
            get { return _tabControl.SelectedTab.Text; }
            set
            {
                foreach (TabPage tab in _tabControl.TabPages)
                    if (tab.Text == value)
                        _tabControl.SelectedTab = tab;
            }
        }

        public void Refresh()
        {
            _tabControl.Refresh();
        }
    }
}
