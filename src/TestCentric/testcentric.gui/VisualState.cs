// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace TestCentric.Gui
{
    using Model;
    using Views;
    using Presenters;
    using Elements;

    /// <summary>
    /// The VisualState class holds the latest visual state for a project.
    /// </summary>
    [Serializable]
    public class VisualState
    {
        #region Fields

        [XmlAttribute]
        public bool ShowCheckBoxes;

        public VisualTreeNode SelectedNode;

        public VisualTreeNode TopNode;

        // TODO: Categories not yet supported
        //public List<string> SelectedCategories;
        //public bool ExcludeCategories;

        [XmlArrayItem("Node")]
        public List<VisualTreeNode> Nodes = new List<VisualTreeNode>();

        //private Dictionary<string, VisualTreeNode> _nodeIndex = new Dictionary<string, VisualTreeNode>();

        #endregion

        #region Public Methods

        public static string GetVisualStateFileName(string testFileName)
        {
            return string.IsNullOrEmpty(testFileName)
                ? "VisualState.xml"
                : testFileName + ".VisualState.xml";
        }

        #region Load VisualState from a Tree

        public VisualState LoadFrom(TreeView treeView)
        {
            ShowCheckBoxes = treeView.CheckBoxes;

            foreach (TreeNode treeNode in treeView.Nodes)
                ProcessTreeNode(treeNode, this.Nodes);

            return this;
        }

        private void ProcessTreeNode(TreeNode treeNode, List<VisualTreeNode> visualNodes)
        {
            // TODO: Currently, we only save testNodes, not groups
            var testNode = treeNode.Tag as TestNode;
            if (testNode != null)
            {
                bool isSelectedNode = treeNode == treeNode.TreeView.SelectedNode;
                bool isTopNode = treeNode == treeNode.TreeView.TopNode;

                var visualNode = new VisualTreeNode()
                {
                    Name = testNode.Name,
                    Expanded = treeNode.IsExpanded,
                    Checked = treeNode.Checked,
                    Selected = isSelectedNode,
                    IsTopNode = isTopNode
                };

                if (treeNode.IsExpanded || treeNode.Checked || isSelectedNode || isTopNode)
                    RecordVisualNode(visualNode, visualNodes);

                foreach (TreeNode childNode in treeNode.Nodes)
                    ProcessTreeNode(childNode, visualNode.Nodes);
            }
        }

        private void RecordVisualNode(VisualTreeNode visualNode, List<VisualTreeNode> visualNodes)
        {
            visualNodes.Add(visualNode);

            if (visualNode.Selected)
                SelectedNode = visualNode;

            if (visualNode.IsTopNode)
                TopNode = visualNode;
        }

        #endregion

        #region Apply VisualState to a Tree

        public void ApplyTo(TreeView treeView)
        {
            // TODO: Use of these members is a HACK! Should remove.
            _selectedTreeNode = null;
            _topTreeNode = null;

            treeView.CheckBoxes = ShowCheckBoxes;

            ApplyVisualNodesToTreeNodes(this.Nodes, treeView.Nodes);

            if (_selectedTreeNode != null)
                treeView.SelectedNode = _selectedTreeNode;

            if (_topTreeNode != null)
                treeView.TopNode = _topTreeNode;
        }

        private void ApplyVisualNodesToTreeNodes(List<VisualTreeNode> visualNodes, TreeNodeCollection treeNodes)
        {
            // Find matching names
            for (int i = 0; i < visualNodes.Count; i++)
            {
                var visualNode = visualNodes[i];

                foreach (TreeNode treeNode in treeNodes)
                {
                    if (treeNode.Text == visualNode.Name)
                    {
                        ApplyVisualNodeToTreeNode(visualNode, treeNode);
                        break;
                    }
                }
            }
        }

        private TreeNode _selectedTreeNode;
        private TreeNode _topTreeNode;

        private void ApplyVisualNodeToTreeNode(VisualTreeNode visualNode, TreeNode treeNode)
        {
            if (treeNode.IsExpanded != visualNode.Expanded)
                treeNode.Toggle();

            treeNode.Checked = visualNode.Checked;

            if (SelectedNode.Matches(treeNode))
                _selectedTreeNode = treeNode;

            if (TopNode.Matches(treeNode))
                _topTreeNode = treeNode;

            ApplyVisualNodesToTreeNodes(visualNode.Nodes, treeNode.Nodes);
        }

        #endregion

        #region Load VisualState from a File

        public static VisualState LoadFrom(string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                return LoadFrom(reader);
            }
        }

        public static VisualState LoadFrom(TextReader reader)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(VisualState));
            return (VisualState)serializer.Deserialize(reader);
        }

        #endregion

        #region Save to a File

        public void Save(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                Save(writer);
            }
        }

        public void Save(TextWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(GetType());
            serializer.Serialize(writer, this);
        }

        #endregion

        // Public for testing
        //public void BuildNodeIndex()
        //{
        //    _nodeIndex.Clear();

        //    foreach (var node in Nodes)
        //        IndexNodeAndChildren(node);
        //}

        //private void IndexNodeAndChildren(VisualTreeNode node)
        //{
        //    _nodeIndex.Add(node.Id, node);

        //    foreach (var child in node.Nodes)
        //        IndexNodeAndChildren(child);
        //}

        #endregion
    }

    [Serializable]
    public class VisualTreeNode
    {
        [XmlAttribute]
        public string Name;

        [XmlAttribute, System.ComponentModel.DefaultValue(false)]
        public bool Expanded;

        [XmlAttribute, System.ComponentModel.DefaultValue(false)]
        public bool Checked;

        [XmlAttribute, System.ComponentModel.DefaultValue(false)]
        public bool Selected;

        [XmlAttribute, System.ComponentModel.DefaultValue(false)]
        public bool IsTopNode;

        [XmlArrayItem("Node")]
        public List<VisualTreeNode> Nodes = new List<VisualTreeNode>();

        /// <summary>
        /// Return true if this VisualTreeNode matches a TreeNode
        /// </summary>
        public bool Matches(TreeNode treeNode)
        {
            return Name == treeNode.Text;
        }

        // Provided for use in test output
        public override string ToString()
        {
            return $"Name={Name},Expanded={Expanded},Checked={Checked},Selected={Selected},IsTopNode={IsTopNode}";
        }

        public override bool Equals(object obj)
        {
            var other = obj as VisualTreeNode;

            return
                other != null &&
                Name == other.Name &&
                Expanded == other.Expanded &&
                Checked == other.Checked &&
                Selected == other.Selected &&
                IsTopNode == other.IsTopNode;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
