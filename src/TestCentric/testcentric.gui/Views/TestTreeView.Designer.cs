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
            this.treeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.runMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.failedAssumptionsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.activeConfigurationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.showCheckBoxesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideTestsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeImages = new System.Windows.Forms.ImageList(this.components);
            this.treePanel.SuspendLayout();
            this.treeMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // treePanel
            // 
            this.treePanel.Controls.Add(this.tree);
            this.treePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treePanel.Location = new System.Drawing.Point(0, 0);
            this.treePanel.Name = "treePanel";
            this.treePanel.Size = new System.Drawing.Size(227, 110);
            this.treePanel.TabIndex = 0;
            // 
            // tree
            // 
            this.tree.AllowDrop = true;
            this.tree.ContextMenuStrip = this.treeMenu;
            this.tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree.HideSelection = false;
            this.tree.ImageIndex = 0;
            this.tree.ImageList = this.treeImages;
            this.tree.Location = new System.Drawing.Point(0, 0);
            this.tree.Name = "tree";
            this.tree.SelectedImageIndex = 0;
            this.tree.Size = new System.Drawing.Size(227, 110);
            this.tree.TabIndex = 0;
            // 
            // treeMenu
            // 
            this.treeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runMenuItem,
            this.propertiesMenuItem,
            this.failedAssumptionsMenuItem,
            this.projectMenuItem,
            this.toolStripSeparator1,
            this.showCheckBoxesMenuItem,
            this.expandAllMenuItem,
            this.collapseAllMenuItem,
            this.hideTestsMenuItem});
            this.treeMenu.Name = "treeMenu";
            this.treeMenu.Size = new System.Drawing.Size(210, 186);
            // 
            // runMenuItem
            // 
            this.runMenuItem.Name = "runMenuItem";
            this.runMenuItem.Size = new System.Drawing.Size(209, 22);
            this.runMenuItem.Text = "&Run";
            // 
            // propertiesMenuItem
            // 
            this.propertiesMenuItem.Name = "propertiesMenuItem";
            this.propertiesMenuItem.Size = new System.Drawing.Size(209, 22);
            this.propertiesMenuItem.Text = "&Properties";
            // 
            // failedAssumptionsMenuItem
            // 
            this.failedAssumptionsMenuItem.Name = "failedAssumptionsMenuItem";
            this.failedAssumptionsMenuItem.Size = new System.Drawing.Size(209, 22);
            this.failedAssumptionsMenuItem.Text = "Show Failed Assumptions";
            // 
            // projectMenuItem
            // 
            this.projectMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.activeConfigurationMenuItem,
            this.editProjectMenuItem});
            this.projectMenuItem.Name = "projectMenuItem";
            this.projectMenuItem.Size = new System.Drawing.Size(209, 22);
            this.projectMenuItem.Text = "Project";
            // 
            // activeConfigurationMenuItem
            // 
            this.activeConfigurationMenuItem.Name = "activeConfigurationMenuItem";
            this.activeConfigurationMenuItem.Size = new System.Drawing.Size(148, 22);
            this.activeConfigurationMenuItem.Text = "Configuration";
            // 
            // editProjectMenuItem
            // 
            this.editProjectMenuItem.Name = "editProjectMenuItem";
            this.editProjectMenuItem.Size = new System.Drawing.Size(148, 22);
            this.editProjectMenuItem.Text = "Edit...";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(206, 6);
            // 
            // showCheckBoxesMenuItem
            // 
            this.showCheckBoxesMenuItem.CheckOnClick = true;
            this.showCheckBoxesMenuItem.Name = "showCheckBoxesMenuItem";
            this.showCheckBoxesMenuItem.Size = new System.Drawing.Size(209, 22);
            this.showCheckBoxesMenuItem.Text = "Show CheckBoxes";
            // 
            // expandAllMenuItem
            // 
            this.expandAllMenuItem.Name = "expandAllMenuItem";
            this.expandAllMenuItem.Size = new System.Drawing.Size(209, 22);
            this.expandAllMenuItem.Text = "Expand All";
            // 
            // collapseAllMenuItem
            // 
            this.collapseAllMenuItem.Name = "collapseAllMenuItem";
            this.collapseAllMenuItem.Size = new System.Drawing.Size(209, 22);
            this.collapseAllMenuItem.Text = "Collapse All";
            // 
            // hideTestsMenuItem
            // 
            this.hideTestsMenuItem.Name = "hideTestsMenuItem";
            this.hideTestsMenuItem.Size = new System.Drawing.Size(209, 22);
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
            // TestTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treePanel);
            this.Name = "TestTreeView";
            this.Size = new System.Drawing.Size(227, 110);
            this.treePanel.ResumeLayout(false);
            this.treeMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList treeImages;
        private System.Windows.Forms.Panel treePanel;
        private System.Windows.Forms.TreeView tree;
        private System.Windows.Forms.ContextMenuStrip treeMenu;
        private System.Windows.Forms.ToolStripMenuItem runMenuItem;
        private System.Windows.Forms.ToolStripMenuItem failedAssumptionsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showCheckBoxesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideTestsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem projectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem activeConfigurationMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editProjectMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}
