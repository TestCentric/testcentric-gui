// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TestCentric.Common;

namespace TestCentric.Gui.Presenters
{
    using Elements;
    using Model;
    using Model.Settings;
    using NUnit.Engine;
    using Views;

    public class TreeViewPresenter
    {
        /// <summary>
        /// Indicates how a tree should be displayed initially
        /// </summary>
        private enum InitialTreeExpansion
        {
            Auto,       // Select based on space available
            Expand,     // Expand fully
            Collapse,   // Collapase fully
            HideTests   // Expand all but the fixtures, leaving
                        // leaf nodes hidden
        }

        private ITestTreeView _view;
        private ITestModel _model;
        private UserSettings _settings;
        private ITreeView _tree;
        private TestNodeFilter _treeFilter = TestNodeFilter.Empty;

        /// <summary>
        /// Hashtable provides direct access to TestNodes
        /// </summary>
        internal Dictionary<string, TreeNode> _treeMap = new Dictionary<string, TreeNode>();

        public Dictionary<string, TreeNode> TreeMap { get { return _treeMap; } }

        public TreeViewPresenter(ITestTreeView view, ITestModel model)
        {
            _view = view;
            _tree = view.Tree;
            _model = model;
            _settings = model.Settings;

            _view.AlternateImageSet = (string)_settings.Gui.TestTree.AlternateImageSet;

            var showCheckBoxes = _settings.Gui.TestTree.ShowCheckBoxes;
            _view.CheckBoxes = showCheckBoxes;
            _view.ShowCheckBoxes.Checked = showCheckBoxes;

            _view.RunCommand.Enabled = false;

            WireUpEvents();
        }

