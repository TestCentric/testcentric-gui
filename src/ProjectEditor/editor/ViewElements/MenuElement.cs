// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Windows.Forms;

namespace NUnit.ProjectEditor.ViewElements
{
    /// <summary>
    /// MenuItemWrapper is the implementation of MenuItem 
    /// used in the actual application.
    /// </summary>
    public class MenuElement : ICommand
    {
        private ToolStripMenuItem menuItem;

        public MenuElement(ToolStripMenuItem menuItem)
        {
            this.menuItem = menuItem;

            menuItem.Click += delegate
                { if (Execute != null) Execute(); };
        }

        public event CommandDelegate Execute;

        public string Name
        {
            get { return menuItem.Name; }
        }

        public bool Enabled
        {
            get { return menuItem.Enabled; }
            set { menuItem.Enabled = value; }
        }

        public string Text
        {
            get { return menuItem.Text; }
            set { menuItem.Text = value; }
        }
    }
}
