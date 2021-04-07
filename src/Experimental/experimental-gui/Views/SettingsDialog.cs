// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TestCentric.Gui.Model.Settings;

namespace TestCentric.Gui.Views
{
    using SettingsPages;
 
    public partial class SettingsDialog : Form, IDialog
    {
        private readonly UserSettings _settings;
        private readonly List<SettingsPage> _pageList = new List<SettingsPage>();
        private readonly Form _owner;

        private SettingsPage _currentPage;

        public SettingsDialog(Form owner, UserSettings settings)
        {
            InitializeComponent();

            //owner.Site.Container.Add(dialog);
            this.Font = owner.Font;

            _settings = settings;

            _pageList.AddRange(new SettingsPage[] {
                new GuiSettingsPage(_settings),
                new TreeSettingsPage(_settings),
                new TextOutputSettingsPage(_settings),
                new AssemblyReloadSettingsPage(_settings)
            });
            _owner = owner;
        }

        public void ApplySettings()
        {
            foreach (SettingsPage page in _pageList)
                if (page.SettingsLoaded)
                    page.ApplySettings();
        }

        void IDialog.ShowDialog()
        {
            ShowDialog(_owner);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            foreach (SettingsPage page in _pageList)
                AddBranchToTree(treeView1.Nodes, page.Key);

            if (treeView1.VisibleCount >= treeView1.GetNodeCount(true))
                treeView1.ExpandAll();

            SelectInitialPage();

            treeView1.Select();
        }

        private void SelectInitialPage()
        {
            string initialPage = _settings.Gui.InitialSettingsPage as string;

            if (initialPage != null)
                SelectPage(initialPage);
            else if (treeView1.Nodes.Count > 0)
                SelectFirstPage(treeView1.Nodes);
        }

        private void SelectPage(string initialPage)
        {
            TreeNode node = FindNode(treeView1.Nodes, initialPage);
            if (node != null)
                treeView1.SelectedNode = node;
            else
                SelectFirstPage(treeView1.Nodes);
        }

        private TreeNode FindNode(TreeNodeCollection nodes, string key)
        {
            int dot = key.IndexOf('.');
            string tail = null;

            if (dot >= 0)
            {
                tail = key.Substring(dot + 1);
                key = key.Substring(0, dot);
            }

            foreach (TreeNode node in nodes)
                if (node.Text == key)
                    return tail == null
                        ? node
                        : FindNode(node.Nodes, tail);

            return null;
        }

        private void SelectFirstPage(TreeNodeCollection nodes)
        {
            if (nodes[0].Nodes.Count == 0)
                treeView1.SelectedNode = nodes[0];
            else
            {
                nodes[0].Expand();
                SelectFirstPage(nodes[0].Nodes);
            }
        }

        private void AddBranchToTree(TreeNodeCollection nodes, string key)
        {
            int dot = key.IndexOf('.');
            if (dot < 0)
            {
                nodes.Add(new TreeNode(key, 2, 2));
                return;
            }

            string name = key.Substring(0, dot);
            key = key.Substring(dot + 1);

            TreeNode node = FindOrAddNode(nodes, name);

            if (key != null)
                AddBranchToTree(node.Nodes, key);
        }

        private TreeNode FindOrAddNode(TreeNodeCollection nodes, string name)
        {
            foreach (TreeNode node in nodes)
                if (node.Text == name)
                    return node;

            TreeNode newNode = new TreeNode(name, 0, 0);
            nodes.Add(newNode);
            return newNode;
        }

        private SettingsPage FindPage(string key)
        {
            foreach (SettingsPage page in _pageList)
                if (page.Key == key)
                    return page;

            return null;
        }

        private void treeView1_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            string key = e.Node.FullPath.Replace('\\', '.');
            SettingsPage page = FindPage(key);
            _settings.Gui.InitialSettingsPage = key;

            if (page != null && page != _currentPage)
            {
                panel1.Controls.Clear();
                panel1.Controls.Add(page);
                page.Dock = DockStyle.Fill;
                _currentPage = page;
                return;
            }
        }

        private void treeView1_AfterExpand(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            e.Node.ImageIndex = e.Node.SelectedImageIndex = 1;
        }

        private void treeView1_AfterCollapse(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            e.Node.ImageIndex = e.Node.SelectedImageIndex = 0;
        }

        private void okButton_Click(object sender, System.EventArgs e)
        {
            //if (Services.TestLoader.IsTestLoaded && this.HasChangesRequiringReload)
            //{
            //    DialogResult answer = MessageDisplay.Ask(
            //        "Some changes will only take effect when you reload the test project. Do you want to reload now?");

            //    if (answer == DialogResult.Yes)
            //        this.reloadProjectOnClose = true;
            //}

            ApplySettings();

            DialogResult = System.Windows.Forms.DialogResult.OK;

            Close();
        }
    }
}
