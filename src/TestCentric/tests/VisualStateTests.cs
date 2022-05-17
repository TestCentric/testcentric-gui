// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NUnit.Framework;

namespace TestCentric.Gui
{
    /// <summary>
    /// Summary description for VisualStateTests.
    /// </summary>
    [TestFixture]
    public class VisualStateTests
    {
        private VisualState _savedState;
        private VisualState _restoredState;

        [OneTimeSetUp]
        public void SaveAndRestoreVisualState()
        {
            _savedState = new VisualState()
            {
                ShowCheckBoxes = true,
                TopNode = "ABC.Test.dll",
                SelectedNode = "NUnit.Tests.MyFixture.MyTest",
                SelectedCategories = new List<string>(new[] { "A", "B", "C" }),
                ExcludeCategories = true,
                Nodes = new List<VisualTreeNode> {
                    new VisualTreeNode() { Id = "1", Expanded = true, Nodes = new VisualTreeNode[] {
                        new VisualTreeNode() { Id = "1-1" },
                        new VisualTreeNode() { Id = "1-2" },
                        new VisualTreeNode() { Id = "1-3" }
                    } },
                    new VisualTreeNode() { Id = "2", Expanded = true, Checked = true },
                    new VisualTreeNode() { Id = "3", Expanded = false },
                    new VisualTreeNode() { Id = "4", Expanded = false, Checked = true} }
            };

            StringWriter writer = new StringWriter();
            _savedState.Save(writer);

            string output = writer.GetStringBuilder().ToString();

            StringReader reader = new StringReader(output);
            _restoredState = VisualState.LoadFrom(reader);
        }

        [Test]
        public void ShowCheckBoxesIsRestored()
        {
            Assert.AreEqual(_savedState.ShowCheckBoxes, _restoredState.ShowCheckBoxes);
        }

        [Test]
        public void TopNodeIsRestored()
        {
            Assert.AreEqual(_savedState.TopNode, _restoredState.TopNode);
        }

        [Test]
        public void SelectedNodeIsRestored()
        {
            Assert.AreEqual(_savedState.SelectedNode, _restoredState.SelectedNode);
        }

        [Test]
        public void SelectedCategoriesAreRestored()
        {
            Assert.AreEqual(_savedState.SelectedCategories, _restoredState.SelectedCategories);
        }

        [Test]
        public void ExcludedCategoriesAreRestored()
        {
            Assert.AreEqual(_savedState.ExcludeCategories, _restoredState.ExcludeCategories);
        }

        [Test]
        public void NodeListIsRestored()
        {
            Assert.That(_restoredState.Nodes, Is.EqualTo(_savedState.Nodes).Using(new NodeComparer()));
        }

        private class NodeComparer : IEqualityComparer<VisualTreeNode>
        {
            public bool Equals(VisualTreeNode x, VisualTreeNode y)
            {
                if (x.Id != y.Id || x.Expanded != y.Expanded || x.Checked != y.Checked || x.Nodes.Length != y.Nodes.Length)
                    return false;

                for (int i = 0; i < x.Nodes.Length; i++)
                    if (!Equals(x.Nodes[i], y.Nodes[i])) return false;

                return true;
            }

            public int GetHashCode(VisualTreeNode obj)
            {
                return obj.Id.GetHashCode();
            }
        }
    }
}
