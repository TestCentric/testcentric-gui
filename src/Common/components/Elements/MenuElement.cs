// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
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
