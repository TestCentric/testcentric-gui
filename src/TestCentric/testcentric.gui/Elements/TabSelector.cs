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
    /// A TabSelector represents which tab of a particular TabControl in
	/// the view is selected.
    /// </summary>
	public class TabSelector : ControlElement<TabControl>, ISelection
    {
		public event CommandHandler SelectionChanged;

		public TabSelector(TabControl tabControl) : base(tabControl)
        {
			tabControl.SelectedIndexChanged += (s, e) =>
			{
				int index = tabControl.SelectedIndex;
				if (index >= 0 && index < tabControl.TabCount)
					SelectionChanged?.Invoke();			
			};
        }

        public int SelectedIndex
		{
			get { return Control.SelectedIndex; }
			set { Control.SelectedIndex = value; }
		}

        public string SelectedItem
		{
			get { return Control.SelectedTab.Text; }
			set
			{
				foreach (TabPage tab in Control.TabPages)
					if (tab.Text == value)
						Control.SelectedTab = tab;
			}
		}

        public void Refresh()
		{
			Control.Refresh();
		}
    }
}
