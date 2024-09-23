// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace TestCentric.Gui.Tests
{
    /// <summary>
    /// Summary description for VisualStateTests.
    /// </summary>
    [TestFixture]
    public class VisualStateTests
    {
        [Test]
        public void SaveAndRestoreVisualState()
        {
            VisualState state = new VisualState()
            {
                ShowCheckBoxes = true,
                TopNode = "ABC.Test.dll",
                SelectedNode = "NUnit.Tests.MyFixture.MyTest",
                SelectedCategories = new List<string>(new[] { "A", "B", "C" }),
                ExcludeCategories = true
            };

            StringWriter writer = new StringWriter();
            state.Save(writer);

            string output = writer.GetStringBuilder().ToString();

            StringReader reader = new StringReader(output);
            VisualState newState = VisualState.LoadFrom(reader);

            Assert.That(newState.ShowCheckBoxes, Is.EqualTo(state.ShowCheckBoxes), "ShowCheckBoxes");
            Assert.That(newState.TopNode, Is.EqualTo(state.TopNode), "TopNode");
            Assert.That(newState.SelectedNode, Is.EqualTo(state.SelectedNode), "SelectedNode");
            Assert.That(newState.SelectedCategories, Is.EqualTo(state.SelectedCategories), "SelectedCategories");
            Assert.That(newState.ExcludeCategories, Is.EqualTo(state.ExcludeCategories), "ExcludeCategories");
        }
    }
}
