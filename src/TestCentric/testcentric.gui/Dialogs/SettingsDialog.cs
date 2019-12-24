// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TestCentric.Gui
{
    using Model;
    using Presenters;
    using SettingsPages;

    /// <summary>
    /// Static class used to display the tree-based SettingsDialog.
    /// </summary>
    public static class SettingsDialog
    {
        public static void Display(TestCentricPresenter presenter, ITestModel model)
        {
            TreeBasedSettingsDialog.Display(presenter, model,
            new GuiSettingsPage("Gui.General"),
            new TreeSettingsPage("Gui.Tree Display"),
            new AssemblyReloadSettingsPage("Gui.Assembly Reload"),
            new TextOutputSettingsPage("Gui.Text Output"),
            new AdvancedLoaderSettingsPage("Engine.Advanced"));
        }
    }
}
