// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TestCentric.Common;

namespace TestCentric.Gui.Presenters
{
    using Model;
    using Views;
    using Dialogs;
    using System.Xml;
    using System.Drawing;
    using System.IO;

    /// <summary>
    /// TreeViewPresenter is the presenter for the TestTreeView
    /// </summary>
    public class TreeViewPresenter
    {
        private ITestTreeView _view;
        private ITestModel _model;
        private Model.Settings.TestTreeSettings _treeSettings;

        // Accessed by tests
        public DisplayStrategy Strategy { get; private set; }

        #region Constructor

        public TreeViewPresenter(ITestTreeView treeView, ITestModel model)
        {
            _view = treeView;
            _model = model;

            _treeSettings = _model.Settings.Gui.TestTree;

            _view.ShowCheckBoxes.Checked = _view.CheckBoxes = _treeSettings.ShowCheckBoxes;
            _view.AlternateImageSet = _treeSettings.AlternateImageSet;

            WireUpEvents();
        }

        #endregion

        #region Private Members

        private void WireUpEvents()
        {
            // Model actions
            _model.Events.TestLoaded += (ea) =>
            {
                EnsureNonRunnableFilesAreVisible(ea.Test);

                bool visualStateLoaded = TryLoadVisualState(out VisualState visualState);
                if (visualStateLoaded)
                {
                    switch (visualState.DisplayStrategy)
                    {
                        case "NUNIT_TREE":
                            Strategy = new NUnitTreeDisplayStrategy(_view, _model);
                            break;
                        case "FIXTURE_LIST":
                            Strategy = new FixtureListDisplayStrategy(_view, _model);
                            break;
                        case "TEST_LIST":
                            Strategy = new TestListDisplayStrategy(_view, _model);
                            break;
                        default:
                            throw new Exception($"Invalid DisplayStrategy: {visualState.DisplayStrategy}");
                    }
                }
                else
                    Strategy = new NUnitTreeDisplayStrategy(_view, _model);

                _view.ShowCheckBoxes.Checked = visualStateLoaded ? visualState.ShowCheckBoxes : _treeSettings.ShowCheckBoxes;
                Strategy.OnTestLoaded(ea.Test, visualState);
                CheckPropertiesDisplay();
                CheckXmlDisplay();
            };

            _model.Events.TestReloaded += (ea) =>
            {
                EnsureNonRunnableFilesAreVisible(ea.Test);

                Strategy.OnTestLoaded(ea.Test, null);
                _view.CheckBoxes = _view.ShowCheckBoxes.Checked; // TODO: View should handle this
            };

            _model.Events.TestUnloaded += (ea) =>
            {
                Strategy.OnTestUnloaded();
            };

            _model.Events.TestsUnloading += ea =>
            {
                Strategy.SaveVisualState();
                ClosePropertiesDisplay();
                CloseXmlDisplay();
            };

            _model.Events.TestsReloading += ea => Strategy.SaveVisualState();


            _model.Events.RunStarting += (ea) =>
            {
                // Save the visual state in case test run causes an exception
                // or user terminates cancels the run.
                Strategy.SaveVisualState();

                CheckPropertiesDisplay();
                CheckXmlDisplay();
            };

            _model.Events.TestFinished += OnTestFinished;
            _model.Events.SuiteFinished += OnTestFinished;

            _model.Settings.Changed += (s, e) =>
            {
                switch (e.SettingName)
                {
                    case "TestCentric.Gui.TestTree.AlternateImageSet":
                        _view.AlternateImageSet = _treeSettings.AlternateImageSet;
                        break;
                    case "TestCentric.Gui.TestTree.DisplayFormat":
                    case "TestCentric.Gui.TestTree.TestList.GroupBy":
                    case "TestCentric.Gui.TestTree.FixtureList.GroupBy":
                        Strategy.Reload();
                        break;
                    case "TestCentric.Gui.TestTree.ShowCheckBoxes":
                        _view.ShowCheckBoxes.Checked = _treeSettings.ShowCheckBoxes;
                        break;
                    case "TestCentric.Gui.GuiLayout":
                        if (_model.Settings.Gui.GuiLayout == "Full")
                            ClosePropertiesDisplay();
                        break;
                }
            };

            // View context commands

            // Test for null is a hack that allows us to avoid
            // a problem under Linux creating a ContextMenuStrip
            // when no display is present.
            _view.ContextMenuOpening += (s, e) => InitializeContextMenu();

            _view.CollapseAllCommand.Execute += () => _view.CollapseAll();
            _view.ExpandAllCommand.Execute += () => _view.ExpandAll();
            _view.CollapseToFixturesCommand.Execute += () => Strategy.CollapseToFixtures();
            _view.ShowCheckBoxes.CheckedChanged += () =>
            {
                _view.CheckBoxes = _view.ShowCheckBoxes.Checked;
            };

            _view.RunContextCommand.Execute += () =>
            {
                if (_view.ContextNode != null)
                {
                    var testNode = _view.ContextNode.Tag as TestNode;
                    if (testNode != null)
                        _model.RunTests(testNode);
                }
            };

            _view.TreeNodeDoubleClick += (treeNode) =>
            {
                var testNode = treeNode.Tag as TestNode;
                if (testNode != null && testNode.Type == "TestCase")
                    _model.RunTests(testNode);
            };

            _view.DebugContextCommand.Execute += () =>
            {
                if (_view.ContextNode != null)
                {
                    var testNode = _view.ContextNode.Tag as TestNode;
                    if (testNode != null)
                        _model.DebugTests(testNode);
                }
            };

            _view.TestPropertiesCommand.Execute += () => ShowPropertiesDisplay();

            _view.ViewAsXmlCommand.Execute += () => ShowXmlDisplayDialog();

            _view.SelectedNodeChanged += (treeNode) =>
            {
                var testItem = treeNode.Tag as ITestItem;
                if (testItem != null)
                {
                    _model.ActiveTestItem = testItem;

                    // Selected item is either a TestSelection or a TestNode. When
                    // CheckBoxes are off, the active item is used as the selection.
                    var selection = testItem as TestSelection;
                    var node = testItem as TestNode;

                    if (!_view.CheckBoxes)
                    {
                        // If it's a TestNode, make a TestSelection
                        if (selection == null && node != null)
                            selection = new TestSelection() { node };
                        _model.SelectedTests = selection;
                    }

                    if (_propertiesDisplay != null)
                    {
                        if (_propertiesDisplay.Pinned)
                            _propertiesDisplay.Display(treeNode);
                        else
                            ClosePropertiesDisplay();
                    }

                    if (_xmlDisplay != null)
                    {
                        if (_xmlDisplay.Pinned)
                            _xmlDisplay.Display(treeNode);
                        else
                            CloseXmlDisplay();
                    }
                }
            };

            _view.AfterCheck += (treeNode) =>
            {
                var checkedNodes = _view.CheckedNodes;
                var selection = new TestSelection();

                foreach (var node in checkedNodes)
                    selection.Add(node.Tag as TestNode);
                
                _model.SelectedTests = selection;
            };

            // Node selected in tree
            //_treeView.SelectedNodesChanged += (nodes) =>
            //{
            //    var selection = new TestSelection();
            //    foreach (TreeNode tn in nodes)
            //    {
            //        var test = tn.Tag as TestNode;
            //        if (test != null)
            //            selection.Add(test);
            //        _model.NotifyTestSelectionChanged(selection);
            //    }

            //    if (_propertiesDisplay != null)
            //    {
            //        if (_propertiesDisplay.Pinned && nodes.Count == 1)
            //            _propertiesDisplay.Display(nodes[0]);
            //        else
            //            ClosePropertiesDisplay();
            //    }

            //    if (_xmlDisplay != null)
            //    {
            //        if (_xmlDisplay.Pinned && nodes.Count == 1)
            //            _xmlDisplay.Display(nodes[0]);
            //        else
            //            CloseXmlDisplay();
            //    }
            //};
        }

