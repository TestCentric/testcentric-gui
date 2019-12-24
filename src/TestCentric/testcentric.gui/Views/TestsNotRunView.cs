// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Windows.Forms;

namespace TestCentric.Gui.Views
{
    public partial class TestsNotRunView : UserControlView, ITestsNotRunView
    {
        public TestsNotRunView()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            InvokeIfRequired(() =>
            {
                treeView1.Nodes.Clear();
            });
        }

        public void AddResult(string name, string reason)
        {
            TreeNode node = new TreeNode(name);
            TreeNode reasonNode = new TreeNode("Reason: " + reason);
            node.Nodes.Add(reasonNode);

            InvokeIfRequired(() =>
            {
                treeView1.Nodes.Add(node);
            });
        }
    }
}
