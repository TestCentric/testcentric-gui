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

namespace TestCentric.Gui.Elements
{
    /// <summary>
    /// This class is responsible to open the CategoryFilter dialog when the ToolStripDropDownButton is clicked
    /// - it will open the dialog as a non-modal dialog which is displayed on top of the main form
    /// - it will place the dialog below the ToolStripDropDownButton when opened for the first time
    ///   or restore the last position/size when opened for the second time
    /// </summary>
    internal class ToolStripCategoryFilterButton : ToolStripElement, ICategoryFilterSelection
    {
        public event CommandHandler SelectionChanged;

        private ToolStripDropDownButton _button;
        private ITestModel _model;
        private CategoryFilterDialog _dialog;
        private Point _dialogLocation;
        private Size _dialogSize;
        IEnumerable<string> _selectedItems;

        public ToolStripCategoryFilterButton(ToolStripDropDownButton button)
            : base(button)
        {
            _button = button;
            _button.Click += OnButtonClicked;
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

        public void Init(ITestModel model)
        {
            _model = model;
            SelectedItems = _model.TestCentricTestFilter.CategoryFilter.ToList();
        }


        public void Close()
        {
            _dialog?.Close();
        }

        protected void OnButtonClicked(object sender, EventArgs e)
        {
            // Dialog is already opened => just set the focus
            if (_dialog != null)
            {
                _dialog.Focus();
                return;
            }

            // Create dialog and set Owner property to place it on top of main form
            _dialog = new CategoryFilterDialog();
            _dialog.Owner = Form.ActiveForm;
            SetDialogSizeAndPosition();

            // Init dialog with available test categories and currently selected categories
            var allCategories = _model.TestCentricTestFilter.AllCategories;
            _dialog.Init(allCategories, _model.TestCentricTestFilter.CategoryFilter);

            _dialog.ApplyButtonClicked += (selectedItems) =>
            {
                SelectedItems = selectedItems;
                SelectionChanged?.Invoke();
            };

            // Dialog closing: save dialog position and size for next usage
            _dialog.FormClosing += (s, args) =>
            {
                _dialogLocation = _dialog.Location;
                _dialogSize = _dialog.Size;
                _dialog = null;
            };

            // Show dialog non-modal
            _dialog.Show();
        }

        private void SetDialogSizeAndPosition()
        {
            _dialog.StartPosition = FormStartPosition.Manual;

            if (_dialogSize != default(Size))
            {
                // Restore previous position and size of dialog
                _dialog.Size = _dialogSize;
                _dialogLocation = _dialog.Location;
            }
            else
            {
                // Determine position of button on screen and place dialog below
                var bounds = _button.AccessibilityObject.Bounds;
                _dialog.Location = new Point(bounds.Left, bounds.Bottom);
            }
        }

        private void UpdateFont()
        {
            var allCategories = _model.TestCentricTestFilter.AllCategories;

            var style = allCategories.Except(SelectedItems).Any() ? FontStyle.Bold : FontStyle.Regular;
            _button.Font = new Font(_button.Font, style);
        }

    }
}
