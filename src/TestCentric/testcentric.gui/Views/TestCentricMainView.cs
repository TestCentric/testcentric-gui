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
using System.IO;
using NUnit.Engine;

namespace TestCentric.Gui.Views
{
    using Controls;
    using Model;
    using Model.Settings;
    using Presenters;
    using Elements;

    public class TestCentricMainView : TestCentricFormBase
    {
        static Logger log = InternalTrace.GetLogger(typeof(TestCentricMainView));

        #region Instance variables

        // Handlers for our recentFiles and recentProjects
        private RecentFileMenuHandler _recentProjectsMenuHandler;

        private string _displayFormat = "Full";

        private System.Drawing.Font _fixedFont;

        private System.ComponentModel.IContainer components;

        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.Splitter treeSplitter;
        private System.Windows.Forms.Panel rightPanel;

        private TestTree testTree;

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button runButton;
        private System.Windows.Forms.Button stopButton;
        private ProgressBarView progressBar;
        private ExpandingLabel runCount;

        private TabControl tabControl;

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
        private MenuItem menuItem3;
        private MenuItem menuItem4;
        private TextOutputView textOutputView1;

        #endregion

        #region Construction and Disposal

        public TestCentricMainView(ITestModel model) : base("NUnit")
        {
            InitializeComponent();

            Model = model;

            UserSettings = Model.Services.UserSettings;

            RunButton = new ButtonElement(runButton);
            StopButton = new ButtonElement(stopButton);

            // Initialize File Menu Commands
            FileMenu = new MenuElement(fileMenu);
            OpenCommand = new MenuElement(openMenuItem);
            CloseCommand = new MenuElement(closeMenuItem);
            AddTestFileCommand = new MenuElement(addTestFileMenuItem);
            ReloadTestsCommand = new MenuElement(reloadTestsMenuItem);
            RuntimeMenu = new MenuElement(runtimeMenuItem);
            SelectedRuntime = new CheckedMenuGroup(runtimeMenuItem);
            RecentFilesMenu = new MenuElement(recentFilesMenu);

            // Initialize Test Menu Commands
            RunAllCommand = new MenuElement(runAllMenuItem);
            RunSelectedCommand = new MenuElement(runSelectedMenuItem);
            RunFailedCommand = new MenuElement(runFailedMenuItem);
            StopRunCommand = new MenuElement(stopRunMenuItem);

            // Initialize Tools Menu Comands
            ProjectEditorCommand = new MenuElement(projectEditorMenuItem);
            SaveResultsCommand = new MenuElement(saveResultsMenuItem);
            DisplaySettingsCommand = new MenuElement(settingsMenuItem);

            //UserSettings.Changed += (object sender, SettingsEventArgs e) =>
            //{
            //    if (e.SettingName == "Gui.Options.DisplayFormat")
            //        LoadFormSettings();
            //};
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.errorTab = new System.Windows.Forms.TabPage();
            this.errorsAndFailuresView1 = new TestCentric.Gui.Views.ErrorsAndFailuresView();
            this.notrunTab = new System.Windows.Forms.TabPage();
            this.testsNotRunView1 = new TestCentric.Gui.Views.TestsNotRunView();
            this.outputTab = new System.Windows.Forms.TabPage();
            this.textOutputView1 = new TestCentric.Gui.Views.TextOutputView();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.testTree = new TestCentric.Gui.Controls.TestTree();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.rightPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.errorTab.SuspendLayout();
            this.notrunTab.SuspendLayout();
            this.outputTab.SuspendLayout();
            this.leftPanel.SuspendLayout();
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
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
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
            this.fullGuiMenuItem.Text = "&Full GUI";
            this.fullGuiMenuItem.Click += new System.EventHandler(this.fullGuiMenuItem_Click);
            // 
            // miniGuiMenuItem
            // 
            this.miniGuiMenuItem.Index = 1;
            this.miniGuiMenuItem.RadioCheck = true;
            this.miniGuiMenuItem.Text = "&Mini GUI";
            this.miniGuiMenuItem.Click += new System.EventHandler(this.miniGuiMenuItem_Click);
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
            this.treeMenuItem.Popup += new System.EventHandler(this.treeMenuItem_Popup);
            // 
            // showCheckboxesMenuItem
            // 
            this.showCheckboxesMenuItem.Index = 0;
            this.showCheckboxesMenuItem.Text = "Show Checkboxes";
            this.showCheckboxesMenuItem.Click += new System.EventHandler(this.showCheckboxesMenuItem_Click);
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
            this.expandMenuItem.Click += new System.EventHandler(this.expandMenuItem_Click);
            // 
            // collapseMenuItem
            // 
            this.collapseMenuItem.Index = 3;
            this.collapseMenuItem.Text = "Collapse";
            this.collapseMenuItem.Click += new System.EventHandler(this.collapseMenuItem_Click);
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
            this.expandAllMenuItem.Click += new System.EventHandler(this.expandAllMenuItem_Click);
            // 
            // collapseAllMenuItem
            // 
            this.collapseAllMenuItem.Index = 6;
            this.collapseAllMenuItem.Text = "Collapse All";
            this.collapseAllMenuItem.Click += new System.EventHandler(this.collapseAllMenuItem_Click);
            // 
            // hideTestsMenuItem
            // 
            this.hideTestsMenuItem.Index = 7;
            this.hideTestsMenuItem.Text = "Hide Tests";
            this.hideTestsMenuItem.Click += new System.EventHandler(this.hideTestsMenuItem_Click);
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
            this.propertiesMenuItem.Click += new System.EventHandler(this.propertiesMenuItem_Click);
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
            this.increaseFontMenuItem.Click += new System.EventHandler(this.increaseFontMenuItem_Click);
            // 
            // decreaseFontMenuItem
            // 
            this.decreaseFontMenuItem.Index = 1;
            this.decreaseFontMenuItem.Text = "&Decrease";
            this.decreaseFontMenuItem.Click += new System.EventHandler(this.decreaseFontMenuItem_Click);
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
            this.fontChangeMenuItem.Click += new System.EventHandler(this.fontChangeMenuItem_Click);
            // 
            // defaultFontMenuItem
            // 
            this.defaultFontMenuItem.Index = 4;
            this.defaultFontMenuItem.Text = "&Restore";
            this.defaultFontMenuItem.Click += new System.EventHandler(this.defaultFontMenuItem_Click);
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
            this.increaseFixedFontMenuItem.Click += new System.EventHandler(this.increaseFixedFontMenuItem_Click);
            // 
            // decreaseFixedFontMenuItem
            // 
            this.decreaseFixedFontMenuItem.Index = 1;
            this.decreaseFixedFontMenuItem.Text = "&Decrease";
            this.decreaseFixedFontMenuItem.Click += new System.EventHandler(this.decreaseFixedFontMenuItem_Click);
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
            this.restoreFixedFontMenuItem.Click += new System.EventHandler(this.restoreFixedFontMenuItem_Click);
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
            this.statusBarMenuItem.Click += new System.EventHandler(this.statusBarMenuItem_Click);
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
            this.toolsMenu.Popup += new System.EventHandler(this.toolsMenu_Popup);
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
            this.extensionsMenuItem.Click += new System.EventHandler(this.extensionsMenuItem_Click);
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
            this.testCentricHelpMenuItem.Click += new System.EventHandler(this.testCentricHelpMenuItem_Click);
            // 
            // nunitHelpMenuItem
            // 
            this.nunitHelpMenuItem.Index = 1;
            this.nunitHelpMenuItem.Shortcut = System.Windows.Forms.Shortcut.F1;
            this.nunitHelpMenuItem.Text = "NUnit &Help...";
            this.nunitHelpMenuItem.Click += new System.EventHandler(this.nunitHelpMenuItem_Click);
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
            this.aboutMenuItem.Click += new System.EventHandler(this.aboutMenuItem_Click);
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
            this.rightPanel.Controls.Add(this.groupBox1);
            this.rightPanel.Controls.Add(this.tabControl);
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
            this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl.Controls.Add(this.errorTab);
            this.tabControl.Controls.Add(this.notrunTab);
            this.tabControl.Controls.Add(this.outputTab);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(498, 407);
            this.tabControl.TabIndex = 2;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
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
            this.errorsAndFailuresView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.errorsAndFailuresView1.EnableToolTips = false;
            this.errorsAndFailuresView1.Location = new System.Drawing.Point(0, 123);
            this.errorsAndFailuresView1.Name = "errorsAndFailuresView1";
            this.errorsAndFailuresView1.Size = new System.Drawing.Size(490, 258);
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
            // testTree
            // 
            this.testTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testTree.Location = new System.Drawing.Point(0, 0);
            this.testTree.Name = "testTree";
            this.testTree.Size = new System.Drawing.Size(240, 407);
            this.testTree.TabIndex = 0;
            this.testTree.SelectedTestsChanged += new TestCentric.Gui.Controls.SelectedTestsChangedEventHandler(this.testTree_SelectedTestsChanged);
            // 
            // leftPanel
            // 
            this.leftPanel.Controls.Add(this.testTree);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftPanel.Location = new System.Drawing.Point(0, 0);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size(240, 407);
            this.leftPanel.TabIndex = 4;
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
            this.Load += new System.EventHandler(this.NUnitForm_Load);
            this.rightPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.errorTab.ResumeLayout(false);
            this.notrunTab.ResumeLayout(false);
            this.outputTab.ResumeLayout(false);
            this.leftPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        #region Events

        public event CommandHandler Startup;

        #endregion

        #region Properties

        public TestCentricPresenter Presenter { get; set; }

        public ICommand RunButton { get; }
        public ICommand StopButton { get; }

        public IMenu FileMenu { get; }
        public ICommand OpenCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand AddTestFileCommand { get; }
        public ICommand ReloadTestsCommand { get; }
        public IMenu RuntimeMenu { get; }
        public ISelection SelectedRuntime { get; }
        public IMenu RecentFilesMenu { get; }

        public ICommand RunAllCommand { get; }
        public ICommand RunSelectedCommand { get; }
        public ICommand RunFailedCommand { get; }
        public ICommand StopRunCommand { get; }

        public ICommand ProjectEditorCommand { get; }
        public ICommand SaveResultsCommand { get; }
        public ICommand DisplaySettingsCommand { get; }

        public TestNode[] SelectedTests
        {
            get { return testTree.SelectedTests; }
        }

        public TestNode[] FailedTests
        {
            get { return testTree.FailedTests; }
        }

        private ITestModel Model { get; }

        private UserSettings UserSettings { get; }

        #region Subordinate Views contained in main form

        public TestSuiteTreeView TreeView { get { return testTree.TreeView; } }

        public ProgressBarView ProgressBarView { get { return progressBar; } }

        public StatusBarView StatusBarView { get { return statusBar; } }

        public ErrorsAndFailuresView ErrorsAndFailuresView { get { return errorsAndFailuresView1; } }

        public TestsNotRunView TestsNotRunView { get { return testsNotRunView1; } }

        public ITextOutputView TextOutputView { get { return textOutputView1; } }

        #endregion

        #endregion

        #region Display Methods

        public void DisplaySummary(string text)
        {
            runCount.Text = text;
        }

        #endregion

        #region Menu Handlers

        #region File Menu

        private void exitMenuItem_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        #endregion

        #region View Menu
        private void statusBarMenuItem_Click(object sender, System.EventArgs e)
        {
            statusBarMenuItem.Checked = !statusBarMenuItem.Checked;
            statusBar.Visible = statusBarMenuItem.Checked;
        }

        private void fontChangeMenuItem_Click(object sender, System.EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.FontMustExist = true;
            fontDialog.Font = Font;
            fontDialog.MinSize = 6;
            fontDialog.MaxSize = 12;
            fontDialog.AllowVectorFonts = false;
            fontDialog.ScriptsOnly = true;
            fontDialog.ShowEffects = false;
            fontDialog.ShowApply = true;
            fontDialog.Apply += new EventHandler(fontDialog_Apply);
            if( fontDialog.ShowDialog() == DialogResult.OK )
                applyFont( fontDialog.Font );
        }

        private void fontDialog_Apply(object sender, EventArgs e)
        {
            applyFont( ((FontDialog)sender).Font );
        }


        private void defaultFontMenuItem_Click(object sender, System.EventArgs e)
        {
            applyFont( System.Windows.Forms.Form.DefaultFont );
        }

        private void fullGuiMenuItem_Click(object sender, System.EventArgs e)
        {
            if ( !fullGuiMenuItem.Checked )
                displayFullGui();
        }

        private void miniGuiMenuItem_Click(object sender, System.EventArgs e)
        {
            if ( !miniGuiMenuItem.Checked )
                displayMiniGui();
        }

        private void displayFullGui()
        {
            fullGuiMenuItem.Checked = true;
            miniGuiMenuItem.Checked = false;

            _displayFormat = "Full";
            UserSettings.Gui.DisplayFormat = "Full";

            leftPanel.Visible = true;
            leftPanel.Dock = DockStyle.Left;
            treeSplitter.Visible = true;
            rightPanel.Visible = true;
            statusBar.Visible = true;

            int x = UserSettings.Gui.MainForm.Left;
            int y = UserSettings.Gui.MainForm.Top;
            Point location = new Point(x, y);

            if (!IsValidLocation(location))
                location = new Point(10, 10);
            Location = location;

            int width = UserSettings.Gui.MainForm.Width;
            int height = UserSettings.Gui.MainForm.Height;
            if (width < 160) width = 160;
            if (height < 32) height = 32;
            Size = new Size(width, height);

            // Set to maximized if required
            if (UserSettings.Gui.MainForm.Maximized)
                WindowState = FormWindowState.Maximized;

            // Set the font to use
            applyFont(UserSettings.Gui.Font);
        }

        private void displayMiniGui()
        {
            miniGuiMenuItem.Checked = true;
            fullGuiMenuItem.Checked = false;
            
            _displayFormat = "Mini";
            UserSettings.Gui.DisplayFormat = "Mini";

            leftPanel.Visible = true;
            leftPanel.Dock = DockStyle.Fill;
            treeSplitter.Visible = false;
            rightPanel.Visible = false;
            statusBar.Visible = false;

            int x = UserSettings.Gui.MiniForm.Left;
            int y = UserSettings.Gui.MiniForm.Top;
            Point location = new Point(x, y);

            if (!IsValidLocation(location))
                location = new Point(10, 10);
            Location = location;

            int width = UserSettings.Gui.MiniForm.Width;
            int height = UserSettings.Gui.MiniForm.Height;
            if (width < 160) width = 160;
            if (height < 32) height = 32;
            Size = new Size(width, height);

            // Set to maximized if required
            if (UserSettings.Gui.MiniForm.Maximized)
                WindowState = FormWindowState.Maximized;

            // Set the font to use
            applyFont(UserSettings.Gui.Font);
        }

        private void increaseFontMenuItem_Click(object sender, System.EventArgs e)
        {
            applyFont( new Font( Font.FontFamily, Font.SizeInPoints * 1.2f, Font.Style ) );
        }

        private void decreaseFontMenuItem_Click(object sender, System.EventArgs e)
        {
            applyFont( new Font( Font.FontFamily, Font.SizeInPoints / 1.2f, Font.Style ) );
        }

        private void applyFont( Font font )
        {
            UserSettings.Gui.Font = Font = font;

            runCount.Font = font.FontFamily.IsStyleAvailable( FontStyle.Bold )
                ? new Font( font, FontStyle.Bold )
                : font;
        }

        private void increaseFixedFontMenuItem_Click(object sender, System.EventArgs e)
        {
            applyFixedFont( new Font( _fixedFont.FontFamily, _fixedFont.SizeInPoints * 1.2f, _fixedFont.Style ) );		
        }

        private void decreaseFixedFontMenuItem_Click(object sender, System.EventArgs e)
        {
            applyFixedFont( new Font( _fixedFont.FontFamily, _fixedFont.SizeInPoints / 1.2f, _fixedFont.Style ) );		
        }

        private void restoreFixedFontMenuItem_Click(object sender, System.EventArgs e)
        {
            applyFixedFont( new Font( FontFamily.GenericMonospace, 8.0f ) );
        }

        private void applyFixedFont(Font font)
        {
            UserSettings.Gui.FixedFont = _fixedFont = font;
        }

        private void treeMenuItem_Popup(object sender, EventArgs e)
        {
            TreeNode selectedNode = TreeView.SelectedNode;

            showCheckboxesMenuItem.Checked = UserSettings.Gui.TestTree.ShowCheckBoxes;

            if (selectedNode != null && selectedNode.Nodes.Count > 0)
            {
                bool isExpanded = selectedNode.IsExpanded;
                collapseMenuItem.Enabled = isExpanded;
                expandMenuItem.Enabled = !isExpanded;
            }
            else
            {
                collapseMenuItem.Enabled = expandMenuItem.Enabled = false;
            }
        }

        private void showCheckboxesMenuItem_Click(object sender, EventArgs e)
        {
            UserSettings.Gui.TestTree.ShowCheckBoxes = !showCheckboxesMenuItem.Checked;
        }

        private void expandMenuItem_Click(object sender, EventArgs e)
        {
            TreeView.SelectedNode.Expand();
        }

        private void collapseMenuItem_Click(object sender, EventArgs e)
        {
            TreeView.SelectedNode.Collapse();
        }

        private void expandAllMenuItem_Click(object sender, EventArgs e)
        {
            TreeView.BeginUpdate();
            TreeView.ExpandAll();
            TreeView.EndUpdate();
        }

        private void collapseAllMenuItem_Click(object sender, EventArgs e)
        {
            TreeView.BeginUpdate();
            TreeView.CollapseAll();
            TreeView.EndUpdate();

            // Compensate for a bug in the underlying control
            if (TreeView.Nodes.Count > 0)
                TreeView.SelectedNode = TreeView.Nodes[0];
        }

        private void hideTestsMenuItem_Click(object sender, EventArgs e)
        {
            TreeView.HideTests();
        }

        private void propertiesMenuItem_Click(object sender, EventArgs e)
        {
            if (TreeView.SelectedTest != null)
                TreeView.ShowPropertiesDialog(TreeView.SelectedTest);
        }

        #endregion

        #region Tools Menu

        private void toolsMenu_Popup(object sender, EventArgs e)
        {
            projectEditorMenuItem.Enabled = File.Exists(Model.ProjectEditorPath);

        }

        private void extensionsMenuItem_Click(object sender, EventArgs e)
        {
            using (var extensionsDialog = new ExtensionDialog(Model.Services.ExtensionService))
            {
                extensionsDialog.Font = Model.Services.UserSettings.Gui.Font;
                extensionsDialog.ShowDialog();
            }
        }

        #endregion

        #region Help Menu


        private void testCentricHelpMenuItem_Click(object sender, EventArgs e)
        {
            MessageDisplay.Error("Not Yet Implemented");
            //System.Diagnostics.Process.Start("");

        }
        private void nunitHelpMenuItem_Click(object sender, System.EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/nunit/docs/wiki/NUnit-Documentation");
        }

        /// <summary>
        /// Display the about box when menu item is selected
        /// </summary>
        private void aboutMenuItem_Click(object sender, System.EventArgs e)
        {
            using( AboutBox aboutBox = new AboutBox() )
            {
                aboutBox.ShowDialog();
            }
        }

        #endregion

        #endregion

        #region Form Level Events

        /// <summary>
        /// Get saved options when form loads
        /// </summary>
        private void NUnitForm_Load(object sender, System.EventArgs e)
        {
            if ( !DesignMode )
            {
                _recentProjectsMenuHandler = new RecentFileMenuHandler(recentFilesMenu, Model, File.Exists);

                LoadFormSettings();

                // Force display  so that any "Loading..." or error 
                // message overlays the main form.
                Show();
                Invalidate();
                Update();

                SubscribeToTestEvents();

                Startup?.Invoke();
            }
        }

        private void LoadFormSettings()
        {
            switch (UserSettings.Gui.DisplayFormat)
            {
                case "Full":
                    displayFullGui();
                    break;
                case "Mini":
                    displayMiniGui();
                    break;
                default:
                    throw new ApplicationException("Invalid Setting");
            }

            // Handle changes to form position
            Move += new System.EventHandler(NUnitForm_Move);
            Resize += new System.EventHandler(NUnitForm_Resize);

            // Set the splitter position
            int splitPosition = UserSettings.Gui.MainForm.SplitPosition;
            if (splitPosition >= treeSplitter.MinSize && splitPosition < ClientSize.Width)
                treeSplitter.SplitPosition = splitPosition;

            // Handle changes in splitter positions
            treeSplitter.SplitterMoved += new SplitterEventHandler(treeSplitter_SplitterMoved);

            // Get the fixed font used by result tabs
            _fixedFont = UserSettings.Gui.FixedFont;

            // Set the selected result tab
            tabControl.SelectedIndex = UserSettings.Gui.SelectedTab;

            // Handle changes in settings
            UserSettings.Changed += (object sender, SettingsEventArgs e) =>
            {
                if (e.SettingName == "Gui.Options.DisplayFormat")
                {
                    if (UserSettings.Gui.DisplayFormat == "Full")
                        displayFullGui();
                    else
                        displayMiniGui();
                }
                //else if (args.SettingName.StartsWith("Gui.TextOutput.") && args.SettingName.EndsWith(".Content"))
                //{
                //    TestLoader.IsTracingEnabled = resultTabs.IsTracingEnabled;
                //    TestLoader.LoggingThreshold = resultTabs.MaximumLogLevel;
                //}
            };
        }

        private bool IsValidLocation( Point location )
        {
            Rectangle myArea = new Rectangle( location, Size );
            bool intersect = false;
            foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens)
            {
                intersect |= myArea.IntersectsWith(screen.WorkingArea);
            }
            return intersect;
        }