        private void WireUpEvents()
        {
            _model.Events.TestsLoading += (e) => _view.RunCommand.Enabled = false;

            _model.Events.TestLoaded += (e) =>
            {
                _view.RunCommand.Enabled = true;
                _view.CheckPropertiesDialog();

                LoadTests(GetTopDisplayNode(e.Test));

                if (_model.Settings.Gui.TestTree.SaveVisualState)
                {
                    string fileName = VisualState.GetVisualStateFileName(_model.TestFiles[0]);
                    if (File.Exists(fileName) && new FileInfo(fileName).Length > 0)
                    {
                        try
                        {
                            var visualState = VisualState.LoadFrom(fileName);
                            visualState.RestoreVisualState(_view, _treeMap);
                            _model.SelectCategories(visualState.SelectedCategories, visualState.ExcludeCategories);
                        }
                        catch (Exception ex)
                        {
                            new MessageDisplay().Error(
                                $"Unable to load visual state from {fileName}{Environment.NewLine}{ex.Message}");
                        }
                    }
                }
            };

            VisualState reloadState = null;

            _model.Events.TestsReloading += (e) =>
            {
                reloadState = VisualState.LoadFrom(_view);

                _view.RunCommand.Enabled = false;
            };

            _model.Events.TestReloaded += (e) =>
            {
                ReloadTests(GetTopDisplayNode(e.Test));

                if (reloadState != null)
                    reloadState.RestoreVisualState(_view, _treeMap);

                if (!_settings.Gui.ClearResultsOnReload)
                    RestoreResults(e.Test);

                if (!_treeFilter.IsEmpty)
                    _view.Accept(new TestFilterVisitor(_treeFilter));

                _view.RunCommand.Enabled = true;
            };

            _model.Events.TestChanged += (e) =>
            {
                if (_settings.Engine.ReloadOnChange)
                {
                    _model.ReloadTests();
                }
            };

            _model.Events.TestsUnloading += (e) =>
            {
                _view.RunCommand.Enabled = false;

                _view.ClosePropertiesDialog();

                if (_settings.Gui.TestTree.SaveVisualState)
                    try
                    {
                        var visualState = VisualState.LoadFrom(_view);
                        visualState.SelectedCategories = _model.SelectedCategories;
                        visualState.ExcludeCategories = _model.ExcludeSelectedCategories;
                        visualState.Save(VisualState.GetVisualStateFileName(_model.TestFiles[0]));
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Unable to save visual state.");
                        Debug.WriteLine(ex);
                    }

                _view.Clear();
                _treeMap.Clear();
            };

            _model.Events.TestUnloaded += (e) => _view.RunCommand.Enabled = false;

            _model.Events.RunStarting += (e) =>
            {
                foreach (var node in _view.Tree.Nodes)
                    ((TestSuiteTreeNode)node).ClearResults();
                _view.RunCommand.Enabled = false;
                _view.CheckPropertiesDialog();
            };

            _model.Events.RunFinished += (e) => _view.RunCommand.Enabled = true;

            _model.Events.TestFinished += (e) => SetTestResult(e.Result);

            _model.Events.SuiteFinished += (e) => SetTestResult(e.Result);

            _model.Events.CategorySelectionChanged += (TestEventArgs e) =>
            {
                TestNodeFilter filter = TestNodeFilter.Empty;

                if (_model.SelectedCategories.Count > 0)
                {
                    filter = new CategoryFilter(_model.SelectedCategories);
                    if (_model.ExcludeSelectedCategories)
                        filter = new NotFilter(filter);
                }

                _view.Accept(new TestFilterVisitor(_treeFilter = filter));
            };

            _settings.Changed += (s, e) =>
            {
                if (e.SettingName == "Gui.TestTree.AlternateImageSet")
                {
                    _view.AlternateImageSet = _settings.Gui.TestTree.AlternateImageSet;
                }
                else if (e.SettingName == "Gui.TestTree.ShowCheckBoxes")
                {
                    var showCheckBoxes = _settings.Gui.TestTree.ShowCheckBoxes;

                    // When turning off checkboxes with a non-empty tree, the
                    // structure of what is expanded and collapsed is lost.
                    // We save that structure as a VisualState and then restore it.
                    VisualState visualState = !showCheckBoxes && _view.Tree.TopNode != null
                        ? VisualState.LoadFrom(_view)
                        : null;

                    _view.CheckBoxes = showCheckBoxes;

                    if (visualState != null)
                    {
                        visualState.ShowCheckBoxes = showCheckBoxes;
                        visualState.RestoreVisualState(_view, _treeMap);
                    }
                }
            };

            _view.FileDrop += _model.LoadTests;

            _view.RunCommand.Execute += () =>
            {
                if (_settings.Engine.ReloadOnRun)
                {
                    _model.ClearResults();
                    _model.ReloadTests();
                }

                if (_view.ContextNode != null)
                    _model.RunTests(_view.ContextNode.Test);
                else
                    _model.RunTests(new TestSelection(_view.SelectedTests));
            };

            _view.Tree.ContextMenu.Popup += (s, e) => InitializeContextMenu();

            _view.ShowCheckBoxes.CheckedChanged += () => _view.CheckBoxes = _view.ShowCheckBoxes.Checked;

            _view.ClearAllCheckBoxes.Execute += () => ClearAllCheckBoxes(_view.Tree.TopNode);

            _view.CheckFailedTests.Execute += () => CheckFailedTests(_view.Tree.TopNode);

            _view.ShowFailedAssumptions.CheckedChanged += () =>
            {
                TestSuiteTreeNode targetNode = _view.ContextNode ?? (TestSuiteTreeNode)_view.Tree.SelectedNode;
                TestSuiteTreeNode theoryNode = targetNode?.GetTheoryNode();
                if (theoryNode != null)
                    theoryNode.ShowFailedAssumptions = _view.ShowFailedAssumptions.Checked;
            };

            _view.EditProject.Execute += () =>
            {
                TestSuiteTreeNode targetNode = _view.ContextNode ?? (TestSuiteTreeNode)_view.Tree.SelectedNode;
                string projectPath = targetNode.TestPackage.FullName;
                EditProject(projectPath);
            };

            _view.ExpandAllCommand.Execute += () => _view.Tree.ExpandAll();

            _view.CollapseAllCommand.Execute += () => _view.Tree.CollapseAll();

            _view.HideTestsCommand.Execute += () => HideTestsUnderNode(_model.Tests);

            _view.PropertiesCommand.Execute += () =>
            {
                TestSuiteTreeNode targetNode = _view.ContextNode ?? (TestSuiteTreeNode)_view.Tree.SelectedNode;
                if (targetNode != null)
                    _view.ShowPropertiesDialog(targetNode);
            };
        }

