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
            this.tree = new System.Windows.Forms.TreeView();
            this.treeMenu = new System.Windows.Forms.ContextMenu();
            this.runMenuItem = new System.Windows.Forms.MenuItem();
            this.propertiesMenuItem = new System.Windows.Forms.MenuItem();
            this.failedAssumptionsMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.showCheckBoxesMenuItem = new System.Windows.Forms.MenuItem();
            this.expandAllMenuItem = new System.Windows.Forms.MenuItem();
            this.collapseAllMenuItem = new System.Windows.Forms.MenuItem();
            this.hideTestsMenuItem = new System.Windows.Forms.MenuItem();
            this.treeImages = new System.Windows.Forms.ImageList(this.components);
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.checkFailedButton = new System.Windows.Forms.Button();
            this.clearAllButton = new System.Windows.Forms.Button();
            this.projectMenuItem = new System.Windows.Forms.MenuItem();
            this.activeConfigurationMenuItem = new System.Windows.Forms.MenuItem();
            this.editProjectMenuItem = new System.Windows.Forms.MenuItem();
            this.treePanel.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // treePanel
            // 
            this.treePanel.Controls.Add(this.tree);
            this.treePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treePanel.Location = new System.Drawing.Point(0, 0);
            this.treePanel.Name = "treePanel";
            this.treePanel.Size = new System.Drawing.Size(227, 70);
            this.treePanel.TabIndex = 0;
            // 
            // tree
            // 
            this.tree.AllowDrop = true;
            this.tree.ContextMenu = this.treeMenu;
            this.tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree.HideSelection = false;
            this.tree.ImageIndex = 0;
            this.tree.ImageList = this.treeImages;
            this.tree.Location = new System.Drawing.Point(0, 0);
            this.tree.Name = "tree";
            this.tree.SelectedImageIndex = 0;
            this.tree.Size = new System.Drawing.Size(227, 70);
            this.tree.TabIndex = 0;
            // 
            // treeMenu
            // 
            this.treeMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.runMenuItem,
            this.propertiesMenuItem,
            this.failedAssumptionsMenuItem,
            this.projectMenuItem,
            this.menuItem1,
            this.showCheckBoxesMenuItem,
            this.expandAllMenuItem,
            this.collapseAllMenuItem,
            this.hideTestsMenuItem});
            // 
            // runMenuItem
            // 
            this.runMenuItem.Index = 0;
            this.runMenuItem.Text = "&Run";
            // 
            // propertiesMenuItem
            // 
            this.propertiesMenuItem.Index = 1;
            this.propertiesMenuItem.Text = "&Properties";
            // 
            // failedAssumptionsMenuItem
            // 
            this.failedAssumptionsMenuItem.Index = 2;
            this.failedAssumptionsMenuItem.Text = "Show Failed Assumptions";
            // 
            // projectMenuItem
            // 
            this.projectMenuItem.Index = 3;
            this.projectMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.activeConfigurationMenuItem,
            this.editProjectMenuItem});
            this.projectMenuItem.Text = "Project";
            // 
            // activeConfigurationMenuItem
            // 
            this.activeConfigurationMenuItem.Index = 0;
            this.activeConfigurationMenuItem.Text = "Configuration";
            // 
            // editProjectMenuItem
            // 
            this.editProjectMenuItem.Index = 1;
            this.editProjectMenuItem.Text = "Edit...";
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 4;
            this.menuItem1.Text = "-";
            // 
            // showCheckBoxesMenuItem
            // 
            this.showCheckBoxesMenuItem.Index = 5;
            this.showCheckBoxesMenuItem.Text = "Show CheckBoxes";
            // 
            // expandAllMenuItem
            // 
            this.expandAllMenuItem.Index = 6;
            this.expandAllMenuItem.Text = "Expand All";
            // 
            // collapseAllMenuItem
            // 
            this.collapseAllMenuItem.Index = 7;
            this.collapseAllMenuItem.Text = "Collapse All";
            // 
            // hideTestsMenuItem
            // 
            this.hideTestsMenuItem.Index = 8;
            this.hideTestsMenuItem.Text = "Hide Tests";
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
            // buttonPanel
            // 
            this.buttonPanel.Controls.Add(this.checkFailedButton);
            this.buttonPanel.Controls.Add(this.clearAllButton);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonPanel.Location = new System.Drawing.Point(0, 70);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(227, 40);
            this.buttonPanel.TabIndex = 1;
            // 
            // checkFailedButton
            // 
            this.checkFailedButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.checkFailedButton.Location = new System.Drawing.Point(130, 8);
            this.checkFailedButton.Name = "checkFailedButton";
            this.checkFailedButton.Size = new System.Drawing.Size(96, 23);
            this.checkFailedButton.TabIndex = 1;
            this.checkFailedButton.Text = "Check Failed";
            // 
            // clearAllButton
            // 
            this.clearAllButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.clearAllButton.Location = new System.Drawing.Point(26, 8);
            this.clearAllButton.Name = "clearAllButton";
            this.clearAllButton.Size = new System.Drawing.Size(96, 23);
            this.clearAllButton.TabIndex = 0;
            this.clearAllButton.Text = "Clear All";
            // 
            // TestTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treePanel);
            this.Controls.Add(this.buttonPanel);
            this.Name = "TestTreeView";
            this.Size = new System.Drawing.Size(227, 110);
            this.treePanel.ResumeLayout(false);
            this.buttonPanel.ResumeLayout(false);
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
        private System.Windows.Forms.MenuItem failedAssumptionsMenuItem;
        private System.Windows.Forms.MenuItem propertiesMenuItem;
        private System.Windows.Forms.MenuItem showCheckBoxesMenuItem;
        private System.Windows.Forms.MenuItem expandAllMenuItem;
        private System.Windows.Forms.MenuItem collapseAllMenuItem;
        private System.Windows.Forms.MenuItem hideTestsMenuItem;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem projectMenuItem;
        private System.Windows.Forms.MenuItem activeConfigurationMenuItem;
        private System.Windows.Forms.MenuItem editProjectMenuItem;
    }
}
