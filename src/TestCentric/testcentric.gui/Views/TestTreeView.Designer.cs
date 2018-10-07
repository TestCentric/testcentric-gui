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
            this.tree = new System.Windows.Forms.TreeView();
            this.treeImages = new System.Windows.Forms.ImageList(this.components);
            this.runMenuItem = new System.Windows.Forms.MenuItem();
            this.failedAssumptionsMenuItem = new System.Windows.Forms.MenuItem();
            this.showCheckBoxesMenuItem = new System.Windows.Forms.MenuItem();
            this.propertiesMenuItem = new System.Windows.Forms.MenuItem();
            this.treeMenu = new System.Windows.Forms.ContextMenu();
            this.treeMenu.Name = "treeMenu";
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
                this.propertiesMenuItem});
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
            // propertiesMenuItem
            //
            this.propertiesMenuItem.Name = "propertiesMenuItem";
            this.propertiesMenuItem.Text = "&Properties";
            // 
            // TestsNotRunView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tree);
            this.Name = "TestTreeView";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList treeImages;
        private System.Windows.Forms.TreeView tree;
        private System.Windows.Forms.ContextMenu treeMenu;
        private System.Windows.Forms.MenuItem runMenuItem;
        private System.Windows.Forms.MenuItem showCheckBoxesMenuItem;
        private System.Windows.Forms.MenuItem failedAssumptionsMenuItem;
        private System.Windows.Forms.MenuItem propertiesMenuItem;
    }
}