        private void InitializeContextMenu()
        {
            _view.ShowCheckBoxes.Checked = _view.CheckBoxes;

            TestSuiteTreeNode targetNode = _view.ContextNode ?? (TestSuiteTreeNode)_view.Tree.SelectedNode;

            if (targetNode != null)
            {
                TestNode test = targetNode.Test;
                TestPackage package = targetNode.TestPackage;

                _view.RunCommand.DefaultItem = _view.RunCommand.Enabled && targetNode.Included &&
                    (test.RunState == RunState.Runnable || test.RunState == RunState.Explicit);

                TestSuiteTreeNode theoryNode = targetNode.GetTheoryNode();
                _view.ShowFailedAssumptions.Visible = _view.ShowFailedAssumptions.Enabled = theoryNode != null;
                _view.ShowFailedAssumptions.Checked = theoryNode?.ShowFailedAssumptions ?? false;

                bool showProjectMenu = package != null && test.IsProject;

                _view.ProjectMenu.Visible = _view.ProjectMenu.Enabled = showProjectMenu;
                _view.ActiveConfiguration.Visible = _view.ActiveConfiguration.Enabled = showProjectMenu;
                _view.EditProject.Visible = _view.EditProject.Enabled = showProjectMenu && Path.GetExtension(package.FullName) == ".nunit";

                if (showProjectMenu)
                {
                    string activeConfig = package.GetActiveConfig();
                    string[] configNames = package.GetConfigNames();

                    if (configNames.Length > 0)
                    {
                        _view.ActiveConfiguration.MenuItems.Clear();
                        foreach (string config in configNames)
                        {
                            var configEntry = new MenuItem(config);
                            configEntry.Checked = config == activeConfig;
                            configEntry.Click += (sender, e) => _model.ReloadPackage(package, ((MenuItem)sender).Text);
                            _view.ActiveConfiguration.MenuItems.Add(configEntry);
                        }

                        _view.ActiveConfiguration.Visible = _view.ActiveConfiguration.Enabled = true;
                    }
                }
            }
        }

        private void EditProject(string projectPath)
        {
            const string Q = "\"";
            string editorPath = "nunit-editor.exe";

            Process p = new Process();

            p.StartInfo.FileName = Q + editorPath + Q;
            p.StartInfo.Arguments = Q + projectPath + Q;

            p.Start();
        }

        public void LoadTests(TestNode topLevelTest)
        {
            InitialTreeExpansion displayStyle = (InitialTreeExpansion)_settings.Gui.TestTree.InitialTreeDisplay;
            var nodeCount = CountTestNodes(topLevelTest);
            if (displayStyle == InitialTreeExpansion.Auto)
                displayStyle = _view.Tree.VisibleCount < nodeCount
                    ? InitialTreeExpansion.HideTests
                    : InitialTreeExpansion.Expand;

            LoadTests(topLevelTest, displayStyle);
        }

        private void LoadTests(TestNode topLevelTest, InitialTreeExpansion displayStyle)
        {
            _treeMap.Clear();

            using (new WaitCursor())
            {
                TreeNode topLevelNode = BuildTestTree(topLevelTest, displayStyle, false);
                _view.Tree.Load(topLevelNode);
            }
        }

        private int CountTestNodes(TestNode rootNode)
        {
            int result = 1;

            if (rootNode.IsSuite)
                foreach (TestNode child in rootNode.Children)
                    result += CountTestNodes(child);

            return result;
        }

