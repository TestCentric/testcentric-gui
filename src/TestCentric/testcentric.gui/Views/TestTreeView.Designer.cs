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
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.runButton = new System.Windows.Forms.ToolStripSplitButton();
            this.runAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runSelectedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.testParametersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.stopRunMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugButton = new System.Windows.Forms.ToolStripSplitButton();
            this.debugAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugSelectedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.formatButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.nunitTreeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fixtureListMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testListMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.byAssemblyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byFixtureMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byCategoryMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byExtendedCategoryMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byOutcomeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byDurationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runSummaryButton = new System.Windows.Forms.ToolStripButton();
            this.testTreeContextMenu.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.ContextMenuStrip = this.testTreeContextMenu;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.HideSelection = false;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.treeImages;
            this.treeView.Location = new System.Drawing.Point(0, 25);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(191, 221);
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
            // toolStrip
            // 
            this.toolStrip.CanOverflow = false;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runButton,
            this.debugButton,
            this.formatButton,
            this.runSummaryButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(191, 25);
            this.toolStrip.Stretch = true;
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // runButton
            // 
            this.runButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.runButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runAllMenuItem,
            this.runSelectedMenuItem,
            this.toolStripSeparator5,
            this.testParametersMenuItem,
            this.toolStripMenuItem9,
            this.stopRunMenuItem});
            this.runButton.Image = ((System.Drawing.Image)(resources.GetObject("runButton.Image")));
            this.runButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(32, 22);
            this.runButton.ToolTipText = "Run All Tests";
            // 
            // runAllMenuItem
            // 
            this.runAllMenuItem.Name = "runAllMenuItem";
            this.runAllMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.runAllMenuItem.Size = new System.Drawing.Size(180, 22);
            this.runAllMenuItem.Text = "Run All";
            this.runAllMenuItem.ToolTipText = "Run all tests displayed";
            // 
            // runSelectedMenuItem
            // 
            this.runSelectedMenuItem.Name = "runSelectedMenuItem";
            this.runSelectedMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.runSelectedMenuItem.Size = new System.Drawing.Size(180, 22);
            this.runSelectedMenuItem.Text = "Run Selected";
            this.runSelectedMenuItem.ToolTipText = "Run the selected tests";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(177, 6);
            // 
            // testParametersMenuItem
            // 
            this.testParametersMenuItem.Name = "testParametersMenuItem";
            this.testParametersMenuItem.Size = new System.Drawing.Size(180, 22);
            this.testParametersMenuItem.Text = "Test Parameters...";
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(177, 6);
            // 
            // stopRunMenuItem
            // 
            this.stopRunMenuItem.Name = "stopRunMenuItem";
            this.stopRunMenuItem.Size = new System.Drawing.Size(180, 22);
            this.stopRunMenuItem.Text = "Stop Run";
            // 
            // debugButton
            // 
            this.debugButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.debugButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.debugAllMenuItem,
            this.debugSelectedMenuItem});
            this.debugButton.Image = ((System.Drawing.Image)(resources.GetObject("debugButton.Image")));
            this.debugButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.debugButton.Name = "debugButton";
            this.debugButton.Size = new System.Drawing.Size(32, 22);
            this.debugButton.ToolTipText = "Debug All Tests";
            // 
            // debugAllMenuItem
            // 
            this.debugAllMenuItem.Name = "debugAllMenuItem";
            this.debugAllMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.debugAllMenuItem.Size = new System.Drawing.Size(202, 22);
            this.debugAllMenuItem.Text = "Debug All";
            this.debugAllMenuItem.ToolTipText = "Debug all tests displayed";
            // 
            // debugSelectedMenuItem
            // 
            this.debugSelectedMenuItem.Name = "debugSelectedMenuItem";
            this.debugSelectedMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F6)));
            this.debugSelectedMenuItem.Size = new System.Drawing.Size(202, 22);
            this.debugSelectedMenuItem.Text = "Debug Selected";
            this.debugSelectedMenuItem.ToolTipText = "Debug the selected tests";
            // 
            // formatButton
            // 
            this.formatButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.formatButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nunitTreeMenuItem,
            this.fixtureListMenuItem,
            this.testListMenuItem,
            this.toolStripMenuItem1,
            this.byAssemblyMenuItem,
            this.byFixtureMenuItem,
            this.byCategoryMenuItem,
            this.byExtendedCategoryMenuItem,
            this.byOutcomeMenuItem,
            this.byDurationMenuItem});
            this.formatButton.Image = ((System.Drawing.Image)(resources.GetObject("formatButton.Image")));
            this.formatButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.formatButton.Name = "formatButton";
            this.formatButton.Size = new System.Drawing.Size(29, 22);
            this.formatButton.Text = "Display";
            // 
            // nunitTreeMenuItem
            // 
            this.nunitTreeMenuItem.Name = "nunitTreeMenuItem";
            this.nunitTreeMenuItem.Size = new System.Drawing.Size(198, 22);
            this.nunitTreeMenuItem.Tag = "NUNIT_TREE";
            this.nunitTreeMenuItem.Text = "NUnit Tree";
            // 
            // fixtureListMenuItem
            // 
            this.fixtureListMenuItem.Name = "fixtureListMenuItem";
            this.fixtureListMenuItem.Size = new System.Drawing.Size(198, 22);
            this.fixtureListMenuItem.Tag = "FIXTURE_LIST";
            this.fixtureListMenuItem.Text = "Fixture List";
            // 
            // testListMenuItem
            // 
            this.testListMenuItem.Name = "testListMenuItem";
            this.testListMenuItem.Size = new System.Drawing.Size(198, 22);
            this.testListMenuItem.Tag = "TEST_LIST";
            this.testListMenuItem.Text = "Test List";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(195, 6);
            // 
            // byAssemblyMenuItem
            // 
            this.byAssemblyMenuItem.Name = "byAssemblyMenuItem";
            this.byAssemblyMenuItem.Size = new System.Drawing.Size(198, 22);
            this.byAssemblyMenuItem.Tag = "ASSEMBLY";
            this.byAssemblyMenuItem.Text = "By Assembly";
            // 
            // byFixtureMenuItem
            // 
            this.byFixtureMenuItem.Name = "byFixtureMenuItem";
            this.byFixtureMenuItem.Size = new System.Drawing.Size(198, 22);
            this.byFixtureMenuItem.Tag = "FIXTURE";
            this.byFixtureMenuItem.Text = "By Fixture";
            // 
            // byCategoryMenuItem
            // 
            this.byCategoryMenuItem.Name = "byCategoryMenuItem";
            this.byCategoryMenuItem.Size = new System.Drawing.Size(198, 22);
            this.byCategoryMenuItem.Tag = "CATEGORY";
            this.byCategoryMenuItem.Text = "By Category";
            // 
            // byExtendedCategoryMenuItem
            // 
            this.byExtendedCategoryMenuItem.Name = "byExtendedCategoryMenuItem";
            this.byExtendedCategoryMenuItem.Size = new System.Drawing.Size(198, 22);
            this.byExtendedCategoryMenuItem.Tag = "CATEGORY_EXTENDED";
            this.byExtendedCategoryMenuItem.Text = "By Category (Extended)";
            // 
            // byOutcomeMenuItem
            // 
            this.byOutcomeMenuItem.Name = "byOutcomeMenuItem";
            this.byOutcomeMenuItem.Size = new System.Drawing.Size(198, 22);
            this.byOutcomeMenuItem.Tag = "OUTCOME";
            this.byOutcomeMenuItem.Text = "By Outcome";
            // 
            // byDurationMenuItem
            // 
            this.byDurationMenuItem.Name = "byDurationMenuItem";
            this.byDurationMenuItem.Size = new System.Drawing.Size(198, 22);
            this.byDurationMenuItem.Tag = "DURATION";
            this.byDurationMenuItem.Text = "By Duration";
            // 
            // runSummaryButton
            // 
            this.runSummaryButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.runSummaryButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.runSummaryButton.Image = ((System.Drawing.Image)(resources.GetObject("runSummaryButton.Image")));
            this.runSummaryButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.runSummaryButton.Name = "runSummaryButton";
            this.runSummaryButton.Size = new System.Drawing.Size(23, 22);
            this.runSummaryButton.Text = "Summary Report";
            this.runSummaryButton.ToolTipText = "Display summary report for last test run";
            // 
            // TestTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.toolStrip);
            this.Name = "TestTreeView";
            this.Size = new System.Drawing.Size(191, 246);
            this.testTreeContextMenu.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripSplitButton runButton;
        private System.Windows.Forms.ToolStripMenuItem runAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runSelectedMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem stopRunMenuItem;
        private System.Windows.Forms.ContextMenuStrip testTreeContextMenu;
        private System.Windows.Forms.ToolStripMenuItem runMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseToFixturesMenuItem;
        private System.Windows.Forms.ImageList treeImages;
        private System.Windows.Forms.ToolStripDropDownButton formatButton;
        private System.Windows.Forms.ToolStripMenuItem nunitTreeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testListMenuItem;
        private System.Windows.Forms.ToolStripMenuItem byFixtureMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem byAssemblyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem byCategoryMenuItem;
        private System.Windows.Forms.ToolStripMenuItem byOutcomeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem byDurationMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fixtureListMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showCheckboxesMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem runCheckedMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugCheckedMenuItem;
        private System.Windows.Forms.ToolStripSplitButton debugButton;
        private System.Windows.Forms.ToolStripMenuItem debugAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugSelectedMenuItem;
        private System.Windows.Forms.ToolStripMenuItem byExtendedCategoryMenuItem;
        private System.Windows.Forms.ToolStripMenuItem activeConfigMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem testParametersMenuItem;
        private System.Windows.Forms.ToolStripButton runSummaryButton;
    }
}
