// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Windows.Forms;

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// TreeViewElement extends ControlElement for wrapping a TreeView.
    /// In addition to the normal shallow adapter provided by an element,
    /// TreeViewElement extends some of the functionality of TreeView:
    ///  * Checkboxes may be turned on and off dynamically, while retaining
    ///    the current visual state of the tree.
    ///  * The CheckedNodes property returns a list of checked TreeNodes.
    /// </summary>
    public class TreeViewElement : ControlElement, ITreeView
    {
        public event TreeNodeActionHandler SelectedNodeChanged;

        private TreeView _treeView;

        public TreeViewElement(TreeView treeView)
            : base(treeView)
        {
            _treeView = treeView;
            _checkBoxes = treeView.CheckBoxes;

            treeView.AfterSelect += (s, e) =>
            {
                if (SelectedNodeChanged != null)
                    SelectedNodeChanged(e.Node);
            };

            treeView.MouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    var treeNode = treeView.GetNodeAt(e.X, e.Y);
                    if (treeNode != null)
                        treeView.SelectedNode = treeNode;
                }
            };
        }

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
                        foreach (TreeNode node in _treeView.Nodes)
                            RecordExpandedNodes(expandedNodes, node);

                    InvokeIfRequired(() => { _treeView.CheckBoxes = _checkBoxes = value; });

                    if (!value)
                        foreach (var node in expandedNodes)
                            node.Expand();
                }
            }
        }

        public TreeNode TopNode
        {
            get
            {
                return _treeView.TopNode;
            }
            set
            {
                InvokeIfRequired(() => _treeView.TopNode = value);
            }
        }

        public int VisibleCount => _treeView.VisibleCount;

        public TreeNode SelectedNode
        {
            get
            {
                return _treeView.SelectedNode;
            }
            set
            {
                InvokeIfRequired(() => _treeView.SelectedNode = value);
            }
        }

        public TreeNodeCollection Nodes => _treeView.Nodes;

        public IList<TreeNode> CheckedNodes => GetCheckedNodes();

        public int GetNodeCount(bool includeSubTrees)
        {
            return _treeView.GetNodeCount(includeSubTrees);
        }

        public void Clear()
        {
            InvokeIfRequired(() => _treeView.Nodes.Clear());
        }

        public void ExpandAll()
        {
            InvokeIfRequired(() => _treeView.ExpandAll());
        }

        public void CollapseAll()
        {
            InvokeIfRequired(() => _treeView.CollapseAll());
        }

        public void Add(TreeNode treeNode)
        {
            Add(treeNode, false);
        }

        public void Load(TreeNode treeNode)
        {
            Add(treeNode, true);
        }


        public void SetImageIndex(TreeNode treeNode, int imageIndex)
        {
            InvokeIfRequired(() =>
            {
                treeNode.ImageIndex = treeNode.SelectedImageIndex = imageIndex;
            });
        }

#region Helper Methods

        private void Add(TreeNode treeNode, bool doClear)
        {
            InvokeIfRequired(() =>
            {
                if (doClear)
                    _treeView.Nodes.Clear();

                _treeView.BeginUpdate();
                _treeView.Nodes.Add(treeNode);
                if (_treeView.SelectedNode == null)
                    _treeView.SelectedNode = treeNode;
                _treeView.EndUpdate();
                _treeView.Select();
            });
        }

        private void RecordExpandedNodes(List<TreeNode> expanded, TreeNode startNode)
        {
            if (startNode.IsExpanded)
                expanded.Add(startNode);

            foreach (TreeNode node in startNode.Nodes)
                RecordExpandedNodes(expanded, node);
        }

        private IList<TreeNode> GetCheckedNodes()
        {
            var checkedNodes = new List<TreeNode>();
            foreach (TreeNode node in _treeView.Nodes)
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

#endregion
    }
}
