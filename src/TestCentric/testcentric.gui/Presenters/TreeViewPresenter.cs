// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Windows.Forms;
using NUnit.Engine;
using TestCentric.Common;

namespace TestCentric.Gui.Presenters
{
    using Model;
    using Views;
    using Dialogs;

    /// <summary>
    /// TreeViewPresenter is the presenter for the TestTreeView
    /// </summary>
    public class TreeViewPresenter
    {
        private ITestTreeView _view;
        private ITestModel _model;

        private DisplayStrategy _strategy;

        private ITestItem _selectedTestItem;

        private Dictionary<string, TreeNode> _nodeIndex = new Dictionary<string, TreeNode>();

        #region Constructor

        public TreeViewPresenter(ITestTreeView treeView, ITestModel model)
        {
            _view = treeView;
            _model = model;

            Settings = _model.Settings.Gui.TestTree;

            _view.ShowCheckBoxes.Checked = Settings.ShowCheckBoxes;
            _view.AlternateImageSet = (string)Settings.AlternateImageSet;

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
                _strategy.OnTestLoaded(ea.Test);
                InitializeRunCommands();
            };

            _model.Events.TestReloaded += (ea) =>
            {
                _strategy.OnTestLoaded(ea.Test);
                InitializeRunCommands();
            };

            _model.Events.TestUnloaded += (ea) =>
            {
                _strategy.OnTestUnloaded();
                InitializeRunCommands();
            };

            _model.Events.RunStarting += (ea) => InitializeRunCommands();
            _model.Events.RunFinished += (ea) => InitializeRunCommands();

            _model.Events.TestFinished += (ea) => _strategy.OnTestFinished(ea.Result);
            _model.Events.SuiteFinished += (ea) => _strategy.OnTestFinished(ea.Result);

            _model.Settings.Changed += (s, e) =>
            {
                if (e.SettingName == "Gui.TestTree.AlternateImageSet")
                    _view.AlternateImageSet = Settings.AlternateImageSet;
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
            _view.CollapseToFixturesCommand.Execute += () => _strategy.CollapseToFixtures();
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

            // Node selected in tree
            _view.Tree.SelectedNodeChanged += (tn) =>
            {
                _selectedTestItem = tn.Tag as ITestItem;
                _model.NotifySelectedItemChanged(_selectedTestItem);
            };

            // Run button and dropdowns
            _view.RunButton.Execute += () =>
            {
                // Necessary test because we don't disable the button click
                if (_model.HasTests && !_model.IsTestRunning)
                    RunAllTests();
            };
            _view.RunAllCommand.Execute += () => RunAllTests();
            _view.RunSelectedCommand.Execute += () => RunTests(_selectedTestItem);
            _view.StopRunButton.Execute += () => _model.StopTestRun(true);
            _view.TestParametersCommand.Execute += () =>
            {
                using (var dlg = new TestParametersDialog())
                {
                    dlg.Font = _model.Settings.Gui.Font;
                    dlg.StartPosition = FormStartPosition.CenterParent;

                    if (_model.PackageOverrides.ContainsKey("TestParametersDictionary"))
                    {
                        var testParms = _model.PackageOverrides["TestParametersDictionary"] as IDictionary<string, string>;
                        foreach (string key in testParms.Keys)
                            dlg.Parameters.Add(key, testParms[key]);
                    }

                    if (dlg.ShowDialog(_view as IWin32Window) == DialogResult.OK)
                    {
                        ChangePackageSettingAndReload("TestParametersDictionary", dlg.Parameters);
                    }
                }
            };

            // Debug button and dropdowns
            _view.DebugButton.Execute += () =>
            {
                // Necessary test because we don't disable the button click
                if (_model.HasTests && !_model.IsTestRunning)
                    _model.DebugAllTests();
            };
            _view.DebugAllCommand.Execute += () => _model.DebugAllTests();
            _view.DebugSelectedCommand.Execute += () => _model.DebugTests(_selectedTestItem);

            // Change of display format
            _view.DisplayFormat.SelectionChanged += () =>
            {
                SetDisplayStrategy(_view.DisplayFormat.SelectedItem);

                _strategy.Reload();
            };

            _view.RunSummaryButton.Execute += () =>
            {
                // HACK needed until we move menu bar to top level view
                var mainView = (_view as Control).FindForm() as IMainView;
                var resultId = _model.GetResultForTest(_model.Tests.Id);
                var summary = ResultSummaryCreator.FromResultNode(resultId);
                string report = ResultSummaryReporter.WriteSummaryReport(summary);
                mainView.DisplayTestRunSummary(report);
            };
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
                TestPackage package = _model.GetPackageForTest(test.Id);
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

                    displayConfigMenu = true;
                }
            }

            _view.ActiveConfiguration.Visible = displayConfigMenu;
        }

        private void InitializeRunCommands()
        {
            bool isRunning = _model.IsTestRunning;
            bool canRun = _model.HasTests && !isRunning;
            bool canRunChecked = canRun && _view.ShowCheckBoxes.Checked;

            // TODO: Figure out how to disable the button click but not the dropdown.
            //_view.RunButton.Enabled = canRun;
            _view.RunAllCommand.Enabled = canRun;
            _view.RunSelectedCommand.Enabled = canRun;
            _view.TestParametersCommand.Enabled = canRun;
            _view.DebugAllCommand.Enabled = canRun;
            _view.DebugSelectedCommand.Enabled = canRun;
            _view.RunCheckedCommand.Visible = canRunChecked;
            _view.DebugCheckedCommand.Visible = canRunChecked;
            _view.StopRunButton.Enabled = isRunning;

            _view.RunSummaryButton.Visible = !isRunning && _model.HasResults;
        }

        private void SetDefaultDisplayStrategy()
        {
            CreateDisplayStrategy(Settings.DisplayFormat);
        }

        private void SetDisplayStrategy(string format)
        {
            CreateDisplayStrategy(format);
            Settings.DisplayFormat = format;
        }

        private void CreateDisplayStrategy(string format)
        {
            switch (format.ToUpperInvariant())
            {
                default:
                case "NUNIT_TREE":
                    _strategy = new NUnitTreeDisplayStrategy(_view, _model);
                    break;
                case "FIXTURE_LIST":
                    _strategy = new FixtureListDisplayStrategy(_view, _model);
                    break;
                case "TEST_LIST":
                    _strategy = new TestListDisplayStrategy(_view, _model);
                    break;
            }

            _view.FormatButton.ToolTipText = _strategy.Description;
            _view.DisplayFormat.SelectedItem = format;
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

        private Model.Settings.TestTreeSettings Settings { get; }

        #endregion
    }
}
