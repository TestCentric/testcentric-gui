// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// MenuElement is the implementation of ToolStripItem 
    /// used in the actual application.
    /// </summary>
    public abstract class AbstractMenuElement
    {
        protected MenuItem _menuItem;
        private Form _form;

        public AbstractMenuElement(MenuItem menuItem)
        {
            _menuItem = menuItem;
            _form = menuItem.GetMainMenu().GetForm();
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

        protected void InvokeIfRequired(MethodInvoker del)
        {
            if (_form.InvokeRequired)
                _form.BeginInvoke(del, new object[0]);
            else
                del();
        }
    }

    public class MenuCommand : AbstractMenuElement, ICommand
    {
        public event CommandHandler Execute;

        public MenuCommand(MenuItem menuItem) : base(menuItem)
        {
            menuItem.Click += (s, e) => Execute?.Invoke();
        }
    }

    public class PopupMenu : AbstractMenuElement, IMenu
    {
        public event CommandHandler Popup;

        public PopupMenu(MenuItem menuItem) : base(menuItem)
        {
            menuItem.Popup += (s, e) => Popup?.Invoke();
        }

        public Menu.MenuItemCollection MenuItems
        {
            get { return _menuItem.MenuItems; }
        }
    }

    public class CheckedMenuItem : AbstractMenuElement, IChecked
    {
        public event CommandHandler CheckedChanged;

        public CheckedMenuItem(MenuItem menuItem) : base(menuItem)
        {
            menuItem.Click += (s, e) =>
            {
                menuItem.Checked = !menuItem.Checked;
                CheckedChanged?.Invoke();
            };
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
    }
}
