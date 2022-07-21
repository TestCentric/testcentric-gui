// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace TestCentric.Gui
{
    /// <summary>
    /// The VisualState class holds the latest visual state for a project.
    /// </summary>
    [Serializable]
    public class VisualState
    {
        // Default constructor is required for serialization
        public VisualState() { }

        public VisualState(string strategyID)
        {
            DisplayStrategy = strategyID;
        }

        public VisualState(string strategyID, string groupID)
        {
            if (strategyID == "NUNIT_TREE")
                throw new ArgumentException("When strategy is NUNIT_TREE, groupID may not be specified", nameof(groupID));

            DisplayStrategy = strategyID;
            GroupBy = groupID;
        }

        #region Fields

        [XmlAttribute]
        public string DisplayStrategy;

        [XmlAttribute]
        public string GroupBy;

        [XmlIgnore]
        public bool GroupBySpecified => GroupBy != null;

        [XmlAttribute, DefaultValue(false)]
        public bool ShowCheckBoxes;

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
            bool isSelectedNode = treeNode == treeNode.TreeView.SelectedNode;
            bool isTopNode = treeNode == treeNode.TreeView.TopNode;

            var visualNode = new VisualTreeNode()
            {
                Name = treeNode.Text,
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

        private void RecordVisualNode(VisualTreeNode visualNode, List<VisualTreeNode> visualNodes)
        {
            visualNodes.Add(visualNode);
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
            foreach (var visualNode in visualNodes)
            {
                Console.WriteLine($"Examining VisualNode {visualNode}");
                foreach (TreeNode treeNode in treeNodes)
                {
                    Console.WriteLine($"Examining TreeNode {treeNode}");
                    if (treeNode.Text == visualNode.Name || treeNode.Text == "Not Run")
                    {
                        Console.WriteLine($"Applying VisualNode {visualNode.Name} to TreeNode {treeNode.Text}");
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

            if (visualNode.IsTopNode)
                _topTreeNode = treeNode;

            if (visualNode.Selected)
                _selectedTreeNode = treeNode;

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

        #region Dump VisualState to Console 

        public void Dump()
        {
            var writer = new StringWriter();
            Save(writer);

            Console.WriteLine(writer.GetStringBuilder().ToString());
        }

        #endregion

        #endregion
    }

    [Serializable]
    public class VisualTreeNode
    {
        [XmlAttribute]
        public string Name;

        [XmlAttribute, DefaultValue(false)]
        public bool Expanded;

        [XmlAttribute, DefaultValue(false)]
        public bool Checked;

        [XmlAttribute, DefaultValue(false)]
        public bool Selected;

        [XmlAttribute, DefaultValue(false)]
        public bool IsTopNode;

        [XmlArrayItem("Node")]
        public List<VisualTreeNode> Nodes = new List<VisualTreeNode>();

        [XmlIgnore]
        public bool NodesSpecified => Nodes.Count > 0;

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
