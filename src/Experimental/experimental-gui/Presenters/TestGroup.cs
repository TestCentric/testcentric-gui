// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Text;
using System.Windows.Forms;
using TestCentric.Engine;

namespace TestCentric.Gui.Presenters
{
    using Model;

    /// <summary>
    /// A TestGroup is essentially a TestSelection with a
    /// name and image index for use in the tree display.
    /// Its TreeNode property is externally set and updated.
    /// It can create a filter for running all the tests
    /// in the group.
    /// </summary>
    public class TestGroup : TestSelection, ITestItem
    {
        #region Constructors

        public TestGroup(string name) : this(name, -1) { }

        public TestGroup(string name, int imageIndex)
        {
            Name = name;
            ImageIndex = imageIndex;
        }

        #endregion

        #region Properties

        public override string Name { get; }

        public int ImageIndex { get; set; }

        public TreeNode TreeNode { get; set; }

        #endregion

        public override TestFilter GetTestFilter()
        {
            StringBuilder sb = new StringBuilder("<filter><or>");

            foreach (TestNode test in this)
                if (test.RunState != RunState.Explicit)
                    sb.AppendFormat("<id>{0}</id>", test.Id);

            sb.Append("</or></filter>");

            return new TestFilter(sb.ToString());
        }
    }
}