        private void EnsureNonRunnableFilesAreVisible(TestNode testNode)
        {
            // HACK: Temporary fix switches the display strategy if no
            // tests are found. Should handle other error situations
            // including one non-runnable file out of several files.
            if (testNode.TestCount == 0)
                Strategy = new NUnitTreeDisplayStrategy(_view, _model);
        }

        private void OnTestFinished(TestResultEventArgs args)
        {
            Strategy.OnTestFinished(args.Result);

            _propertiesDisplay?.OnTestFinished(args.Result);
            _xmlDisplay?.OnTestFinished(args.Result);
        }

        private bool TryLoadVisualState(out VisualState visualState)
        {
            visualState = null;

            if (_model.TestCentricProject.TestFiles.Count > 0)
            {
                var filename = VisualState.GetVisualStateFileName(_model.TestCentricProject.TestFiles[0]);
                if (File.Exists(filename))
                    visualState = VisualState.LoadFrom(filename);
            }

            return visualState != null;
        }

        TestPropertiesDialog _propertiesDisplay;

        private void ShowPropertiesDisplay()
        {
            if (_propertiesDisplay == null)
            {
                var mainForm = ((Control)_view).FindForm();

                _propertiesDisplay = new TestPropertiesDialog(_model, _view)
                {
                    Owner = mainForm,
                    Font = mainForm.Font,
                    StartPosition = FormStartPosition.Manual
                };

                var midScreen = Screen.FromHandle(mainForm.Handle).WorkingArea.Width / 2;
                var midForm = (mainForm.Left + mainForm.Right) / 2;

                _propertiesDisplay.Left = midForm < midScreen
                    ? mainForm.Right
                    : Math.Max(0, mainForm.Left - _propertiesDisplay.Width);

                _propertiesDisplay.Top = mainForm.Top;

                _propertiesDisplay.Closed += (s, e) => _propertiesDisplay = null;
            }

            _propertiesDisplay.Display(_view.ContextNode);
        }

