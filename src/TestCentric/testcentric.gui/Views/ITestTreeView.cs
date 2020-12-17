// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Windows.Forms;

namespace TestCentric.Gui.Views
{
    using Elements;
    using Model; // To be removed

    public delegate void FileDropEventHandler(IList<string> fileNames);

    public interface ITestTreeView
    {
        event FileDropEventHandler FileDrop;

        ContextMenuStrip ContextMenuStrip { get; }

        ICommand RunCommand { get; }
        IChecked ShowFailedAssumptions { get; }
        IToolStripMenu ProjectMenu { get; }
        IToolStripMenu ActiveConfiguration { get; }
        ICommand EditProject { get; }
        ICommand PropertiesCommand { get; }
        IChecked ShowCheckBoxes { get; }
        ICommand ExpandAllCommand { get; }
        ICommand CollapseAllCommand { get; }
        ICommand HideTestsCommand { get; }

        // TODO: Can we eliminate need for having this in addition to ShowCheckBoxes?
        bool CheckBoxes { get; set; }

        ICommand ClearAllCheckBoxes { get; }
        ICommand CheckFailedTests { get; }

        string AlternateImageSet { get; set; }

        ITreeView Tree { get; }
        TestSuiteTreeNode ContextNode { get; }
        TestNode[] SelectedTests { get; }

        void Clear();

        void Accept(TestSuiteTreeNodeVisitor visitor);

        void ShowPropertiesDialog(TestSuiteTreeNode node);
        void ClosePropertiesDialog();
        void CheckPropertiesDialog();
    }
}
