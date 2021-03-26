// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.ComponentModel;
using System.Windows.Forms;
using TestCentric.Gui.Controls;

namespace TestCentric.Gui.Views
{
    using Elements;

    public partial class MainForm : Form, IMainView
    {
        public event CommandHandler MainViewClosing;
        public event DragEventHandler DragDropFiles;

        public MainForm()
        {
            InitializeComponent();

            InitializeViewElements();
        }

        private void InitializeViewElements()
        {
            // File Menu
            FileMenu = new PopupMenuElement(fileToolStripMenuItem);
            NewProjectCommand = new CommandMenuElement(newProjectToolStripMenuItem);
            OpenProjectCommand = new CommandMenuElement(openProjectToolStripMenuItem);
            CloseCommand = new CommandMenuElement(closeToolStripMenuItem);
            AddTestFilesCommand = new CommandMenuElement(addTestFilesToolStripMenuItem);
            SaveCommand = new CommandMenuElement(saveToolStripMenuItem);
            SaveAsCommand = new CommandMenuElement(saveAsToolStripMenuItem);
            SaveResultsCommand = new CommandMenuElement(saveResultsToolStripMenuItem);
            ReloadTestsCommand = new CommandMenuElement(reloadTestsToolStripMenuItem);
            SelectRuntimeMenu = new PopupMenuElement(selectRuntimeToolStripMenuItem);
            RunAsX86 = new CheckedMenuElement(loadAsX86ToolStripMenuItem);
            RecentProjectsMenu = new PopupMenuElement(recentProjectsToolStripMenuItem);
            ExitCommand = new CommandMenuElement(exitToolStripMenuItem);

            // View Menu
            FullGuiCommand = new CommandMenuElement(fullGuiToolStripMenuItem);
            MiniGuiCommand = new CommandMenuElement(miniGuiToolStripMenuItem);
            IncreaseFontCommand = new CommandMenuElement(increaseToolStripMenuItem);
            DecreaseFontCommand = new CommandMenuElement(decreaseToolStripMenuItem);
            ChangeFontCommand = new CommandMenuElement(changeToolStripMenuItem);
            RestoreFontCommand = new CommandMenuElement(restoreToolStripMenuItem);
            StatusBarCommand = new CheckedMenuElement(statusBarToolStripMenuItem);

            // Project Menu
            ProjectMenu = new PopupMenuElement(projectToolStripMenuItem);

            // Tools Menu
            SettingsCommand = new CommandMenuElement(settingsToolStripMenuItem);
            ExtensionsCommand = new CommandMenuElement(extensionsToolStripMenuItem);

            // Help Menu
            NUnitHelpCommand = new CommandMenuElement(nUnitHelpToolStripMenuItem);
            AboutNUnitCommand = new CommandMenuElement(aboutNUnitToolStripMenuItem);

            TestResult = new ControlElement(testResult);
            TestName = new ControlElement(testName);

            DialogManager = new DialogManager();
            MessageDisplay = new MessageBoxDisplay();
            LongRunningOperation = new LongRunningOperationDisplay(this);
        }

        #region Public Properties

        #region IMainView Members

        public bool IsMaximized
        {
            get { return WindowState == FormWindowState.Maximized; }
            set { WindowState = value ? FormWindowState.Maximized : FormWindowState.Normal; }
        }

        // File Menu
        public IPopup FileMenu { get; private set; }
        public ICommand NewProjectCommand { get; private set; }
        public ICommand OpenProjectCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }
        public ICommand AddTestFilesCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand SaveAsCommand { get; private set; }
        public ICommand SaveResultsCommand { get; private set; }
        public ICommand ReloadTestsCommand { get; private set; }
        public IPopup SelectRuntimeMenu { get; private set; }
        public IChecked RunAsX86 { get; private set; }
        public IPopup RecentProjectsMenu { get; private set; }
        public ICommand ExitCommand { get; private set; }

        // View Menu
        public ICommand FullGuiCommand { get; private set; }
        public ICommand MiniGuiCommand { get; private set; }
        public ICommand IncreaseFontCommand { get; private set; }
        public ICommand DecreaseFontCommand { get; private set; }
        public ICommand ChangeFontCommand { get; private set; }
        public ICommand RestoreFontCommand { get; private set; }
        public IChecked StatusBarCommand { get; private set; }

        // Project Menu
        public IToolStripMenu ProjectMenu { get; private set; }

        // Tools Menu
        public ICommand SettingsCommand { get; private set; }
        public ICommand ExtensionsCommand { get; private set; }

        // Help Menu
        public ICommand NUnitHelpCommand { get; private set; }
        public ICommand AboutNUnitCommand { get; private set; }

        public IViewElement TestResult { get; private set; }
        public IViewElement TestName { get; private set; }

        public IDialogManager DialogManager { get; private set; }
        public IMessageDisplay MessageDisplay { get; private set; }
        public ILongRunningOperationDisplay LongRunningOperation { get; private set; }

        #endregion

        #region Subordinate Views Contained in MainForm

        // NOTE that these are available to the form as actual classes rather than interfaces.

        public TestTreeView TestTreeView { get { return testTreeView; } }
        public ProgressBarView ProgressBarView { get { return progressBarView; } }
        public StatusBarView StatusBarView { get { return statusBarView; } }
        public TestPropertiesView PropertiesView { get { return propertiesView; } }
        public XmlView XmlView { get { return xmlView; } }
        public TextOutputView TextOutputView { get { return textOutputView; } }

        #endregion

        #endregion

        private void statusBarToolStripMenuItem_CheckedChanged(object sender, System.EventArgs e)
        {
            statusBarView.Visible = statusBarToolStripMenuItem.Checked;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            MainViewClosing?.Invoke();
            base.OnClosing(e);
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
            {
                drgevent.Effect = DragDropEffects.Copy;
            }
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);

            string[] files = (string[])drgevent.Data.GetData(DataFormats.FileDrop);
            if (files != null)
                DragDropFiles?.Invoke(files);
        }

        private void TabControlSelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (tabControl1.SelectedTab.Equals(tabPage2))
                XmlView.InvokeFocus();
        }
    }
}
