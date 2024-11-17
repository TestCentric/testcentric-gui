// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TestCentric.Gui.Dialogs;
using TestCentric.Gui.Model;
using TestCentric.Gui.Views;

namespace TestCentric.Gui.Elements
{
    internal class ToolStripDropDownButtonElement : ToolStripElement, ICommand
    {
        private ToolStripDropDownButton _button;

        public event CommandHandler Execute;

        public ToolStripDropDownButtonElement(ToolStripDropDownButton button) : base(button)
        {
            _button = button;

            button.Click += OnButtonClicked;
        }

        protected virtual void OnButtonClicked(object sender, EventArgs e)
        {
            Execute?.Invoke();
        }
    }

    internal class ToolStripFilterByCategoryButton : IMultiSelection
    {
        private ToolStripDropDownButton _button;
        private ITestModel _model;
        private CategoryFilterDialog _view;
        private Point _dialogLocation;
        private Size _dialogSize;
        public event CommandHandler SelectionChanged;
        IEnumerable<string> _selectedItems;

        public ToolStripFilterByCategoryButton(ToolStripDropDownButton button)
        {
            _button = button;
            button.Click += OnButtonClicked;
        }

        public IEnumerable<string> SelectedItems 
        { 
            get => _selectedItems;
            set
            {
                _selectedItems = value;
                UpdateFont();
            }
        }

        public bool Enabled
        {
            get => _button.Enabled;
            set => _button.Enabled = value;
        }

        public bool Visible
        {
            get => _button.Visible;
            set => _button.Visible = value;
        }

        public string Text
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void InvokeIfRequired(MethodInvoker _delegate)
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        internal void InitModel(ITestModel model)
        {
            _model = model;
        }

        protected void OnButtonClicked(object sender, EventArgs e)
        {
            // Dialog is already opened
            if (_view != null)
            {
                _view.Focus();
                return;
            }

            _view = new CategoryFilterDialog();
            if (_dialogSize != default(Size))
            {
                _view.StartPosition = FormStartPosition.Manual;
                _view.Location = _dialogLocation;
                _view.Size = _dialogSize;
            }

            var allCategories = _model.TestCentricTestFilter.AllCategories;
            _view.Init(allCategories, _model.TestCentricTestFilter.CategoryFilter);
            _view.ApplyButtonClicked += (selectedItems) =>
            {
                SelectedItems = selectedItems;
                if (SelectionChanged != null)
                    SelectionChanged();
            };

            _view.FormClosing += (s, args) =>
            {
                _dialogLocation = _view.Location;
                _dialogSize = _view.Size;
                _view = null;
            };

            _view.Show();
        }

        private void UpdateFont()
        {
            var allCategories = _model.TestCentricTestFilter.AllCategories;

            var style = allCategories.Except(SelectedItems).Any() ? FontStyle.Bold : FontStyle.Regular;
            _button.Font = new Font(_button.Font, style);
        }

    }
}
