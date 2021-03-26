// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Drawing;

namespace TestCentric.Gui.Views
{
    using Elements;

    /// <summary>
    /// Handler when the main view has files dragged and dropped
    /// </summary>
    /// <param name="filesNames">list of file names of files dropped. Never null, potentially empty</param>
    public delegate void DragEventHandler(string[] filesNames);

    /// <summary>
    /// Event when a close event is called by main view
    /// </summary>
    public interface IMainView : IView
    {
        // General Window info
        Point Location { get; set; }
        Size Size { get; set; }
        bool IsMaximized { get; set; }
        Font Font { get; set; }
        void Close();

        event CommandHandler MainViewClosing;
        event DragEventHandler DragDropFiles;

        // File Menu
        IPopup FileMenu { get; }
        ICommand NewProjectCommand { get; }
        ICommand OpenProjectCommand { get; }
        ICommand CloseCommand { get; }
        ICommand AddTestFilesCommand { get; }
        ICommand SaveCommand { get; }
        ICommand SaveAsCommand { get; }
        ICommand SaveResultsCommand { get; }
        ICommand ReloadTestsCommand { get; }
        IPopup SelectRuntimeMenu { get; }
        IChecked RunAsX86 { get; }
        IPopup RecentProjectsMenu { get; }
        ICommand ExitCommand { get; }

        // View Menu
        // Full Gui
        // Mini Gui
        ICommand IncreaseFontCommand { get; }
        ICommand DecreaseFontCommand { get; }
        ICommand ChangeFontCommand { get; }
        ICommand RestoreFontCommand { get; }
        // Fixed Font
        // Status Bar

        // Project Menu
        IToolStripMenu ProjectMenu { get; }
        // Configurations
        // Insert Assembly
        // Insert Project
        // Edit

        // Tools Menu
        // Test Assemblies
        // Exception Details
        // Open Log Directory
        ICommand SettingsCommand { get; }
        ICommand ExtensionsCommand { get; }

        // Help Menu
        ICommand NUnitHelpCommand { get; }
        ICommand AboutNUnitCommand { get; }

        // Tabs
        // Test
        IViewElement TestResult { get; }
        IViewElement TestName { get; }

        // Result

        // Output

        // Dialogs
        IDialogManager DialogManager { get; }

        // Messages
        IMessageDisplay MessageDisplay { get; }
        ILongRunningOperationDisplay LongRunningOperation { get; }
    }
}
