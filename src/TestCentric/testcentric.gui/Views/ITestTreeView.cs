// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;

namespace TestCentric.Gui.Views
{
    using Elements;

    // Interface used for testing
    public interface ITestTreeView : IView
    {
        ICommand RunContextCommand { get; }
        ICommand RunCheckedCommand { get; }
        ICommand DebugContextCommand { get; }
        ICommand DebugCheckedCommand { get; }
        IToolStripMenu ActiveConfiguration { get; }
        IChecked ShowCheckBoxes { get; }
        ICommand ExpandAllCommand { get; }
        ICommand CollapseAllCommand { get; }
        ICommand CollapseToFixturesCommand { get; }
        ICommand TestPropertiesCommand { get; }

        string AlternateImageSet { get; set; }

        void ExpandAll();
        void CollapseAll();

        ITreeView Tree { get; }
        TreeNode ContextNode { get; }
    }
}
