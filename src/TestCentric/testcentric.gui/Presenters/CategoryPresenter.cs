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
using System.Collections.Generic;

namespace TestCentric.Gui.Presenters
{
    using Views;
    using Model;
    using Model.Settings;

    public class CategoryPresenter
    {
        private ITestModel _model;
        private ICategoryView _view;
        private UserSettings _settings;

        public CategoryPresenter(ICategoryView view, ITestModel model)
        {
            _model = model;
            _view = view;
            _settings = model.Services.UserSettings;

            WireUpEvents();
        }

        private void WireUpEvents()
        {
            _view.AddButton.Execute += AddSelectedItems;

            _view.AvailableList.DoubleClick += AddSelectedItems;

            _view.RemoveButton.Execute += RemoveSelectedItems;

            _view.SelectedList.DoubleClick += RemoveSelectedItems;

            _view.ExcludeCategories.CheckedChanged += UpdateCategorySelection;

            _model.Events.TestLoaded += (e) => ReloadCategories();

            _model.Events.TestReloaded += (e) => ReloadCategories();

            _model.Events.TestUnloaded += (e) => _view.Clear();
        }

        private void AddSelectedItems()
        {
            if (_view.AvailableList.SelectedItems.Count > 0)
            {
                // Create a separate list of selected items because
                // we modify the list in the next loop.
                var categories = new List<string>();
                foreach (string category in _view.AvailableList.SelectedItems)
                    categories.Add(category);

                foreach (string category in categories)
                {
                    _view.SelectedList.Add(category);
                    _view.AvailableList.Remove(category);
                }

                if (_view.SelectedList.Items.Count > 0)
                    _view.ExcludeCategories.Enabled = true;

                UpdateCategorySelection();
            }
        }

        private void RemoveSelectedItems()
        {
            if (_view.SelectedList.SelectedItems.Count > 0)
            {
                // Create a separate list of selected items because
                // we modify the list in the next loop.
                var categories = new List<string>();
                foreach (string category in _view.SelectedList.SelectedItems)
                    categories.Add(category);

                foreach (string category in categories)
                {
                    _view.SelectedList.Remove(category);
                    _view.AvailableList.Add(category);
                }

                if (_view.SelectedList.Items.Count == 0)
                {
                    _view.ExcludeCategories.Checked = false;
                    _view.ExcludeCategories.Enabled = false;
                }

                UpdateCategorySelection();
            }
        }

        private void UpdateCategorySelection()
        {
            var categories = new List<string>();

            foreach (string category in _view.SelectedList.Items)
                categories.Add(category);

            _model.SelectCategories(categories, _view.ExcludeCategories.Checked);
        }

        private void ReloadCategories()
        {
            _view.Clear();

            foreach (string item in _model.AvailableCategories)
                _view.AvailableList.Items.Add(item);

            if (_model.SelectedCategories != null)
            {
                foreach (string item in _model.SelectedCategories)
                {
                    _view.SelectedList.Items.Add(item);
                    _view.AvailableList.Items.Remove(item);
                }

                _view.ExcludeCategories.Checked = _model.ExcludeSelectedCategories;
            }
        }
    }
}
