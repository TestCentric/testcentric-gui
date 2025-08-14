// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TestCentric.Gui.Model
{
    /// <summary>
    /// TestSelection is a collection of TestNodes, implementing
    /// the ITestItem interface so that the entire selection
    /// may be selected for execution.
    /// </summary>
    public class TestSelection : IEnumerable<TestNode>, ITestItem
    {
        private HashSet<TestNode> _nodes;

        public TestSelection() 
        { 
            _nodes = new HashSet<TestNode>();
        }

        public TestSelection(IEnumerable<TestNode> tests) 
        { 
            _nodes = new HashSet<TestNode>(tests);
        }

        public virtual string Name { get { return GetType().Name; } }

        public virtual TestFilter GetTestFilter()
        {
            return _nodes.Count == 1
                ? this.First().GetTestFilter() // This allows nodes with special handling for filters to apply it.
                : TestFilter.MakeIdFilter(this);
        }

        // <summary>
        /// Remove the test node with a specific id from the selection
        /// </summary>
        /// <param name="id"></param>
        public void RemoveId(string id)
        {
            TestNode testNode = this.FirstOrDefault(x => x.Id == id);
            if (testNode != null)
                _nodes.Remove(testNode);
        }

        /// <summary>
        /// Add a ITestItem to the TestSelection (either a TestNode or a TestSelection)
        /// </summary>
        public void Add(ITestItem item)
        {
            if (item == null) 
                throw new ArgumentNullException("item");

            if (item is TestNode testNode)
                _nodes.Add(testNode);
            else if (item is TestSelection testSelection)
            {
                foreach (var subItem in testSelection)
                    _nodes.Add(subItem);
            }
        }

        /// <summary>
        /// Clear all nodes from collection
        /// </summary>
        public void Clear()
        {
            _nodes.Clear();
        }

        public IEnumerator<TestNode> GetEnumerator()
        {
            return _nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
