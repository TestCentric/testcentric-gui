// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using NUnit.Engine;

namespace TestCentric.Gui.Model
{
    public delegate string GroupingFunction(TestNode testNode);

    /// <summary>
    /// TestSelection is an List of TestNodes, implementing
    /// the ITestItem interface so that the entire selection
    /// may be selected for execution.
    /// </summary>
    public class TestSelection : List<TestNode>, ITestItem
    {
        public TestSelection() : base() { }

        public TestSelection(IEnumerable<TestNode> tests) : base(tests) { }

        public virtual string Name { get { return GetType().Name; } }

        public virtual TestFilter GetTestFilter()
        {
            return Count == 1
                ? this[0].GetTestFilter() // This allows nodes with special handling for filters to apply it.
                : TestFilter.MakeIdFilter(this);
        }

        // <summary>
        /// Remove the test node with a specific id from the selection
        /// </summary>
        /// <param name="id"></param>
        public void RemoveId(string id)
        {
            for (int index = 0; index < Count; index++)
                if (this[index].Id == id)
                {
                    RemoveAt(index);
                    break;
                }
        }

        // TODO: Not used now but wait till all experimental classes are ported
        //public IDictionary<string, TestSelection> GroupBy(GroupingFunction groupingFunction)
        //{
        //    var groups = new Dictionary<string, TestSelection>();

        //    foreach (TestNode testNode in this)
        //    {
        //        var groupName = groupingFunction(testNode);

        //        TestSelection group = null;
        //        if (!groups.ContainsKey(groupName))
        //        {
        //            group = new TestSelection();
        //            groups[groupName] = group;
        //        }
        //        else
        //        {
        //            group = groups[groupName];
        //        }

        //        group.Add(testNode);
        //    }

        //    return groups;
        //}
    }
}