        private TestSuiteTreeNode BuildTestTree(TestNode testNode, InitialTreeExpansion displayStyle, bool highlight)
        {
            var package = _model.GetPackageForTest(testNode.Id);
            var treeNode = new TestSuiteTreeNode(testNode, package);
            if (highlight) treeNode.ForeColor = Color.Blue;
            _treeMap.Add(testNode.Id, treeNode);
            treeNode.Tag = testNode.Id;

            if (testNode.IsSuite)
            {
                if (testNode.Type == "TestFixture" && displayStyle == InitialTreeExpansion.HideTests)
                    displayStyle = InitialTreeExpansion.Collapse;

                foreach (TestNode child in testNode.Children)
                    treeNode.Nodes.Add(BuildTestTree(child, displayStyle, highlight));

                if (displayStyle == InitialTreeExpansion.Expand || displayStyle == InitialTreeExpansion.HideTests)
                    treeNode.Expand();
            }

            return treeNode;
        }

        /// <summary>
        /// Reload the tree with a changed test hierarchy
        /// while maintaining as much gui state as possible.
        /// </summary>
        /// <param name="test">Test suite to be loaded</param>
        public void ReloadTests(TestNode test)
        {
            VisualState visualState = VisualState.LoadFrom(_view);
            LoadTests(test, InitialTreeExpansion.Collapse);
            visualState.RestoreVisualState(_view, _treeMap);
        }

        private void HideTestsUnderNode(TestNode test)
        {
            if (test.IsSuite && _treeMap.ContainsKey(test.Id))
            {
                TreeNode node = _treeMap[test.Id];

                if (test.Type == "TestFixture")
                    node.Collapse();
                else
                {
                    node.Expand();

                    foreach (TestNode child in test.Children)
                        HideTestsUnderNode(child);
                }
            }
        }

        /// <summary>
        /// Add the result of a test to the tree
        /// </summary>
        /// <param name="result">The result of a test</param>
        private void SetTestResult(ResultNode result)
        {
            if (_treeMap.ContainsKey(result.Id))
            {
                TestSuiteTreeNode node = (TestSuiteTreeNode)_treeMap[result.Id];

                node.Result = result;

                if (result.Type == "Theory")
                    node.RepopulateTheoryNode();
            }
            else
            {
                Debug.WriteLine("Test not found in tree: " + result.FullName);
            }
        }

        public void RestoreResults(TestNode testNode)
        {
            var result = _model.GetResultForTest(testNode.Id);

            if (result != null)
            {
                SetTestResult(result);

                foreach (TestNode child in testNode.Children)
                    RestoreResults(child);
            }
        }

        private static TestNode GetTopDisplayNode(TestNode node)
        {
            return node.Xml.Name == "test-run" && node.Children.Count == 1
                ? node.Children[0]
                : node;
        }

        private static void ClearAllCheckBoxes(TreeNode node)
        {
            node.Checked = false;

            foreach (TreeNode child in node.Nodes)
                ClearAllCheckBoxes(child);
        }

        private void CheckFailedTests(TreeNode node)
        {
            if (node.Nodes.Count == 0) // Only terminal nodes
            {
                string id = (string)node.Tag;
                var result = _model.GetResultForTest(id);

                if (result != null &&
                    !result.IsSuite &&
                    result.Outcome.Status == TestStatus.Failed)
                {
                    node.Checked = true;
                    node.EnsureVisible();
                }
                else
                {
                    node.Checked = false;
                }
            }
            else
                foreach (TreeNode child in node.Nodes)
                    CheckFailedTests(child);
        }

        #region TestFilterVisitor

        public class TestFilterVisitor : TestSuiteTreeNodeVisitor
        {
            private TestNodeFilter filter;

            public TestFilterVisitor(TestNodeFilter filter)
            {
                this.filter = filter;
            }

            public override void Visit(TestSuiteTreeNode node)
            {
                node.Included = filter.Pass(node.Test);
            }
        }

        #endregion
    }
}
