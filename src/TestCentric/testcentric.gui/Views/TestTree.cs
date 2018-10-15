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
using System.IO;
using System.Windows.Forms;
using NUnit.Engine;

namespace TestCentric.Gui.Views
{
    using Model;
    using Controls;

    /// <summary>
    /// Summary description for TestTree.
    /// </summary>
    public class TestTree : UserControl, IViewControl
    {
        #region Instance Variables

        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage testPage;
        private System.Windows.Forms.TabPage categoryPage;
        private System.Windows.Forms.Panel testPanel;
        private System.Windows.Forms.Panel categoryPanel;
        private TestTreeView tests;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox availableList;
        private System.Windows.Forms.GroupBox selectedCategories;
        private System.Windows.Forms.ListBox selectedList;
        private System.Windows.Forms.Panel categoryButtonPanel;
        private System.Windows.Forms.Button addCategory;
        private System.Windows.Forms.Button removeCategory;
        private System.Windows.Forms.CheckBox excludeCheckbox;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        #endregion

        #region Properties

        public TestTreeView TreeView
        {
            get { return tests; }
        }

        public string[] SelectedCategories
        {
            get
            {
                int n = this.selectedList.Items.Count;
                string[] categories = new string[n];
                for (int i = 0; i < n; i++)
                    categories[i] = this.selectedList.Items[i].ToString();
                return categories;
            }
        }

        private ITestModel Model { get; set; }

        #endregion

        #region Construction and Initialization

        public TestTree()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            //tests.SelectedTestChanged += new SelectedTestChangedHandler(tests_SelectedTestChanged);
            //tests.CheckedTestChanged += new CheckedTestChangedHandler(tests_CheckedTestChanged);

            this.excludeCheckbox.Enabled = false;
        }

        public void ClearSelectedCategories()
        {
            foreach (string cat in selectedList.Items)
                availableList.Items.Add(cat);
            selectedList.Items.Clear();
            UpdateCategorySelection();
        }

