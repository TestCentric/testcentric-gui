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
    /// MenuElement wraps a MenuItem
    /// </summary>
    public abstract class MenuElement
    {
        protected MenuItem _menuItem;
        private Form _form;

        public MenuElement(MenuItem menuItem)
        {
            _menuItem = menuItem;
            _form = menuItem.GetMainMenu()?.GetForm();
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

        public bool DefaultItem
        {
            get { return _menuItem.DefaultItem; }
            set
            {
                InvokeIfRequired(() =>
                {
                    _menuItem.DefaultItem = value;
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

        public void InvokeIfRequired(MethodInvoker del)
        {
            if (_form != null && _form.InvokeRequired)
                _form.BeginInvoke(del, new object[0]);
            else
                del();
        }
    }
}
