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
            this.filterToolStrip = new System.Windows.Forms.ToolStrip();
            this.filterOutcomeLabel = new System.Windows.Forms.ToolStripLabel();
            this.filterOutcomePassedButton = new System.Windows.Forms.ToolStripButton();
            this.filterOutcomeFailedButton = new System.Windows.Forms.ToolStripButton();
            this.filterOutcomeWarningButton = new System.Windows.Forms.ToolStripButton();
            this.filterOutcomeNotRunButton = new System.Windows.Forms.ToolStripButton();
            this.testTreeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.runMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.testPropertiesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewAsXmlMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.activeConfigMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.showCheckboxesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showTestDurationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            // filterOutcomeLabel
            // 
            this.filterOutcomeLabel.Name = "filterOutcomeLabel";
            this.filterOutcomeLabel.Size = new System.Drawing.Size(54, 29);
            this.filterOutcomeLabel.Text = "Filter:";
            // 
            // filterOutcomePassedButton
            // 
            this.filterOutcomePassedButton.Name = "filterOutcomePassedButton";
            this.filterOutcomePassedButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.filterOutcomePassedButton.Size = new System.Drawing.Size(70, 29);
            this.filterOutcomePassedButton.Text = "Passed";
            this.filterOutcomePassedButton.Tag = "Passed";
            this.filterOutcomePassedButton.ToolTipText = "Show all passed tests";
            this.filterOutcomePassedButton.CheckOnClick = true;
            // 
            // filterOutcomeFailedButton
            // 
            this.filterOutcomeFailedButton.Name = "filterOutcomeFailedButton";
            this.filterOutcomeFailedButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.filterOutcomeFailedButton.Size = new System.Drawing.Size(70, 29);
            this.filterOutcomeFailedButton.Text = "Failed";
            this.filterOutcomeFailedButton.Tag = "Failed";
            this.filterOutcomeFailedButton.ToolTipText = "Show all failed tests";
            this.filterOutcomeFailedButton.CheckOnClick = true;
            // 
            // filterOutcomeWarningButton
            // 
            this.filterOutcomeWarningButton.Name = "filterOutcomeWarningButton";
            this.filterOutcomeWarningButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.filterOutcomeWarningButton.Size = new System.Drawing.Size(70, 29);
            this.filterOutcomeWarningButton.Text = "Warning";
            this.filterOutcomeWarningButton.Tag = "Warning";
            this.filterOutcomeWarningButton.ToolTipText = "Show all inconclusive, skipped or ignored tests";
            this.filterOutcomeWarningButton.CheckOnClick = true;
            // 
            // filterOutcomeNotRunButton
            // 
            this.filterOutcomeNotRunButton.Name = "filterOutcomeNotRunButton";
            this.filterOutcomeNotRunButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.filterOutcomeNotRunButton.Size = new System.Drawing.Size(70, 29);
            this.filterOutcomeNotRunButton.Text = "Not Run";
            this.filterOutcomeNotRunButton.Tag = "Not Run";
            this.filterOutcomeNotRunButton.ToolTipText = "Show all tests not run yet";
            this.filterOutcomeNotRunButton.CheckOnClick = true;
            // 
            // filterToolStrip
            // 
            this.filterToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filterOutcomeLabel,
            this.filterOutcomePassedButton,
            this.filterOutcomeFailedButton,
            this.filterOutcomeWarningButton,
            this.filterOutcomeNotRunButton,
            });
            this.filterToolStrip.Location = new System.Drawing.Point(0, 0);
            this.filterToolStrip.Name = "filterToolStrip";
            this.filterToolStrip.Visible = false;
            this.filterToolStrip.Size = new System.Drawing.Size(744, 24);
            this.filterToolStrip.TabIndex = 0;
            // 
            // testTreeContextMenu
            // 
            this.testTreeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runMenuItem,
            this.debugMenuItem,
            this.contextMenuSeparator1,
            this.testPropertiesMenuItem,
            this.viewAsXmlMenuItem,
            this.activeConfigMenuItem,
            this.contextMenuSeparator2,
            this.showCheckboxesMenuItem,
            this.showTestDurationMenuItem,
            this.expandAllMenuItem,
            this.collapseAllMenuItem,
            this.collapseToFixturesMenuItem});
            this.testTreeContextMenu.Name = "testTreeContextMenu";
            this.testTreeContextMenu.Size = new System.Drawing.Size(181, 236);
            // 
            // runMenuItem
            // 
            this.runMenuItem.Name = "runMenuItem";
            this.runMenuItem.Size = new System.Drawing.Size(190, 22);
            this.runMenuItem.Text = "Run";
            // 
            // debugMenuItem
            // 
            this.debugMenuItem.Name = "debugMenuItem";
            this.debugMenuItem.Size = new System.Drawing.Size(190, 22);
            this.debugMenuItem.Text = "Debug";
            // 
            // contextMenuSeparator1
            // 
            this.contextMenuSeparator1.Name = "contextMenuSeparator1";
            this.contextMenuSeparator1.Size = new System.Drawing.Size(187, 6);
            // 
            // testPropertiesMenuItem
            // 
            this.testPropertiesMenuItem.Name = "testPropertiesMenuItem";
            this.testPropertiesMenuItem.Size = new System.Drawing.Size(190, 22);
            this.testPropertiesMenuItem.Text = "Properties...";
            // 
            // viewAsXmlMenuItem
            // 
            this.viewAsXmlMenuItem.Name = "viewAsXmlMenuItem";
            this.viewAsXmlMenuItem.Size = new System.Drawing.Size(190, 22);
            this.viewAsXmlMenuItem.Text = "View as XML...";
            // 
            // activeConfigMenuItem
            // 
            this.activeConfigMenuItem.Name = "activeConfigMenuItem";
            this.activeConfigMenuItem.Size = new System.Drawing.Size(190, 22);
            this.activeConfigMenuItem.Text = "Active Config";
            // 
            // contextMenuSeparator2
            // 
            this.contextMenuSeparator2.Name = "contextMenuSeparator2";
            this.contextMenuSeparator2.Size = new System.Drawing.Size(187, 6);
            // 
            // showCheckboxesMenuItem
            // 
            this.showCheckboxesMenuItem.CheckOnClick = true;
            this.showCheckboxesMenuItem.Name = "showCheckboxesMenuItem";
            this.showCheckboxesMenuItem.Size = new System.Drawing.Size(190, 22);
            this.showCheckboxesMenuItem.Text = "Show Checkboxes";
            // 
            // showTestDurationMenuItem
            // 
            this.showTestDurationMenuItem.CheckOnClick = true;
            this.showTestDurationMenuItem.Name = "showTestDurationMenuItem";
            this.showTestDurationMenuItem.Size = new System.Drawing.Size(190, 22);
            this.showTestDurationMenuItem.Text = "Show Test Duration";
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
            this.collapseToFixturesMenuItem.Text = "Display Fixtures";
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
            this.Controls.Add(this.filterToolStrip);
            this.Name = "TestTreeView";
            this.Size = new System.Drawing.Size(191, 246);
            this.testTreeContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ContextMenuStrip testTreeContextMenu;
        private System.Windows.Forms.ToolStrip filterToolStrip;
        private System.Windows.Forms.ToolStripLabel filterOutcomeLabel;
        private System.Windows.Forms.ToolStripButton filterOutcomePassedButton;
        private System.Windows.Forms.ToolStripButton filterOutcomeFailedButton;
        private System.Windows.Forms.ToolStripButton filterOutcomeWarningButton;
        private System.Windows.Forms.ToolStripButton filterOutcomeNotRunButton;
        private System.Windows.Forms.ToolStripMenuItem runMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseToFixturesMenuItem;
        private System.Windows.Forms.ImageList treeImages;
        private System.Windows.Forms.ToolStripMenuItem showCheckboxesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showTestDurationMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugMenuItem;
        private System.Windows.Forms.ToolStripMenuItem activeConfigMenuItem;
        private System.Windows.Forms.ToolStripSeparator contextMenuSeparator2;
        private System.Windows.Forms.ToolStripMenuItem testPropertiesMenuItem;
        private System.Windows.Forms.ToolStripSeparator contextMenuSeparator1;
        private System.Windows.Forms.ToolStripMenuItem viewAsXmlMenuItem;
    }
}