        private void ClosePropertiesDisplay()
        {
            if (_propertiesDisplay != null)
            {
                _propertiesDisplay.Close();
                _propertiesDisplay = null;
            }
        }

        private void CheckPropertiesDisplay()
        {
            if (_propertiesDisplay != null && !_propertiesDisplay.Pinned)
                ClosePropertiesDisplay();
        }

        private XmlDisplay _xmlDisplay;

        private void ShowXmlDisplayDialog()
        {
            if (_xmlDisplay == null)
            {
                var treeView = (Control)_view;
                var mainForm = treeView.FindForm();

                _xmlDisplay = new XmlDisplay(_model)
                {
                    Owner = mainForm,
                    Font = mainForm.Font,
                    StartPosition = FormStartPosition.Manual
                };

                var midForm = (mainForm.Left + mainForm.Right) / 2;
                var screenArea = Screen.FromHandle(mainForm.Handle).WorkingArea;
                var midScreen = screenArea.Width / 2;

                var myLeft = mainForm.Left;
                var myRight = mainForm.Right;

                if (_propertiesDisplay != null)
                {
                    myLeft = Math.Min(myLeft, _propertiesDisplay.Left);
                    myRight = Math.Max(myRight, _propertiesDisplay.Right);
                }

                _xmlDisplay.Left = myLeft > screenArea.Width - myRight
                    ? Math.Max(0, myLeft - _xmlDisplay.Width)
                    : Math.Min(myRight, screenArea.Width - _xmlDisplay.Width);

                _xmlDisplay.Top = mainForm.Top + (mainForm.Height - _xmlDisplay.Height) / 2;

                _xmlDisplay.Closed += (s, e) => _xmlDisplay = null;
            }

            _xmlDisplay.Display(_view.ContextNode);
        }

        private void CloseXmlDisplay()
        {
            _xmlDisplay?.Close();
        }

        private void CheckXmlDisplay()
        {
            if (_xmlDisplay != null && !_xmlDisplay.Pinned)
                _xmlDisplay.Close();

        }

        private void InitializeContextMenu()
        {
            // TODO: Config Menu is hidden until changing the config actually works
            bool displayConfigMenu = false;
            //var test = _treeView.ContextNode?.Tag as TestNode;
            //if (test != null && test.IsProject)
            //{
            //    NUnit.Engine.TestPackage package = _model.GetPackageForTest(test.Id);
            //    string activeConfig = package.GetActiveConfig();
            //    string[] configNames = package.GetConfigNames();

            //    if (configNames.Length > 0)
            //    {
            //        _treeView.ActiveConfiguration.MenuItems.Clear();
            //        foreach (string config in configNames)
            //        {
            //            var configEntry = new ToolStripMenuItem(config);
            //            configEntry.Checked = config == activeConfig;
            //            configEntry.Click += (sender, e) => _model.ReloadPackage(package, ((ToolStripMenuItem)sender).Text);
            //            _treeView.ActiveConfiguration.MenuItems.Add(configEntry);
            //        }

            //        //displayConfigMenu = true;
            //    }
            //}

            _view.ActiveConfiguration.Visible = displayConfigMenu;

            var layout = _model.Settings.Gui.GuiLayout;
            _view.TestPropertiesCommand.Visible = layout == "Mini";
        }

        #endregion
    }
}
