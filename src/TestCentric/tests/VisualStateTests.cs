// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.IO;
using System.Windows.Forms;
using NUnit.Framework;
using TestCentric.TestUtilities;

namespace TestCentric.Gui
{
    using Model;

    [TestFixtureSource(nameof(TestData))]
    public class VisualStateTests
    {
        private VisualStateTestData _data;
        private TreeView ExpandedTreeView;
        private VisualState ExpectedVisualState;

        public VisualStateTests(VisualStateTestData data)
        {
            _data = data;
            ExpandedTreeView = data.ExpandedTreeView;
            ExpectedVisualState = data.ExpectedVisualState;
        }

        [Test]
        public void CanCreateVisualStateFromTree()
        {
            var visualState = new VisualState().LoadFrom(ExpandedTreeView);

            Assert.Multiple(() =>
            {
                Assert.That(visualState.ShowCheckBoxes, Is.EqualTo(ExpandedTreeView.CheckBoxes));
                Assert.That(visualState.Nodes, Is.EqualTo(ExpectedVisualState.Nodes));
            });
        }

        [Test]
        public void CanApplyVisualStateToTree_Unchanged()
        {
            var treeView = _data.GetInitialTreeView();

            ExpectedVisualState.ApplyTo(treeView);

            VerifyTreeView(treeView, selectedNode:"Test2");
        }

        [Test]
        public void CanApplyVisualStateToTree_TestsAdded()
        {
            var treeView = _data.GetInitialTreeView();

            AddSomeTests(treeView);

            ExpectedVisualState.ApplyTo(treeView);

            VerifyTreeView(treeView, selectedNode:"Test2");
        }

        [Test]
        public void CanApplyVisualStateToTree_TestsAddedAndDeleted ()
        {
            var treeView = _data.GetInitialTreeView();

            AddSomeTests(treeView);

            // Test4 and Test5 are deleted before FixtureA, which may not be displayed
            treeView.Remove("Test3", "Test4", "Test5", "FixtureA");

            ExpectedVisualState.ApplyTo(treeView);

            VerifyTreeView(treeView, selectedNode: "Test2");
        }

        private void AddSomeTests(TreeView treeView)
        {
            // Add NewFixture, SomeTest, NewTest, FixtureB, Test1
            if (_data.DisplayStrategy == "TEST_LIST") // Just add the test cases
            {
                treeView.Search("Test1").AddSibling(TN("test-case", "SomeTest", "1-10"));
                treeView.Search("Test1").AddSibling(TN("test-case", "NewTest", "1-11"));
                treeView.Search("Test4").AddSibling(TN("test-case", "Test7", "2-4"));
            }
            else // Add both fixtures and cases
            {
                treeView.Search("MyFixture").AddSibling(
                    TN("test-suite", "New Fixture", "1-9",
                        TN("test-case", "SomeTest", "1-10")));
                treeView.Search("MyFixture").Nodes.Add(TN("test-case", "NewTest", "1-11"));
                treeView.Search("FixtureA").AddSibling(
                    TN("test-suite", "FixtureB", "2-5",
                        TN("test-case", "Test7", "2-4")));
            }
        }

        [Test]
        public void CanApplyVisualStateToTree_SingleDeletion()
        {
            var treeView = _data.GetInitialTreeView();

            treeView.Remove("Test2");

            ExpectedVisualState.ApplyTo(treeView);

            VerifyTreeView(treeView, selectedNode:null);
        }

