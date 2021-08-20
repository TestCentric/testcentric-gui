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

            _view.ShowCheckBoxes.Checked = _treeSettings.ShowCheckBoxes;
            _view.AlternateImageSet = _treeSettings.AlternateImageSet;

            InitializeRunCommands();

            WireUpEvents();
        }

        #endregion

        #region Private Members

        private void WireUpEvents()
        {
            // Model actions
            _model.Events.TestLoaded += (ea) =>
            {
                Strategy.OnTestLoaded(ea.Test);
                InitializeRunCommands();
                CheckPropertiesDialog();
            };

            _model.Events.TestReloaded += (ea) =>
            {
                Strategy.OnTestLoaded(ea.Test);
                InitializeRunCommands();
            };

            _model.Events.TestUnloaded += (ea) =>
            {
                Strategy.OnTestUnloaded();
                InitializeRunCommands();
            };

            _model.Events.TestsUnloading += ea =>
            {
                Strategy.OnTestUnloading();
                ClosePropertiesDialog();
            };

            _model.Events.RunStarting += (ea) =>
            {
                InitializeRunCommands();
                CheckPropertiesDialog();
            };
            _model.Events.RunFinished += (ea) =>
            {
                InitializeRunCommands();
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
            if (_view.Tree.ContextMenuStrip != null)
                _view.Tree.ContextMenuStrip.Opening += (s, e) => InitializeContextMenu();

            _view.CollapseAllCommand.Execute += () => _view.CollapseAll();
            _view.ExpandAllCommand.Execute += () => _view.ExpandAll();
            _view.CollapseToFixturesCommand.Execute += () => Strategy.CollapseToFixtures();
            _view.ShowCheckBoxes.CheckedChanged += () =>
            {
                _view.RunCheckedCommand.Visible =
                _view.DebugCheckedCommand.Visible =
                _view.Tree.CheckBoxes = _view.ShowCheckBoxes.Checked;
            };
            _view.RunContextCommand.Execute += () =>
            {
                if (_selectedTestItem != null)
                    _model.RunTests(_selectedTestItem);
            };
            _view.RunCheckedCommand.Execute += RunCheckedTests;
            _view.DebugContextCommand.Execute += () =>
            {
                if (_selectedTestItem != null) _model.DebugTests(_selectedTestItem);
            };
            _view.DebugCheckedCommand.Execute += DebugCheckedTests;

            _view.TestPropertiesCommand.Execute += () => ShowPropertiesDialog();

            // Node selected in tree
            _view.Tree.SelectedNodeChanged += (tn) =>
            {
                _selectedTestItem = tn.Tag as ITestItem;
                _model.NotifySelectedItemChanged(_selectedTestItem);

                if (_propertiesDialog != null)
                {
                    if (_propertiesDialog.Pinned)
                        _propertiesDialog.Display(tn);
                    else
                        _propertiesDialog.Close();
                }
            };
        }

        private void OnTestFinished(TestResultEventArgs args)
        {
            Strategy.OnTestFinished(args.Result);

            _propertiesDialog?.OnTestFinished(args.Result);
        }

        TestPropertiesDialog _propertiesDialog;

        private void ShowPropertiesDialog()
        {
            if (_propertiesDialog == null)
                _propertiesDialog = CreatePropertiesDialog();

            _propertiesDialog.Display(_view.ContextNode);
        }

        private TestPropertiesDialog CreatePropertiesDialog()
        {
            var mainForm = ((Control)_view).FindForm();

            var propertiesDialog = new TestPropertiesDialog(_model, _view)
            {
                Owner = mainForm,
                Font = mainForm.Font,
                StartPosition = FormStartPosition.Manual
            };

            var midScreen = Screen.FromHandle(mainForm.Handle).WorkingArea.Width / 2;
            var midForm = (mainForm.Left + mainForm.Right) / 2;

            propertiesDialog.Left = midForm < midScreen
                ? mainForm.Right
                : Math.Max(0, mainForm.Left - propertiesDialog.Width);

            propertiesDialog.Top = mainForm.Top;

            propertiesDialog.Closed += (s, e) => _propertiesDialog = null;

            return propertiesDialog;
        }

        private void ClosePropertiesDialog()
        {
            if (_propertiesDialog != null)
                _propertiesDialog.Close();
        }

        private void CheckPropertiesDialog()
        {
            if (_propertiesDialog != null && !_propertiesDialog.Pinned)
                _propertiesDialog.Close();
        }

        private void RunAllTests()
        {
            if (_model.Settings.Engine.ReloadOnRun)
                _model.ReloadTests();

            _model.RunAllTests();
        }

        private void RunTests(ITestItem testItem)
        {
            if (_model.Settings.Engine.ReloadOnRun)
                _model.ReloadTests();

            _model.RunTests(testItem);
        }

        private void RunCheckedTests()
        {
            var tests = new TestGroup("RunTests");

            foreach (var treeNode in _view.Tree.CheckedNodes)
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

            foreach (var treeNode in _view.Tree.CheckedNodes)
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
            bool displayConfigMenu = false;
            var test = _view.ContextNode?.Tag as TestNode;
            if (test != null && test.IsProject)
            {
                NUnit.Engine.TestPackage package = _model.GetPackageForTest(test.Id);
                string activeConfig = package.GetActiveConfig();
                string[] configNames = package.GetConfigNames();

                if (configNames.Length > 0)
                {
                    _view.ActiveConfiguration.MenuItems.Clear();
                    foreach (string config in configNames)
                    {
                        var configEntry = new ToolStripMenuItem(config);
                        configEntry.Checked = config == activeConfig;
                        configEntry.Click += (sender, e) => _model.ReloadPackage(package, ((ToolStripMenuItem)sender).Text);
                        _view.ActiveConfiguration.MenuItems.Add(configEntry);
                    }

                    // TODO: Menu is hidden until changing the config actually works
                    //displayConfigMenu = true;
                }
            }

            _view.ActiveConfiguration.Visible = displayConfigMenu;

            var layout = _model.Settings.Gui.GuiLayout;
            _view.TestPropertiesCommand.Visible = layout == "Mini";
        }

        private void InitializeRunCommands()
        {
            bool isRunning = _model.IsTestRunning;
            bool canRun = _model.HasTests && !isRunning;
            bool canRunChecked = canRun && _view.ShowCheckBoxes.Checked;

            _view.RunCheckedCommand.Visible =
            _view.DebugCheckedCommand.Visible = canRunChecked;
        }

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
