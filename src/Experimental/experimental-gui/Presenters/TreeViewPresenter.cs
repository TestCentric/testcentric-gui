// ***********************************************************************
// Copyright (c) 2016 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System.Collections.Generic;
using System.Windows.Forms;

namespace TestCentric.Gui.Presenters
{
    using Model;
    using Views;

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

            Settings = _model.Services.UserSettings.Gui.TestTree;

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

            _model.Services.UserSettings.Changed += (s, e) =>
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
            _view.RunFailedCommand.Execute += () => RunAllTests(); // RunFailed NYI
            _view.StopRunCommand.Execute += () => _model.CancelTestRun(true);

            // Debug button and dropdowns
            _view.DebugButton.Execute += () =>
            {
                // Necessary test because we don't disable the button click
                if (_model.HasTests && !_model.IsTestRunning)
                    _model.DebugAllTests();
            };
            _view.DebugAllCommand.Execute += () => _model.DebugAllTests();
            _view.DebugSelectedCommand.Execute += () => _model.DebugTests(_selectedTestItem);
            _view.DebugFailedCommand.Execute += () => _model.DebugAllTests(); // NYI

            // Change of display format
            _view.DisplayFormat.SelectionChanged += () =>
            {
                SetDisplayStrategy(_view.DisplayFormat.SelectedItem);

                _strategy.Reload();
            };
        }

        private void RunAllTests()
        {
            if (_model.Services.UserSettings.Engine.ReloadOnRun)
                _model.ReloadTests();

            _model.RunAllTests();
        }

        private void RunTests(ITestItem testItem)
        {
            if (_model.Services.UserSettings.Engine.ReloadOnRun)
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

        private void InitializeRunCommands()
        {
            bool isRunning = _model.IsTestRunning;
            bool canRun = _model.HasTests && !isRunning;
            bool canRunChecked = canRun && _view.ShowCheckBoxes.Checked;

            // TODO: Figure out how to disable the button click but not the dropdown.
            //_view.RunButton.Enabled = canRun;
            _view.RunAllCommand.Enabled = canRun;
            _view.RunSelectedCommand.Enabled = canRun;
            _view.RunFailedCommand.Enabled = canRun;
            _view.DebugAllCommand.Enabled = canRun;
            _view.DebugSelectedCommand.Enabled = canRun;
            _view.DebugFailedCommand.Enabled = canRun;
            _view.RunCheckedCommand.Visible = canRunChecked;
            _view.DebugCheckedCommand.Visible = canRunChecked;
            _view.StopRunCommand.Enabled = isRunning;
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

        private Model.Settings.TestTreeSettings Settings { get; }

        #endregion
    }
}
