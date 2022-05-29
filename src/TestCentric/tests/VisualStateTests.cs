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
    using Presenters;

    public class VisualStateTests
    {
        private static TreeView OriginalTreeView;
        private static VisualState ExpectedVisualState;

        [OneTimeSetUp]
        public void InitializeFixture()
        {
            OriginalTreeView = CreateTreeView(true);

            var selectedNode = new VisualTreeNode() { Id = "id-2", Selected = true };
            var topNode = new VisualTreeNode() { Id = "id-7", Expanded = true, IsTopNode = true };

            ExpectedVisualState = new VisualState()
            {
                ShowCheckBoxes = true,
                SelectedNode = selectedNode,
                TopNode = topNode,
                Nodes = new List<VisualTreeNode> {
                        topNode,
                        new VisualTreeNode() { Id = "id-6", Expanded = true },
                        new VisualTreeNode() { Id = "id-5", Expanded = true },
                        new VisualTreeNode() { Id = "id-4", Expanded = true },
                        new VisualTreeNode() { Id = "id-1", Checked = true },
                        selectedNode,
                        new VisualTreeNode() { Id = "id-3", Checked = true },
                        new VisualTreeNode() { Id = "id-14", Expanded = true },
                        new VisualTreeNode() { Id = "id-13", Expanded = true },
                        new VisualTreeNode() { Id = "id-10", Checked = true, Expanded = true }
                    }
            };
            ExpectedVisualState.BuildNodeIndex();

            Assert.That(OriginalTreeView.SelectedNode.Text, Is.EqualTo("Test2"));
        }

        // Helper to create a TreeView for testing
        private TreeView CreateTreeView(bool expanded)
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

            // Create the TreeView and add Nodes to it
            TreeView tv = new TreeView() { CheckBoxes = true };
            tv.Nodes.AddRange(new[] { assembly1, assembly2 });

            if (expanded)
            {
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

                // Select a node and set TopNode
                tv.SelectedNode = assembly1.Nodes[0].Nodes[0].Nodes[0].Nodes[1]; // Test2
                tv.TopNode = assembly1;
            }

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
            var visualState = new VisualState().LoadFrom(OriginalTreeView);

            Assert.Multiple(() =>
            {
                Assert.That(visualState.ShowCheckBoxes, Is.EqualTo(OriginalTreeView.CheckBoxes));
                Assert.That(visualState.SelectedNode, Is.EqualTo(new VisualTreeNode() { Id = "id-2", Selected=true }));
                Assert.That(visualState.TopNode, Is.EqualTo(new VisualTreeNode() { Id = "id-7", Expanded = true, IsTopNode = true }));
                Assert.That(visualState.Nodes, Is.EqualTo(ExpectedVisualState.Nodes));
            });
        }

        [Test]
        public void CanApplyVisualStateToTree()
        {
            var tv = CreateTreeView(false);
            Assert.That(tv.CheckBoxes, Is.True);
            Assert.That(tv.SelectedNode, Is.Null);
            Assert.That(tv.Nodes[0].IsExpanded, Is.False);
            Assert.That(tv.Nodes[1].IsExpanded, Is.False);
            Assert.That(ExpectedVisualState.ShowCheckBoxes, Is.True);
            Assert.That(ExpectedVisualState.SelectedNode, Is.Not.Null);
            Assert.That(ExpectedVisualState.SelectedNode.Id, Is.EqualTo("id-2"));
            Assert.That(ExpectedVisualState.TopNode.Id, Is.EqualTo("id-7"));
            ExpectedVisualState.ApplyTo(tv);

            Assert.Multiple(() =>
            {
                Assert.That(tv.CheckBoxes, Is.True, "CheckBoxes");
                Assert.That(tv.SelectedNode?.Text, Is.EqualTo("Test2"), "SelectedNode");
                Assert.That(tv.TopNode?.Text, Is.EqualTo("Assembly1"), "TopNode");
                Assert.That(tv.Nodes[0].IsExpanded, "Assembly1 not expanded");
                Assert.That(tv.Nodes[0].Nodes[0].IsExpanded, "NUnit not expanded");
                Assert.That(tv.Nodes[0].Nodes[0].Nodes[0].IsExpanded, "Tests not expanded");
                Assert.That(tv.Nodes[0].Nodes[0].Nodes[0].Nodes[0].IsExpanded, "MyFixture not expanded");
                Assert.That(tv.Nodes[1].IsExpanded, "Assembly2 not expanded");
                Assert.That(tv.Nodes[1].Nodes[0].IsExpanded, "Tests not expanded");
                Assert.That(tv.Nodes[1].Nodes[0].Nodes[0].IsExpanded, "FixtureA not expanded");
                Assert.That(tv.Nodes[0].Nodes[0].Nodes[0].Nodes[0].Nodes[0].Checked, "Test1 not checked");
                Assert.That(tv.Nodes[0].Nodes[0].Nodes[0].Nodes[0].Nodes[2].Checked, "Test3 not checked");
                Assert.That(tv.Nodes[1].Nodes[0].Nodes[0].Checked, "FixtureA not checked");
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
                Assert.That(restoredState.SelectedNode, Is.EqualTo(new VisualTreeNode() { Id = "id-2", Selected = true }));
                Assert.That(restoredState.TopNode, Is.EqualTo(new VisualTreeNode() { Id = "id-7", Expanded = true, IsTopNode = true }));
                // TODO: Categories not yet supported
                //Assert.AreEqual(ExpectedVisualState.SelectedCategories, restoredState.SelectedCategories);
                //Assert.AreEqual(ExpectedVisualState.ExcludeCategories, restoredState.ExcludeCategories);
                Assert.That(restoredState.Nodes, Is.EqualTo(ExpectedVisualState.Nodes));
            });

        }
    }
}
