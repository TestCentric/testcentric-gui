// ***********************************************************************
// Copyright (c) 2018 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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

        ContextMenu ContextMenu { get; }

        ICommand RunCommand { get; }
        IChecked ShowFailedAssumptions { get; }
        // TODO: Can we eliminate need for having both of the following?
        IChecked ShowCheckBoxes { get; }
        //IChecked CheckboxesCommand { get; }
        ICommand ExpandAllCommand { get; }
        ICommand CollapseAllCommand { get; }
        ICommand HideTestsCommand { get; }
        ICommand PropertiesCommand { get; }

        bool CheckBoxes { get; set; }

        ICommand ClearAllCheckBoxes { get; }
        ICommand CheckFailedTests { get; }

        string AlternateImageSet { get; set; }

        ITreeView Tree { get; }
        TestSuiteTreeNode ContextNode { get; }
        TestNode[] SelectedTests { get; }

        TestNodeFilter TreeFilter { get; set; }

        void Clear();

        void ShowPropertiesDialog(TestSuiteTreeNode node);
        void ClosePropertiesDialog();
        void CheckPropertiesDialog();
    }
}