        [Test]
        public void CanApplyVisualStateToTree_MultipleDeletions()
        {
            var treeView = _data.GetInitialTreeView();

            // Test4 and Test5 are deleted before FixtureA, which may not be displayed
            treeView.Remove("Test2", "Test4", "Test5", "FixtureA");

            ExpectedVisualState.ApplyTo(treeView);

            VerifyTreeView(treeView, selectedNode: null);
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
                // TODO: Categories not yet supported
                //Assert.AreEqual(ExpectedVisualState.SelectedCategories, restoredState.SelectedCategories);
                //Assert.AreEqual(ExpectedVisualState.ExcludeCategories, restoredState.ExcludeCategories);
                //Assert.That(restoredState.Nodes, Is.EqualTo(ExpectedVisualState.Nodes));
                Assert.That(restoredState.Nodes[0], Is.EqualTo(ExpectedVisualState.Nodes[0]));
            });
        }

        // Helper used to create a TreeNode
        // TODO: Move to test utilities
        public static TreeNode TN(string element, string name, string id, params TreeNode[] childNodes)
        {
            var treeNode = new TreeNode(name) { Tag = new TestNode($"<{element} name='{name}' id='{id}' />") };
            if (childNodes.Length > 0)
                treeNode.Nodes.AddRange(childNodes);
            return treeNode;
        }

        #region Helper method to handle common asserts

        private void VerifyTreeView(TreeView treeView, string selectedNode = null)
        {
            var expectedTopNode = treeView.Nodes[0].Text;

            Assert.Multiple(() =>
            {
            Assert.That(treeView.CheckBoxes, Is.True, "CheckBoxes");
            Assert.That(treeView.SelectedNode?.Text, Is.EqualTo(selectedNode), "SelectedNode");
            Assert.That(treeView.TopNode?.Text, Is.EqualTo(expectedTopNode), "TopNode");

            var fixtureA = treeView.Search("FixtureA");

            // NOTE: The following code contains lots of duplication by design.
            // It's kept this way to make it easier to see the assertions for
            // each strategy and grouping all together in one place.

            switch (_data.DisplayStrategy)
            {
                case "NUNIT_TREE":
                    Assert.That(treeView.Search("Assembly1").IsExpanded, "Assembly1 not expanded");
                    Assert.That(treeView.Search("Assembly2").IsExpanded, "Assembly2 not expanded");
                    Assert.That(treeView.Search("NUnit").IsExpanded, "NUnit namespace not expanded");
                    Assert.That(treeView.Search("Tests").IsExpanded, "NUnit/Tests namespace not expanded");
                    Assert.That(treeView.Search("UnitTests").IsExpanded, "UnitTests namespace not expanded");
                    Assert.That(treeView.Search("MyFixture").IsExpanded, "MyFixture not expanded");
                    if (fixtureA != null) // In case it was deleted
                    {
                        Assert.That(fixtureA.IsExpanded, "MyFixture not expanded");
                        Assert.That(fixtureA.Checked, "FixtureA not checked");
                    }
                    break;

                case "FIXTURE_LIST":
                    switch (_data.Grouping)
                    {
                        case "ASSEMBLY":
                            Assert.That(treeView.Search("Assembly1").IsExpanded, "Assembly1 not expanded");
                            Assert.That(treeView.Search("Assembly2").IsExpanded, "Assembly2 not expanded");
                            break;
                            case "CATEGORY":
                            case "CATEGORY_EXTENDED":
                                Assert.That(treeView.Search("None").IsExpanded, "Category 'None' not expanded");
                                break;
                            case "OUTCOME":
                            case "DURATION":
                                Assert.That(treeView.Search("Not Run").IsExpanded, "'Not Run' not expanded");
                                break;
                        }
                        Assert.That(treeView.Search("MyFixture").IsExpanded, "MyFixture not expanded");
                    if (fixtureA != null) // In case it was deleted
                    {
                        Assert.That(fixtureA.IsExpanded, "MyFixture not expanded");
                        Assert.That(fixtureA.Checked, "FixtureA not checked");
                    }
                    break;

                case "TEST_LIST":
                    switch(_data.Grouping)
                    {
                        case "Assembly":
                            Assert.That(treeView.Search("Assembly1").IsExpanded, "Assembly1 not expanded");
                            Assert.That(treeView.Search("Assembly2").IsExpanded, "Assembly2 not expanded");
                            break;
                        case "FIXTURE":
                            Assert.That(treeView.Search("MyFixture").IsExpanded, "MyFixture not expanded");
                            if (fixtureA != null) // In case it was deleted
                            {
                                Assert.That(fixtureA.IsExpanded, "MyFixture not expanded");
                                Assert.That(fixtureA.Checked, "FixtureA not checked");
                            }
                            break;
                    }
                    break;

                }

                Assert.That(treeView.Search("Test1").Checked, "Test1 not checked");
                var test3 = treeView.Search("Test3");
                if (test3 != null) // Removed in some scenarios
                    Assert.That(test3.Checked, "Test3 not checked");
            });
        }

        #endregion

        #region Test Data

        private static VisualStateTestData[] TestData = new VisualStateTestData[]
        {
            new VisualStateTestData("NUNIT_TREE"),
            new VisualStateTestData("FIXTURE_LIST", "ASSEMBLY"),
            new VisualStateTestData("FIXTURE_LIST", "CATEGORY"),
            new VisualStateTestData("FIXTURE_LIST", "CATEGORY_EXTENDED"),
            new VisualStateTestData("FIXTURE_LIST", "OUTCOME"),
            new VisualStateTestData("FIXTURE_LIST", "DURATION"),
            new VisualStateTestData("TEST_LIST", "ASSEMBLY"),
            new VisualStateTestData("TEST_LIST", "FIXTURE"),
            new VisualStateTestData("TEST_LIST", "CATEGORY"),
            new VisualStateTestData("TEST_LIST", "CATEGORY_EXTENDED"),
            new VisualStateTestData("TEST_LIST", "OUTCOME"),
            new VisualStateTestData("TEST_LIST", "DURATION")
        };

        public class VisualStateTestData
        {
            public string DisplayStrategy;
            public string Grouping;
            public VisualState InitialVisualState;

            public VisualStateTestData(string strategy)
            {
                DisplayStrategy = strategy;
            }

            public VisualStateTestData(string strategy, string grouping)
            {
                DisplayStrategy = strategy;
                Grouping = grouping;
            }

            public TreeView GetInitialTreeView()
            {
                switch (DisplayStrategy)
                {
                    case "NUNIT_TREE":
                        return TreeViewFactory.CreateTreeView(
                            true,
                            TN("test-suite", "Assembly1", "1-7",
                                TN("test-suite", "NUnit", "1-6",
                                    TN("test-suite", "Tests", "1-5",
                                        TN("test-suite", "MyFixture", "1-4",
                                            TN("test-case", "Test1", "1-1"),
                                            TN("test-case", "Test2", "1-2"),
                                            TN("test-case", "Test3", "1-3"))))),
                            TN("test-suite", "Assembly2", "2-7",
                                TN("test-suite", "UnitTests", "2-6",
                                    TN("test-suite", "FixtureA", "2-3",
                                        TN("test-case", "Test4", "2-1"),
                                        TN("test-case", "Test5", "2-2")),
                                    TN("test-suite", "FixtureB", "2-5",
                                        TN("test-case", "Test6", "2-4")))));

                    case "FIXTURE_LIST":
                        switch (Grouping)
                        {
                            case "ASSEMBLY":
                                return TreeViewFactory.CreateTreeView(
                                    true,
                                    TN("test-suite", "Assembly1", "1-7",
                                        TN("test-suite", "MyFixture", "1-4",
                                            TN("test-case", "Test1", "1-1"),
                                            TN("test-case", "Test2", "1-2"),
                                            TN("test-case", "Test3", "1-3"))),
                                    TN("test-suite", "Assembly2", "2-7",
                                        TN("test-suite", "FixtureA", "2-3",
                                            TN("test-case", "Test4", "2-1"),
                                            TN("test-case", "Test5", "2-2")),
                                        TN("test-suite", "FixtureB", "2-5",
                                            TN("test-case", "Test6", "2-4"))));

                            case "CATEGORY":
                            case "CATEGORY_EXTENDED":
                                return TreeViewFactory.CreateTreeView(
                                    true,
                                    TN("test-suite", "None", "1-7",
                                        TN("test-suite", "MyFixture", "1-4",
                                            TN("test-case", "Test1", "1-1"),
                                            TN("test-case", "Test2", "1-2"),
                                            TN("test-case", "Test3", "1-3")),
                                        TN("test-suite", "FixtureA", "2-3",
                                            TN("test-case", "Test4", "2-1"),
                                            TN("test-case", "Test5", "2-2")),
                                        TN("test-suite", "FixtureB", "2-5",
                                            TN("test-case", "Test6", "2-4"))));

                            case "OUTCOME":
                            case "DURATION":
                                return TreeViewFactory.CreateTreeView(
                                    true,
                                    TN("test-suite", "Not Run", "1-7",
                                        TN("test-suite", "MyFixture", "1-4",
                                            TN("test-case", "Test1", "1-1"),
                                            TN("test-case", "Test2", "1-2"),
                                            TN("test-case", "Test3", "1-3")),
                                        TN("test-suite", "FixtureA", "2-3",
                                            TN("test-case", "Test4", "2-1"),
                                            TN("test-case", "Test5", "2-2")),
                                        TN("test-suite", "FixtureB", "2-5",
                                            TN("test-case", "Test6", "2-4"))));

                            default:
                                throw new Exception($"Grouping {Grouping} is not recognized");
                        }

                    case "TEST_LIST":
                        switch (Grouping)
                        {
                            case "ASSEMBLY":
                                return TreeViewFactory.CreateTreeView(
                                    true,
                                    TN("test-suite", "Assembly1", "1-7",
                                        TN("test-case", "Test1", "1-1"),
                                        TN("test-case", "Test2", "1-2"),
                                        TN("test-case", "Test3", "1-3")),
                                    TN("test-suite", "Assembly2", "2-7",
                                        TN("test-case", "Test4", "2-1"),
                                        TN("test-case", "Test5", "2-2"),
                                        TN("test-case", "Test6", "2-4")));

                            case "FIXTURE":
                                return TreeViewFactory.CreateTreeView(
                                    true,
                                    TN("test-suite", "MyFixture", "1-4",
                                        TN("test-case", "Test1", "1-1"),
                                        TN("test-case", "Test2", "1-2"),
                                        TN("test-case", "Test3", "1-3")),
                                    TN("test-suite", "FixtureA", "2-3",
                                        TN("test-case", "Test4", "2-1"),
                                        TN("test-case", "Test5", "2-2")),
                                    TN("test-suite", "FixtureB", "2-5",
                                        TN("test-case", "Test6", "2-4")));

                            case "CATEGORY":
                            case "CATEGORY_EXTENDED":
                                return TreeViewFactory.CreateTreeView(
                                    true,
                                    TN("test-suite", "None", "1-7",
                                        TN("test-case", "Test1", "1-1"),
                                        TN("test-case", "Test2", "1-2"),
                                        TN("test-case", "Test3", "1-3"),
                                        TN("test-case", "Test4", "2-1"),
                                        TN("test-case", "Test5", "2-2"),
                                        TN("test-case", "Test6", "2-4")));

                            case "OUTCOME":
                            case "DURATION":
                                return TreeViewFactory.CreateTreeView(
                                    true,
                                    TN("test-suite", "Not Run", "1-7",
                                        TN("test-case", "Test1", "1-1"),
                                        TN("test-case", "Test2", "1-2"),
                                        TN("test-case", "Test3", "1-3"),
                                        TN("test-case", "Test4", "2-1"),
                                        TN("test-case", "Test5", "2-2"),
                                        TN("test-case", "Test6", "2-4")));

                            default:
                                throw new Exception($"Grouping {Grouping} is not recognized");
                        }

                    default:
                        throw new Exception($"DisplayStrategy {DisplayStrategy} is not recognized");
                }
            }

            public TreeView ExpandedTreeView
            {
                get
                {
                    var tv = GetInitialTreeView();

                    tv.Expand("Assembly1", "NUnit", "Tests", "MyFixture", "Assembly2", "Tests", "FixtureA");
                    tv.Check("Test1", "Test3", "FixtureA");
                    tv.SelectedNode = tv.Search("Test2");
                    if (DisplayStrategy == "NUNIT_TREE" || Grouping == "ASSEMBLY")
                        tv.TopNode = tv.Search("Assembly1");
                    else if (Grouping == "FIXTURE")
                        tv.TopNode = tv.Search("MyFixture");
                    else if (Grouping == "CATEGORY" || Grouping == "CATEGORY_EXTENDED")
                        tv.TopNode = tv.Search("None");
                    else if (Grouping == "OUTCOME" || Grouping == "DURATION")
                        tv.TopNode = tv.Search("Not Run");

                    return tv;
                }
            }

            public VisualState ExpectedVisualState
            {
                get
                {
                    switch (DisplayStrategy)
                    {
                        case "NUNIT_TREE":
                            return VisualStateFactory.CreateVisualState(
                                DisplayStrategy,
                                true,
                                VTN("1-7", "Assembly1", EXP + TOP,
                                    VTN("1-6", "NUnit", EXP,
                                        VTN("1-5", "Tests", EXP,
                                            VTN("1-4", "MyFixture", EXP,
                                                VTN("1-1", "Test1", CHK),
                                                VTN("1-2", "Test2", SEL),
                                                VTN("1-3", "Test3", CHK))))),
                                VTN("2-7", "Assembly2", EXP,
                                    VTN("2-6", "UnitTests", EXP,
                                        VTN("2-3", "FixtureA", EXP + CHK))));

                        case "FIXTURE_LIST":
                            switch (Grouping)
                            {
                                case "ASSEMBLY":
                                    return VisualStateFactory.CreateVisualState(
                                        DisplayStrategy,
                                        true,
                                        VTN("1-7", "Assembly1", EXP + TOP,
                                            VTN("1-4", "MyFixture", EXP,
                                                VTN("1-1", "Test1", CHK),
                                                VTN("1-2", "Test2", SEL),
                                                VTN("1-3", "Test3", CHK))),
                                        VTN("2-7", "Assembly2", EXP,
                                            VTN("2-3", "FixtureA", EXP + CHK)));

                                case "CATEGORY":
                                case "CATEGORY_EXTENDED":
                                    return VisualStateFactory.CreateVisualState(
                                        DisplayStrategy,
                                        true,
                                        VTN("1-7", "None", EXP + TOP,
                                            VTN("1-4", "MyFixture", EXP,
                                                VTN("1-1", "Test1", CHK),
                                                VTN("1-2", "Test2", SEL),
                                                VTN("1-3", "Test3", CHK)),
                                            VTN("2-3", "FixtureA", EXP + CHK)));

                                case "OUTCOME":
                                case "DURATION":
                                    return VisualStateFactory.CreateVisualState(
                                        DisplayStrategy,
                                        true,
                                        VTN("1-7", "Not Run", EXP + TOP,
                                            VTN("1-4", "MyFixture", EXP,
                                                VTN("1-1", "Test1", CHK),
                                                VTN("1-2", "Test2", SEL),
                                                VTN("1-3", "Test3", CHK)),
                                            VTN("2-3", "FixtureA", EXP + CHK)));

                                default:
                                    throw new Exception($"ExpectedVisualState: Fixture list grouping {Grouping} is not recognized");
                            }

                        case "TEST_LIST":
                            switch (Grouping)
                            {
                                case "ASSEMBLY":
                                    return VisualStateFactory.CreateVisualState(
                                        DisplayStrategy,
                                        true,
                                        VTN("1-7", "Assembly1", EXP + TOP,
                                            VTN("1-1", "Test1", CHK),
                                            VTN("1-2", "Test2", SEL),
                                            VTN("1-3", "Test3", CHK)),
                                        VTN("2-7", "Assembly2", EXP));

                                case "FIXTURE":
                                    return VisualStateFactory.CreateVisualState(
                                        DisplayStrategy,
                                        true,
                                        VTN("1-4", "MyFixture", EXP + TOP,
                                            VTN("1-1", "Test1", CHK),
                                            VTN("1-2", "Test2", SEL),
                                            VTN("1-3", "Test3", CHK)),
                                        VTN("2-3", "FixtureA", EXP + CHK));

                                case "CATEGORY":
                                case "CATEGORY_EXTENDED":
                                    return VisualStateFactory.CreateVisualState(
                                        DisplayStrategy,
                                        true,
                                        VTN("1-4", "None", EXP + TOP,
                                            VTN("1-1", "Test1", CHK),
                                            VTN("1-2", "Test2", SEL),
                                            VTN("1-3", "Test3", CHK)));

                                case "OUTCOME":
                                case "DURATION":
                                    return VisualStateFactory.CreateVisualState(
                                        DisplayStrategy,
                                        true,
                                        VTN("1-4", "Not Run", EXP + TOP,
                                            VTN("1-1", "Test1", CHK),
                                            VTN("1-2", "Test2", SEL),
                                            VTN("1-3", "Test3", CHK)));

                                default:
                                    throw new Exception($"ExpectedVisualState: Test list grouping {Grouping} is not recognized");
                            }
                    }

                    throw new Exception($"ExpectedVisualState: Unrecognized DisplayStrategy: {DisplayStrategy}");
                }
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

                var visualNode = new VisualTreeNode() { Name = name, Expanded = expand, Checked = check, Selected = select, IsTopNode = top };

                if (childNodes.Length > 0)
                    visualNode.Nodes.AddRange(childNodes);

                return visualNode;
            }

            // Override ToString so tests deplay clearly
            public override string ToString()
            {
                return DisplayStrategy == "NUNIT_TREE"
                    ? "Strategy=NUNIT_TREE"
                    : $"Strategy={DisplayStrategy} Grouping={Grouping}";
            }
        }

        #endregion
    }
}
