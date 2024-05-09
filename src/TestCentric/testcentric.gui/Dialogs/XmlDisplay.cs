// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Xml;
using System.Windows.Forms;
using TestCentric.Gui.Model;

namespace TestCentric.Gui.Dialogs
{
    public partial class XmlDisplay : PinnableDisplay
    {
        private ITestModel _model;

        private TreeNode _treeNode;
        private TestNode _testNode;
        
        public XmlDisplay(ITestModel model)
        {
            InitializeComponent();

            _model = model;

            selectAllToolStripMenuItem.Click += (s, e) =>
            {
                xmlTextBox.SelectAll();
            };

            copyToolStripMenuItem.Click += (s, e) =>
            {
                xmlTextBox.Copy();
            };

            wordWrapToolStripMenuItem.CheckedChanged += (s, e) =>
            {
                xmlTextBox.WordWrap = wordWrapToolStripMenuItem.Checked;
            };
        }

        public void Display(TreeNode treeNode)
        {
            if (treeNode == null)
                throw new ArgumentNullException(nameof(treeNode));

            _treeNode = treeNode;
            _testNode = treeNode.Tag as TestNode;

            SuspendLayout();
            TestName = _testNode.Name;
            var fullXml = GetFullXml(_testNode);
            xmlTextBox.Rtf = new Xml2RtfConverter(2).Convert(fullXml);

            ResumeLayout();

            Show();
        }

        public void OnTestFinished(ResultNode result)
        {
            if (result.Id == _testNode.Id)
                Invoke(new Action(() => Display(_treeNode)));
        }

        private XmlNode GetFullXml(TestNode testNode)
        {
            ResultNode resultNode = _model.GetResultForTest(testNode.Id);
            XmlNode currentXml;
            if (resultNode != null)
            {
                currentXml = resultNode.Xml.Clone();
                foreach (TestNode child in testNode.Children)
                {
                    XmlNode childXml = GetFullXml(child);
                    XmlNode importedChildXml = currentXml.OwnerDocument.ImportNode(childXml, true);
                    currentXml.AppendChild(importedChildXml);
                }
            }
            else
            {
                currentXml = testNode.Xml.Clone();
                foreach (TestNode child in testNode.Children)
                {
                    XmlNode childXml = GetFullXml(child);
                    XmlNode importedChildXml = currentXml.OwnerDocument.ImportNode(childXml, true);
                    var oldChild = FindXmlNode(currentXml, child);
                    if (oldChild != null)
                        currentXml.ReplaceChild(importedChildXml, oldChild);
                    else
                        currentXml.AppendChild(importedChildXml);
                }
            }
            return currentXml;
        }

        private static XmlNode FindXmlNode(XmlNode currentXml, TestNode testNodeChild)
        {
            foreach (XmlNode child in currentXml.ChildNodes)
            {
                if ((child.LocalName == "test-case" || child.LocalName == "test-suite")
                    && testNodeChild.FullName == child.Attributes["fullname"].Value)
                    return child;
            }
            return null;
        }
    }
}
