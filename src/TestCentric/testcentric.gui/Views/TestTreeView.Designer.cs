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
            this.treeView = new System.Windows.Forms.TreeView();
            this.testTreeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.runMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runCheckedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.debugMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugCheckedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.activeConfigMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.showCheckboxesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.expandAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseToFixturesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeImages = new System.Windows.Forms.ImageList(this.components);
            this.testTreeContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.ContextMenuStrip = this.testTreeContextMenu;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.HideSelection = false;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.treeImages;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(191, 246);
            this.treeView.TabIndex = 1;
            // 
            // testTreeContextMenu
            // 
            this.testTreeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runMenuItem,
            this.runCheckedMenuItem,
            this.toolStripSeparator1,
            this.debugMenuItem,
            this.debugCheckedMenuItem,
            this.toolStripSeparator2,
            this.activeConfigMenuItem,
            this.toolStripSeparator3,
            this.showCheckboxesMenuItem,
            this.toolStripSeparator4,
            this.expandAllMenuItem,
            this.collapseAllMenuItem,
            this.collapseToFixturesMenuItem});
            this.testTreeContextMenu.Name = "testTreeContextMenu";
            this.testTreeContextMenu.Size = new System.Drawing.Size(191, 226);
            // 
            // runMenuItem
            // 
            this.runMenuItem.Name = "runMenuItem";
            this.runMenuItem.Size = new System.Drawing.Size(190, 22);
            this.runMenuItem.Text = "Run";
            // 
            // runCheckedMenuItem
            // 
            this.runCheckedMenuItem.Name = "runCheckedMenuItem";
            this.runCheckedMenuItem.Size = new System.Drawing.Size(190, 22);
            this.runCheckedMenuItem.Text = "Run Checked Items";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(187, 6);
            // 
            // debugMenuItem
            // 
            this.debugMenuItem.Name = "debugMenuItem";
            this.debugMenuItem.Size = new System.Drawing.Size(190, 22);
            this.debugMenuItem.Text = "Debug";
            // 
            // debugCheckedMenuItem
            // 
            this.debugCheckedMenuItem.Name = "debugCheckedMenuItem";
            this.debugCheckedMenuItem.Size = new System.Drawing.Size(190, 22);
            this.debugCheckedMenuItem.Text = "Debug Checked Items";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(187, 6);
            // 
            // activeConfigMenuItem
            // 
            this.activeConfigMenuItem.Name = "activeConfigMenuItem";
            this.activeConfigMenuItem.Size = new System.Drawing.Size(190, 22);
            this.activeConfigMenuItem.Text = "Active Config";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(187, 6);
            // 
            // showCheckboxesMenuItem
            // 
            this.showCheckboxesMenuItem.CheckOnClick = true;
            this.showCheckboxesMenuItem.Name = "showCheckboxesMenuItem";
            this.showCheckboxesMenuItem.Size = new System.Drawing.Size(190, 22);
            this.showCheckboxesMenuItem.Text = "Show Checkboxes";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(187, 6);
            // 
            // expandAllMenuItem
            // 
            this.expandAllMenuItem.Name = "expandAllMenuItem";
            this.expandAllMenuItem.Size = new System.Drawing.Size(190, 22);
            this.expandAllMenuItem.Text = "Expand All";
            // 
            // collapseAllMenuItem
            // 
            this.collapseAllMenuItem.Name = "collapseAllMenuItem";
            this.collapseAllMenuItem.Size = new System.Drawing.Size(190, 22);
            this.collapseAllMenuItem.Text = "Collapse All";
            // 
            // collapseToFixturesMenuItem
            // 
            this.collapseToFixturesMenuItem.Name = "collapseToFixturesMenuItem";
            this.collapseToFixturesMenuItem.Size = new System.Drawing.Size(190, 22);
            this.collapseToFixturesMenuItem.Text = "Collapse to Fixtures";
            // 
            // treeImages
            // 
            this.treeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeImages.ImageStream")));
            this.treeImages.TransparentColor = System.Drawing.Color.Transparent;
            this.treeImages.Images.SetKeyName(0, "Skipped.png");
            this.treeImages.Images.SetKeyName(1, "Inconclusive.png");
            this.treeImages.Images.SetKeyName(2, "Success.png");
            this.treeImages.Images.SetKeyName(3, "Warning.png");
            this.treeImages.Images.SetKeyName(4, "Failure.png");
            // 
            // TestTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeView);
            this.Name = "TestTreeView";
            this.Size = new System.Drawing.Size(191, 246);
            this.testTreeContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ContextMenuStrip testTreeContextMenu;
        private System.Windows.Forms.ToolStripMenuItem runMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseToFixturesMenuItem;
        private System.Windows.Forms.ImageList treeImages;
        private System.Windows.Forms.ToolStripMenuItem showCheckboxesMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem runCheckedMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugCheckedMenuItem;
        private System.Windows.Forms.ToolStripMenuItem activeConfigMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}
