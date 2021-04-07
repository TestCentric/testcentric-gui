// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TestCentric.Gui.Views
{
    using Controls;
    using Elements;

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
        private ExpandingLabel runSummary;

        private TabControl resultTabs;

        private StatusBarView statusBar;

        private System.Windows.Forms.ToolTip toolTip;

        private System.Windows.Forms.MenuStrip mainMenu;

        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem openMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentFilesMenu;
        private System.Windows.Forms.ToolStripMenuItem closeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;

        private System.Windows.Forms.ToolStripMenuItem toolsMenu;
        private System.Windows.Forms.ToolStripMenuItem settingsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveResultsMenuItem;

        private System.Windows.Forms.ToolStripMenuItem nunitHelpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpItem;
        private System.Windows.Forms.ToolStripSeparator helpMenuSeparator1;
        private System.Windows.Forms.ToolStripMenuItem aboutMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewMenu;
        private System.Windows.Forms.ToolStripMenuItem statusBarMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miniGuiMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fullGuiMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fontChangeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defaultFontMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decreaseFontMenuItem;
        private System.Windows.Forms.ToolStripMenuItem increaseFontMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testMenu;
        private System.Windows.Forms.ToolStripMenuItem runAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runSelectedMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runFailedMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopRunMenuItem;
        private System.Windows.Forms.ToolStripMenuItem guiFontMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fixedFontMenuItem;
        private System.Windows.Forms.ToolStripMenuItem increaseFixedFontMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decreaseFixedFontMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreFixedFontMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadTestsMenuItem;
        private ToolStripMenuItem runtimeMenuItem;
        private ToolStripMenuItem addTestFileMenuItem;
        private ToolStripMenuItem extensionsMenuItem;
        private ToolStripMenuItem testCentricHelpMenuItem;
        private TabPage errorTab;
        private TabPage notrunTab;
        private TabPage outputTab;
        private ErrorsAndFailuresView errorsAndFailuresView1;
        private TestsNotRunView testsNotRunView1;
        private ToolStripMenuItem recentFilesDummyMenuItem;
        private ToolStripMenuItem openWorkDirectoryMenuItem;
        private ToolStripMenuItem saveResultsAsMenuItem;
        private ToolStripMenuItem runParametersMenuItem;
        private ToolStripMenuItem runtimeDummyMenuItem;
        private ToolStripMenuItem forceStopMenuItem;
        private Button forceStopButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripMenuItem runAsX86MenuItem;
        private TextOutputView textOutputView1;

        #endregion

        #region Construction and Disposal

        public TestCentricMainView() : base("TestCentric")
        {
            InitializeComponent();

            treeSplitter.SplitterMoved += (s, e) =>
            {
                SplitterPositionChanged?.Invoke(s, e);
            };

            // UI Elements on main form
            RunButton = new ButtonElement(runButton);
            StopButton = new ButtonElement(stopButton);
            ForceStopButton = new ButtonElement(forceStopButton);
            RunSummary = new ControlElement(runSummary);
            ResultTabs = new TabSelector(resultTabs);

            // Initialize File Menu Commands
            FileMenu = new PopupMenuElement(fileMenu);
            OpenCommand = new CommandMenuElement(openMenuItem);
            CloseCommand = new CommandMenuElement(closeMenuItem);
            AddTestFilesCommand = new CommandMenuElement(addTestFileMenuItem);
            ReloadTestsCommand = new CommandMenuElement(reloadTestsMenuItem);
            RuntimeMenu = new PopupMenuElement(runtimeMenuItem);
            //SelectedRuntime = new CheckedMenuGroup(runtimeMenuItem);
            RunAsX86 = new CheckedMenuElement(runAsX86MenuItem);
            RecentFilesMenu = new PopupMenuElement(recentFilesMenu);
            ExitCommand = new CommandMenuElement(exitMenuItem);

            // Initialize View Menu Commands
            DisplayFormat = new CheckedToolStripMenuGroup("", fullGuiMenuItem, miniGuiMenuItem);
            IncreaseFontCommand = new CommandMenuElement(increaseFontMenuItem);
            DecreaseFontCommand = new CommandMenuElement(decreaseFontMenuItem);
            ChangeFontCommand = new CommandMenuElement(fontChangeMenuItem);
            RestoreFontCommand = new CommandMenuElement(defaultFontMenuItem);
            IncreaseFixedFontCommand = new CommandMenuElement(increaseFixedFontMenuItem);
            DecreaseFixedFontCommand = new CommandMenuElement(decreaseFixedFontMenuItem);
            RestoreFixedFontCommand = new CommandMenuElement(restoreFixedFontMenuItem);
            StatusBarCommand = new CheckedMenuElement(statusBarMenuItem);

            // Initialize Test Menu Commands
            RunAllCommand = new CommandMenuElement(runAllMenuItem);
            RunSelectedCommand = new CommandMenuElement(runSelectedMenuItem);
            RunFailedCommand = new CommandMenuElement(runFailedMenuItem);
            StopRunCommand = new CommandMenuElement(stopRunMenuItem);
            ForceStopCommand = new CommandMenuElement(forceStopMenuItem);
            TestParametersCommand = new CommandMenuElement(runParametersMenuItem);

            // Initialize Tools Menu Comands
            ToolsMenu = new PopupMenuElement(toolsMenu);
            SaveResultsCommand = new CommandMenuElement(saveResultsMenuItem);
            SaveResultsAsMenu = new PopupMenuElement(saveResultsAsMenuItem);
            OpenWorkDirectoryCommand = new CommandMenuElement(openWorkDirectoryMenuItem);
            ExtensionsCommand = new CommandMenuElement(extensionsMenuItem);
            SettingsCommand = new CommandMenuElement(settingsMenuItem);

            TestCentricHelpCommand = new CommandMenuElement(testCentricHelpMenuItem);
            NUnitHelpCommand = new CommandMenuElement(nunitHelpMenuItem);
            AboutCommand = new CommandMenuElement(aboutMenuItem);

            DialogManager = new DialogManager();
            LongRunningOperation = new LongRunningOperationDisplay(this);
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
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addTestFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.reloadTestsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.runtimeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runtimeDummyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.recentFilesMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.recentFilesDummyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.fullGuiMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miniGuiMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.guiFontMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.increaseFontMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decreaseFontMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.fontChangeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultFontMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fixedFontMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.increaseFixedFontMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decreaseFixedFontMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.restoreFixedFontMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.statusBarMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.runAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runSelectedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runFailedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.runParametersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.stopRunMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forceStopMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.saveResultsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveResultsAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openWorkDirectoryMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.extensionsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testCentricHelpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nunitHelpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeSplitter = new System.Windows.Forms.Splitter();
            this.rightPanel = new System.Windows.Forms.Panel();
            this.resultTabs = new System.Windows.Forms.TabControl();
            this.errorTab = new System.Windows.Forms.TabPage();
            this.errorsAndFailuresView1 = new TestCentric.Gui.Views.ErrorsAndFailuresView();
            this.notrunTab = new System.Windows.Forms.TabPage();
            this.testsNotRunView1 = new TestCentric.Gui.Views.TestsNotRunView();
            this.outputTab = new System.Windows.Forms.TabPage();
            this.textOutputView1 = new TestCentric.Gui.Views.TextOutputView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.runSummary = new TestCentric.Gui.Controls.ExpandingLabel();
            this.forceStopButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.runButton = new System.Windows.Forms.Button();
            this.progressBar = new TestCentric.Gui.Views.ProgressBarView();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.categoryView = new TestCentric.Gui.Views.CategoryView();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.tabs = new System.Windows.Forms.TabControl();
            this.testPage = new System.Windows.Forms.TabPage();
            this.testPanel = new System.Windows.Forms.Panel();
            this.treeView = new TestCentric.Gui.Views.TestTreeView();
            this.categoryPage = new System.Windows.Forms.TabPage();
            this.categoryPanel = new System.Windows.Forms.Panel();
            this.runAsX86MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu.SuspendLayout();
            this.rightPanel.SuspendLayout();
            this.resultTabs.SuspendLayout();
            this.errorTab.SuspendLayout();
            this.notrunTab.SuspendLayout();
            this.outputTab.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.leftPanel.SuspendLayout();
            this.tabs.SuspendLayout();
            this.testPage.SuspendLayout();
            this.testPanel.SuspendLayout();
            this.categoryPage.SuspendLayout();
            this.categoryPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusBar
            // 
            this.statusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusBar.Location = new System.Drawing.Point(0, 407);
            this.statusBar.MinimumSize = new System.Drawing.Size(0, 24);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(744, 24);
            this.statusBar.TabIndex = 0;
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.viewMenu,
            this.testMenu,
            this.toolsMenu,
            this.helpItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(744, 24);
            this.mainMenu.TabIndex = 5;
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMenuItem,
            this.closeMenuItem,
            this.addTestFileMenuItem,
            this.toolStripSeparator1,
            this.reloadTestsMenuItem,
            this.toolStripSeparator2,
            this.runtimeMenuItem,
            this.runAsX86MenuItem,
            this.toolStripSeparator3,
            this.recentFilesMenu,
            this.toolStripSeparator4,
            this.exitMenuItem});
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(37, 20);
            this.fileMenu.Text = "&File";
            // 
            // openMenuItem
            // 
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openMenuItem.Text = "&Open...";
            // 
            // closeMenuItem
            // 
            this.closeMenuItem.Name = "closeMenuItem";
            this.closeMenuItem.Size = new System.Drawing.Size(180, 22);
            this.closeMenuItem.Text = "&Close";
            // 
            // addTestFileMenuItem
            // 
            this.addTestFileMenuItem.Name = "addTestFileMenuItem";
            this.addTestFileMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addTestFileMenuItem.Text = "&Add Test File...";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // reloadTestsMenuItem
            // 
            this.reloadTestsMenuItem.Name = "reloadTestsMenuItem";
            this.reloadTestsMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.reloadTestsMenuItem.Size = new System.Drawing.Size(180, 22);
            this.reloadTestsMenuItem.Text = "&Reload Tests";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // runtimeMenuItem
            // 
            this.runtimeMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runtimeDummyMenuItem});
            this.runtimeMenuItem.Name = "runtimeMenuItem";
            this.runtimeMenuItem.Size = new System.Drawing.Size(180, 22);
            this.runtimeMenuItem.Text = "Select R&untime";
            // 
            // runtimeDummyMenuItem
            // 
            this.runtimeDummyMenuItem.Name = "runtimeDummyMenuItem";
            this.runtimeDummyMenuItem.Size = new System.Drawing.Size(232, 22);
            this.runtimeDummyMenuItem.Text = "Dummy  entry to force Popup";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(177, 6);
            // 
            // recentFilesMenu
            // 
            this.recentFilesMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recentFilesDummyMenuItem});
            this.recentFilesMenu.Name = "recentFilesMenu";
            this.recentFilesMenu.Size = new System.Drawing.Size(180, 22);
            this.recentFilesMenu.Text = "Recent &Files";
            // 
            // recentFilesDummyMenuItem
            // 
            this.recentFilesDummyMenuItem.Name = "recentFilesDummyMenuItem";
            this.recentFilesDummyMenuItem.Size = new System.Drawing.Size(271, 22);
            this.recentFilesDummyMenuItem.Text = "Dummy Entry to force PopUp initially";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(177, 6);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitMenuItem.Text = "E&xit";
            // 
            // viewMenu
            // 
            this.viewMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fullGuiMenuItem,
            this.miniGuiMenuItem,
            this.toolStripSeparator8,
            this.guiFontMenuItem,
            this.fixedFontMenuItem,
            this.toolStripSeparator9,
            this.statusBarMenuItem});
            this.viewMenu.Name = "viewMenu";
            this.viewMenu.Size = new System.Drawing.Size(44, 20);
            this.viewMenu.Text = "&View";
            // 
            // fullGuiMenuItem
            // 
            this.fullGuiMenuItem.Checked = true;
            this.fullGuiMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fullGuiMenuItem.Name = "fullGuiMenuItem";
            this.fullGuiMenuItem.Size = new System.Drawing.Size(129, 22);
            this.fullGuiMenuItem.Tag = "Full";
            this.fullGuiMenuItem.Text = "&Full GUI";
            // 
            // miniGuiMenuItem
            // 
            this.miniGuiMenuItem.Name = "miniGuiMenuItem";
            this.miniGuiMenuItem.Size = new System.Drawing.Size(129, 22);
            this.miniGuiMenuItem.Tag = "Mini";
            this.miniGuiMenuItem.Text = "&Mini GUI";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(126, 6);
            // 
            // guiFontMenuItem
            // 
            this.guiFontMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.increaseFontMenuItem,
            this.decreaseFontMenuItem,
            this.toolStripSeparator10,
            this.fontChangeMenuItem,
            this.defaultFontMenuItem});
            this.guiFontMenuItem.Name = "guiFontMenuItem";
            this.guiFontMenuItem.Size = new System.Drawing.Size(129, 22);
            this.guiFontMenuItem.Text = "GUI Fo&nt";
            // 
            // increaseFontMenuItem
            // 
            this.increaseFontMenuItem.Name = "increaseFontMenuItem";
            this.increaseFontMenuItem.Size = new System.Drawing.Size(124, 22);
            this.increaseFontMenuItem.Text = "&Increase";
            // 
            // decreaseFontMenuItem
            // 
            this.decreaseFontMenuItem.Name = "decreaseFontMenuItem";
            this.decreaseFontMenuItem.Size = new System.Drawing.Size(124, 22);
            this.decreaseFontMenuItem.Text = "&Decrease";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(121, 6);
            // 
            // fontChangeMenuItem
            // 
            this.fontChangeMenuItem.Name = "fontChangeMenuItem";
            this.fontChangeMenuItem.Size = new System.Drawing.Size(124, 22);
            this.fontChangeMenuItem.Text = "&Change...";
            // 
            // defaultFontMenuItem
            // 
            this.defaultFontMenuItem.Name = "defaultFontMenuItem";
            this.defaultFontMenuItem.Size = new System.Drawing.Size(124, 22);
            this.defaultFontMenuItem.Text = "&Restore";
            // 
            // fixedFontMenuItem
            // 
            this.fixedFontMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.increaseFixedFontMenuItem,
            this.decreaseFixedFontMenuItem,
            this.toolStripSeparator11,
            this.restoreFixedFontMenuItem});
            this.fixedFontMenuItem.Name = "fixedFontMenuItem";
            this.fixedFontMenuItem.Size = new System.Drawing.Size(129, 22);
            this.fixedFontMenuItem.Text = "Fi&xed Font";
            // 
            // increaseFixedFontMenuItem
            // 
            this.increaseFixedFontMenuItem.Name = "increaseFixedFontMenuItem";
            this.increaseFixedFontMenuItem.Size = new System.Drawing.Size(121, 22);
            this.increaseFixedFontMenuItem.Text = "&Increase";
            // 
            // decreaseFixedFontMenuItem
            // 
            this.decreaseFixedFontMenuItem.Name = "decreaseFixedFontMenuItem";
            this.decreaseFixedFontMenuItem.Size = new System.Drawing.Size(121, 22);
            this.decreaseFixedFontMenuItem.Text = "&Decrease";
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(118, 6);
            // 
            // restoreFixedFontMenuItem
            // 
            this.restoreFixedFontMenuItem.Name = "restoreFixedFontMenuItem";
            this.restoreFixedFontMenuItem.Size = new System.Drawing.Size(121, 22);
            this.restoreFixedFontMenuItem.Text = "&Restore";
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(126, 6);
            // 
            // statusBarMenuItem
            // 
            this.statusBarMenuItem.Checked = true;
            this.statusBarMenuItem.CheckOnClick = true;
            this.statusBarMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.statusBarMenuItem.Name = "statusBarMenuItem";
            this.statusBarMenuItem.Size = new System.Drawing.Size(129, 22);
            this.statusBarMenuItem.Text = "&Status Bar";
            // 
            // testMenu
            // 
            this.testMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runAllMenuItem,
            this.runSelectedMenuItem,
            this.runFailedMenuItem,
            this.toolStripSeparator5,
            this.runParametersMenuItem,
            this.toolStripSeparator6,
            this.stopRunMenuItem,
            this.forceStopMenuItem});
            this.testMenu.Name = "testMenu";
            this.testMenu.Size = new System.Drawing.Size(44, 20);
            this.testMenu.Text = "&Tests";
            // 
            // runAllMenuItem
            // 
            this.runAllMenuItem.Name = "runAllMenuItem";
            this.runAllMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.runAllMenuItem.Size = new System.Drawing.Size(165, 22);
            this.runAllMenuItem.Text = "&Run All";
            // 
            // runSelectedMenuItem
            // 
            this.runSelectedMenuItem.Name = "runSelectedMenuItem";
            this.runSelectedMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.runSelectedMenuItem.Size = new System.Drawing.Size(165, 22);
            this.runSelectedMenuItem.Text = "Run &Selected";
            // 
            // runFailedMenuItem
            // 
            this.runFailedMenuItem.Enabled = false;
            this.runFailedMenuItem.Name = "runFailedMenuItem";
            this.runFailedMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.runFailedMenuItem.Size = new System.Drawing.Size(165, 22);
            this.runFailedMenuItem.Text = "Run &Failed";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(162, 6);
            // 
            // runParametersMenuItem
            // 
            this.runParametersMenuItem.Name = "runParametersMenuItem";
            this.runParametersMenuItem.Size = new System.Drawing.Size(165, 22);
            this.runParametersMenuItem.Text = "Test Parameters...";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(162, 6);
            // 
            // stopRunMenuItem
            // 
            this.stopRunMenuItem.Name = "stopRunMenuItem";
            this.stopRunMenuItem.Size = new System.Drawing.Size(165, 22);
            this.stopRunMenuItem.Text = "S&top Run";
            // 
            // forceStopMenuItem
            // 
            this.forceStopMenuItem.Name = "forceStopMenuItem";
            this.forceStopMenuItem.Size = new System.Drawing.Size(165, 22);
            this.forceStopMenuItem.Text = "Force Stop";
            // 
            // toolsMenu
            // 
            this.toolsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveResultsMenuItem,
            this.saveResultsAsMenuItem,
            this.openWorkDirectoryMenuItem,
            this.toolStripSeparator7,
            this.extensionsMenuItem,
            this.settingsMenuItem});
            this.toolsMenu.Name = "toolsMenu";
            this.toolsMenu.Size = new System.Drawing.Size(46, 20);
            this.toolsMenu.Text = "T&ools";
            // 
            // saveResultsMenuItem
            // 
            this.saveResultsMenuItem.Name = "saveResultsMenuItem";
            this.saveResultsMenuItem.Size = new System.Drawing.Size(194, 22);
            this.saveResultsMenuItem.Text = "&Save Test Results...";
            // 
            // saveResultsAsMenuItem
            // 
            this.saveResultsAsMenuItem.Name = "saveResultsAsMenuItem";
            this.saveResultsAsMenuItem.Size = new System.Drawing.Size(194, 22);
            this.saveResultsAsMenuItem.Text = "Save Test Results As";
            // 
            // openWorkDirectoryMenuItem
            // 
            this.openWorkDirectoryMenuItem.Name = "openWorkDirectoryMenuItem";
            this.openWorkDirectoryMenuItem.Size = new System.Drawing.Size(194, 22);
            this.openWorkDirectoryMenuItem.Text = "Open Work Directory...";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(191, 6);
            // 
            // extensionsMenuItem
            // 
            this.extensionsMenuItem.Name = "extensionsMenuItem";
            this.extensionsMenuItem.Size = new System.Drawing.Size(194, 22);
            this.extensionsMenuItem.Text = "Extensions...";
            // 
            // settingsMenuItem
            // 
            this.settingsMenuItem.Name = "settingsMenuItem";
            this.settingsMenuItem.Size = new System.Drawing.Size(194, 22);
            this.settingsMenuItem.Text = "&Settings...";
            // 
            // helpItem
            // 
            this.helpItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testCentricHelpMenuItem,
            this.nunitHelpMenuItem,
            this.helpMenuSeparator1,
            this.aboutMenuItem});
            this.helpItem.Name = "helpItem";
            this.helpItem.Size = new System.Drawing.Size(44, 20);
            this.helpItem.Text = "&Help";
            // 
            // testCentricHelpMenuItem
            // 
            this.testCentricHelpMenuItem.Name = "testCentricHelpMenuItem";
            this.testCentricHelpMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.testCentricHelpMenuItem.Size = new System.Drawing.Size(177, 22);
            this.testCentricHelpMenuItem.Text = "TestCentric ...";
            // 
            // nunitHelpMenuItem
            // 
            this.nunitHelpMenuItem.Name = "nunitHelpMenuItem";
            this.nunitHelpMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F1)));
            this.nunitHelpMenuItem.Size = new System.Drawing.Size(177, 22);
            this.nunitHelpMenuItem.Text = "NUnit ...";
            // 
            // helpMenuSeparator1
            // 
            this.helpMenuSeparator1.Name = "helpMenuSeparator1";
            this.helpMenuSeparator1.Size = new System.Drawing.Size(174, 6);
            // 
            // aboutMenuItem
            // 
            this.aboutMenuItem.Name = "aboutMenuItem";
            this.aboutMenuItem.Size = new System.Drawing.Size(177, 22);
            this.aboutMenuItem.Text = "&About TestCentric...";
            // 
            // treeSplitter
            // 
            this.treeSplitter.Location = new System.Drawing.Point(240, 24);
            this.treeSplitter.MinSize = 240;
            this.treeSplitter.Name = "treeSplitter";
            this.treeSplitter.Size = new System.Drawing.Size(6, 383);
            this.treeSplitter.TabIndex = 2;
            this.treeSplitter.TabStop = false;
            // 
            // rightPanel
            // 
            this.rightPanel.BackColor = System.Drawing.SystemColors.Control;
            this.rightPanel.Controls.Add(this.resultTabs);
            this.rightPanel.Controls.Add(this.groupBox1);
            this.rightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightPanel.Location = new System.Drawing.Point(246, 24);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Size = new System.Drawing.Size(498, 383);
            this.rightPanel.TabIndex = 3;
            // 
            // resultTabs
            // 
            this.resultTabs.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.resultTabs.Controls.Add(this.errorTab);
            this.resultTabs.Controls.Add(this.notrunTab);
            this.resultTabs.Controls.Add(this.outputTab);
            this.resultTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultTabs.Location = new System.Drawing.Point(0, 120);
            this.resultTabs.Name = "resultTabs";
            this.resultTabs.SelectedIndex = 0;
            this.resultTabs.Size = new System.Drawing.Size(498, 263);
            this.resultTabs.TabIndex = 2;
            // 
            // errorTab
            // 
            this.errorTab.Controls.Add(this.errorsAndFailuresView1);
            this.errorTab.Location = new System.Drawing.Point(4, 4);
            this.errorTab.Name = "errorTab";
            this.errorTab.Size = new System.Drawing.Size(490, 237);
            this.errorTab.TabIndex = 0;
            this.errorTab.Text = "Errors and Failures";
            this.errorTab.UseVisualStyleBackColor = true;
            // 
            // errorsAndFailuresView1
            // 
            this.errorsAndFailuresView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorsAndFailuresView1.EnableToolTips = false;
            this.errorsAndFailuresView1.Location = new System.Drawing.Point(0, 0);
            this.errorsAndFailuresView1.Name = "errorsAndFailuresView1";
            this.errorsAndFailuresView1.Size = new System.Drawing.Size(490, 237);
            this.errorsAndFailuresView1.SourceCodeDisplay = true;
            this.errorsAndFailuresView1.SourceCodeSplitOrientation = System.Windows.Forms.Orientation.Vertical;
            this.errorsAndFailuresView1.SourceCodeSplitterDistance = 0.3F;
            this.errorsAndFailuresView1.SplitterPosition = 128;
            this.errorsAndFailuresView1.TabIndex = 0;
            // 
            // notrunTab
            // 
            this.notrunTab.Controls.Add(this.testsNotRunView1);
            this.notrunTab.Location = new System.Drawing.Point(4, 4);
            this.notrunTab.Name = "notrunTab";
            this.notrunTab.Size = new System.Drawing.Size(490, 237);
            this.notrunTab.TabIndex = 1;
            this.notrunTab.Text = "Tests Not Run";
            this.notrunTab.UseVisualStyleBackColor = true;
            // 
            // testsNotRunView1
            // 
            this.testsNotRunView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testsNotRunView1.Location = new System.Drawing.Point(0, 0);
            this.testsNotRunView1.Name = "testsNotRunView1";
            this.testsNotRunView1.Size = new System.Drawing.Size(490, 237);
            this.testsNotRunView1.TabIndex = 0;
            // 
            // outputTab
            // 
            this.outputTab.Controls.Add(this.textOutputView1);
            this.outputTab.Location = new System.Drawing.Point(4, 4);
            this.outputTab.Name = "outputTab";
            this.outputTab.Size = new System.Drawing.Size(490, 237);
            this.outputTab.TabIndex = 2;
            this.outputTab.Text = "Text Output";
            this.outputTab.UseVisualStyleBackColor = true;
            // 
            // textOutputView1
            // 
            this.textOutputView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textOutputView1.Location = new System.Drawing.Point(0, 0);
            this.textOutputView1.Name = "textOutputView1";
            this.textOutputView1.Size = new System.Drawing.Size(490, 237);
            this.textOutputView1.TabIndex = 0;
            this.textOutputView1.WordWrap = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.runSummary);
            this.groupBox1.Controls.Add(this.forceStopButton);
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
            // runSummary
            // 
            this.runSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.runSummary.AutoEllipsis = true;
            this.runSummary.Location = new System.Drawing.Point(8, 89);
            this.runSummary.Name = "runSummary";
            this.runSummary.Size = new System.Drawing.Size(480, 21);
            this.runSummary.TabIndex = 5;
            // 
            // forceStopButton
            // 
            this.forceStopButton.Location = new System.Drawing.Point(75, 16);
            this.forceStopButton.Name = "forceStopButton";
            this.forceStopButton.Size = new System.Drawing.Size(75, 31);
            this.forceStopButton.TabIndex = 6;
            this.forceStopButton.Text = "Force Stop";
            this.forceStopButton.UseVisualStyleBackColor = true;
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
            // categoryView
            // 
            this.categoryView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.categoryView.Location = new System.Drawing.Point(0, 0);
            this.categoryView.Name = "categoryView";
            this.categoryView.Size = new System.Drawing.Size(213, 375);
            this.categoryView.TabIndex = 0;
            // 
            // leftPanel
            // 
            this.leftPanel.Controls.Add(this.tabs);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftPanel.Location = new System.Drawing.Point(0, 24);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size(240, 383);
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
            this.tabs.Size = new System.Drawing.Size(240, 383);
            this.tabs.TabIndex = 0;
            // 
            // testPage
            // 
            this.testPage.Controls.Add(this.testPanel);
            this.testPage.Location = new System.Drawing.Point(23, 4);
            this.testPage.Name = "testPage";
            this.testPage.Size = new System.Drawing.Size(213, 375);
            this.testPage.TabIndex = 0;
            this.testPage.Text = "Tests";
            // 
            // testPanel
            // 
            this.testPanel.Controls.Add(this.treeView);
            this.testPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testPanel.Location = new System.Drawing.Point(0, 0);
            this.testPanel.Name = "testPanel";
            this.testPanel.Size = new System.Drawing.Size(213, 375);
            this.testPanel.TabIndex = 0;
            // 
            // treeView
            // 
            this.treeView.AllowDrop = true;
            this.treeView.AlternateImageSet = null;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(213, 375);
            this.treeView.TabIndex = 0;
            // 
            // categoryPage
            // 
            this.categoryPage.Controls.Add(this.categoryPanel);
            this.categoryPage.Location = new System.Drawing.Point(23, 4);
            this.categoryPage.Name = "categoryPage";
            this.categoryPage.Size = new System.Drawing.Size(213, 375);
            this.categoryPage.TabIndex = 1;
            this.categoryPage.Text = "Categories";
            // 
            // categoryPanel
            // 
            this.categoryPanel.Controls.Add(this.categoryView);
            this.categoryPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.categoryPanel.Location = new System.Drawing.Point(0, 0);
            this.categoryPanel.Name = "categoryPanel";
            this.categoryPanel.Size = new System.Drawing.Size(213, 375);
            this.categoryPanel.TabIndex = 0;
            // 
            // runAsX86MenuItem
            // 
            this.runAsX86MenuItem.Name = "runAsX86MenuItem";
            this.runAsX86MenuItem.Size = new System.Drawing.Size(180, 22);
            this.runAsX86MenuItem.Text = "Run as X86";
            // 
            // TestCentricMainView
            // 
            this.ClientSize = new System.Drawing.Size(744, 431);
            this.Controls.Add(this.rightPanel);
            this.Controls.Add(this.treeSplitter);
            this.Controls.Add(this.leftPanel);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.mainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size(160, 32);
            this.Name = "TestCentricMainView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "TestCentric Runner for NUnit";
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.rightPanel.ResumeLayout(false);
            this.resultTabs.ResumeLayout(false);
            this.errorTab.ResumeLayout(false);
            this.notrunTab.ResumeLayout(false);
            this.outputTab.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.leftPanel.ResumeLayout(false);
            this.tabs.ResumeLayout(false);
            this.testPage.ResumeLayout(false);
            this.testPanel.ResumeLayout(false);
            this.categoryPage.ResumeLayout(false);
            this.categoryPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Events and Properties

        public event EventHandler SplitterPositionChanged;

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

        public int SplitterPosition
        { 
            get { return treeSplitter.SplitPosition; }
            set { treeSplitter.SplitPosition = value; }
        }

        // UI Elements
        public ICommand RunButton { get; }
        public ICommand StopButton { get; }
        public ICommand ForceStopButton { get; }
        public IControlElement RunSummary { get; }
        public ISelection ResultTabs { get; }

        // File Menu Items
        public IPopup FileMenu { get; }
        public ICommand OpenCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand AddTestFilesCommand { get; }
        public ICommand ReloadTestsCommand { get; }
        public IPopup RuntimeMenu { get; }
        public IChecked RunAsX86 { get; private set; }
        public IPopup RecentFilesMenu { get; }
        public ICommand ExitCommand { get; }

        // View Menu Items
        public ISelection DisplayFormat { get; }
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
        public ICommand ForceStopCommand { get; }
        public ICommand TestParametersCommand { get; }

        // Tools Menu Items
        public IPopup ToolsMenu { get; }
        public ICommand SaveResultsCommand { get; }
        public IPopup SaveResultsAsMenu { get; }
        public ICommand OpenWorkDirectoryCommand { get; }
        public ICommand ExtensionsCommand { get; }
        public ICommand SettingsCommand { get; }

        // Help Menu Items
        public ICommand TestCentricHelpCommand { get; }
        public ICommand NUnitHelpCommand { get; }
        public ICommand AboutCommand { get; }

        public IDialogManager DialogManager { get; }
        public ILongRunningOperationDisplay LongRunningOperation { get;  }

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

        #region Overrides

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            runSummary.Font = MakeBold(Font);
        }

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

        #region Helper Methods

        /// <summary>
        /// Set the title bar based on the loaded file or project
        /// </summary>
        /// <param name="fileName"></param>
        public void SetTitleBar(string fileName)
        {
            Text = fileName == null
                ? "NUnit"
                : string.Format("{0} - NUnit", Path.GetFileName(fileName));
        }

        private static Font MakeBold(Font font)
        {
            return font.FontFamily.IsStyleAvailable(FontStyle.Bold)
                       ? new Font(font, FontStyle.Bold) : font;
        }

        #endregion
    }
}

