namespace TestCentric.Gui.Dialogs
{
    partial class CategoryFilterDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CategoryFilterDialog));
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonApplyAndClose = new System.Windows.Forms.Button();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.checkedListBoxCategory = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // buttonApply
            // 
            this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApply.Location = new System.Drawing.Point(254, 411);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(104, 35);
            this.buttonApply.TabIndex = 5;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.OnApplyButtonClicked);
            // 
            // buttonApplyAndClose
            // 
            this.buttonApplyAndClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApplyAndClose.Location = new System.Drawing.Point(369, 411);
            this.buttonApplyAndClose.Name = "buttonApplyAndClose";
            this.buttonApplyAndClose.Size = new System.Drawing.Size(127, 35);
            this.buttonApplyAndClose.TabIndex = 6;
            this.buttonApplyAndClose.Text = "Apply + Close";
            this.buttonApplyAndClose.UseVisualStyleBackColor = true;
            this.buttonApplyAndClose.Click += new System.EventHandler(this.OnApplyAndCloseButtonClicked);
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Location = new System.Drawing.Point(12, 12);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(104, 35);
            this.buttonSelectAll.TabIndex = 1;
            this.buttonSelectAll.Text = "Select all";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.OnSelectAllButtonClicked);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(125, 12);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(104, 35);
            this.buttonClear.TabIndex = 2;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.OnClearButtonClicked);
            // 
            // searchTextBox
            // 
            this.searchTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTextBox.Location = new System.Drawing.Point(237, 16);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(258, 26);
            this.searchTextBox.TabIndex = 3;
            // 
            // checkedListBoxCategory
            // 
            this.checkedListBoxCategory.Anchor = (System.Windows.Forms.AnchorStyles)System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.checkedListBoxCategory.CheckOnClick = true;
            this.checkedListBoxCategory.FormattingEnabled = true;
            this.checkedListBoxCategory.Location = new System.Drawing.Point(12, 60);
            this.checkedListBoxCategory.Name = "checkedListBoxCategory";
            this.checkedListBoxCategory.Size = new System.Drawing.Size(483, 349);
            this.checkedListBoxCategory.TabIndex = 4;
            // 
            // CategoryFilterDialog
            // 
            this.AcceptButton = this.buttonApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 466);
            this.Controls.Add(this.checkedListBoxCategory);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonSelectAll);
            this.Controls.Add(this.buttonApplyAndClose);
            this.Controls.Add(this.buttonApply);
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "CategoryFilterDialog";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Text = "Category filter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonApplyAndClose;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.CheckedListBox checkedListBoxCategory;
    }
}

