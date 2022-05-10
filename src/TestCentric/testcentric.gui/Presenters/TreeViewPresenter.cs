// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
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

    /// <summary>
    /// TreeViewPresenter is the presenter for the TestTreeView
    /// </summary>
    public class TreeViewPresenter
    {
        private ITestTreeView _view;
        private ITestModel _model;
        private Model.Settings.TestTreeSettings _treeSettings;

        private ITestItem _selectedTestItem;

        //private Dictionary<string, TreeNode> _nodeIndex = new Dictionary<string, TreeNode>();

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

            //InitializeRunCommands();

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

                Strategy.OnTestLoaded(ea.Test);
                //InitializeRunCommands();
                CheckPropertiesDisplay();
                CheckXmlDisplay();
            };

            _model.Events.TestReloaded += (ea) =>
            {
                EnsureNonRunnableFilesAreVisible(ea.Test);

                Strategy.OnTestLoaded(ea.Test);
                _view.CheckBoxes = _view.ShowCheckBoxes.Checked; // TODO: View should handle this
                //InitializeRunCommands();
            };

            _model.Events.TestUnloaded += (ea) =>
            {
                Strategy.OnTestUnloaded();
                //InitializeRunCommands();
            };

            _model.Events.TestsUnloading += ea =>
            {
                Strategy.OnTestUnloading();
                ClosePropertiesDisplay();
                CloseXmlDisplay();
            };

            _model.Events.RunStarting += (ea) =>
            {
                //InitializeRunCommands();
                CheckPropertiesDisplay();
                CheckXmlDisplay();
            };
            _model.Events.RunFinished += (ea) =>
            {
                //InitializeRunCommands();
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
                        CreateDisplayStrategy(_treeSettings.DisplayFormat);
                        Strategy.Reload();
                        break;
                }
            };

            // View actions - Initial Load
            _view.Load += (s, e) =>
            {
                SetDefaultDisplayStrategy();
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

            _view.RunCheckedCommand.Execute += RunCheckedTests;
            _view.DebugContextCommand.Execute += () =>
            {
                if (_selectedTestItem != null) _model.DebugTests(_selectedTestItem);
            };
            _view.DebugCheckedCommand.Execute += DebugCheckedTests;

            _view.TestPropertiesCommand.Execute += () => ShowPropertiesDisplay();

            _view.ViewAsXmlCommand.Execute += () => ShowXmlDisplayDialog();

            _view.SelectedNodeChanged += (treeNode) =>
            {
                var testItem = treeNode.Tag as ITestItem;
                if (testItem != null)
                {
                    _model.ActiveTestItem = testItem;
                    if (!_view.CheckBoxes)
                    {
                        var selection = testItem as TestSelection;
                        var node = testItem as TestNode;
                        if (selection == null && node != null)
                            selection = new TestSelection() { node };
                        _model.SelectedTests = selection;
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
            //_view.SelectedNodesChanged += (nodes) =>
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
        }

        TestPropertiesDisplay _propertiesDisplay;

        private void ShowPropertiesDisplay()
        {
            if (_propertiesDisplay == null)
            {
                var mainForm = ((Control)_view).FindForm();

                _propertiesDisplay = new TestPropertiesDisplay(_model, _view)
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

        private void RunCheckedTests()
        {
            var tests = new TestGroup("RunTests");

            foreach (var treeNode in _view.CheckedNodes)
            {
                var testNode = treeNode.Tag as TestNode;
                if (testNode != null)
                    tests.Add(testNode);
                else
                {
                    var group = treeNode.Tag as TestGroup;
                    if (group != null)
                        tests.AddRange(group);
                }
            }

            _model.RunTests(tests);
        }

        private void DebugCheckedTests()
        {
            var tests = new TestGroup("DebugTests");

            foreach (var treeNode in _view.CheckedNodes)
            {
                var testNode = treeNode.Tag as TestNode;
                if (testNode != null) tests.Add(testNode);
                else
                {
                    var group = treeNode.Tag as TestGroup;
                    if (group != null) tests.AddRange(group);
                }
            }

            _model.DebugTests(tests);
        }

        private void InitializeContextMenu()
        {
            // TODO: Config Menu is hidden until changing the config actually works
            bool displayConfigMenu = false;
            //var test = _view.ContextNode?.Tag as TestNode;
            //if (test != null && test.IsProject)
            //{
            //    NUnit.Engine.TestPackage package = _model.GetPackageForTest(test.Id);
            //    string activeConfig = package.GetActiveConfig();
            //    string[] configNames = package.GetConfigNames();

            //    if (configNames.Length > 0)
            //    {
            //        _view.ActiveConfiguration.MenuItems.Clear();
            //        foreach (string config in configNames)
            //        {
            //            var configEntry = new ToolStripMenuItem(config);
            //            configEntry.Checked = config == activeConfig;
            //            configEntry.Click += (sender, e) => _model.ReloadPackage(package, ((ToolStripMenuItem)sender).Text);
            //            _view.ActiveConfiguration.MenuItems.Add(configEntry);
            //        }

            //        //displayConfigMenu = true;
            //    }
            //}

            _view.RunCheckedCommand.Visible =
            _view.DebugCheckedCommand.Visible = _view.ShowCheckBoxes.Checked;

            _view.ActiveConfiguration.Visible = displayConfigMenu;

            var layout = _model.Settings.Gui.GuiLayout;
            _view.TestPropertiesCommand.Visible = layout == "Mini";
        }

        //private void InitializeRunCommands()
        //{
        //    bool isRunning = _model.IsTestRunning;
        //    bool canRun = _model.HasTests && !isRunning;
        //    bool canRunChecked = canRun && _view.ShowCheckBoxes.Checked;

        //    _view.RunCheckedCommand.Visible =
        //    _view.DebugCheckedCommand.Visible = canRunChecked;
        //}

        private void SetDefaultDisplayStrategy()
        {
            CreateDisplayStrategy(_treeSettings.DisplayFormat);
        }

        private void SetDisplayStrategy(string format)
        {
            CreateDisplayStrategy(format);
            _treeSettings.DisplayFormat = format;
        }

        private void CreateDisplayStrategy(string format)
        {
            switch (format.ToUpperInvariant())
            {
                default:
                case "NUNIT_TREE":
                    Strategy = new NUnitTreeDisplayStrategy(_view, _model);
                    break;
                case "FIXTURE_LIST":
                    Strategy = new FixtureListDisplayStrategy(_view, _model);
                    break;
                case "TEST_LIST":
                    Strategy = new TestListDisplayStrategy(_view, _model);
                    break;
            }
        }

        private void ChangePackageSettingAndReload(string key, object setting)
        {
            if (setting == null || setting as string == "DEFAULT")
                _model.PackageOverrides.Remove(key);
            else
                _model.PackageOverrides[key] = setting;

            // Even though the _model has a Reload method, we cannot use it because Reload
            // does not re-create the Engine.  Since we just changed a setting, we must
            // re-create the Engine by unloading/reloading the tests. We make a copy of
            // __model.TestFiles because the method does an unload before it loads.
            _model.LoadTests(new List<string>(_model.TestFiles));
        }

        #endregion
    }
}
