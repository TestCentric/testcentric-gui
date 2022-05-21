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

        public string TopNode;

        public string SelectedNode;

        public List<string> SelectedCategories;

        public bool ExcludeCategories;

        [XmlArrayItem("Node")]
        public List<VisualTreeNode> Nodes = new List<VisualTreeNode>();
        #endregion

        #region Public Methods

        public static string GetVisualStateFileName(string testFileName)
        {
            return string.IsNullOrEmpty(testFileName)
                ? "VisualState.xml"
                : testFileName + ".VisualState.xml";
        }

        public static VisualState LoadFrom(TreeView treeView)
        {
            var visualState = new VisualState()
            {
                ShowCheckBoxes = treeView.CheckBoxes,
                TopNode = ((TestNode)treeView.TopNode?.Tag)?.Id,
                SelectedNode = ((TestNode)treeView.SelectedNode?.Tag)?.Id
            };

            foreach (TreeNode treeNode in treeView.Nodes)
                visualState.ProcessChildNode(treeNode);

            return visualState;
        }

        private void ProcessChildNode(TreeNode treeNode)
        {
            if (treeNode.IsExpanded || treeNode.Checked)
                Nodes.Add(new VisualTreeNode()
                {
                    Id = ((TestNode)treeNode.Tag)?.Id,
                    Expanded = treeNode.IsExpanded,
                    Checked = treeNode.Checked
                });

            foreach (TreeNode childNode in treeNode.Nodes)
                ProcessChildNode(childNode);
        }

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

        [XmlArrayItem("Node")]
        public VisualTreeNode[] Nodes = new VisualTreeNode[0];

        public void ApplyTo(TreeNode treeNode)
        {
            if ((string)treeNode.Tag != Id)
                throw new ArgumentException("Attempting to apply visual state to a node but Ids do not match");

            if (treeNode.IsExpanded != Expanded)
                treeNode.Toggle();

            treeNode.Checked = Checked;
        }

        // Provided for use in test output
        public override string ToString()
        {
            return $"Id={Id},Expanded={Expanded},Checked={Checked},Nodes={Nodes.Length}";
        }
    }
}
