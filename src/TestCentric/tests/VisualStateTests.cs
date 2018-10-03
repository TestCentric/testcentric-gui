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

using System;
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
                SelectedCategories = new string[] { "A", "B", "C" },
                ExcludeCategories = true
			};

            StringWriter writer = new StringWriter();
            state.Save( writer );

            string output = writer.GetStringBuilder().ToString();

            StringReader reader = new StringReader( output );
            VisualState newState = VisualState.LoadFrom( reader );

            Assert.AreEqual( state.ShowCheckBoxes, newState.ShowCheckBoxes, "ShowCheckBoxes" );
            Assert.AreEqual( state.TopNode, newState.TopNode, "TopNode" );
            Assert.AreEqual( state.SelectedNode, newState.SelectedNode, "SelectedNode" );
            Assert.AreEqual( state.SelectedCategories, newState.SelectedCategories, "SelectedCategories" );
            Assert.AreEqual( state.ExcludeCategories, newState.ExcludeCategories, "ExcludeCategories" );
        }
    }
}
