// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TestCentric.Gui.Elements;

namespace TestCentric.Gui.Dialogs
{
    /// <summary>
    /// This dialog is responsible for the selection of test categories
    /// It displays all categories in a non-modal dialog which is on top of the main form.
    /// Users can select categories within this dialog and can observe the impact to the tree view immediately without leaving the dialog
    /// </summary>
    public partial class CategoryFilterDialog : Form
    {
        public delegate void ApplyButtonClickedEventHandler(IList<string> selectedCategories);

        public event ApplyButtonClickedEventHandler ApplyButtonClicked;

        public CategoryFilterDialog()
        {
            InitializeComponent();

            // Init search text box with 'Search...' content
            searchTextBox.ForeColor = System.Drawing.Color.LightGray;
            TextBoxElement = new TextBoxElement(searchTextBox, "Search...");
            TextBoxElement.Changed += OnTextBoxChanged;
        }

        private IList<string> AllCategories { get; set; }

        private TextBoxElement TextBoxElement { get; set; }

        private IList<string> SelectedCategories
        {
            get
            {
                var selectedItems = new List<string>();
                foreach (var item in checkedListBoxCategory.CheckedItems)
                {
                    selectedItems.Add(item.ToString());
                }

                return selectedItems;
            }
        }

        internal void Init(IEnumerable<string> allCategories, IEnumerable<string> selectedCategories)
        {
            AllCategories = allCategories.ToList();

            checkedListBoxCategory.SuspendLayout();
            foreach (string item in allCategories)
            {
                int index = checkedListBoxCategory.Items.Add(item);
                if (selectedCategories.Contains(item))
                    checkedListBoxCategory.SetItemChecked(index, true);
            }
            checkedListBoxCategory.ResumeLayout();
        }

        internal void UpdateCheckedItems(IEnumerable<string> selectedCategories)
        {
            checkedListBoxCategory.SuspendLayout();
            for (int i = 0; i < checkedListBoxCategory.Items.Count; i++)
            {
                string category = checkedListBoxCategory.Items[i].ToString();
                bool isChecked = selectedCategories.Contains(category);
                checkedListBoxCategory.SetItemChecked(i, isChecked);
            }
            checkedListBoxCategory.ResumeLayout();
        }

        private void OnApplyAndCloseButtonClicked(object sender, EventArgs e)
        {
            ApplyButtonClicked?.Invoke(SelectedCategories);
            Close();
        }

        private void OnApplyButtonClicked(object sender, EventArgs e)
        {
            ApplyButtonClicked?.Invoke(SelectedCategories);
        }

        private void OnSelectAllButtonClicked(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxCategory.Items.Count; i++)
            {
                checkedListBoxCategory.SetItemChecked(i, true);
            }
        }

        private void OnClearButtonClicked(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxCategory.Items.Count; i++)
            {
                checkedListBoxCategory.SetItemChecked(i, false);
            }
        }

        private void OnTextBoxChanged()
        {
            string searchText = searchTextBox.Text;
            IList<string> selectedCategories = SelectedCategories;
            checkedListBoxCategory.SuspendLayout();

            checkedListBoxCategory.Items.Clear();
            foreach (string item in AllCategories)
            {
                if (string.IsNullOrEmpty(searchText) || item.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    int index = checkedListBoxCategory.Items.Add(item);
                    if (selectedCategories.Contains(item))
                        checkedListBoxCategory.SetItemChecked(index, true);
                }
            }

            checkedListBoxCategory.ResumeLayout();
        }
    }
}
