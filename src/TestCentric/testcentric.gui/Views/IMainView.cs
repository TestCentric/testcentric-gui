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
        IViewParameter<Font> FontSelector { get; }
        IViewParameter<int> SplitterPosition { get; }

        // UI Elements
        ICommand RunButton { get; }
        ICommand StopButton { get; }
        IControlElement<ExpandingLabel> RunSummary { get; }
        ISelection ResultTabs { get; }

        // File Menu Items
        IMenu FileMenu { get; }
        ICommand OpenCommand { get; }
        ICommand CloseCommand { get; }
        ICommand AddTestFileCommand { get; }
        ICommand ReloadTestsCommand { get; }
        IMenu RuntimeMenu { get; }
        ISelection SelectedRuntime { get; }
        IMenu RecentFilesMenu { get; }
        ICommand ExitCommand { get; }

        // View Menu Items
        ISelection DisplayFormat { get; }
        IMenu TreeMenu { get; }
        IChecked CheckboxesCommand { get; }
        ICommand ExpandCommand { get; }
        ICommand CollapseCommand { get; }
        ICommand ExpandAllCommand { get; }
        ICommand CollapseAllCommand { get; }
        ICommand HideTestsCommand { get; }
        ICommand PropertiesCommand { get; }
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

        // Tools Menu Items
        IMenu ToolsMenu { get; }
        ICommand ProjectEditorCommand { get; }
        ICommand SaveResultsCommand { get; }
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

        // Methods used by Presenter
        void Configure(bool useFullGui);
        LongRunningOperationDisplay LongOperationDisplay(string text);

        // Form methods that we have to use
        void Activate();
        void Close();

        // Form Events that we use
        event EventHandler Load;
        event EventHandler Shown;
        event EventHandler Move;
        event EventHandler Resize;
        event FormClosingEventHandler FormClosing;
    }
}
