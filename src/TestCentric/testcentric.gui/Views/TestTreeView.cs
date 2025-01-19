// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace TestCentric.Gui.Views
{
    using System;
    using System.Collections.Generic;
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

        public event TreeNodeActionHandler SelectedNodeChanged;
        public event TreeNodeActionHandler AfterCheck;
        public event TreeNodeActionHandler TreeNodeDoubleClick;
        public event EventHandler ContextMenuOpening;

        public TestTreeView()
        {
            InitializeComponent();

            RunContextCommand = new CommandMenuElement(this.runMenuItem);
            DebugContextCommand = new CommandMenuElement(this.debugMenuItem);
            ActiveConfiguration = new PopupMenuElement(this.activeConfigMenuItem);
            ShowCheckBoxes = new CheckedMenuElement(showCheckboxesMenuItem);
            ShowTestDuration = new CheckedMenuElement(showTestDurationMenuItem);
            ExpandAllCommand = new CommandMenuElement(expandAllMenuItem);
            CollapseAllCommand = new CommandMenuElement(collapseAllMenuItem);
            CollapseToFixturesCommand = new CommandMenuElement(collapseToFixturesMenuItem);
            TestPropertiesCommand = new CommandMenuElement(testPropertiesMenuItem);
            ViewAsXmlCommand = new CommandMenuElement(viewAsXmlMenuItem);
            OutcomeFilter = new MultiCheckedToolStripButtonGroup(new[] { filterOutcomePassedButton, filterOutcomeFailedButton, filterOutcomeWarningButton, filterOutcomeNotRunButton });
            TextFilter = new ToolStripTextBoxElement(filterTextBox, "Filter...");
            TreeView = treeView;

            // NOTE: We use MouseDown here rather than MouseUp because
            // the menu strip Opening event occurs before MouseUp.
            treeView.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    ContextNode = TreeView.GetNodeAt(e.X, e.Y);
                }
            };

            treeView.ContextMenuStrip.Opening += (s, e) =>
            {
                bool clickedOnNode = ContextNode != null;

                runMenuItem.Visible =
                debugMenuItem.Visible =
                contextMenuSeparator1.Visible =
                viewAsXmlMenuItem.Visible =
                contextMenuSeparator2.Visible = clickedOnNode;

                ContextMenuOpening?.Invoke(s, e);

                // In case presenter cancels
                if (!e.Cancel && clickedOnNode)
                    SelectedNode = ContextNode;
            };

            treeView.NodeMouseDoubleClick += (s, e) => TreeNodeDoubleClick?.Invoke(e.Node);

            treeView.AfterSelect += (s, e) => SelectedNodeChanged?.Invoke(e.Node);

            treeView.AfterCheck += (s, e) => AfterCheck?.Invoke(e.Node);
        }

        #region Properties

        private bool _checkBoxes;
        public bool CheckBoxes
        {
            get { return _checkBoxes; }
            set
            {
                if (_checkBoxes != value)
                {
                    var expandedNodes = new List<TreeNode>();

                    // Turning off checkboxes collapses everything, so we
                    // have to save and restore the expanded nodes.
                    if (!value)
                        foreach (TreeNode node in treeView.Nodes)
                            RecordExpandedNodes(expandedNodes, node);

                    InvokeIfRequired(() => { treeView.CheckBoxes = _checkBoxes = value; });

                    if (!value)
                        foreach (var node in expandedNodes)
                            node.Expand();
                }
            }
        }

        public ICommand RunContextCommand { get; private set; }
        public ICommand DebugContextCommand { get; private set; }
        public IToolStripMenu ActiveConfiguration { get; private set; }
        public IChecked ShowCheckBoxes { get; private set; }
        public IChecked ShowTestDuration { get; private set; }
        public ICommand ExpandAllCommand { get; private set; }
        public ICommand CollapseAllCommand { get; private set; }
        public ICommand CollapseToFixturesCommand { get; private set; }
        public ICommand TestPropertiesCommand { get; private set; }
        public ICommand ViewAsXmlCommand { get; private set; }

        public TreeView TreeView { get; private set; }

        public IMultiSelection OutcomeFilter { get; private set; }

        public IChanged TextFilter { get; private set; }

        public TreeNode ContextNode { get; private set; }
        public ContextMenuStrip TreeContextMenu => TreeView.ContextMenuStrip;

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

        public TreeNodeCollection Nodes => treeView.Nodes;

        public int VisibleNodeCount => treeView.VisibleCount;
        public TreeNode TopNode
        {
            get { return treeView.TopNode; }
            set { InvokeIfRequired(() => treeView.TopNode = value); }
        }

        public TreeNode SelectedNode
        {
            get { return treeView.SelectedNode; }
            set { InvokeIfRequired(() => treeView.SelectedNode = value); }
        }

        //public IList<TreeNode> SelectedNodes => treeView.SelectedNodes;

        public IList<TreeNode> CheckedNodes => GetCheckedNodes();

        #endregion

        #region Public Methods

        public void Clear() => InvokeIfRequired(() => treeView.Nodes.Clear());
        public void Add(TreeNode treeNode) => InvokeIfRequired(() =>
        {
            treeView.BeginUpdate();
            treeView.Nodes.Add(treeNode);
            if (treeView.SelectedNode == null)
                treeView.SelectedNode = treeNode;
            treeView.EndUpdate();
            treeView.Select();
        });
        public void ExpandAll() => InvokeIfRequired(() => treeView.ExpandAll());
        public void CollapseAll() => InvokeIfRequired(() => treeView.CollapseAll());
        public void SetImageIndex(TreeNode treeNode, int imageIndex) =>
            InvokeIfRequired(() => treeNode.ImageIndex = treeNode.SelectedImageIndex = imageIndex);

        public void ResetAllTreeNodeImages()
        {
            InvokeIfRequired(() => ResetAllTreeNodeImages(treeView.Nodes));
        }

        private void ResetAllTreeNodeImages(TreeNodeCollection treeNodes)
        {
            foreach (TreeNode treeNode in treeNodes)
            {
                treeNode.ImageIndex = treeNode.SelectedImageIndex = InitIndex;
                ResetAllTreeNodeImages(treeNode.Nodes);
            }
        }

        public void InvokeIfRequired(MethodInvoker _delegate)
        {
            if (treeView.InvokeRequired)
                treeView.Invoke(_delegate);
            else
                _delegate();
        }

        public void SetTestFilterVisibility(bool isVisible)
        {
            filterToolStrip.Visible = isVisible;
            filterTextToolStrip.Visible = isVisible;
        }

        public void EnableTestFilter(bool enable)
        {
            filterToolStrip.Enabled = enable;
            filterTextToolStrip.Enabled = enable;
        }

        public void LoadAlternateImages(string imageSet)
        {
            string[] imageNames = { "Skipped", "Inconclusive", "Success", "Ignored", "Failure" };
            string imageDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                Path.Combine("Images", Path.Combine("Tree", imageSet)));

            // 1. Load all images once
            IDictionary<string, Image> images = new Dictionary<string, Image>();
            foreach (string imageName in imageNames)
                images[imageName] = LoadAlternateImage(imageName, imageDir);

            // 2. Set tree images
            treeImages.Images.Clear();
            foreach (string imageName in imageNames)
                treeImages.Images.Add(imageName, images[imageName]);

            // 3. Set filter outcome toolbar images
            filterOutcomeFailedButton.Image = images["Failure"];
            filterOutcomePassedButton.Image = images["Success"];
            filterOutcomeWarningButton.Image = images["Ignored"];
            filterOutcomeNotRunButton.Image = images["Skipped"];

            this.Invalidate();
            this.Refresh();
        }

        #endregion

        #region Helper Methods

        private Image LoadAlternateImage(string name, string imageDir)
        {
            string[] extensions = { ".png", ".jpg" };

            foreach (string ext in extensions)
            {
                string filePath = Path.Combine(imageDir, name + ext);
                if (File.Exists(filePath))
                {
                    return Image.FromFile(filePath);
                }
            }

            return null;
        }

        private IList<TreeNode> GetCheckedNodes()
        {
            var checkedNodes = new List<TreeNode>();
            foreach (TreeNode node in treeView.Nodes)
                CollectCheckedNodes(checkedNodes, node);
            return checkedNodes;
        }

        private void CollectCheckedNodes(List<TreeNode> checkedNodes, TreeNode node)
        {
            if (node.Checked)
                checkedNodes.Add(node);
            else
                foreach (TreeNode child in node.Nodes)
                    CollectCheckedNodes(checkedNodes, child);
        }

        private void RecordExpandedNodes(List<TreeNode> expanded, TreeNode startNode)
        {
            if (startNode.IsExpanded)
                expanded.Add(startNode);

            foreach (TreeNode node in startNode.Nodes)
                RecordExpandedNodes(expanded, node);
        }

        #endregion
    }
}
