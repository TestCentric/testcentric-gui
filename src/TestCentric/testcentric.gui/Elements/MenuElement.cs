// ***********************************************************************
// Copyright (c) 2015-2018 Charlie Poole
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
    /// MenuElement is the implementation of ToolStripItem 
    /// used in the actual application.
    /// </summary>
    public class MenuElement : IMenu
    {
        public event CommandHandler Execute;
        public event CommandHandler Popup;
        //public event CommandHandler CheckedChanged;

        private MenuItem _menuItem;
        private Form _form;

        public MenuElement(MenuItem menuItem)
        {
            _menuItem = menuItem;
            _form = menuItem.GetMainMenu().GetForm();

            menuItem.Click += delegate { if (Execute != null) Execute(); };

            menuItem.Popup += delegate { if (Popup != null) Popup(); };
            //menuItem.CheckedChanged += delegate { if (CheckedChanged != null) CheckedChanged(); };
        }

        public MenuElement(string text) : this(new MenuItem(text)) { }

        public MenuElement(string text, CommandHandler execute) : this(text)
        {
            this.Execute = execute;
        }

        public string Name
        {
            get { return _menuItem.Name; }
        }

        public bool Checked
        {
            get { return _menuItem.Checked; }
            set
            {
                if (_menuItem.Checked != value)
                {
                    InvokeIfRequired(() =>
                    {
                        _menuItem.Checked = value;
                    });
                }
            }
        }

        public bool Enabled
        {
            get { return _menuItem.Enabled; }
            set
            {
                InvokeIfRequired(() =>
                {
                    _menuItem.Enabled = value;
                });
            }
        }

        public string Text
        {
            get { return _menuItem.Text; }
            set
            {
                InvokeIfRequired(() =>
                {
                    _menuItem.Text = value;
                });
            }
        }

        public bool Visible
        {
            get { return _menuItem.Visible; }
            set
            {
                InvokeIfRequired(() =>
                {
                    _menuItem.Visible = value;
                });
            }
        }

        public Menu.MenuItemCollection MenuItems
        {
            get { return _menuItem.MenuItems; }
        }

        private void InvokeIfRequired(MethodInvoker del)
        {
            if (_form.InvokeRequired)
                _form.BeginInvoke(del, new object[0]);
            else
                del();
        }
    }
}
