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
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;
using System.Windows.Forms;
using System.IO;
using NUnit.Engine;

namespace TestCentric.Gui
{
    using Controls;
    using Model;
    using Model.Settings;

    public class TestCentricMainForm : TestCentricFormBase
    {
        static Logger log = InternalTrace.GetLogger(typeof(TestCentricMainForm));

        #region Instance variables

        // Handlers for our recentFiles and recentProjects
        private RecentFileMenuHandler _recentProjectsMenuHandler;

        private string _displayFormat = "Full";

        private LongRunningOperationDisplay _longOpDisplay;

        private System.Drawing.Font _fixedFont;

        private System.ComponentModel.IContainer components;

        private System.Windows.Forms.Panel leftPanel;
        public System.Windows.Forms.Splitter treeSplitter;
        public System.Windows.Forms.Panel rightPanel;

        private TestTree testTree;

        public System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.Button runButton;
        private System.Windows.Forms.Button stopButton;
        public TestProgressBar progressBar;
        private ExpandingLabel runCount;

        public ResultTabs resultTabs;

        public StatusBar statusBar;

        public System.Windows.Forms.ToolTip toolTip;

        public System.Windows.Forms.MainMenu mainMenu;

        public System.Windows.Forms.MenuItem fileMenu;
        private System.Windows.Forms.MenuItem openMenuItem;
        private System.Windows.Forms.MenuItem recentProjectsMenu;
        private System.Windows.Forms.MenuItem fileMenuSeparator1;
        public System.Windows.Forms.MenuItem fileMenuSeparator4;
        private System.Windows.Forms.MenuItem closeMenuItem;
        public System.Windows.Forms.MenuItem exitMenuItem;

        private System.Windows.Forms.MenuItem toolsMenu;
        private System.Windows.Forms.MenuItem settingsMenuItem;
        private System.Windows.Forms.MenuItem saveResultsMenuItem;

        public System.Windows.Forms.MenuItem nunitHelpMenuItem;
        public System.Windows.Forms.MenuItem helpItem;
        public System.Windows.Forms.MenuItem helpMenuSeparator1;
        public System.Windows.Forms.MenuItem aboutMenuItem;
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
        private ExpandingLabel suiteName;

        #endregion

        #region Construction and Disposal

