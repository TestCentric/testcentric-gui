// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using TestCentric.Gui.Model;

namespace TestCentric.Gui.Presenters.NUnitGrouping
{
    /// <summary>
    /// Helper class to fold a TestNode hierarchy of namespace nodes into only single node
    /// </summary>
    public class FoldNamespaceNodesHandler
    {

        /// <summary>
        /// Check if a TestNode is a Namespace node
        /// </summary>
        public static bool IsNamespaceNode(TestNode testNode)
        {
            return testNode != null && testNode.IsSuite && (testNode.Type == "TestSuite" || testNode.Type == "SetUpFixture");
        }


        /// <summary>
        /// Returns the list of TestNodes which can be folded. Therefore the TestNode and all child TestNodes are checked, if
        ///  - they are Namespace nodes
        ///  - they contain only one single child node
        /// </summary>
        public static IList<TestNode> FoldNamespaceNodes(TestNode testNode)
        {
            if (!IsNamespaceNode(testNode))
            {
                return new List<TestNode>();
            }

            // If a namespace node only contains one child item which is also a namespace node, we can fold them.
            List<TestNode> namespaceNodes = new List<TestNode>() { testNode };
            if (testNode.Children.Count == 1 && IsNamespaceNode(testNode.Children[0]))
            {
                namespaceNodes.AddRange(FoldNamespaceNodes(testNode.Children[0]));
            }

            return namespaceNodes;
        }

        /// <summary>
        /// Gets the name of the folded TestNodes
        /// </summary>
        public static string GetFoldedNamespaceName(IList<TestNode> foldedNamespaces)
        {
            var namespaceNames = foldedNamespaces.Select(x => x.Name);
            return String.Join(".", namespaceNames);
        }
    }
}
