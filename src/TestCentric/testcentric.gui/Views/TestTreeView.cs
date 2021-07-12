// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace TestCentric.Gui.Views
{
    using Elements;

    public partial class TestTreeView : UserControl, ITestTreeView
    {
        /// <summary>
        /// Image indices for various test states - the values 
        /// must match the indices of the image list used and
        /// are ordered so that the higher values are those
        /// that propagate upwards.
        /// </summary>
        public const int InitIndex = 0;
        public const int SkippedIndex = 0;
        public const int InconclusiveIndex = 1;
        public const int SuccessIndex = 2;
        public const int WarningIndex = 3;
        public const int FailureIndex = 4;

        public TestTreeView()
        {
            InitializeComponent();

            RunButton = new SplitButtonElement(runButton);
            RunAllCommand = new CommandMenuElement(runAllMenuItem);
            RunSelectedCommand = new CommandMenuElement(runSelectedMenuItem);
            TestParametersCommand = new CommandMenuElement(testParametersMenuItem);

            DebugButton = new SplitButtonElement(debugButton);
            DebugAllCommand = new CommandMenuElement(debugAllMenuItem);
            DebugSelectedCommand = new CommandMenuElement(debugSelectedMenuItem);

            StopRunButton = new ButtonElement(stopRunButton);
            ForceStopButton = new ButtonElement(forceStopButton);

            FormatButton = new ToolStripElement(formatButton);
            DisplayFormat = new CheckedToolStripMenuGroup(
                "displayFormat",
                nunitTreeMenuItem, fixtureListMenuItem, testListMenuItem);
            GroupBy = new CheckedToolStripMenuGroup(
                "testGrouping",
                byAssemblyMenuItem, byFixtureMenuItem, byCategoryMenuItem, byExtendedCategoryMenuItem, byOutcomeMenuItem, byDurationMenuItem);

            RunSummaryButton = new ButtonElement(runSummaryButton);

            RunContextCommand = new CommandMenuElement(this.runMenuItem);
            RunCheckedCommand = new CommandMenuElement(this.runCheckedMenuItem);
            DebugContextCommand = new CommandMenuElement(this.debugMenuItem);
            DebugCheckedCommand = new CommandMenuElement(this.debugCheckedMenuItem);
            ActiveConfiguration = new PopupMenuElement(this.activeConfigMenuItem);
            ShowCheckBoxes = new CheckedMenuElement(showCheckboxesMenuItem);
            ExpandAllCommand = new CommandMenuElement(expandAllMenuItem);
            CollapseAllCommand = new CommandMenuElement(collapseAllMenuItem);
            CollapseToFixturesCommand = new CommandMenuElement(collapseToFixturesMenuItem);

            Tree = new TreeViewElement(treeView);
            treeView.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    ContextNode = treeView.GetNodeAt(e.X, e.Y);
                }
            };

        }

        #region Properties

        public ICommand RunButton { get; private set; }
        public ICommand RunAllCommand { get; private set; }
        public ICommand RunSelectedCommand { get; private set; }
        public ICommand TestParametersCommand { get; private set; }

        public ICommand DebugButton { get; private set; }
        public ICommand DebugAllCommand { get; private set; }
        public ICommand DebugSelectedCommand { get; private set; }

        public ICommand StopRunButton { get; private set; }
        public ICommand ForceStopButton { get; private set; }

        public ICommand RunSummaryButton { get; private set; }

        public ICommand RunContextCommand { get; private set; }
        public ICommand RunCheckedCommand { get; private set; }
        public ICommand DebugContextCommand { get; private set; }
        public ICommand DebugCheckedCommand { get; private set; }
        public IToolStripMenu ActiveConfiguration { get; private set; }
        public IChecked ShowCheckBoxes { get; private set; }
        public ICommand ExpandAllCommand { get; private set; }
        public ICommand CollapseAllCommand { get; private set; }
        public ICommand CollapseToFixturesCommand { get; private set; }

        public IToolTip FormatButton { get; private set; }
        public ISelection DisplayFormat { get; private set; }
        public ISelection GroupBy { get; private set; }

        public ITreeView Tree { get; private set; }
        public TreeNode ContextNode { get; private set; }

        private string _alternateImageSet;
        public string AlternateImageSet
        {
            get { return _alternateImageSet; }
            set
            {
                _alternateImageSet = value;
                if (!string.IsNullOrEmpty(value))
                    LoadAlternateImages(value);
            }
        }

        #endregion

        #region Public Methods

        public void ExpandAll()
        {
            Tree.ExpandAll();
        }

        public void CollapseAll()
        {
            Tree.CollapseAll();
        }

        #endregion

        #region Helper Methods

        private void InvokeIfRequired(MethodInvoker _delegate)
        {
            if (treeView.InvokeRequired)
                treeView.Invoke(_delegate);
            else
                _delegate();
        }

        public void LoadAlternateImages(string imageSet)
        {
            string[] imageNames = { "Skipped", "Inconclusive", "Success", "Ignored", "Failure" };

            string imageDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                Path.Combine("Images", Path.Combine("Tree", imageSet)));

            for (int index = 0; index < imageNames.Length; index++)
                LoadAlternateImage(index, imageNames[index], imageDir);
            this.Invalidate();
            this.Refresh();
        }

        private void LoadAlternateImage(int index, string name, string imageDir)
        {
            string[] extensions = { ".png", ".jpg" };

            foreach (string ext in extensions)
            {
                string filePath = Path.Combine(imageDir, name + ext);
                if (File.Exists(filePath))
                {
                    treeImages.Images[index] = Image.FromFile(filePath);
                    break;
                }
            }
        }

        #endregion
    }
}