        // Save settings changed by moving the form
        private void NUnitForm_Move(object sender, System.EventArgs e)
        {
            switch( _displayFormat )
            {
                case "Full":
                default:
                    if ( WindowState == FormWindowState.Normal )
                    {
                        UserSettings.Gui.MainForm.Left = Location.X;
                        UserSettings.Gui.MainForm.Top = Location.Y;
                        UserSettings.Gui.MainForm.Maximized = false;
                    }
                    break;
                case "Mini":
                    if ( WindowState == FormWindowState.Normal )
                    {
                        UserSettings.Gui.MiniForm.Left = Location.X;
                        UserSettings.Gui.MiniForm.Top = Location.Y;
                        UserSettings.Gui.MiniForm.Maximized = false;
                    }
                    break;
            }
        }

        // Save settings that change when window is resized
        private void NUnitForm_Resize(object sender,System.EventArgs e)
        {
            if ( WindowState == FormWindowState.Normal )
            {
                if (_displayFormat == "Full")
                {
                    UserSettings.Gui.MainForm.Width = Size.Width;
                    UserSettings.Gui.MainForm.Height = Size.Height;
                }
                else
                {
                    UserSettings.Gui.MiniForm.Width = Size.Width;
                    UserSettings.Gui.MiniForm.Height = Size.Height;
                }
            }
        }

