// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NUnit.Framework;

namespace TestCentric.Gui
{
    using Model;

    public class VisualStateTests
    {
        private static TreeView OriginalTreeView;
        private static VisualState ExpectedVisualState;

        [OneTimeSetUp]
        public void InitializeFixture()
        {
            OriginalTreeView = CreateTreeView();

            ExpectedVisualState = new VisualState()
            {
                ShowCheckBoxes = true,
                TopNode = "id-7",
                SelectedNode = "id-2",
                Nodes = new List<VisualTreeNode> {
                        new VisualTreeNode() { Id = "id-7", Expanded = true },
                        new VisualTreeNode() { Id = "id-6", Expanded = true },
                        new VisualTreeNode() { Id = "id-5", Expanded = true },
                        new VisualTreeNode() { Id = "id-4", Expanded = true },
                        new VisualTreeNode() { Id = "id-1", Checked = true },
                        new VisualTreeNode() { Id = "id-3", Checked = true },
                        new VisualTreeNode() { Id = "id-14", Expanded = true },
                        new VisualTreeNode() { Id = "id-13", Expanded = true },
                        new VisualTreeNode() { Id = "id-10", Checked = true, Expanded = true }
                    }
            };
        }

        // Helper to create a TreeView for testing
        private TreeView CreateTreeView()
        {
            // Create TreeNodes for two assemblies
            TreeNode assembly1 =
                TN("test-suite", "Assembly1", "id-7",
                    TN("test-suite", "NUnit", "id-6",
                        TN("test-suite", "Tests", "id-5",
                            TN("test-suite", "MyFixture", "id-4",
                                TN("test-case", "Test1", "id-1"),
                                TN("test-case", "Test2", "id-2"),
                                TN("test-case", "Test3", "id-3")))));
            TreeNode assembly2 =
                TN("test-suite", "Assembly2", "id-14",
                    TN("test-suite", "Tests", "id-13",
                        TN("test-suite", "FixtureA", "id-10",
                            TN("test-case", "Test1", "id-8"),
                            TN("test-case", "Test1", "id-9")),
                        TN("test-suite", "FixtureB", "id-12",
                            TN("test-case", "Test1", "id-11"))));

            // Create the TreeView
            TreeView tv = new TreeView()
            {
                CheckBoxes = true,
                TopNode = assembly1,
                SelectedNode = assembly1.Nodes[0].Nodes[0].Nodes[0].Nodes[1] // Test2
            };
            tv.Nodes.AddRange(new[] { assembly1, assembly2 });

            // Expand some Nodes
            assembly1.Expand();
            assembly1.Nodes[0].Expand();
            assembly1.Nodes[0].Nodes[0].Expand();
            assembly1.Nodes[0].Nodes[0].Nodes[0].Expand();
            assembly2.Expand();
            assembly2.Nodes[0].Expand();
            assembly2.Nodes[0].Nodes[0].Expand();

            // Check some nodes
            assembly1.Nodes[0].Nodes[0].Nodes[0].Nodes[0].Checked = true; // Test1
            assembly1.Nodes[0].Nodes[0].Nodes[0].Nodes[2].Checked = true; // Test3
            assembly2.Nodes[0].Nodes[0].Checked = true; // FixtureA

            return tv;
        }

        // Helper used to create a tree node
        private static TreeNode TN(string element, string name, string id, params TreeNode[] childNodes)
        {
            var treeNode = new TreeNode(name) { Tag = new TestNode($"<{element} name='{name}' id='{id}' />") };
            if (childNodes.Length > 0)
                treeNode.Nodes.AddRange(childNodes);
            return treeNode;
        }

        [Test]
        public void CanCreateVisualStateFromTree()
        {
            var visualState = VisualState.LoadFrom(OriginalTreeView);

            Assert.Multiple(() =>
            {
                Assert.That(visualState.ShowCheckBoxes, Is.EqualTo(OriginalTreeView.CheckBoxes));
                Assert.That(visualState.TopNode, Is.EqualTo(((TestNode)OriginalTreeView.TopNode?.Tag)?.Id));
                Assert.That(visualState.SelectedNode, Is.EqualTo(((TestNode)OriginalTreeView.SelectedNode?.Tag)?.Id));
                Assert.That(visualState.Nodes, Is.EqualTo(ExpectedVisualState.Nodes).Using(new VisualTreeNodeComparer()));
            });
        }

        [Test]
        public void CanSaveAndReloadVisualState()
        {
            StringWriter writer = new StringWriter();
            ExpectedVisualState.Save(writer);

            string output = writer.GetStringBuilder().ToString();
            StringReader reader = new StringReader(output);

            VisualState restoredState = VisualState.LoadFrom(reader);

            Assert.Multiple( () => {
                Assert.That(restoredState.ShowCheckBoxes, Is.EqualTo(ExpectedVisualState.ShowCheckBoxes));
                Assert.That(restoredState.TopNode, Is.EqualTo(ExpectedVisualState.TopNode));
                Assert.That(restoredState.SelectedNode, Is.EqualTo(ExpectedVisualState.SelectedNode));
                // TODO: Categories not yet supported
                //Assert.AreEqual(ExpectedVisualState.SelectedCategories, restoredState.SelectedCategories);
                //Assert.AreEqual(ExpectedVisualState.ExcludeCategories, restoredState.ExcludeCategories);
                Assert.That(restoredState.Nodes, Is.EqualTo(ExpectedVisualState.Nodes).Using(new VisualTreeNodeComparer()));
            });

        }
    }

    internal class VisualTreeNodeComparer : IEqualityComparer<VisualTreeNode>
    {
        public bool Equals(VisualTreeNode x, VisualTreeNode y)
        {
            if (x.Id != y.Id || x.Expanded != y.Expanded || x.Checked != y.Checked || x.Nodes.Length != y.Nodes.Length)
                return false;

            for (int i = 0; i < x.Nodes.Length; i++)
                if (!Equals(x.Nodes[i], y.Nodes[i])) return false;

            return true;
        }

        public int GetHashCode(VisualTreeNode obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
