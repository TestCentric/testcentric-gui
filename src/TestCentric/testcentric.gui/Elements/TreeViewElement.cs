// ***********************************************************************
// Copyright (c) 2015 Charlie Poole
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
    public class TreeViewElement : ControlElement<TreeView>, ITreeView
    {
        public event TreeNodeActionHandler SelectedNodeChanged;

        public TreeViewElement(TreeView treeView)
            : base(treeView)
        {
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

#if EXPERIMENTAL
       private IContextMenuElement contextMenu;
        public IContextMenuElement ContextMenu
        {
            get 
            {
                if (contextMenu == null && Control.ContextMenuStrip != null)
                    contextMenu = new ContextMenuElement(Control.ContextMenuStrip);

                return contextMenu;
            }
            set 
            {
                InvokeIfRequired(() =>
                {
                    contextMenu = value;
                    Control.ContextMenuStrip = contextMenu.Control;
                });
            }
        }
#endif

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
                        foreach (TreeNode node in Control.Nodes)
                            RecordExpandedNodes(expandedNodes, node);

                    InvokeIfRequired(() => { Control.CheckBoxes = _checkBoxes = value; });

                    if (!value)
                        foreach (var node in expandedNodes)
                            node.Expand();
                }
            }
        }

        public TreeNode TopNode
        {
            get => Control.TopNode;
            set => Control.TopNode = value;
        }

        public int VisibleCount => Control.VisibleCount;

        public TreeNode SelectedNode
        {
            get => Control.SelectedNode;
            set => Control.SelectedNode = value;
        }

        public IList<TreeNode> Nodes
        {
            get
            {
                var nodes = new List<TreeNode>();
                foreach (TreeNode node in Control.Nodes)
                    nodes.Add(node);
                return nodes;
            }
        }

        public IList<TreeNode> CheckedNodes => GetCheckedNodes();

        public int GetNodeCount(bool includeSubTrees)
        {
            return Control.GetNodeCount(includeSubTrees);
        }

        public void Clear()
        {
            InvokeIfRequired(() => Control.Nodes.Clear());
        }

        public void ExpandAll()
        {
            InvokeIfRequired(() => Control.ExpandAll());
        }

        public void CollapseAll()
        {
            InvokeIfRequired(() => Control.CollapseAll());
        }

        public void Select()
        {
            InvokeIfRequired(() => Control.Select());
        }

        public void Load(TreeNode treeNode)
        {
            Add(treeNode, true);
        }

        #if EXPERIMENTAL
        public void Add(TreeNode treeNode)
        {
            Add(treeNode, false);
        }

        public void Insert(int index, TreeNode treeNode)
        {
            InvokeIfRequired(() =>
            {
                Control.Nodes.Insert(index, treeNode);
            });
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
#endif

#region Helper Methods

        private void Add(TreeNode treeNode, bool doClear)
        {
            InvokeIfRequired(() =>
            {
                if (doClear)
                    Control.Nodes.Clear();

                Control.BeginUpdate();
                Control.Nodes.Add(treeNode);
                if (Control.SelectedNode == null)
                    Control.SelectedNode = treeNode;
                Control.EndUpdate();
                Control.Select();
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
            foreach (TreeNode node in Control.Nodes)
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