        // Splitter moved so save it's position
        private void treeSplitter_SplitterMoved( object sender, SplitterEventArgs e )
        {
            UserSettings.Gui.MainForm.SplitPosition = treeSplitter.SplitPosition;
        }

        #endregion

        #region Other UI Event Handlers

        private void testTree_SelectedTestsChanged(object sender, SelectedTestsChangedEventArgs e)
        {
            if (!Model.IsTestRunning)
            {
                //statusBar.Initialize(e.TestCount, e.TestName);
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int index = tabControl.SelectedIndex;
            if (index >= 0 && index < tabControl.TabCount)
                UserSettings.Gui.SelectedTab = index;
        }

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
        private void SetTitleBar( string fileName )
        {
            Text = fileName == null 
                ? "NUnit"
                : string.Format( "{0} - NUnit", Path.GetFileName( fileName ) );
        }

        private void SubscribeToTestEvents()
        {
            Model.Events.RunFinished += (TestResultEventArgs e) =>
            {
                //EnableStopCommand(false);

                ////if ( e.Exception != null )
                ////{
                ////    if ( ! ( e.Exception is System.Threading.ThreadAbortException ) )
                ////        MessageDisplay.Error("NUnit Test Run Failed", e.Exception);
                ////}

                ResultSummary summary = ResultSummaryCreator.FromResultNode(e.Result);
                DisplaySummary(string.Format(
                    "Passed: {0}   Failed: {1}   Errors: {2}   Inconclusive: {3}   Invalid: {4}   Ignored: {5}   Skipped: {6}   Time: {7}",
                    summary.PassCount, summary.FailedCount, summary.ErrorCount, summary.InconclusiveCount, summary.InvalidCount, summary.IgnoreCount, summary.SkipCount, summary.Duration));

                ////string resultPath = Path.Combine(TestProject.BasePath, "TestResult.xml");
                ////try
                ////{
                ////    TestLoader.SaveLastResult(resultPath);
                ////    log.Debug("Saved result to {0}", resultPath);
                ////}
                ////catch (Exception ex)
                ////{
                ////    log.Warning("Unable to save result to {0}\n{1}", resultPath, ex.ToString());
                ////}

                //EnableRunCommand(true);
                //saveResultsMenuItem.Enabled = true;

                if (e.Result.Outcome.Status == TestStatus.Failed)
                    Activate();
            };

            Model.Events.TestLoaded += (TestNodeEventArgs e) =>
            {
                foreach (var assembly in Model.TestAssemblies)
                    if (assembly.RunState == RunState.NotRunnable)
                        MessageDisplay.Error(assembly.GetProperty("_SKIPREASON"));

                ////if ( TestLoader.TestCount == 0 )
                ////{
                ////    foreach( TestAssemblyInfo info in TestLoader.AssemblyInfo )
                ////        if ( info.TestFrameworks.Count > 0 ) return;

                ////    MessageDisplay.Error("This assembly was not built with any known testing framework.");
                ////}
            };

            //Model.Events.TestLoadFailed += new TestEventHandler(OnTestLoadFailure);
            //Model.Events.TestsUnloading += new TestEventHandler(OnTestUnloadStarting);

            Model.Events.TestUnloaded += (TestEventArgs) =>
            {
                runCount.Text = null;
                //EnableRunCommand(false);
                //saveResultsMenuItem.Enabled = false;
                Refresh();
            };

            //Model.Events.TestReloading += new TestEventHandler(OnReloadStarting);

            Model.Events.TestReloaded += (TestNodeEventArgs ea) =>
            {
                //SetTitleBar(TestProject.Name);

                if (UserSettings.Gui.ClearResultsOnReload)
                    runCount.Text = null;

            };
        }

        #endregion
    }
}

