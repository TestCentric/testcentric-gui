// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TestCentric.Gui.Dialogs
{
    public partial class CategoryFilterDialog : Form
    {
        public delegate void ApplyButtonClickedEventHandler(IList<string> selectedCategories);

        public event ApplyButtonClickedEventHandler ApplyButtonClicked;

        public CategoryFilterDialog()
        {
            InitializeComponent();

            // Init search text box with 'Search...' content
            searchTextBox.ForeColor = System.Drawing.Color.LightGray;
            OnSearchTextBoxLostFocus(null, EventArgs.Empty);
        }

        private IList<string> AllCategories { get; set; }

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

        private void OnSearchTextBoxGotFocus(object sender, EventArgs e)
        {
            if (PlaceHolderTextUsed)
            {
                searchTextBox.Text = "";
                searchTextBox.ForeColor = System.Drawing.Color.Black;
                PlaceHolderTextUsed = false;
            }
        }

        private void OnSearchTextBoxLostFocus(object sender, EventArgs e)
        {
            string searchText = searchTextBox.Text;
            if (string.IsNullOrEmpty(searchText))
            {
                PlaceHolderTextUsed = true;
                searchTextBox.Text = "Search...";
                searchTextBox.ForeColor = System.Drawing.Color.LightGray;
            }
        }

        private bool PlaceHolderTextUsed { get; set; }
        private Timer _typingTimer;

        private void OnSearchTextBoxTextChanged(object sender, EventArgs e)
        {
            if (PlaceHolderTextUsed)
                return;

            if (_typingTimer == null)
            {
                _typingTimer = new Timer();
                _typingTimer.Interval = 600;
                _typingTimer.Tick += TypingTimerTimeout;
            }

            _typingTimer.Stop();
            _typingTimer.Start();
        }

        private void TypingTimerTimeout(object sender, EventArgs e)
        {
            var timer = sender as Timer;
            if (timer == null)
                return;

            // The timer must be stopped!
            timer.Stop();

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