        public TestCentricMainForm(ITestModel model, CommandLineOptions options) : base("NUnit")
        {
            InitializeComponent();

            Model = model;
            Presenter = new TestCentricPresenter(this, model, options);
            Options = options;

            UserSettings = Model.Services.UserSettings;
            RecentFiles = Model.Services.RecentFiles;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestCentricMainForm));
            this.statusBar = new TestCentric.Gui.Controls.StatusBar();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.fileMenu = new System.Windows.Forms.MenuItem();
            this.openMenuItem = new System.Windows.Forms.MenuItem();
            this.closeMenuItem = new System.Windows.Forms.MenuItem();
            this.addTestFileMenuItem = new System.Windows.Forms.MenuItem();
            this.fileMenuSeparator1 = new System.Windows.Forms.MenuItem();
            this.reloadTestsMenuItem = new System.Windows.Forms.MenuItem();
            this.runtimeMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.recentProjectsMenu = new System.Windows.Forms.MenuItem();
            this.fileMenuSeparator4 = new System.Windows.Forms.MenuItem();
            this.exitMenuItem = new System.Windows.Forms.MenuItem();
            this.viewMenu = new System.Windows.Forms.MenuItem();
            this.fullGuiMenuItem = new System.Windows.Forms.MenuItem();
            this.miniGuiMenuItem = new System.Windows.Forms.MenuItem();
            this.viewMenuSeparator1 = new System.Windows.Forms.MenuItem();
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
            this.suiteName = new TestCentric.Gui.Controls.ExpandingLabel();
            this.runCount = new TestCentric.Gui.Controls.ExpandingLabel();
            this.stopButton = new System.Windows.Forms.Button();
            this.runButton = new System.Windows.Forms.Button();
            this.progressBar = new TestCentric.Gui.Controls.TestProgressBar();
            this.resultTabs = new TestCentric.Gui.Controls.ResultTabs();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.testTree = new TestCentric.Gui.Controls.TestTree();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.rightPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.leftPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusBar
            // 
            this.statusBar.DisplayTestProgress = true;
            this.statusBar.Location = new System.Drawing.Point(0, 407);
            this.statusBar.Name = "statusBar";
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = new System.Drawing.Size(744, 24);
            this.statusBar.TabIndex = 0;
            this.statusBar.Text = "Status";
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
            this.recentProjectsMenu,
            this.fileMenuSeparator4,
            this.exitMenuItem});
            this.fileMenu.Text = "&File";
            this.fileMenu.Popup += new System.EventHandler(this.fileMenu_Popup);
            // 
            // openMenuItem
            // 
            this.openMenuItem.Index = 0;
            this.openMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.openMenuItem.Text = "&Open...";
            this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
            // 
            // closeMenuItem
            // 
            this.closeMenuItem.Index = 1;
            this.closeMenuItem.Text = "&Close";
            this.closeMenuItem.Click += new System.EventHandler(this.closeMenuItem_Click);
            // 
            // addTestFileMenuItem
            // 
            this.addTestFileMenuItem.Index = 2;
            this.addTestFileMenuItem.Text = "&Add Test File...";
            this.addTestFileMenuItem.Click += new System.EventHandler(this.addTestFileMenuItem_Click);
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
            this.reloadTestsMenuItem.Click += new System.EventHandler(this.reloadTestsMenuItem_Click);
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
            // recentProjectsMenu
            // 
            this.recentProjectsMenu.Index = 7;
            this.recentProjectsMenu.Text = "Recent &Files";
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
            // viewMenuSeparator2
            // 
            this.viewMenuSeparator2.Index = 3;
            this.viewMenuSeparator2.Text = "-";
            // 
            // guiFontMenuItem
            // 
            this.guiFontMenuItem.Index = 4;
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
            this.fixedFontMenuItem.Index = 5;
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
            this.viewMenuSeparator3.Index = 6;
            this.viewMenuSeparator3.Text = "-";
            // 
            // statusBarMenuItem
            // 
            this.statusBarMenuItem.Checked = true;
            this.statusBarMenuItem.Index = 7;
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
            this.runAllMenuItem.Click += new System.EventHandler(this.runAllMenuItem_Click);
            // 
            // runSelectedMenuItem
            // 
            this.runSelectedMenuItem.Index = 1;
            this.runSelectedMenuItem.Shortcut = System.Windows.Forms.Shortcut.F6;
            this.runSelectedMenuItem.Text = "Run &Selected";
            this.runSelectedMenuItem.Click += new System.EventHandler(this.runSelectedMenuItem_Click);
            // 
            // runFailedMenuItem
            // 
            this.runFailedMenuItem.Enabled = false;
            this.runFailedMenuItem.Index = 2;
            this.runFailedMenuItem.Shortcut = System.Windows.Forms.Shortcut.F7;
            this.runFailedMenuItem.Text = "Run &Failed";
            this.runFailedMenuItem.Click += new System.EventHandler(this.runFailedMenuItem_Click);
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
            this.stopRunMenuItem.Click += new System.EventHandler(this.stopRunMenuItem_Click);
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
            this.projectEditorMenuItem.Click += new System.EventHandler(this.projectEditorMenuItem_Click);
            // 
            // saveResultsMenuItem
            // 
            this.saveResultsMenuItem.Index = 1;
            this.saveResultsMenuItem.Text = "&Save Test Results...";
            this.saveResultsMenuItem.Click += new System.EventHandler(this.saveResultsMenuItem_Click);
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
            this.settingsMenuItem.Click += new System.EventHandler(this.settingsMenuItem_Click);
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
            this.rightPanel.Controls.Add(this.resultTabs);
            this.rightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightPanel.Location = new System.Drawing.Point(246, 0);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Size = new System.Drawing.Size(498, 407);
            this.rightPanel.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.suiteName);
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
            // suiteName
            // 
            this.suiteName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.suiteName.AutoEllipsis = true;
            this.suiteName.Location = new System.Drawing.Point(145, 21);
            this.suiteName.Name = "suiteName";
            this.suiteName.Size = new System.Drawing.Size(343, 23);
            this.suiteName.TabIndex = 1;
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
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // runButton
            // 
            this.runButton.Location = new System.Drawing.Point(8, 16);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(64, 31);
            this.runButton.TabIndex = 3;
            this.runButton.Text = "&Run";
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
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
            this.progressBar.Maximum = 100;
            this.progressBar.Minimum = 0;
            this.progressBar.Name = "progressBar";
            this.progressBar.Segmented = true;
            this.progressBar.Size = new System.Drawing.Size(480, 28);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 0;
            this.progressBar.Value = 0;
            // 
            // resultTabs
            // 
            this.resultTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultTabs.Location = new System.Drawing.Point(0, 120);
            this.resultTabs.Name = "resultTabs";
            this.resultTabs.Size = new System.Drawing.Size(498, 284);
            this.resultTabs.TabIndex = 2;
            // 
            // testTree
            // 
            this.testTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testTree.Location = new System.Drawing.Point(0, 0);
            this.testTree.Name = "testTree";
            this.testTree.ShowCheckBoxes = false;
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
            // TestCentricMainForm
            // 
            this.ClientSize = new System.Drawing.Size(744, 431);
            this.Controls.Add(this.rightPanel);
            this.Controls.Add(this.treeSplitter);
            this.Controls.Add(this.leftPanel);
            this.Controls.Add(this.statusBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size(160, 32);
            this.Name = "TestCentricMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "TestCentric Runner for NUnit";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.NUnitForm_Closing);
            this.Load += new System.EventHandler(this.NUnitForm_Load);
            this.rightPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.leftPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        #region Properties

        public TestCentricPresenter Presenter { get; set; }

        public TestNode[] SelectedTests
        {
            get { return testTree.SelectedTests; }
        }

        public TestNode[] FailedTests
        {
            get { return testTree.FailedTests; }
        }

        private CommandLineOptions Options {  get; }

        private ITestModel Model { get; }

        private UserSettings UserSettings { get; }

        private IRecentFiles RecentFiles { get; }

        #endregion

        #region Display Methods

        public void DisplaySummary(string text)
        {
            runCount.Text = text;
        }

        #endregion

        #region Menu Handlers

        #region File Menu

        private void fileMenu_Popup(object sender, System.EventArgs e)
        {
            openMenuItem.Enabled = !Model.IsTestRunning;
            closeMenuItem.Enabled = Model.IsPackageLoaded && !Model.IsTestRunning;

            reloadTestsMenuItem.Enabled = Model.IsPackageLoaded && !Model.IsTestRunning;

            var frameworks = Model.AvailableRuntimes;

            runtimeMenuItem.Visible = frameworks.Count > 1;

            if (runtimeMenuItem.Visible && runtimeMenuItem.Enabled && runtimeMenuItem.MenuItems.Count == 0)
            {
                var defaultMenuItem = new MenuItem("Default");
                defaultMenuItem.Name = "defaultMenuItem";
                defaultMenuItem.Tag = "DEFAULT";
                defaultMenuItem.Checked = true;
                
                runtimeMenuItem.MenuItems.Add(defaultMenuItem);

                foreach (IRuntimeFramework framework in frameworks)
                {
                    MenuItem item = new MenuItem(framework.DisplayName);
                    item.Tag = framework;
                    item.Click += new EventHandler(runtimeFrameworkMenuItem_Click);

                    runtimeMenuItem.MenuItems.Add(item);
                }
            }

            recentProjectsMenu.Enabled = !Model.IsTestRunning;

            if (!Model.IsTestRunning)
            {
                _recentProjectsMenuHandler.Load();
            }
        }

        private void openMenuItem_Click(object sender, System.EventArgs e)
        {
            Presenter.OpenProject();
        }

        private void closeMenuItem_Click(object sender, System.EventArgs e)
        {
            Presenter.CloseProject();
        }

        private void addTestFileMenuItem_Click(object sender, System.EventArgs e)
        {
            Presenter.AddTestFile();
        }

        private void runtimeFrameworkMenuItem_Click(object sender, System.EventArgs e)
        {
            //Presenter.ReloadProject(((MenuItem)sender).Tag as RuntimeFramework);
        }

        private void reloadTestsMenuItem_Click(object sender, System.EventArgs e)
        {
            Presenter.ReloadTests();
        }

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

        private void applyFixedFont( Font font )
        {
            UserSettings.Gui.FixedFont = _fixedFont = font;
        }
        #endregion

        #region Test Menu

        private void runAllMenuItem_Click(object sender, System.EventArgs e)
        {
            Presenter.RunAllTests();
        }

        private void runSelectedMenuItem_Click(object sender, System.EventArgs e)
        {
            Presenter.RunSelectedTests();
        
        }

        private void runFailedMenuItem_Click(object sender, System.EventArgs e)
        {
            Presenter.RunFailedTests();
        }

        private void stopRunMenuItem_Click(object sender, System.EventArgs e)
        {
            CancelRun();
        }

        #endregion

        #region Tools Menu

        private void toolsMenu_Popup(object sender, EventArgs e)
        {
            projectEditorMenuItem.Enabled = File.Exists(Model.ProjectEditorPath);

        }

        private void projectEditorMenuItem_Click(object sender, EventArgs e)
        {
            Presenter.DisplayProjectEditor();
        }

        private void saveResultsMenuItem_Click(object sender, System.EventArgs e)
        {
            Presenter.SaveResults();
        }

        private void extensionsMenuItem_Click(object sender, EventArgs e)
        {
            using (var extensionsDialog = new ExtensionDialog(Model.Services.ExtensionService))
            {
                Site.Container.Add(extensionsDialog);
                extensionsDialog.ShowDialog();
            }
        }

        private void settingsMenuItem_Click(object sender, System.EventArgs e)
        {
            Presenter.DisplaySettings();
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
                Site.Container.Add( aboutBox );
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
                // TODO: Can the controls add its own menu?
                viewMenu.MenuItems.Add(3, testTree.TreeMenu);

                EnableRunCommand( false );
                EnableStopCommand( false );

                _recentProjectsMenuHandler = new RecentFileMenuHandler(recentProjectsMenu, Model, File.Exists);

                LoadFormSettings();

                // Force display  so that any "Loading..." or error 
                // message overlays the main form.
                Show();
                Invalidate();
                Update();

                SubscribeToTestEvents();

                Presenter.OnStartup();
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

                        statusBar.SizingGrip = true;
                    }
                    else if ( WindowState == FormWindowState.Maximized )
                    {
                        //userSettings.SaveSetting( "Gui.MainForm.Maximized", true );

                        statusBar.SizingGrip = false;
                    }
                    break;
                case "Mini":
                    if ( WindowState == FormWindowState.Normal )
                    {
                        UserSettings.Gui.MiniForm.Left = Location.X;
                        UserSettings.Gui.MiniForm.Top = Location.Y;
                        UserSettings.Gui.MiniForm.Maximized = false;

                        statusBar.SizingGrip = true;
                    }
                    else if ( WindowState == FormWindowState.Maximized )
                    {
                        UserSettings.Gui.MiniForm.Maximized = true;

                        statusBar.SizingGrip = false;
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

        /// <summary>
        ///	Form is about to close, first see if we 
        ///	have a test run going on and if so whether
        ///	we should cancel it. Then unload the 
        ///	test and save the latest form position.
        /// </summary>
        private void NUnitForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Model.IsPackageLoaded)
            {
                if (Model.IsTestRunning) // TODO: Job for Presenter?
                {
                    DialogResult dialogResult = MessageDisplay.Ask(
                        "A test is running, do you want to stop the test and exit?");

                    if (dialogResult == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }

                    Model.CancelTestRun();
                }

                if (Presenter.CloseProject() == DialogResult.Cancel)
                    e.Cancel = true;
            }
        }

        #endregion

        #region Other UI Event Handlers

        /// <summary>
        /// When the Run Button is clicked, run the selected test.
        /// </summary>
        private void runButton_Click(object sender, System.EventArgs e)
        {
            Presenter.RunSelectedTests();
        }

        /// <summary>
        /// When the Stop Button is clicked, cancel running test
        /// </summary>
        private void stopButton_Click(object sender, System.EventArgs e)
        {
            CancelRun();
        }

        private void CancelRun()
        {
            EnableStopCommand( false );

            if (Model.IsTestRunning)
            {
                DialogResult dialogResult = MessageDisplay.Ask(
                    "Do you want to cancel the running test?");

                if (dialogResult == DialogResult.No)
                    EnableStopCommand(true);
                else
                    Model.CancelTestRun();
            }
        }

        private void testTree_SelectedTestsChanged(object sender, SelectedTestsChangedEventArgs e)
        {
            if (!Model.IsTestRunning)
            {
                suiteName.Text = e.TestName;
                statusBar.Initialize(e.TestCount, e.TestName);
            }
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

        //private void OnTestLoadStarting(TestFilesLoadingEventArgs e)
        //{
        //    Presenter.EnableRunCommand( false );
        //    longOpDisplay = new LongRunningOperationDisplay( this, "Loading..." );
        //}

        //private void OnTestUnloadStarting( object sender, TestEventArgs e )
        //{
        //    Presenter.EnableRunCommand( false );
        //}

        //private void OnReloadStarting( object sender, TestEventArgs e )
        //{
        //    Presenter.EnableRunCommand( false );
        //    longOpDisplay = new LongRunningOperationDisplay( this, "Reloading..." );
        //}

        ///// <summary>
        ///// A test suite has been loaded, so update 
        ///// recent assemblies and display the tests in the UI
        ///// </summary>
        //private void OnTestLoaded( TestNodeEventArgs e )
        //{
        //    if ( longOpDisplay != null )
        //    {
        //        longOpDisplay.Dispose();
        //        longOpDisplay = null;
        //    }
        //    Presenter.EnableRunCommand( true );

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
        //    suiteName.Text = null;
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

        public void EnableRunCommand( bool enable )
        {
            runButton.Enabled = enable;
            runAllMenuItem.Enabled = enable;
            runSelectedMenuItem.Enabled = enable;
            runFailedMenuItem.Enabled = enable && Model.HasResults && testTree.FailedTests != null;
        }

        public void EnableStopCommand( bool enable )
        {
            stopButton.Enabled = enable;
            stopRunMenuItem.Enabled = enable;
        }

        private void SubscribeToTestEvents()
        {
            Model.Events.RunStarting += (RunStartingEventArgs e) =>
            {
                //suiteName.Text = e.Name;
                EnableRunCommand(false);
                EnableStopCommand(true);
                saveResultsMenuItem.Enabled = false;
            };

            Model.Events.RunFinished += (TestResultEventArgs e) =>
            {
                EnableStopCommand(false);

                //if ( e.Exception != null )
                //{
                //    if ( ! ( e.Exception is System.Threading.ThreadAbortException ) )
                //        MessageDisplay.Error("NUnit Test Run Failed", e.Exception);
                //}

                ResultSummary summary = ResultSummaryCreator.FromResultNode(e.Result);
                DisplaySummary(string.Format(
                    "Passed: {0}   Failed: {1}   Errors: {2}   Inconclusive: {3}   Invalid: {4}   Ignored: {5}   Skipped: {6}   Time: {7}",
                    summary.PassCount, summary.FailedCount, summary.ErrorCount, summary.InconclusiveCount, summary.InvalidCount, summary.IgnoreCount, summary.SkipCount, summary.Duration));

                //string resultPath = Path.Combine(TestProject.BasePath, "TestResult.xml");
                //try
                //{
                //    TestLoader.SaveLastResult(resultPath);
                //    log.Debug("Saved result to {0}", resultPath);
                //}
                //catch (Exception ex)
                //{
                //    log.Warning("Unable to save result to {0}\n{1}", resultPath, ex.ToString());
                //}

                EnableRunCommand(true);
                saveResultsMenuItem.Enabled = true;

                if (e.Result.Outcome.Status == TestStatus.Failed)
                    Activate();
            };

            Model.Events.TestsLoading += (TestFilesLoadingEventArgs e) =>
            {
                EnableRunCommand(false);
                saveResultsMenuItem.Enabled = false;
                _longOpDisplay = new LongRunningOperationDisplay(this, "Loading...");
            };

            Model.Events.TestLoaded += (TestNodeEventArgs e) =>
            {
                if (_longOpDisplay != null)
                {
                    _longOpDisplay.Dispose();
                    _longOpDisplay = null;
                }

                EnableRunCommand(true);
                saveResultsMenuItem.Enabled = false;

                foreach (var assembly in Model.TestAssemblies)
                    if (assembly.RunState == RunState.NotRunnable)
                        MessageDisplay.Error(assembly.GetProperty("_SKIPREASON"));

                //if ( TestLoader.TestCount == 0 )
                //{
                //    foreach( TestAssemblyInfo info in TestLoader.AssemblyInfo )
                //        if ( info.TestFrameworks.Count > 0 ) return;

                //    MessageDisplay.Error("This assembly was not built with any known testing framework.");
                //}
            };

            //Model.Events.TestLoadFailed += new TestEventHandler(OnTestLoadFailure);
            //Model.Events.TestsUnloading += new TestEventHandler(OnTestUnloadStarting);

            Model.Events.TestUnloaded += (TestEventArgs) =>
            {
                suiteName.Text = null;
                runCount.Text = null;
                EnableRunCommand(false);
                saveResultsMenuItem.Enabled = false;
                Refresh();
            };

            //Model.Events.TestReloading += new TestEventHandler(OnReloadStarting);

            Model.Events.TestReloaded += (TestNodeEventArgs ea) =>
            {
                //SetTitleBar(TestProject.Name);

                if (_longOpDisplay != null)
                {
                    _longOpDisplay.Dispose();
                    _longOpDisplay = null;
                }

                if (UserSettings.Gui.ClearResultsOnReload)
                    runCount.Text = null;

                EnableRunCommand(true);
                saveResultsMenuItem.Enabled = false;
            };
        }

        #endregion
    }
}

