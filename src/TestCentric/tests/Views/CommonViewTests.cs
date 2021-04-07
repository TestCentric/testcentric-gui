// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace TestCentric.Gui.Views
{
    using Elements;

    [TestFixture(typeof(TestCentricMainView))]
    //[TestFixture(typeof(TestTreeView))]
    [Platform(Exclude = "Linux", Reason = "Uninitialized form causes an error in Travis-CI")]
    public class CommonViewTests<T> where T : new()
    {
        protected T View { get; private set; }

        [SetUp]
        public void CreateView()
        {
            this.View = new T();
        }

        [TestCaseSource("GetViewElementProperties")]
        public void ViewElementsAreInitialized(PropertyInfo prop)
        {
            var element = prop.GetValue(View, new object[0]) as IViewElement;

            Assert.NotNull(element, $"Element {prop.Name} was not initialized");
        }

        static protected IEnumerable<PropertyInfo> GetViewElementProperties()
        {
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                if (typeof(IViewElement).IsAssignableFrom(prop.PropertyType))
                    yield return prop;
            }
        }
    }
}
