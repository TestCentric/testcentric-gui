// ***********************************************************************
// Copyright (c) 2018 Charlie Poole
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

using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TestCentric.Gui
{
    using Views;

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

        public string[] SelectedCategories;
        
        public bool ExcludeCategories;

		[XmlArrayItem("Node")]
		public List<VisualTreeNode> Nodes = new List<VisualTreeNode>();
        #endregion

        #region Static Methods

        public static string GetVisualStateFileName( string testFileName )
        {
			return string.IsNullOrEmpty(testFileName)
				? "VisualState.xml"
				: testFileName + ".VisualState.xml";
        }

        public static VisualState LoadFrom( string fileName )
        {
            using ( StreamReader reader = new StreamReader( fileName ) )
            {
                return LoadFrom( reader );
            }
        }

        public static VisualState LoadFrom( TextReader reader )
        {
            XmlSerializer serializer = new XmlSerializer( typeof( VisualState) );
            return (VisualState)serializer.Deserialize( reader );
        }

        #endregion

        #region Constructors

        public void ProcessTreeNodes(TestSuiteTreeNode node)
        {
            if (IsInteresting(node))
                this.Nodes.Add(new VisualTreeNode(node));

            foreach (TestSuiteTreeNode childNode in node.Nodes)
                ProcessTreeNodes(childNode);
        }

        private bool IsInteresting(TestSuiteTreeNode node)
        {
            return node.IsExpanded || node.Checked;
        }

        #endregion

        #region Instance Methods

        public void Save( string fileName )
        {
            using ( StreamWriter writer = new StreamWriter( fileName ) )
            {
                Save( writer );
            }
        }

        public void Save( TextWriter writer )
        {
            XmlSerializer serializer = new XmlSerializer( GetType() );
            serializer.Serialize( writer, this );
        }

        #endregion
    }

    [Serializable]
    public class VisualTreeNode
    {
        [XmlAttribute]
        public string Id;

        [XmlAttribute,System.ComponentModel.DefaultValue(false)]
        public bool Expanded;

        [XmlAttribute,System.ComponentModel.DefaultValue(false)]
        public bool Checked;

        [XmlArrayItem("Node")]
        public VisualTreeNode[] Nodes;

        public VisualTreeNode() { }

        public VisualTreeNode( TestSuiteTreeNode treeNode )
        {
            Id = treeNode.Test.Id;
            Expanded = treeNode.IsExpanded;
            Checked = treeNode.Checked;
        }
    }
}
