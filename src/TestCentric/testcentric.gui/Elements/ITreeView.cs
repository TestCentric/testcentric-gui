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
    public delegate void TreeNodeActionHandler(TreeNode treeNode);

    /// <summary>
    /// The ITreeViewElement interface provides additional methods
    /// used when wrapping a TreeView.
    /// </summary>
    public interface ITreeView : IControlElement<TreeView>
    {
        event TreeNodeActionHandler SelectedNodeChanged;

        bool CheckBoxes { get; set; }

        TreeNode TopNode { get; set; }

        TreeNode SelectedNode { get; set; }

        IList<TreeNode> Nodes { get; }
        int VisibleCount { get; }

#if EXPERIMENTAL
        IList<TreeNode> CheckedNodes { get; }
        IContextMenuElement ContextMenu { get; set; }
#endif

        void Clear();
        void ExpandAll();
        void CollapseAll();
        void Load(TreeNode treeNode);
        int GetNodeCount(bool includeSubTrees);
        void Select();

#if EXPERIMENTAL
        void Add(TreeNode treeNode);
        void SetImageIndex(TreeNode treeNode, int imageIndex);
#endif
    }
}
