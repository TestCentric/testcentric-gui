namespace TestCentric.Gui.Views
{
    partial class TestTreeView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestTreeView));
            this.treePanel = new System.Windows.Forms.Panel();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.tree = new System.Windows.Forms.TreeView();
            this.treeImages = new System.Windows.Forms.ImageList(this.components);
            this.runMenuItem = new System.Windows.Forms.MenuItem();
            this.failedAssumptionsMenuItem = new System.Windows.Forms.MenuItem();
            this.showCheckBoxesMenuItem = new System.Windows.Forms.MenuItem();
            this.expandAllMenuItem = new System.Windows.Forms.MenuItem();
            this.collapseAllMenuItem = new System.Windows.Forms.MenuItem();
            this.hideTestsMenuItem = new System.Windows.Forms.MenuItem();
            this.propertiesMenuItem = new System.Windows.Forms.MenuItem();
            this.treeMenu = new System.Windows.Forms.ContextMenu();
            this.treeMenu.Name = "treeMenu";
            this.checkFailedButton = new System.Windows.Forms.Button();
            this.clearAllButton = new System.Windows.Forms.Button();
            this.treePanel.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();
			// 
            // treeImages
            // 
            this.treeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeImages.ImageStream")));
            this.treeImages.TransparentColor = System.Drawing.Color.White;
            this.treeImages.Images.SetKeyName(0, "Skipped.png");
            this.treeImages.Images.SetKeyName(1, "Failure.png");
            this.treeImages.Images.SetKeyName(2, "Success.png");
            this.treeImages.Images.SetKeyName(3, "Ignored.png");
            this.treeImages.Images.SetKeyName(4, "Inconclusive.png");
            // 
            // treePanel
            // 
            this.treePanel.Controls.Add(this.tree);
            this.treePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treePanel.Location = new System.Drawing.Point(0, 0);
            this.treePanel.Name = "treePanel";
            this.treePanel.Size = new System.Drawing.Size(219, 448);
            this.treePanel.TabIndex = 0;
            // 
            // tree
            //
            this.tree.ContextMenu = this.treeMenu;
            this.tree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tree.HideSelection = false;
            this.tree.Location = new System.Drawing.Point(0, 0);
			this.tree.ImageList = treeImages;
            this.tree.ImageIndex = 0;
            this.tree.SelectedImageIndex = 0;
            this.tree.AllowDrop = true;
            this.tree.Name = "treeView1";
            this.tree.Size = new System.Drawing.Size(150, 150);
            this.tree.TabIndex = 0;
            //
            // treeMenu
            //
            this.treeMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[]{
                this.runMenuItem,
                new System.Windows.Forms.MenuItem("-"),
                this.failedAssumptionsMenuItem,
                this.showCheckBoxesMenuItem,
                new System.Windows.Forms.MenuItem("-"),
                this.expandAllMenuItem,
                this.collapseAllMenuItem,
                this.hideTestsMenuItem,
                new System.Windows.Forms.MenuItem("-"),
                this.propertiesMenuItem});
            // 
            // buttonPanel
            // 
            this.buttonPanel.Controls.Add(this.checkFailedButton);
            this.buttonPanel.Controls.Add(this.clearAllButton);
            //this.buttonPanel.Visible = true;
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonPanel.Location = new System.Drawing.Point(0, 448);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(219, 40);
            this.buttonPanel.TabIndex = 1;
            // 
            // checkFailedButton
            // 
            this.checkFailedButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.checkFailedButton.Location = new System.Drawing.Point(117, 8);
            this.checkFailedButton.Name = "checkFailedButton";
            this.checkFailedButton.Size = new System.Drawing.Size(96, 23);
            this.checkFailedButton.TabIndex = 1;
            this.checkFailedButton.Text = "Check Failed";
            // 
            // clearAllButton
            // 
            this.clearAllButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.clearAllButton.Location = new System.Drawing.Point(13, 8);
            this.clearAllButton.Name = "clearAllButton";
            this.clearAllButton.Size = new System.Drawing.Size(96, 23);
            this.clearAllButton.TabIndex = 0;
            this.clearAllButton.Text = "Clear All";
           //
            // runMenuItem
            //
            this.runMenuItem.Name = "runMenuItem";
            this.runMenuItem.Text = "&Run";
            //
            // failedAssumptionsMenuItem
            //
            this.failedAssumptionsMenuItem.Name = "failedAssumptionsMenuItem";
            this.failedAssumptionsMenuItem.Text = "Show Failed Assumptions";
            //
            // showCheckBoxesMenuItem
            //
            this.showCheckBoxesMenuItem.Name = "showCheckBoxesMenuItem";
            this.showCheckBoxesMenuItem.Text = "Show CheckBoxes";
            // 
            // expandAllMenuItem
            // 
            //this.expandAllMenuItem.Index = 5;
            this.expandAllMenuItem.Text = "Expand All";
            // 
            // collapseAllMenuItem
            // 
            //this.collapseAllMenuItem.Index = 6;
            this.collapseAllMenuItem.Text = "Collapse All";
            // 
            // hideTestsMenuItem
            // 
            //this.hideTestsMenuItem.Index = 7;
            this.hideTestsMenuItem.Text = "Hide Tests";
            //
            // propertiesMenuItem
            //
            this.propertiesMenuItem.Name = "propertiesMenuItem";
            this.propertiesMenuItem.Text = "&Properties";
            // 
            // TestsNotRunView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.AddRange(new System.Windows.Forms.Control[]{
                this.treePanel,
                this.buttonPanel});
            this.Name = "TestTreeView";
            this.buttonPanel.ResumeLayout();
            this.treePanel.ResumeLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList treeImages;
        private System.Windows.Forms.Panel treePanel;
        private System.Windows.Forms.TreeView tree;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Button clearAllButton;
        private System.Windows.Forms.Button checkFailedButton;
        private System.Windows.Forms.ContextMenu treeMenu;
        private System.Windows.Forms.MenuItem runMenuItem;
        private System.Windows.Forms.MenuItem showCheckBoxesMenuItem;
        private System.Windows.Forms.MenuItem failedAssumptionsMenuItem;
        private System.Windows.Forms.MenuItem expandAllMenuItem;
        private System.Windows.Forms.MenuItem collapseAllMenuItem;
        private System.Windows.Forms.MenuItem hideTestsMenuItem;
        private System.Windows.Forms.MenuItem propertiesMenuItem;
    }
}
