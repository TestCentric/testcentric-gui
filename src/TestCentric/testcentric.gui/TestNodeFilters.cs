// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;

namespace TestCentric.Gui
{
    using Model;

    // NOTE: In NUnit V2, the same filter Types are used in the framework
    // and the GUI. That's possible because the GUI actually uses the same
    // test representation as the framework.
    //
    // In this back-port of the GUI to run NUnit 3 tests, we needed to
    // do something different because the GUI now only knows about tests
    // and filters through their XML representation. The `TestNode` type
    // used in the GUI has no connection to the `ITest` and `Test` types
    // that the framework uses.
    //
    // To overcome that problem, a `TestNodeFilter` is used to filter and
    // colorize the tree. We keep the same structure as that used by the
    // framework-side filters, even though a more efficient tree-descent
    // approach is possible. The performance hit is pretty much undetectable
    // and this version of the GUI is not expected to be maintained for long.

    public abstract class TestNodeFilter
    {
        public virtual bool IsEmpty { get { return true; } }

        public virtual bool Pass(TestNode testNode)
        {
            return Match(testNode) || testNode.RunState != RunState.Explicit && MatchParent(testNode) || MatchDescendant(testNode);
        }

        public abstract bool Match(TestNode testNode);

        public bool MatchParent(TestNode testNode)
        {
            var parentXmlNode = testNode.Xml.ParentNode;
            if (parentXmlNode == null || parentXmlNode.NodeType != System.Xml.XmlNodeType.Element)
                return false;

            var parentNode = new TestNode(parentXmlNode);

            return Match(parentNode) || MatchParent(parentNode);
        }

        protected virtual bool MatchDescendant(TestNode testNode)
        {
            if (testNode.Children == null)
                return false;

            foreach (var child in testNode.Children)
            {
                if (Match(child) || MatchDescendant(child))
                    return true;
            }

            return false;
        }

        public static TestNodeFilter Empty { get { return new EmptyFilter(); } }

        private class EmptyFilter : TestNodeFilter
        {
            public override bool Pass(TestNode testNode)
            {
                return Match(testNode);
            }

            public override bool Match(TestNode testNode)
            {
                return testNode.RunState != RunState.Explicit;
            }
        }
    }

    public class CategoryFilter : TestNodeFilter
    {
        IList<string> _categories;

        public override bool IsEmpty
        {
            get { return _categories.Count == 0; }
        }

        public CategoryFilter(IList<string> categories)
        {
            _categories = categories;
        }

        public override bool Match(TestNode testNode)
        {
            var testCategories = testNode.GetProperties("Category");
            if (testCategories.Length == 0)
                return false;

            foreach (string cat1 in testCategories)
                foreach (string cat2 in _categories)
                    if (cat1 == cat2) return true;

            return false;
        }
    }

    public class NotFilter : TestNodeFilter
    {
        private TestNodeFilter _baseFilter;

        public override bool IsEmpty
        {
            get { return _baseFilter.IsEmpty; }
        }

        public NotFilter(TestNodeFilter baseFilter)
        {
            _baseFilter = baseFilter;
        }

        public override bool Pass(TestNode testNode)
        {
            return Match(testNode) || MatchDescendant(testNode);
        }

        public override bool Match(TestNode testNode)
        {
            return testNode.RunState != RunState.Explicit && !_baseFilter.Pass(testNode);
        }
    }
}
