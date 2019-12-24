// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    public class MenuCommand : MenuElement, IMenuCommand
    {
        public event CommandHandler Execute;

        public MenuCommand(MenuItem menuItem) : base(menuItem)
        {
            menuItem.Click += (s, e) => Execute?.Invoke();
        }
    }
}