        public void SelectCategories(string[] categories, bool exclude)
        {
            foreach (string category in categories)
            {
                if (Model.AvailableCategories.Contains(category))
                {
                    if (!selectedList.Items.Contains(category))
                    {
                        selectedList.Items.Add(category);
                    }
                    availableList.Items.Remove(category);

                    this.excludeCheckbox.Checked = exclude;
                }
            }

            UpdateCategorySelection();

            if (this.SelectedCategories.Length > 0)
                this.excludeCheckbox.Enabled = true;
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabs = new System.Windows.Forms.TabControl();
            this.testPage = new System.Windows.Forms.TabPage();
            this.testPanel = new System.Windows.Forms.Panel();
            this.tests = new TestTreeView();
            this.categoryPage = new System.Windows.Forms.TabPage();
            this.categoryPanel = new System.Windows.Forms.Panel();
            this.categoryButtonPanel = new System.Windows.Forms.Panel();
            this.removeCategory = new System.Windows.Forms.Button();
            this.addCategory = new System.Windows.Forms.Button();
            this.selectedCategories = new System.Windows.Forms.GroupBox();
            this.selectedList = new System.Windows.Forms.ListBox();
            this.excludeCheckbox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.availableList = new System.Windows.Forms.ListBox();
            this.tabs.SuspendLayout();
            this.testPage.SuspendLayout();
            this.testPanel.SuspendLayout();
            this.categoryPage.SuspendLayout();
            this.categoryPanel.SuspendLayout();
            this.categoryButtonPanel.SuspendLayout();
            this.selectedCategories.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabs
            // 
            this.tabs.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabs.Controls.Add(this.testPage);
            this.tabs.Controls.Add(this.categoryPage);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Location = new System.Drawing.Point(0, 0);
            this.tabs.Multiline = true;
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(248, 496);
            this.tabs.TabIndex = 0;
            // 
            // testPage
            // 
            this.testPage.Controls.Add(this.testPanel);
            this.testPage.Location = new System.Drawing.Point(25, 4);
            this.testPage.Name = "testPage";
            this.testPage.Size = new System.Drawing.Size(219, 488);
            this.testPage.TabIndex = 0;
            this.testPage.Text = "Tests";
            // 
            // testPanel
            // 
            this.testPanel.Controls.Add(this.tests);
            this.testPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testPanel.Location = new System.Drawing.Point(0, 0);
            this.testPanel.Name = "testPanel";
            this.testPanel.Size = new System.Drawing.Size(219, 488);
            this.testPanel.TabIndex = 0;
            // 
            // tests
            // 
            this.tests.AllowDrop = true;
            this.tests.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.tests.HideSelection = false;
            this.tests.Location = new System.Drawing.Point(0, 0);
            this.tests.Name = "tests";
            this.tests.Size = new System.Drawing.Size(219, 448);
            this.tests.TabIndex = 0;
            // 
            // categoryPage
            // 
            this.categoryPage.Controls.Add(this.categoryPanel);
            this.categoryPage.Location = new System.Drawing.Point(25, 4);
            this.categoryPage.Name = "categoryPage";
            this.categoryPage.Size = new System.Drawing.Size(219, 488);
            this.categoryPage.TabIndex = 1;
            this.categoryPage.Text = "Categories";
            // 
            // categoryPanel
            // 
            this.categoryPanel.Controls.Add(this.categoryButtonPanel);
            this.categoryPanel.Controls.Add(this.selectedCategories);
            this.categoryPanel.Controls.Add(this.groupBox1);
            this.categoryPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.categoryPanel.Location = new System.Drawing.Point(0, 0);
            this.categoryPanel.Name = "categoryPanel";
            this.categoryPanel.Size = new System.Drawing.Size(219, 488);
            this.categoryPanel.TabIndex = 0;
            // 
            // categoryButtonPanel
            // 
            this.categoryButtonPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.categoryButtonPanel.Controls.Add(this.removeCategory);
            this.categoryButtonPanel.Controls.Add(this.addCategory);
            this.categoryButtonPanel.Location = new System.Drawing.Point(8, 280);
            this.categoryButtonPanel.Name = "categoryButtonPanel";
            this.categoryButtonPanel.Size = new System.Drawing.Size(203, 40);
            this.categoryButtonPanel.TabIndex = 1;
            // 
            // removeCategory
            // 
            this.removeCategory.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.removeCategory.Location = new System.Drawing.Point(109, 8);
            this.removeCategory.Name = "removeCategory";
            this.removeCategory.TabIndex = 1;
            this.removeCategory.Text = "Remove";
            this.removeCategory.Click += new System.EventHandler(this.removeCategory_Click);
            // 
            // addCategory
            // 
            this.addCategory.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.addCategory.Location = new System.Drawing.Point(21, 8);
            this.addCategory.Name = "addCategory";
            this.addCategory.TabIndex = 0;
            this.addCategory.Text = "Add";
            this.addCategory.Click += new System.EventHandler(this.addCategory_Click);
            // 
            // selectedCategories
            // 
            this.selectedCategories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.selectedCategories.Controls.Add(this.selectedList);
            this.selectedCategories.Controls.Add(this.excludeCheckbox);
            this.selectedCategories.Location = new System.Drawing.Point(8, 328);
            this.selectedCategories.Name = "selectedCategories";
            this.selectedCategories.Size = new System.Drawing.Size(203, 144);
            this.selectedCategories.TabIndex = 2;
            this.selectedCategories.TabStop = false;
            this.selectedCategories.Text = "Selected Categories";
            // 
            // selectedList
            // 
            this.selectedList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.selectedList.ItemHeight = 16;
            this.selectedList.Location = new System.Drawing.Point(8, 16);
            this.selectedList.Name = "selectedList";
            this.selectedList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.selectedList.Size = new System.Drawing.Size(187, 84);
            this.selectedList.TabIndex = 0;
            this.selectedList.DoubleClick += new System.EventHandler(this.removeCategory_Click);
            // 
            // excludeCheckbox
            // 
            this.excludeCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.excludeCheckbox.Location = new System.Drawing.Point(8, 120);
            this.excludeCheckbox.Name = "excludeCheckbox";
            this.excludeCheckbox.Size = new System.Drawing.Size(179, 16);
            this.excludeCheckbox.TabIndex = 1;
            this.excludeCheckbox.Text = "Exclude these categories";
            this.excludeCheckbox.CheckedChanged += new System.EventHandler(this.excludeCheckbox_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.availableList);
            this.groupBox1.Location = new System.Drawing.Point(8, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(203, 272);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Available Categories";
            // 
            // availableList
            // 
            this.availableList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.availableList.ItemHeight = 16;
            this.availableList.Location = new System.Drawing.Point(8, 24);
            this.availableList.Name = "availableList";
            this.availableList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.availableList.Size = new System.Drawing.Size(187, 244);
            this.availableList.TabIndex = 0;
            this.availableList.DoubleClick += new System.EventHandler(this.addCategory_Click);
            // 
            // TestTree
            // 
            this.Controls.Add(this.tabs);
            this.Name = "TestTree";
            this.Size = new System.Drawing.Size(248, 496);
            this.tabs.ResumeLayout(false);
            this.testPage.ResumeLayout(false);
            this.testPanel.ResumeLayout(false);
            this.categoryPage.ResumeLayout(false);
            this.categoryPanel.ResumeLayout(false);
            this.categoryButtonPanel.ResumeLayout(false);
            this.selectedCategories.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void addCategory_Click(object sender, System.EventArgs e)
        {
            if (availableList.SelectedItems.Count > 0)
            {
                // Create a separate list to avoid exception
                // when using the list box directly.
                List<string> categories = new List<string>();
                foreach (string category in availableList.SelectedItems)
                    categories.Add(category);

                foreach (string category in categories)
                {
                    selectedList.Items.Add(category);
                    availableList.Items.Remove(category);
                }

                UpdateCategorySelection();
                if (this.SelectedCategories.Length > 0)
                    this.excludeCheckbox.Enabled = true;
            }
        }

        private void removeCategory_Click(object sender, System.EventArgs e)
        {
            if (selectedList.SelectedItems.Count > 0)
            {
                // Create a separate list to avoid exception
                // when using the list box directly.
                List<string> categories = new List<string>();
                foreach (string category in selectedList.SelectedItems)
                    categories.Add(category);

                foreach (string category in categories)
                {
                    selectedList.Items.Remove(category);
                    availableList.Items.Add(category);
                }

                UpdateCategorySelection();
                if (this.SelectedCategories.Length == 0)
                {
                    this.excludeCheckbox.Checked = false;
                    this.excludeCheckbox.Enabled = false;
                }
            }
        }

        private void excludeCheckbox_CheckedChanged(object sender, System.EventArgs e)
        {
            UpdateCategorySelection();
        }

        private void UpdateCategorySelection()
        {
            Model.SelectCategories(SelectedCategories, excludeCheckbox.Checked);

            TestNodeFilter treeFilter = TestNodeFilter.Empty;

            if (SelectedCategories.Length > 0)
            {
                treeFilter = new CategoryFilter(SelectedCategories);
                if (excludeCheckbox.Checked)
                    treeFilter = new NotFilter(treeFilter);
            }

            tests.TreeFilter = treeFilter;
        }

        #region IViewControl Implementation

        public void InitializeView(ITestModel model)
        {
            Model = model;

            model.Events.TestLoaded += (TestNodeEventArgs e) =>
            {
                availableList.Items.Clear();
                selectedList.Items.Clear();

                availableList.SuspendLayout();
                foreach (string category in model.AvailableCategories)
                    availableList.Items.Add(category);

                // We need to ensure the tree loads first and restore the
                // visual state before checking the selected categories.
                tests.LoadTests(e.Test);

                if (model.Services.UserSettings.Gui.TestTree.SaveVisualState)
                {
                    string fileName = VisualState.GetVisualStateFileName(Model.TestFiles[0]);
                    if (File.Exists(fileName) && new FileInfo(fileName).Length > 0)
                    {
                        try
                        {
                            VisualState visualState = VisualState.LoadFrom(fileName);
                            tests.RestoreVisualState(visualState);
                            model.SelectCategories(visualState.SelectedCategories, visualState.ExcludeCategories);
                        }
                        catch (Exception exception)
                        {
                            var messageDisplay = new MessageDisplay();
                            messageDisplay.Error($"There was an error loading the Visual State from {fileName}");
                        }
                    }
                }

                // Reflect any changes in the controls
                if (model.SelectedCategories != null && model.SelectedCategories.Length > 0)
                {
                    selectedList.Items.AddRange(model.SelectedCategories);
                    excludeCheckbox.Checked = model.ExcludeSelectedCategories;

                    foreach (string cat in model.SelectedCategories)
                        if (model.AvailableCategories.Contains(cat))
                        {
                            availableList.Items.Remove(cat);
                            excludeCheckbox.Enabled = true;
                        }

                    UpdateCategorySelection();
                }

                availableList.ResumeLayout();
            };

            model.Events.TestReloaded += (TestNodeEventArgs e) =>
            {
                // Remove any selected items that are no longer available
                int index = selectedList.Items.Count;
                selectedList.SuspendLayout();
                while (--index >= 0)
                {
                    string category = selectedList.Items[index].ToString();
                    if (!model.AvailableCategories.Contains(category))
                        selectedList.Items.RemoveAt(index);
                }
                selectedList.ResumeLayout();

                // Clear check box if there are no more selected items.
                if (selectedList.Items.Count == 0)
                    excludeCheckbox.Checked = excludeCheckbox.Enabled = false;

                // Put any unselected available items on availableList
                availableList.Items.Clear();
                availableList.SuspendLayout();
                foreach (string category in model.AvailableCategories)
                    if (selectedList.FindStringExact(category) < 0)
                        availableList.Items.Add(category);
                availableList.ResumeLayout();

                // Tell the tree what is selected
                UpdateCategorySelection();
            };

            model.Events.TestUnloaded += (TestEventArgs e) =>
            {
                availableList.Items.Clear();
                selectedList.Items.Clear();
                excludeCheckbox.Checked = false;
                excludeCheckbox.Enabled = false;
            };
        }

        #endregion
    }
}
