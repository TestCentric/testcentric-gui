// ***********************************************************************
// Copyright (c) 2018 Charlie Poole
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

using System;
using System.Diagnostics;
using System.IO;

namespace TestCentric.Gui.Presenters
{
    using Views;
    using Model;
    using Model.Settings;

    public class TreeViewPresenter
    {
        private ITestTreeView _view;
        private ITestModel _model;
        private UserSettings _settings;

        public TreeViewPresenter(ITestTreeView view, ITestModel model)
        {
            _view = view;
            _model = model;
            _settings = model.Services.UserSettings;

            _view.DisplayStyle = (DisplayStyle)_settings.Gui.TestTree.InitialTreeDisplay;
            _view.AlternateImageSet = (string)_settings.Gui.TestTree.AlternateImageSet;

            //_view.ShowCheckBoxes.Checked = _view.CheckBoxes = _settings.Gui.TestTree.ShowCheckBoxes;

            _view.RunCommand.Enabled = false;

            WireUpEvents();
        }

        private void WireUpEvents()
        {
            _model.Events.TestsLoading += (e) =>
            {
                _view.RunCommand.Enabled = false;
            };

            _model.Events.TestLoaded += (e) =>
            {
                _view.RunCommand.Enabled = true;
                _view.CheckPropertiesDialog();

                _view.LoadTests(e.Test);

                if (_model.Services.UserSettings.Gui.TestTree.SaveVisualState)
                {
                    string fileName = VisualState.GetVisualStateFileName(_model.TestFiles[0]);
                    if (File.Exists(fileName))
                    {
                        var visualState = VisualState.LoadFrom(fileName);
                        _view.RestoreVisualState(visualState);
                        _model.SelectCategories(visualState.SelectedCategories, visualState.ExcludeCategories);
                    }
                }
            };

            _model.Events.TestsReloading += (e) =>
            {
                _view.RunCommand.Enabled = false;
            };

            _model.Events.TestReloaded += (e) =>
            {
                _view.Reload(e.Test);

                if (!_settings.Gui.ClearResultsOnReload)
                    RestoreResults(e.Test);

                _view.RunCommand.Enabled = true;
            };

            _model.Events.TestsUnloading += (e) =>
            {
                _view.RunCommand.Enabled = false;

                _view.ClosePropertiesDialog();

                if (_settings.Gui.TestTree.SaveVisualState)
                    try
                    {
                        var visualState = _view.GetVisualState();
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
            };

            _model.Events.TestUnloaded += (e) =>
            {
                _view.RunCommand.Enabled = false;
            };

            _model.Events.RunStarting += (e) =>
            {
                _view.RunCommand.Enabled = false;
                _view.CheckPropertiesDialog();
            };

            _model.Events.RunFinished += (e) =>
            {
                _view.RunCommand.Enabled = true;
            };

            _model.Events.TestFinished += (e) =>
            {
                _view.SetTestResult(e.Result);
            };

            _model.Events.SuiteFinished += (e) =>
            {
                _view.SetTestResult(e.Result);
            };

            //_settings.Changed += (s, e) =>
            //{
            //    if (e.SettingName == "Gui.TestTree.AlternateImageSet")
            //   {
            //       _view.LoadAlternateImages();
            //       _view.Invalidate();
            //   }
            //};

            _model.Events.CategorySelectionChanged += (TestEventArgs e) =>
            {
                TestNodeFilter filter = TestNodeFilter.Empty;

                if (_model.SelectedCategories.Count > 0)
                {
                    filter = new CategoryFilter(_model.SelectedCategories);
                    if (_model.ExcludeSelectedCategories)
                        filter = new NotFilter(filter);
                }

                _view.TreeFilter = filter;
            };

            _view.FileDrop += _model.LoadTests;

            _view.RunCommand.Execute += () =>
            {
                if (_settings.Gui.ReloadOnRun)
                    _model.ClearResults();

                if (_view.ContextNode != null)
                    _model.RunTests(_view.ContextNode.Test);
                else
                    _model.RunTests(new TestSelection(_view.SelectedTests));
            };

            _view.ShowCheckBoxes.CheckedChanged += () =>
            {
                _settings.Gui.TestTree.ShowCheckBoxes = _view.CheckBoxes = _view.ShowCheckBoxes.Checked;
            };

            _view.ShowFailedAssumptions.CheckedChanged += () =>
            {
                TestSuiteTreeNode targetNode = _view.ContextNode ?? (TestSuiteTreeNode)_view.SelectedNode;
                TestSuiteTreeNode theoryNode = targetNode?.GetTheoryNode();
                if (theoryNode != null)
                    theoryNode.ShowFailedAssumptions = _view.ShowFailedAssumptions.Checked;
            };

            _view.PropertiesCommand.Execute += () =>
            {
                TestSuiteTreeNode targetNode = _view.ContextNode ?? (TestSuiteTreeNode)_view.SelectedNode;
                if (targetNode != null)
                    _view.ShowPropertiesDialog(targetNode);
            };
        }

        public void RestoreResults(TestNode testNode)
        {
            var result = _model.GetResultForTest(testNode);

            if (result != null)
            {
                _view.SetTestResult(result);

                foreach (TestNode child in testNode.Children)
                    RestoreResults(child);
            }
        }
    }
}
