// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;

namespace TestCentric.Gui.Views
{
    using Controls;
    using Elements;

    public interface IMainView
    {
        // View Parameters
        Point Location { get; set; }
        Size Size { get; set; }
        bool Maximized { get; set; }
        Font Font { get; set; }
        int SplitterPosition { get; set; }

        // UI Elements
        ICommand RunButton { get; }
        ICommand StopButton { get; }
        ICommand ForceStopButton { get; }
        IControlElement RunSummary { get; }
        ISelection ResultTabs { get; }

        // File Menu Items
        IToolStripMenu FileMenu { get; }
        ICommand OpenCommand { get; }
        ICommand CloseCommand { get; }
        ICommand AddTestFilesCommand { get; }
        ICommand ReloadTestsCommand { get; }
        IToolStripMenu RuntimeMenu { get; }
        IChecked RunAsX86 { get; }
        IToolStripMenu RecentFilesMenu { get; }
        ICommand ExitCommand { get; }

        // View Menu Items
        ISelection DisplayFormat { get; }
        ICommand IncreaseFontCommand { get; }
        ICommand DecreaseFontCommand { get; }
        ICommand ChangeFontCommand { get; }
        ICommand RestoreFontCommand { get; }
        ICommand IncreaseFixedFontCommand { get; }
        ICommand DecreaseFixedFontCommand { get; }
        ICommand RestoreFixedFontCommand { get; }
        IChecked StatusBarCommand { get; }

        // Test Menu Items
        ICommand RunAllCommand { get; }
        ICommand RunSelectedCommand { get; }
        ICommand RunFailedCommand { get; }
        ICommand StopRunCommand { get; }
        ICommand ForceStopCommand { get; }
        ICommand TestParametersCommand { get; }

        // Tools Menu Items
        IToolStripMenu ToolsMenu { get; }
        ICommand SaveResultsCommand { get; }
        IToolStripMenu SaveResultsAsMenu { get; }
        ICommand OpenWorkDirectoryCommand { get; }
        ICommand ExtensionsCommand { get; }
        ICommand SettingsCommand { get; }

        // Help Menu Items
        ICommand TestCentricHelpCommand { get; }
        ICommand NUnitHelpCommand { get; }
        ICommand AboutCommand { get; }

        // SubViews
        TestTreeView TreeView { get; }
        StatusBarView StatusBarView { get; }
        IMessageDisplay MessageDisplay { get; }
        ILongRunningOperationDisplay LongRunningOperation { get; }

        // Dialog Manager
        IDialogManager DialogManager { get; }

        // Methods used by Presenter
        void Configure(bool useFullGui);
        void SetTitleBar(string fileName);

        // Form methods that we have to use
        void Activate();
        void Close();

        // Form Events that we use
        event EventHandler Load;
        event EventHandler Shown;
        event EventHandler Move;
        event EventHandler Resize;
        event FormClosingEventHandler FormClosing;

        // Our own events
        event EventHandler SplitterPositionChanged;
    }
}
