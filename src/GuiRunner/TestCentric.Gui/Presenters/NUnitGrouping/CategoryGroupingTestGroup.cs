// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TestCentric.Gui.Presenters.NUnitGrouping
{
    using TestCentric.Gui.Model;

    /// <summary>
    /// A specialist TestGroup class which provides the Category for the TestFilter
    /// </summary>
    public class CategoryGroupingTestGroup : GroupingTestGroup
    {
        /// <inheritdoc />
        public CategoryGroupingTestGroup(TestNode associatedTestNode, string category, string name)
            : base(associatedTestNode, name)
        {
            Category = category;
        }

        public string Category { get; set; }
    }
}
