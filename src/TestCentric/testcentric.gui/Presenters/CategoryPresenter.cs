// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Collections.Generic;

namespace TestCentric.Gui.Presenters
{
    using Model;
    using Model.Settings;
    using Views;

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
