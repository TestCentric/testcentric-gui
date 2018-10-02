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
using System.Collections;
using System.Reflection;
using NUnit.Framework;

namespace TestCentric.Gui.Tests
{
    using Views;

    [TestFixture, Ignore("Same internal fields no longer exist to test this. Need to rewrite.")]
    public class TestTreeTests
    {
        [Test]
        public void SameCategoryShouldNotBeSelectedMoreThanOnce()
        {
            // arrange
            TestTree target = new TestTree();

            // we need to populate the available categories
            // this can be done via TestLoader but this way the test is isolated
            FieldInfo fieldInfo = typeof (TestTree).GetField("availableCategories", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(fieldInfo, "The field 'availableCategories' should be found.");
            object fieldValue = fieldInfo.GetValue(target);
            Assert.IsNotNull(fieldValue, "The value of 'availableCategories' should not be null.");
            IList availableCategories = fieldValue as IList;
            Assert.IsNotNull(availableCategories, "'availableCategories' field should be of type IList.");

            string[] expectedSelectedCategories = new string[] { "Foo", "MockCategory" };
            foreach (string availableCategory in expectedSelectedCategories)
            {
                availableCategories.Add(availableCategory);
            }

            // act
            //target.SelectCategories(expectedSelectedCategories, true);
            //target.SelectCategories(expectedSelectedCategories, true);
            string[] actualSelectedCategories = target.SelectedCategories;

            // assert
            CollectionAssert.AreEquivalent(expectedSelectedCategories, actualSelectedCategories);
        }
    }
}
