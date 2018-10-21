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

using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TestCentric.Gui.Views
{
    using Controls;
    using Elements;
    using Model;

    public class TestCentricMainView : TestCentricFormBase, IMainView
    {
        static Logger log = InternalTrace.GetLogger(typeof(TestCentricMainView));

        #region Instance variables

        private System.ComponentModel.IContainer components;

        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.Splitter treeSplitter;
        private System.Windows.Forms.Panel rightPanel;

        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage testPage;
        private System.Windows.Forms.Panel testPanel;
        private TestCentric.Gui.Views.TestTreeView treeView;
        private System.Windows.Forms.TabPage categoryPage;
        private System.Windows.Forms.Panel categoryPanel;
        private TestCentric.Gui.Views.CategoryView categoryView;

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button runButton;
        private System.Windows.Forms.Button stopButton;
        private ProgressBarView progressBar;
        private ExpandingLabel runCount;

        private TabControl resultTabs;

        private StatusBarView statusBar;

        private System.Windows.Forms.ToolTip toolTip;

        private System.Windows.Forms.MainMenu mainMenu;

        private System.Windows.Forms.MenuItem fileMenu;
        private System.Windows.Forms.MenuItem openMenuItem;
        private System.Windows.Forms.MenuItem recentFilesMenu;
        private System.Windows.Forms.MenuItem fileMenuSeparator1;
        private System.Windows.Forms.MenuItem fileMenuSeparator4;
        private System.Windows.Forms.MenuItem closeMenuItem;
        private System.Windows.Forms.MenuItem exitMenuItem;

        private System.Windows.Forms.MenuItem toolsMenu;
        private System.Windows.Forms.MenuItem settingsMenuItem;
        private System.Windows.Forms.MenuItem saveResultsMenuItem;

        private System.Windows.Forms.MenuItem nunitHelpMenuItem;
        private System.Windows.Forms.MenuItem helpItem;
        private System.Windows.Forms.MenuItem helpMenuSeparator1;
        private System.Windows.Forms.MenuItem aboutMenuItem;
        private System.Windows.Forms.MenuItem viewMenu;
        private System.Windows.Forms.MenuItem statusBarMenuItem;
        private System.Windows.Forms.MenuItem miniGuiMenuItem;
        private System.Windows.Forms.MenuItem fullGuiMenuItem;
        private System.Windows.Forms.MenuItem fontChangeMenuItem;
        private System.Windows.Forms.MenuItem defaultFontMenuItem;
        private System.Windows.Forms.MenuItem decreaseFontMenuItem;
        private System.Windows.Forms.MenuItem increaseFontMenuItem;
        private System.Windows.Forms.MenuItem testMenu;
        private System.Windows.Forms.MenuItem runAllMenuItem;
        private System.Windows.Forms.MenuItem runSelectedMenuItem;
        private System.Windows.Forms.MenuItem runFailedMenuItem;
        private System.Windows.Forms.MenuItem stopRunMenuItem;
        private System.Windows.Forms.MenuItem viewMenuSeparator1;
        private System.Windows.Forms.MenuItem viewMenuSeparator2;
        private System.Windows.Forms.MenuItem viewMenuSeparator3;
        private System.Windows.Forms.MenuItem fontMenuSeparator;
        private System.Windows.Forms.MenuItem testMenuSeparator;
        private System.Windows.Forms.MenuItem guiFontMenuItem;
        private System.Windows.Forms.MenuItem fixedFontMenuItem;
        private System.Windows.Forms.MenuItem increaseFixedFontMenuItem;
        private System.Windows.Forms.MenuItem decreaseFixedFontMenuItem;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem restoreFixedFontMenuItem;
        private System.Windows.Forms.MenuItem reloadTestsMenuItem;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem toolsMenuSeparator1;
        private MenuItem runtimeMenuItem;
        private MenuItem addTestFileMenuItem;
        private MenuItem extensionsMenuItem;
        private MenuItem testCentricHelpMenuItem;
        private MenuItem projectEditorMenuItem;
        private TabPage errorTab;
        private TabPage notrunTab;
        private TabPage outputTab;
        private ErrorsAndFailuresView errorsAndFailuresView1;
        private TestsNotRunView testsNotRunView1;
        private MenuItem treeMenuItem;
        private MenuItem showCheckboxesMenuItem;
        private MenuItem menuItem5;
        private MenuItem expandMenuItem;
        private MenuItem collapseMenuItem;
        private MenuItem menuItem8;
        private MenuItem expandAllMenuItem;
        private MenuItem collapseAllMenuItem;
        private MenuItem hideTestsMenuItem;
        private MenuItem menuItem12;
        private MenuItem propertiesMenuItem;
        private MenuItem menuItem4;
        private TextOutputView textOutputView1;

        #endregion

        #region Construction and Disposal

        public TestCentricMainView() : base("TestCentric")
        {
            InitializeComponent();

            // View Parameters
            FontSelector = new FontSelector(this);
            SplitterPosition = new SplitterPosition(treeSplitter);

            // UI Elements on main form
            RunButton = new ButtonElement(runButton);
            StopButton = new ButtonElement(stopButton);
            RunSummary = new ControlElement<ExpandingLabel>(runCount);
            ResultTabs = new TabSelector(resultTabs);

            // Initialize File Menu Commands
            FileMenu = new PopupMenu(fileMenu);
            OpenCommand = new MenuCommand(openMenuItem);
            CloseCommand = new MenuCommand(closeMenuItem);
            AddTestFileCommand = new MenuCommand(addTestFileMenuItem);
            ReloadTestsCommand = new MenuCommand(reloadTestsMenuItem);
            RuntimeMenu = new PopupMenu(runtimeMenuItem);
            SelectedRuntime = new CheckedMenuGroup(runtimeMenuItem);
            RecentFilesMenu = new PopupMenu(recentFilesMenu);
            ExitCommand = new MenuCommand(exitMenuItem);

            // Initialize View Menu Commands
            DisplayFormat = new CheckedMenuGroup(fullGuiMenuItem, miniGuiMenuItem);
            TreeMenu = new PopupMenu(treeMenuItem);
            CheckboxesCommand = new CheckedMenuItem(showCheckboxesMenuItem);
            ExpandCommand = new MenuCommand(expandMenuItem);
            CollapseCommand = new MenuCommand(collapseMenuItem);
            ExpandAllCommand = new MenuCommand(expandAllMenuItem);
            CollapseAllCommand = new MenuCommand(collapseAllMenuItem);
            HideTestsCommand = new MenuCommand(hideTestsMenuItem);
            PropertiesCommand = new MenuCommand(propertiesMenuItem);
            IncreaseFontCommand = new MenuCommand(increaseFontMenuItem);
            DecreaseFontCommand = new MenuCommand(decreaseFontMenuItem);
            ChangeFontCommand = new MenuCommand(fontChangeMenuItem);
            RestoreFontCommand = new MenuCommand(defaultFontMenuItem);
            IncreaseFixedFontCommand = new MenuCommand(increaseFixedFontMenuItem);
            DecreaseFixedFontCommand = new MenuCommand(decreaseFixedFontMenuItem);
            RestoreFixedFontCommand = new MenuCommand(restoreFixedFontMenuItem);
            StatusBarCommand = new CheckedMenuItem(statusBarMenuItem);

            // Initialize Test Menu Commands
            RunAllCommand = new MenuCommand(runAllMenuItem);
            RunSelectedCommand = new MenuCommand(runSelectedMenuItem);
            RunFailedCommand = new MenuCommand(runFailedMenuItem);
            StopRunCommand = new MenuCommand(stopRunMenuItem);

            // Initialize Tools Menu Comands
            ToolsMenu = new PopupMenu(toolsMenu);
            ProjectEditorCommand = new MenuCommand(projectEditorMenuItem);
            SaveResultsCommand = new MenuCommand(saveResultsMenuItem);
            ExtensionsCommand = new MenuCommand(extensionsMenuItem);
            SettingsCommand = new MenuCommand(settingsMenuItem);

            TestCentricHelpCommand = new MenuCommand(testCentricHelpMenuItem);
            NUnitHelpCommand = new MenuCommand(nunitHelpMenuItem);
            AboutCommand = new MenuCommand(aboutMenuItem);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestCentricMainView));
            this.statusBar = new TestCentric.Gui.Views.StatusBarView();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.fileMenu = new System.Windows.Forms.MenuItem();
            this.openMenuItem = new System.Windows.Forms.MenuItem();
            this.closeMenuItem = new System.Windows.Forms.MenuItem();
            this.addTestFileMenuItem = new System.Windows.Forms.MenuItem();
            this.fileMenuSeparator1 = new System.Windows.Forms.MenuItem();
            this.reloadTestsMenuItem = new System.Windows.Forms.MenuItem();
            this.runtimeMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.recentFilesMenu = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.fileMenuSeparator4 = new System.Windows.Forms.MenuItem();
            this.exitMenuItem = new System.Windows.Forms.MenuItem();
            this.viewMenu = new System.Windows.Forms.MenuItem();
            this.fullGuiMenuItem = new System.Windows.Forms.MenuItem();
            this.miniGuiMenuItem = new System.Windows.Forms.MenuItem();
            this.viewMenuSeparator1 = new System.Windows.Forms.MenuItem();
            this.treeMenuItem = new System.Windows.Forms.MenuItem();
            this.showCheckboxesMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.expandMenuItem = new System.Windows.Forms.MenuItem();
            this.collapseMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.expandAllMenuItem = new System.Windows.Forms.MenuItem();
            this.collapseAllMenuItem = new System.Windows.Forms.MenuItem();
            this.hideTestsMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.propertiesMenuItem = new System.Windows.Forms.MenuItem();
            this.viewMenuSeparator2 = new System.Windows.Forms.MenuItem();
            this.guiFontMenuItem = new System.Windows.Forms.MenuItem();
            this.increaseFontMenuItem = new System.Windows.Forms.MenuItem();
            this.decreaseFontMenuItem = new System.Windows.Forms.MenuItem();
            this.fontMenuSeparator = new System.Windows.Forms.MenuItem();
            this.fontChangeMenuItem = new System.Windows.Forms.MenuItem();
            this.defaultFontMenuItem = new System.Windows.Forms.MenuItem();
            this.fixedFontMenuItem = new System.Windows.Forms.MenuItem();
            this.increaseFixedFontMenuItem = new System.Windows.Forms.MenuItem();
            this.decreaseFixedFontMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.restoreFixedFontMenuItem = new System.Windows.Forms.MenuItem();
            this.viewMenuSeparator3 = new System.Windows.Forms.MenuItem();
            this.statusBarMenuItem = new System.Windows.Forms.MenuItem();
            this.testMenu = new System.Windows.Forms.MenuItem();
            this.runAllMenuItem = new System.Windows.Forms.MenuItem();
            this.runSelectedMenuItem = new System.Windows.Forms.MenuItem();
            this.runFailedMenuItem = new System.Windows.Forms.MenuItem();
            this.testMenuSeparator = new System.Windows.Forms.MenuItem();
            this.stopRunMenuItem = new System.Windows.Forms.MenuItem();
            this.toolsMenu = new System.Windows.Forms.MenuItem();
            this.projectEditorMenuItem = new System.Windows.Forms.MenuItem();
            this.saveResultsMenuItem = new System.Windows.Forms.MenuItem();
            this.toolsMenuSeparator1 = new System.Windows.Forms.MenuItem();
            this.extensionsMenuItem = new System.Windows.Forms.MenuItem();
            this.settingsMenuItem = new System.Windows.Forms.MenuItem();
            this.helpItem = new System.Windows.Forms.MenuItem();
            this.testCentricHelpMenuItem = new System.Windows.Forms.MenuItem();
            this.nunitHelpMenuItem = new System.Windows.Forms.MenuItem();
            this.helpMenuSeparator1 = new System.Windows.Forms.MenuItem();
            this.aboutMenuItem = new System.Windows.Forms.MenuItem();
            this.treeSplitter = new System.Windows.Forms.Splitter();
            this.rightPanel = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.runCount = new TestCentric.Gui.Controls.ExpandingLabel();
            this.stopButton = new System.Windows.Forms.Button();
            this.runButton = new System.Windows.Forms.Button();
            this.progressBar = new TestCentric.Gui.Views.ProgressBarView();
            this.resultTabs = new System.Windows.Forms.TabControl();
            this.errorTab = new System.Windows.Forms.TabPage();
            this.errorsAndFailuresView1 = new TestCentric.Gui.Views.ErrorsAndFailuresView();
            this.notrunTab = new System.Windows.Forms.TabPage();
            this.testsNotRunView1 = new TestCentric.Gui.Views.TestsNotRunView();
            this.outputTab = new System.Windows.Forms.TabPage();
            this.textOutputView1 = new TestCentric.Gui.Views.TextOutputView();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.categoryView = new TestCentric.Gui.Views.CategoryView();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.tabs = new System.Windows.Forms.TabControl();
            this.testPage = new System.Windows.Forms.TabPage();
            this.testPanel = new System.Windows.Forms.Panel();
            this.treeView = new TestCentric.Gui.Views.TestTreeView();
            this.categoryPage = new System.Windows.Forms.TabPage();
            this.categoryPanel = new System.Windows.Forms.Panel();
            this.testPanel.SuspendLayout();
            this.tabs.SuspendLayout();
            this.categoryPanel.SuspendLayout();
            this.rightPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.resultTabs.SuspendLayout();
            this.errorTab.SuspendLayout();
            this.notrunTab.SuspendLayout();
            this.outputTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusBar
            // 
            this.statusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusBar.Location = new System.Drawing.Point(0, 407);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(744, 24);
            this.statusBar.TabIndex = 0;
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.fileMenu,
            this.viewMenu,
            this.testMenu,
            this.toolsMenu,
            this.helpItem});
            // 
            // fileMenu
            // 
            this.fileMenu.Index = 0;
            this.fileMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.openMenuItem,
            this.closeMenuItem,
            this.addTestFileMenuItem,
            this.fileMenuSeparator1,
            this.reloadTestsMenuItem,
            this.runtimeMenuItem,
            this.menuItem2,
            this.recentFilesMenu,
            this.fileMenuSeparator4,
            this.exitMenuItem});
            this.fileMenu.Text = "&File";
            // 
            // openMenuItem
            // 
            this.openMenuItem.Index = 0;
            this.openMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.openMenuItem.Text = "&Open...";
            // 
            // closeMenuItem
            // 
            this.closeMenuItem.Index = 1;
            this.closeMenuItem.Text = "&Close";
            // 
            // addTestFileMenuItem
            // 
            this.addTestFileMenuItem.Index = 2;
            this.addTestFileMenuItem.Text = "&Add Test File...";
            // 
            // fileMenuSeparator1
            // 
            this.fileMenuSeparator1.Index = 3;
            this.fileMenuSeparator1.Text = "-";
            // 
            // reloadTestsMenuItem
            // 
            this.reloadTestsMenuItem.Index = 4;
            this.reloadTestsMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
            this.reloadTestsMenuItem.Text = "&Reload Tests";
            // 
            // runtimeMenuItem
            // 
            this.runtimeMenuItem.Index = 5;
            this.runtimeMenuItem.Text = "  Select R&untime";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 6;
            this.menuItem2.Text = "-";
            // 
            // recentFilesMenu
            // 
            this.recentFilesMenu.Index = 7;
            this.recentFilesMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem4});
            this.recentFilesMenu.Text = "Recent &Files";
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 0;
            this.menuItem4.Text = "Dummy Entry to force PopUp initially";
            // 
            // fileMenuSeparator4
            // 
            this.fileMenuSeparator4.Index = 8;
            this.fileMenuSeparator4.Text = "-";
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Index = 9;
            this.exitMenuItem.Text = "E&xit";
            // 
            // viewMenu
            // 
            this.viewMenu.Index = 1;
            this.viewMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.fullGuiMenuItem,
            this.miniGuiMenuItem,
            this.viewMenuSeparator1,
            this.treeMenuItem,
            this.viewMenuSeparator2,
            this.guiFontMenuItem,
            this.fixedFontMenuItem,
            this.viewMenuSeparator3,
            this.statusBarMenuItem});
            this.viewMenu.Text = "&View";
            // 
            // fullGuiMenuItem
            // 
            this.fullGuiMenuItem.Checked = true;
            this.fullGuiMenuItem.Index = 0;
            this.fullGuiMenuItem.RadioCheck = true;
            this.fullGuiMenuItem.Tag = "Full";
            this.fullGuiMenuItem.Text = "&Full GUI";
            // 
            // miniGuiMenuItem
            // 
            this.miniGuiMenuItem.Index = 1;
            this.miniGuiMenuItem.RadioCheck = true;
            this.miniGuiMenuItem.Tag = "Mini";
            this.miniGuiMenuItem.Text = "&Mini GUI";
            // 
            // viewMenuSeparator1
            // 
            this.viewMenuSeparator1.Index = 2;
            this.viewMenuSeparator1.Text = "-";
            // 
            // treeMenuItem
            // 
            this.treeMenuItem.Index = 3;
            this.treeMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.showCheckboxesMenuItem,
            this.menuItem5,
            this.expandMenuItem,
            this.collapseMenuItem,
            this.menuItem8,
            this.expandAllMenuItem,
            this.collapseAllMenuItem,
            this.hideTestsMenuItem,
            this.menuItem12,
            this.propertiesMenuItem});
            this.treeMenuItem.Text = "Tree";
            // 
            // showCheckboxesMenuItem
            // 
            this.showCheckboxesMenuItem.Index = 0;
            this.showCheckboxesMenuItem.Text = "Show Checkboxes";
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 1;
            this.menuItem5.Text = "-";
            // 
            // expandMenuItem
            // 
            this.expandMenuItem.Index = 2;
            this.expandMenuItem.Text = "Expand";
            // 
            // collapseMenuItem
            // 
            this.collapseMenuItem.Index = 3;
            this.collapseMenuItem.Text = "Collapse";
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 4;
            this.menuItem8.Text = "-";
            // 
            // expandAllMenuItem
            // 
            this.expandAllMenuItem.Index = 5;
            this.expandAllMenuItem.Text = "Expand All";
            // 
            // collapseAllMenuItem
            // 
            this.collapseAllMenuItem.Index = 6;
            this.collapseAllMenuItem.Text = "Collapse All";
            // 
            // hideTestsMenuItem
            // 
            this.hideTestsMenuItem.Index = 7;
            this.hideTestsMenuItem.Text = "Hide Tests";
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 8;
            this.menuItem12.Text = "-";
            // 
            // propertiesMenuItem
            // 
            this.propertiesMenuItem.Index = 9;
            this.propertiesMenuItem.Text = "Properties...";
            // 
            // viewMenuSeparator2
            // 
            this.viewMenuSeparator2.Index = 4;
            this.viewMenuSeparator2.Text = "-";
            // 
            // guiFontMenuItem
            // 
            this.guiFontMenuItem.Index = 5;
            this.guiFontMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.increaseFontMenuItem,
            this.decreaseFontMenuItem,
            this.fontMenuSeparator,
            this.fontChangeMenuItem,
            this.defaultFontMenuItem});
            this.guiFontMenuItem.Text = "GUI Fo&nt";
            // 
            // increaseFontMenuItem
            // 
            this.increaseFontMenuItem.Index = 0;
            this.increaseFontMenuItem.Text = "&Increase";
            // 
            // decreaseFontMenuItem
            // 
            this.decreaseFontMenuItem.Index = 1;
            this.decreaseFontMenuItem.Text = "&Decrease";
            // 
            // fontMenuSeparator
            // 
            this.fontMenuSeparator.Index = 2;
            this.fontMenuSeparator.Text = "-";
            // 
            // fontChangeMenuItem
            // 
            this.fontChangeMenuItem.Index = 3;
            this.fontChangeMenuItem.Text = "&Change...";
            // 
            // defaultFontMenuItem
            // 
            this.defaultFontMenuItem.Index = 4;
            this.defaultFontMenuItem.Text = "&Restore";
            // 
            // fixedFontMenuItem
            // 
            this.fixedFontMenuItem.Index = 6;
            this.fixedFontMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.increaseFixedFontMenuItem,
            this.decreaseFixedFontMenuItem,
            this.menuItem1,
            this.restoreFixedFontMenuItem});
            this.fixedFontMenuItem.Text = "Fi&xed Font";
            // 
            // increaseFixedFontMenuItem
            // 
            this.increaseFixedFontMenuItem.Index = 0;
            this.increaseFixedFontMenuItem.Text = "&Increase";
            // 
            // decreaseFixedFontMenuItem
            // 
            this.decreaseFixedFontMenuItem.Index = 1;
            this.decreaseFixedFontMenuItem.Text = "&Decrease";
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 2;
            this.menuItem1.Text = "-";
            // 
            // restoreFixedFontMenuItem
            // 
            this.restoreFixedFontMenuItem.Index = 3;
            this.restoreFixedFontMenuItem.Text = "&Restore";
            // 
            // viewMenuSeparator3
            // 
            this.viewMenuSeparator3.Index = 7;
            this.viewMenuSeparator3.Text = "-";
            // 
            // statusBarMenuItem
            // 
            this.statusBarMenuItem.Checked = true;
            this.statusBarMenuItem.Index = 8;
            this.statusBarMenuItem.Text = "&Status Bar";
            // 
            // testMenu
            // 
            this.testMenu.Index = 2;
            this.testMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.runAllMenuItem,
            this.runSelectedMenuItem,
            this.runFailedMenuItem,
            this.testMenuSeparator,
            this.stopRunMenuItem});
            this.testMenu.Text = "&Tests";
            // 
            // runAllMenuItem
            // 
            this.runAllMenuItem.Index = 0;
            this.runAllMenuItem.Shortcut = System.Windows.Forms.Shortcut.F5;
            this.runAllMenuItem.Text = "&Run All";
            // 
            // runSelectedMenuItem
            // 
            this.runSelectedMenuItem.Index = 1;
            this.runSelectedMenuItem.Shortcut = System.Windows.Forms.Shortcut.F6;
            this.runSelectedMenuItem.Text = "Run &Selected";
            // 
            // runFailedMenuItem
            // 
            this.runFailedMenuItem.Enabled = false;
            this.runFailedMenuItem.Index = 2;
            this.runFailedMenuItem.Shortcut = System.Windows.Forms.Shortcut.F7;
            this.runFailedMenuItem.Text = "Run &Failed";
            // 
            // testMenuSeparator
            // 
            this.testMenuSeparator.Index = 3;
            this.testMenuSeparator.Text = "-";
            // 
            // stopRunMenuItem
            // 
            this.stopRunMenuItem.Index = 4;
            this.stopRunMenuItem.Text = "S&top Run";
            // 
            // toolsMenu
            // 
            this.toolsMenu.Index = 3;
            this.toolsMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.projectEditorMenuItem,
            this.saveResultsMenuItem,
            this.toolsMenuSeparator1,
            this.extensionsMenuItem,
            this.settingsMenuItem});
            this.toolsMenu.Text = "T&ools";
            // 
            // projectEditorMenuItem
            // 
            this.projectEditorMenuItem.Index = 0;
            this.projectEditorMenuItem.Text = "Project Editor...";
            // 
            // saveResultsMenuItem
            // 
            this.saveResultsMenuItem.Index = 1;
            this.saveResultsMenuItem.Text = "&Save Test Results...";
            // 
            // toolsMenuSeparator1
            // 
            this.toolsMenuSeparator1.Index = 2;
            this.toolsMenuSeparator1.Text = "-";
            // 
            // extensionsMenuItem
            // 
            this.extensionsMenuItem.Index = 3;
            this.extensionsMenuItem.Text = "Extensions...";
            // 
            // settingsMenuItem
            // 
            this.settingsMenuItem.Index = 4;
            this.settingsMenuItem.Text = "&Settings...";
            // 
            // helpItem
            // 
            this.helpItem.Index = 4;
            this.helpItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.testCentricHelpMenuItem,
            this.nunitHelpMenuItem,
            this.helpMenuSeparator1,
            this.aboutMenuItem});
            this.helpItem.Text = "&Help";
            // 
            // testCentricHelpMenuItem
            // 
            this.testCentricHelpMenuItem.Index = 0;
            this.testCentricHelpMenuItem.Text = "TestCentric Help...";
            // 
            // nunitHelpMenuItem
            // 
            this.nunitHelpMenuItem.Index = 1;
            this.nunitHelpMenuItem.Shortcut = System.Windows.Forms.Shortcut.F1;
            this.nunitHelpMenuItem.Text = "NUnit &Help...";
            // 
            // helpMenuSeparator1
            // 
            this.helpMenuSeparator1.Index = 2;
            this.helpMenuSeparator1.Text = "-";
            // 
            // aboutMenuItem
            // 
            this.aboutMenuItem.Index = 3;
            this.aboutMenuItem.Text = "&About TestCentric...";
            // 
            // treeSplitter
            // 
            this.treeSplitter.Location = new System.Drawing.Point(240, 0);
            this.treeSplitter.MinSize = 240;
            this.treeSplitter.Name = "treeSplitter";
            this.treeSplitter.Size = new System.Drawing.Size(6, 407);
            this.treeSplitter.TabIndex = 2;
            this.treeSplitter.TabStop = false;
            // 
            // rightPanel
            // 
            this.rightPanel.BackColor = System.Drawing.SystemColors.Control;
            this.rightPanel.Controls.Add(this.resultTabs);
            this.rightPanel.Controls.Add(this.groupBox1);
            this.rightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightPanel.Location = new System.Drawing.Point(246, 0);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Size = new System.Drawing.Size(498, 407);
            this.rightPanel.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.runCount);
            this.groupBox1.Controls.Add(this.stopButton);
            this.groupBox1.Controls.Add(this.runButton);
            this.groupBox1.Controls.Add(this.progressBar);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(498, 120);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // runCount
            // 
            this.runCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.runCount.AutoEllipsis = true;
            this.runCount.Location = new System.Drawing.Point(8, 89);
            this.runCount.Name = "runCount";
            this.runCount.Size = new System.Drawing.Size(480, 21);
            this.runCount.TabIndex = 5;
            // 
            // stopButton
            // 
            this.stopButton.AutoSize = true;
            this.stopButton.Location = new System.Drawing.Point(75, 16);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(64, 31);
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "&Stop";
            // 
            // runButton
            // 
            this.runButton.Location = new System.Drawing.Point(8, 16);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(64, 31);
            this.runButton.TabIndex = 3;
            this.runButton.Text = "&Run";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.BackColor = System.Drawing.SystemColors.Control;
            this.progressBar.CausesValidation = false;
            this.progressBar.Enabled = false;
            this.progressBar.ForeColor = System.Drawing.SystemColors.Highlight;
            this.progressBar.Location = new System.Drawing.Point(8, 54);
            this.progressBar.Name = "progressBar";
            this.progressBar.Progress = 0;
            this.progressBar.Size = new System.Drawing.Size(480, 16);
            this.progressBar.Status = TestCentric.Gui.Views.ProgressBarStatus.Success;
            this.progressBar.TabIndex = 0;
            // 
            // tabControl
            // 
            this.resultTabs.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.resultTabs.Controls.Add(this.errorTab);
            this.resultTabs.Controls.Add(this.notrunTab);
            this.resultTabs.Controls.Add(this.outputTab);
            this.resultTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultTabs.Location = new System.Drawing.Point(0, 120);
            this.resultTabs.Name = "tabControl";
            this.resultTabs.SelectedIndex = 0;
            this.resultTabs.Size = new System.Drawing.Size(498, 407);
            this.resultTabs.TabIndex = 2;
            // 
            // errorTab
            // 
            this.errorTab.Controls.Add(this.errorsAndFailuresView1);
            this.errorTab.Location = new System.Drawing.Point(4, 4);
            this.errorTab.Name = "errorTab";
            this.errorTab.Size = new System.Drawing.Size(490, 381);
            this.errorTab.TabIndex = 0;
            this.errorTab.Text = "Errors and Failures";
            this.errorTab.UseVisualStyleBackColor = true;
            // 
            // errorsAndFailuresView1
            // 
            this.errorsAndFailuresView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorsAndFailuresView1.EnableToolTips = false;
            this.errorsAndFailuresView1.Location = new System.Drawing.Point(0, 123);
            this.errorsAndFailuresView1.Name = "errorsAndFailuresView1";
            this.errorsAndFailuresView1.Size = new System.Drawing.Size(490, 381);
            this.errorsAndFailuresView1.SourceCodeDisplay = true;
            this.errorsAndFailuresView1.SourceCodeSplitOrientation = System.Windows.Forms.Orientation.Vertical;
            this.errorsAndFailuresView1.SourceCodeSplitterDistance = 0.3F;
            this.errorsAndFailuresView1.SplitterPosition = 128;
            this.errorsAndFailuresView1.TabIndex = 0;
            this.errorsAndFailuresView1.WordWrap = false;
            // 
            // notrunTab
            // 
            this.notrunTab.Controls.Add(this.testsNotRunView1);
            this.notrunTab.Location = new System.Drawing.Point(4, 4);
            this.notrunTab.Name = "notrunTab";
            this.notrunTab.Size = new System.Drawing.Size(490, 381);
            this.notrunTab.TabIndex = 1;
            this.notrunTab.Text = "Tests Not Run";
            this.notrunTab.UseVisualStyleBackColor = true;
            // 
            // testsNotRunView1
            // 
            this.testsNotRunView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testsNotRunView1.Location = new System.Drawing.Point(0, 0);
            this.testsNotRunView1.Name = "testsNotRunView1";
            this.testsNotRunView1.Size = new System.Drawing.Size(490, 381);
            this.testsNotRunView1.TabIndex = 0;
            // 
            // outputTab
            // 
            this.outputTab.Controls.Add(this.textOutputView1);
            this.outputTab.Location = new System.Drawing.Point(4, 4);
            this.outputTab.Name = "outputTab";
            this.outputTab.Size = new System.Drawing.Size(490, 381);
            this.outputTab.TabIndex = 2;
            this.outputTab.Text = "Text Output";
            this.outputTab.UseVisualStyleBackColor = true;
            // 
            // textOutputView1
            // 
            this.textOutputView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textOutputView1.Location = new System.Drawing.Point(0, 0);
            this.textOutputView1.Name = "textOutputView1";
            this.textOutputView1.Size = new System.Drawing.Size(490, 381);
            this.textOutputView1.TabIndex = 0;
            this.textOutputView1.WordWrap = true;
            // 
            // leftPanel
            // 
            this.leftPanel.Controls.Add(this.tabs);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftPanel.Location = new System.Drawing.Point(0, 0);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size(240, 407);
            this.leftPanel.TabIndex = 4;
            // 
            // tabs
            // 
            this.tabs.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabs.Controls.Add(this.testPage);
            this.tabs.Controls.Add(this.categoryPage);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Location = new System.Drawing.Point(0, 0);
            this.tabs.Multiline = true;
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(248, 496);
            this.tabs.TabIndex = 0;
            // 
            // testPage
            // 
            this.testPage.Controls.Add(this.testPanel);
            this.testPage.Location = new System.Drawing.Point(25, 4);
            this.testPage.Name = "testPage";
            this.testPage.Size = new System.Drawing.Size(219, 488);
            this.testPage.TabIndex = 0;
            this.testPage.Text = "Tests";
            // 
            // testPanel
            // 
            this.testPanel.Controls.Add(this.treeView);
            this.testPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testPanel.Location = new System.Drawing.Point(0, 0);
            this.testPanel.Name = "testPanel";
            this.testPanel.Size = new System.Drawing.Size(219, 488);
            this.testPanel.TabIndex = 0;
            // 
            // treeView
            // 
            this.treeView.AllowDrop = true;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "tests";
            this.treeView.Size = new System.Drawing.Size(219, 448);
            this.treeView.TabIndex = 0;
            // 
            // categoryPage
            // 
            this.categoryPage.Controls.Add(this.categoryPanel);
            this.categoryPage.Location = new System.Drawing.Point(25, 4);
            this.categoryPage.Name = "categoryPage";
            this.categoryPage.Size = new System.Drawing.Size(219, 488);
            this.categoryPage.TabIndex = 1;
            this.categoryPage.Text = "Categories";
            // 
            // categoryPanel
            // 
            this.categoryPanel.Controls.Add(this.categoryView);
            this.categoryPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.categoryPanel.Location = new System.Drawing.Point(0, 0);
            this.categoryPanel.Name = "categoryPanel";
            this.categoryPanel.Size = new System.Drawing.Size(219, 488);
            this.categoryPanel.TabIndex = 0;
            // 
            // categoryView
            // 
            this.categoryView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.categoryView.Location = new System.Drawing.Point(0, 0);
            this.categoryView.Name = "testTree";
            this.categoryView.Size = new System.Drawing.Size(240, 407);
            this.categoryView.TabIndex = 0;
            // 
            // TestCentricMainView
            // 
            this.ClientSize = new System.Drawing.Size(744, 431);
            this.Controls.Add(this.rightPanel);
            this.Controls.Add(this.treeSplitter);
            this.Controls.Add(this.leftPanel);
            this.Controls.Add(this.statusBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size(160, 32);
            this.Name = "TestCentricMainView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "TestCentric Runner for NUnit";
            this.rightPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.resultTabs.ResumeLayout(false);
            this.errorTab.ResumeLayout(false);
            this.notrunTab.ResumeLayout(false);
            this.outputTab.ResumeLayout(false);
            this.categoryPanel.ResumeLayout(false);
            this.testPanel.ResumeLayout(false);
            this.tabs.ResumeLayout(false);
            this.leftPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        #region Properties

        public bool Maximized
        {
            get { return WindowState == FormWindowState.Maximized; }
            set
            {
                if (value)
                    WindowState = FormWindowState.Maximized;
                else if (WindowState == FormWindowState.Maximized)
                    WindowState = FormWindowState.Normal;
                // No actionif minimized
            }
        }
        // View Parameters
        public IViewParameter<Font> FontSelector { get; }
        public IViewParameter<int> SplitterPosition { get; }

        // UI Elements
        public ICommand RunButton { get; }
        public ICommand StopButton { get; }
        public IControlElement<ExpandingLabel> RunSummary { get; }
        public ISelection ResultTabs { get; }

        // File Menu Items
        public IMenu FileMenu { get; }
        public ICommand OpenCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand AddTestFileCommand { get; }
        public ICommand ReloadTestsCommand { get; }
        public IMenu RuntimeMenu { get; }
        public ISelection SelectedRuntime { get; }
        public IMenu RecentFilesMenu { get; }
        public ICommand ExitCommand { get; }

        // View Menu Items
        public ISelection DisplayFormat { get; }
        public IMenu TreeMenu { get; }
        public IChecked CheckboxesCommand { get; }
        public ICommand ExpandCommand { get; }
        public ICommand CollapseCommand { get; }
        public ICommand ExpandAllCommand { get; }
        public ICommand CollapseAllCommand { get; }
        public ICommand HideTestsCommand { get; }
        public ICommand PropertiesCommand { get; }
        public ICommand IncreaseFontCommand { get; }
        public ICommand DecreaseFontCommand { get; }
        public ICommand ChangeFontCommand { get; }
        public ICommand RestoreFontCommand { get; }
        public ICommand IncreaseFixedFontCommand { get; }
        public ICommand DecreaseFixedFontCommand { get; }
        public ICommand RestoreFixedFontCommand { get; }
        public IChecked StatusBarCommand { get; }

        // Test Menu Items
        public ICommand RunAllCommand { get; }
        public ICommand RunSelectedCommand { get; }
        public ICommand RunFailedCommand { get; }
        public ICommand StopRunCommand { get; }

        // Tools Menu Items
        public IMenu ToolsMenu { get; }
        public ICommand ProjectEditorCommand { get; }
        public ICommand SaveResultsCommand { get; }
        public ICommand ExtensionsCommand { get; }
        public ICommand SettingsCommand { get; }

        // Help Menu Items
        public ICommand TestCentricHelpCommand { get; }
        public ICommand NUnitHelpCommand { get; }
        public ICommand AboutCommand { get; }

        public TestNode[] FailedTests
        {
            get { return TreeView.FailedTests; }
        }

        public LongRunningOperationDisplay LongOperationDisplay(string text)
        {
            return new LongRunningOperationDisplay(this, text);
        }

        #region Subordinate Views contained in main form

        public TestTreeView TreeView { get { return treeView; } }

        public CategoryView CategoryView { get { return categoryView; } }

        public ProgressBarView ProgressBarView { get { return progressBar; } }

        public StatusBarView StatusBarView { get { return statusBar; } }

        public ErrorsAndFailuresView ErrorsAndFailuresView { get { return errorsAndFailuresView1; } }

        public TestsNotRunView TestsNotRunView { get { return testsNotRunView1; } }

        public ITextOutputView TextOutputView { get { return textOutputView1; } }

        #endregion

        #endregion

        #region Menu Handlers

        #region View Menu

        public void Configure(bool useFullGui)
        {
            leftPanel.Visible = true;
            leftPanel.Dock = useFullGui
                ? DockStyle.Left
                : DockStyle.Fill;
            treeSplitter.Visible = useFullGui;
            rightPanel.Visible = useFullGui;
            statusBar.Visible = useFullGui;
        }

        #endregion

        #endregion

        #region Event Handlers for Project Load and Unload

        //private void OnTestProjectLoaded( object sender, TestNodeEventArgs e )
        //{
        //    string projectPath = e.Test.Name;

        //    //SetTitleBar( projectPath );
        //    runCount.Text = "";

        //    // If this is an NUnit project, set up watcher
        //    //if (NUnitProject.IsNUnitProjectFile(projectPath) && File.Exists(projectPath))
        //    //    presenter.WatchProject(projectPath);
        //}

        //private void OnTestProjectUnloading(object sender, TestEventArgs e)
        //{
        //    // Remove any watcher
        //    //if (e.Name != null && File.Exists(e.Name))
        //    //{
        //    //    presenter.RemoveWatcher();

        //    //    Version version = Environment.Version;
        //    //    foreach( TestAssemblyInfo info in TestLoader.AssemblyInfo )
        //    //        if ( info.ImageRuntimeVersion < version )
        //    //            version = info.ImageRuntimeVersion;

        //    //    recentFilesService.SetMostRecent( new RecentFileEntry( e.Name, version ) );
        //    //}
        //}

        //private void OnTestProjectUnloaded( object sender, TestEventArgs e )
        //{
        //    SetTitleBar( null );
        //    runCount.Text = "";
        //}

        //private void OnProjectLoadFailure(object sender, TestEventArgs e)
        //{
        //    //MessageDisplay.Error("Project Not Loaded", e.Exception);

        //    //recentFilesService.Remove(e.Name);

        //    //EnableRunCommand(IsProjectLoaded);
        //}

        #endregion

        #region Event Handlers for Test Load and Unload


        ///// <summary>
        ///// A test suite has been loaded, so update 
        ///// recent assemblies and display the tests in the UI
        ///// </summary>
        //private void OnTestLoaded( TestNodeEventArgs e )
        //{
        //    //if ( TestLoader.TestCount == 0 )
        //    //{
        //    //    foreach( TestAssemblyInfo info in TestLoader.AssemblyInfo )
        //    //        if ( info.TestFrameworks.Count > 0 ) return;

        //    //    MessageDisplay.Error("This assembly was not built with any known testing framework.");
        //    //}
        //}

        ///// <summary>
        ///// A test suite has been unloaded, so clear the UI
        ///// and remove any references to the suite.
        ///// </summary>
        //private void OnTestUnloaded( TestEventArgs e )
        //{
        //    runCount.Text = null;
        //    Presenter.EnableRunCommand( false );
        //    Refresh();
        //}

        ///// <summary>
        ///// The current test suite has changed in some way,
        ///// so update the info in the UI and clear the
        ///// test results, since they are no longer valid.
        ///// </summary>
        //private void OnTestChanged( TestEventArgs e )
        //{
        //    //SetTitleBar(TestProject.Name);

        //    if ( longOpDisplay != null )
        //    {
        //        longOpDisplay.Dispose();
        //        longOpDisplay = null;
        //    }

        //    if (UserSettings.ClearResultsOnReload)
        //        runCount.Text = null;

        //    Presenter.EnableRunCommand( true );
        //}

        ///// <summary>
        ///// Event handler for assembly load failures. We do this via
        ///// an event since some errors may occur asynchronously.
        ///// </summary>
        //private void OnTestLoadFailure( object sender, TestEventArgs e )
        //{
        //    if ( longOpDisplay != null )
        //    {
        //        longOpDisplay.Dispose();
        //        longOpDisplay = null;
        //    }

        //    //string message = e.Action == NUnit.Util.TestAction.TestReloadFailed
        //    //    ? "Test reload failed!"
        //    //    : "Test load failed!";
        //    //string NL = Environment.NewLine;
        //    //if ( e.Exception is BadImageFormatException )
        //    //    message += string.Format(NL + NL +
        //    //        "The assembly could not be loaded by NUnit. PossibleProblems include:" + NL + NL +
        //    //        "1. The assembly may not be a valid .NET assembly." + NL + NL +
        //    //        "2. You may be attempting to load an assembly built with a later version of the CLR than the version under which NUnit is currently running ({0})." + NL + NL +
        //    //        "3. You may be attempting to load a 64-bit assembly into a 32-bit process.",
        //    //        Environment.Version.ToString(3) );

        //    //MessageDisplay.Error(message, e.Exception);

        //    //if ( !IsTestLoaded )
        //    //    OnTestUnloaded( sender, e );
        //    //else
        //    //    EnableRunCommand( true );
        //}

        #endregion

        #region Helper Methods

        /// <summary>
        /// Set the title bar based on the loaded file or project
        /// </summary>
        /// <param name="fileName"></param>
        private void SetTitleBar(string fileName)
        {
            Text = fileName == null
                ? "NUnit"
                : string.Format("{0} - NUnit", Path.GetFileName(fileName));
        }

        #endregion
    }
}

