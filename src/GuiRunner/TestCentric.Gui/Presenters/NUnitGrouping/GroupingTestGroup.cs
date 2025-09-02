// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Presenters.NUnitGrouping
{
    using TestCentric.Gui.Model;

    /// <summary>
    /// A specialist TestGroup class which provides the associated TestNode within the NUnit tree
    /// </summary>
    public class GroupingTestGroup : TestGroup
    {
        /// <inheritdoc />
        public GroupingTestGroup(TestNode associatedTestNode, string name) : base(name)
        {
            AssociatedTestNode = associatedTestNode;
        }

        /// <inheritdoc />
        public GroupingTestGroup(TestNode associatedTestNode, string name, int imageIndex) : base(name, imageIndex)
        {
            AssociatedTestNode = associatedTestNode;
        }

        public TestNode AssociatedTestNode { get; set; }
    }
}
