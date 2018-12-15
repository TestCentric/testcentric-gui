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

using System.Windows.Forms;

namespace TestCentric.Gui.Views
{
    using Elements;

    /// <summary>
    /// CategoryView displays and allows selecting the available categories.
    /// </summary>
    public class CategoryView : UserControl, ICategoryView
    {
        #region Instance Variables

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

        #region Construction and Initialization

        public CategoryView()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            AvailableList = new ListBoxElement(availableList);
            SelectedList = new ListBoxElement(selectedList);
            ExcludeCategories = new CheckBoxElement(excludeCheckbox);
            AddButton = new ButtonElement(addCategory);
            RemoveButton = new ButtonElement(removeCategory);

            this.excludeCheckbox.Enabled = false;
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
            this.categoryButtonPanel = new System.Windows.Forms.Panel();
            this.removeCategory = new System.Windows.Forms.Button();
            this.addCategory = new System.Windows.Forms.Button();
            this.selectedCategories = new System.Windows.Forms.GroupBox();
            this.selectedList = new System.Windows.Forms.ListBox();
            this.excludeCheckbox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.availableList = new System.Windows.Forms.ListBox();
            this.categoryButtonPanel.SuspendLayout();
            this.selectedCategories.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
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
            // 
            // addCategory
            // 
            this.addCategory.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.addCategory.Location = new System.Drawing.Point(21, 8);
            this.addCategory.Name = "addCategory";
            this.addCategory.TabIndex = 0;
            this.addCategory.Text = "Add";
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
            // 
            // CategoryView
            // 
            this.Controls.Add(this.categoryButtonPanel);
            this.Controls.Add(this.selectedCategories);
            this.Controls.Add(this.groupBox1);
            this.Name = "CategoryView";
            this.Size = new System.Drawing.Size(248, 496);
            this.categoryButtonPanel.ResumeLayout(false);
            this.selectedCategories.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region ICategoryView Implementation

        public IListBox AvailableList { get; }
        public IListBox SelectedList { get; }
        public IChecked ExcludeCategories { get; }
        public ICommand AddButton { get; }
        public ICommand RemoveButton { get; }

        public void Clear()
        {
            availableList.Items.Clear();
            selectedList.Items.Clear();
            excludeCheckbox.Enabled = excludeCheckbox.Checked = false;
        }

        #endregion
    }
}
