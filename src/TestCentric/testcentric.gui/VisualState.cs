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

        private Dictionary<string, VisualTreeNode> _nodeIndex = new Dictionary<string, VisualTreeNode>();

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
                ProcessTreeNode(treeNode);

            BuildNodeIndex();

            return this;
        }

        private void ProcessTreeNode(TreeNode treeNode)
        {
            // TODO: Currently, we only save testNodes, not groups
            var testNode = treeNode.Tag as TestNode;
            if (testNode != null)
            {
                bool isSelectedNode = treeNode == treeNode.TreeView.SelectedNode;
                bool isTopNode = treeNode == treeNode.TreeView.TopNode;

                var visualNode = new VisualTreeNode()
                {
                    Id = testNode.Id,
                    Expanded = treeNode.IsExpanded,
                    Checked = treeNode.Checked,
                    Selected = isSelectedNode,
                    IsTopNode = isTopNode
                };

                if (treeNode.IsExpanded || treeNode.Checked || isSelectedNode || isTopNode)
                    RecordVisualNode(visualNode);

                foreach (TreeNode childNode in treeNode.Nodes)
                    ProcessTreeNode(childNode);
            }
        }

        private void RecordVisualNode(VisualTreeNode visualNode)
        {
            Nodes.Add(visualNode);

            if (visualNode.Selected)
                SelectedNode = visualNode;

            if (visualNode.IsTopNode)
                TopNode = visualNode;
        }

        #endregion

        #region Apply VisualState to a Tree

        public void ApplyTo(TreeView treeView)
        {
            treeView.CheckBoxes = ShowCheckBoxes;

            foreach (TreeNode treeNode in treeView.Nodes)
                ApplyTo(treeNode);

            if (_selectedTreeNode != null)
                treeView.SelectedNode = _selectedTreeNode;

            if (_topTreeNode != null)
                treeView.TopNode = _topTreeNode;
        }

        private TreeNode _selectedTreeNode;
        private TreeNode _topTreeNode;

        private void ApplyTo(TreeNode treeNode)
        {
            var id = (treeNode.Tag as TestNode)?.Id;
            if (_nodeIndex.TryGetValue(id, out var visualNode))
            {
                if (treeNode.IsExpanded != visualNode.Expanded)
                    treeNode.Toggle();

                treeNode.Checked = visualNode.Checked;

                if (id == SelectedNode.Id)
                    _selectedTreeNode = treeNode;

                if (id == TopNode.Id)
                    _topTreeNode = treeNode;

                foreach (TreeNode child in treeNode.Nodes)
                    ApplyTo(child);
            }
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
            var result = (VisualState)serializer.Deserialize(reader);
            result.BuildNodeIndex();
            return result;
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
        public void BuildNodeIndex()
        {
            _nodeIndex.Clear();

            foreach (var node in Nodes)
                _nodeIndex.Add(node.Id, node);
        }

        #endregion
    }

    [Serializable]
    public class VisualTreeNode
    {
        [XmlAttribute]
        public string Id;

        [XmlAttribute, System.ComponentModel.DefaultValue(false)]
        public bool Expanded;

        [XmlAttribute, System.ComponentModel.DefaultValue(false)]
        public bool Checked;

        [XmlAttribute, System.ComponentModel.DefaultValue(false)]
        public bool Selected;

        [XmlAttribute, System.ComponentModel.DefaultValue(false)]
        public bool IsTopNode;

        // Provided for use in test output
        public override string ToString()
        {
            return $"Id={Id},Expanded={Expanded},Checked={Checked}, Selected={Selected}, IsTopNode={IsTopNode}";
        }

        public override bool Equals(object obj)
        {
            var other = obj as VisualTreeNode;

            return
                other != null &&
                Id == other.Id &&
                Expanded == other.Expanded &&
                Checked == other.Checked &&
                Selected == other.Selected &&
                IsTopNode == other.IsTopNode;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
