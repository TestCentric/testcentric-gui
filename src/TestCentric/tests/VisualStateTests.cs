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
            OriginalTreeView = CreateOriginalTreeView();
            ExpectedVisualState = CreateExpectedVisualState();
        }

        [Test]
        public void CanCreateVisualStateFromTree()
        {
            var visualState = new VisualState().LoadFrom(OriginalTreeView);

            Assert.Multiple(() =>
            {
                Assert.That(visualState.ShowCheckBoxes, Is.EqualTo(OriginalTreeView.CheckBoxes));
                Assert.That(visualState.SelectedNode, Is.EqualTo(VTN("1-2", "Test2", SEL)), "SelectedNode");
                Assert.That(visualState.TopNode, Is.EqualTo(VTN("1-7", "Assembly1", EXP+TOP)), "TopNode");
                Assert.That(visualState.Nodes, Is.EqualTo(ExpectedVisualState.Nodes));
            });
        }

        [Test]
        public void CanApplyVisualStateToTree_Unchanged()
        {
            var tv = CreateBaseTreeView();
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
        public void CanApplyVisualStateToTree_SingleDeletion()
        {
            // Remove Test2
            var tv = new TestTreeView(
                TN("test-suite", "Assembly1", "1-6",
                    TN("test-suite", "NUnit", "1-5",
                        TN("test-suite", "Tests", "1-4",
                            TN("test-suite", "MyFixture", "1-3",
                                TN("test-case", "Test1", "1-1"),
                                TN("test-case", "Test3", "1-2"))))),
                TN("test-suite", "Assembly2", "2-7",
                    TN("test-suite", "Tests", "2-6",
                        TN("test-suite", "FixtureA", "2-3",
                            TN("test-case", "Test1", "2-1"),
                            TN("test-case", "Test1", "2-2")),
                        TN("test-suite", "FixtureB", "2-5",
                            TN("test-case", "Test1", "2-4")))));

            ExpectedVisualState.ApplyTo(tv);

            Assert.Multiple(() =>
            {
                Assert.That(tv.CheckBoxes, Is.True, "CheckBoxes");

                // No SelectedNode since Test2 is no longer present
                Assert.That(tv.SelectedNode, Is.Null, "SelectedNode");

                // Remaining Tests continue to pass
                Assert.That(tv.TopNode?.Text, Is.EqualTo("Assembly1"), "TopNode");
                Assert.That(tv.Nodes[0].IsExpanded, "Assembly1 not expanded");
                Assert.That(tv.Nodes[0].Nodes[0].IsExpanded, "NUnit not expanded");
                Assert.That(tv.Nodes[0].Nodes[0].Nodes[0].IsExpanded, "Tests not expanded");
                Assert.That(tv.Nodes[0].Nodes[0].Nodes[0].Nodes[0].IsExpanded, "MyFixture not expanded");
                Assert.That(tv.Nodes[1].IsExpanded, "Assembly2 not expanded");
                Assert.That(tv.Nodes[1].Nodes[0].IsExpanded, "Tests not expanded");
                Assert.That(tv.Nodes[1].Nodes[0].Nodes[0].IsExpanded, "FixtureA not expanded");
                Assert.That(tv.Nodes[0].Nodes[0].Nodes[0].Nodes[0].Nodes[0].Checked, "Test1 not checked");
                Assert.That(tv.Nodes[0].Nodes[0].Nodes[0].Nodes[0].Nodes[1].Checked, "Test3 not checked");
                Assert.That(tv.Nodes[1].Nodes[0].Nodes[0].Checked, "FixtureA not checked");
            });
        }

        [Test]
        public void CanApplyVisualStateToTree_MultipleDeletions()
        {
            // Remove Test2 and FixtureA
            var tv = new TestTreeView(
                TN("test-suite", "Assembly1", "1-6",
                    TN("test-suite", "NUnit", "1-5",
                        TN("test-suite", "Tests", "1-4",
                            TN("test-suite", "MyFixture", "1-3",
                                TN("test-case", "Test1", "1-1"),
                                TN("test-case", "Test3", "1-2"))))),
                TN("test-suite", "Assembly2", "2-7",
                    TN("test-suite", "Tests", "2-6",
                        TN("test-suite", "FixtureB", "2-5",
                            TN("test-case", "Test1", "2-4")))));

            ExpectedVisualState.ApplyTo(tv);

            Assert.Multiple(() =>
            {
                Assert.That(tv.CheckBoxes, Is.True, "CheckBoxes");

                // No SelectedNode since Test2 is no longer present
                Assert.That(tv.SelectedNode, Is.Null, "SelectedNode");

                // Remaining Tests continue to pass except for FixtureA, which is no longer present
                Assert.That(tv.TopNode?.Text, Is.EqualTo("Assembly1"), "TopNode");
                Assert.That(tv.Nodes[0].IsExpanded, "Assembly1 not expanded");
                Assert.That(tv.Nodes[0].Nodes[0].IsExpanded, "NUnit not expanded");
                Assert.That(tv.Nodes[0].Nodes[0].Nodes[0].IsExpanded, "Tests not expanded");
                Assert.That(tv.Nodes[0].Nodes[0].Nodes[0].Nodes[0].IsExpanded, "MyFixture not expanded");
                Assert.That(tv.Nodes[1].IsExpanded, "Assembly2 not expanded");
                Assert.That(tv.Nodes[1].Nodes[0].IsExpanded, "Tests not expanded");
                Assert.That(tv.Nodes[0].Nodes[0].Nodes[0].Nodes[0].Nodes[0].Checked, "Test1 not checked");
                Assert.That(tv.Nodes[0].Nodes[0].Nodes[0].Nodes[0].Nodes[1].Checked, "Test3 not checked");
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

            Assert.Multiple(() =>
            {
                Assert.That(restoredState.ShowCheckBoxes, Is.EqualTo(ExpectedVisualState.ShowCheckBoxes));
                Assert.That(restoredState.SelectedNode, Is.EqualTo(VTN("1-2", "Test2", SEL)));
                Assert.That(restoredState.TopNode, Is.EqualTo(VTN("1-7", "Assembly1", EXP+TOP)));
                // TODO: Categories not yet supported
                //Assert.AreEqual(ExpectedVisualState.SelectedCategories, restoredState.SelectedCategories);
                //Assert.AreEqual(ExpectedVisualState.ExcludeCategories, restoredState.ExcludeCategories);
                Assert.That(restoredState.Nodes, Is.EqualTo(ExpectedVisualState.Nodes));
            });
        }

        private TreeView CreateOriginalTreeView()
        {
            var tv = CreateBaseTreeView();

            // Expand some Nodes
            tv.Nodes[0].Expand();
            tv.Nodes[0].Nodes[0].Expand();
            tv.Nodes[0].Nodes[0].Nodes[0].Expand();
            tv.Nodes[0].Nodes[0].Nodes[0].Nodes[0].Expand();
            tv.Nodes[1].Expand();
            tv.Nodes[1].Nodes[0].Expand();
            tv.Nodes[1].Nodes[0].Nodes[0].Expand();

            // Check some nodes
            tv.Nodes[0].Nodes[0].Nodes[0].Nodes[0].Nodes[0].Checked = true; // Test1
            tv.Nodes[0].Nodes[0].Nodes[0].Nodes[0].Nodes[2].Checked = true; // Test3
            tv.Nodes[1].Nodes[0].Nodes[0].Checked = true; // FixtureA

            // Select a node and set TopNode
            tv.SelectedNode = tv.Nodes[0].Nodes[0].Nodes[0].Nodes[0].Nodes[1]; // Test2
            tv.TopNode = tv.Nodes[0];

            return tv;
        }

        private TreeView CreateBaseTreeView()
        {
            return new TestTreeView(
                TN("test-suite", "Assembly1", "1-7",
                    TN("test-suite", "NUnit", "1-6",
                        TN("test-suite", "Tests", "1-5",
                            TN("test-suite", "MyFixture", "1-4",
                                TN("test-case", "Test1", "1-1"),
                                TN("test-case", "Test2", "1-2"),
                                TN("test-case", "Test3", "1-3"))))),
                TN("test-suite", "Assembly2", "2-7",
                    TN("test-suite", "Tests", "2-6",
                        TN("test-suite", "FixtureA", "2-3",
                            TN("test-case", "Test1", "2-1"),
                            TN("test-case", "Test1", "2-2")),
                        TN("test-suite", "FixtureB", "2-5",
                            TN("test-case", "Test1", "2-4")))));
        }

        private class TestTreeView : TreeView
        {
            public TestTreeView(params TreeNode[] nodes)
            {
                CheckBoxes = true;

                foreach (TreeNode node in nodes)
                    Nodes.Add(node);
            }
        }

        // Helper used to create a TreeNode
        private static TreeNode TN(string element, string name, string id, params TreeNode[] childNodes)
        {
            var treeNode = new TreeNode(name) { Tag = new TestNode($"<{element} name='{name}' id='{id}' />") };
            if (childNodes.Length > 0)
                treeNode.Nodes.AddRange(childNodes);
            return treeNode;
        }

        private VisualState CreateExpectedVisualState()
        {
            //var selectedNode = VTN("1-2", "Test2", SEL);
            //var topNode = VTN("1-7", "Assembly1", EXP+TOP);

            return new VisualState()
            {
                ShowCheckBoxes = true,
                SelectedNode = VTN("1-2", "Test2", SEL),
                TopNode = VTN("1-7", "Assembly1", EXP + TOP),
                Nodes = new List<VisualTreeNode> {
                    VTN("1-7", "Assembly1", EXP + TOP,
                        VTN("1-6", "NUnit", EXP,
                            VTN("1-5", "Tests", EXP,
                                VTN("1-4", "MyFixture", EXP,
                                    VTN("1-1", "Test1", CHK),
                                    VTN("1-2", "Test2", SEL),
                                    VTN("1-3", "Test3", CHK))))),
                    VTN("2-7", "Assembly2", EXP,
                        VTN("2-6", "Tests", EXP,
                            VTN("2-3", "FixtureA", EXP+CHK)))
                }
            };
        }

        const int EXP = 1;
        const int CHK = 2;
        const int SEL = 4;
        const int TOP = 8;

        // Helper used to create a VisualTreeNode
        private static VisualTreeNode VTN(string id, string name, int flags, params VisualTreeNode[] childNodes)
        {
            bool expand = (flags & EXP) == EXP;
            bool check = (flags & CHK) == CHK;
            bool select = (flags & SEL) == SEL;
            bool top = (flags & TOP) == TOP;
            Console.WriteLine($"flags={flags} gives expand={expand}, check={check}, select={select}, top={top}");

            var visualNode = new VisualTreeNode() { Name = name, Expanded = expand, Checked = check, Selected = select, IsTopNode = top };
            
            if (childNodes.Length > 0)
                visualNode.Nodes.AddRange(childNodes);
            
            return visualNode;
        }
    }
}
