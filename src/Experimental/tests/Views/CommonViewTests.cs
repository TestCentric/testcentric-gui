// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace TestCentric.Gui.Views
{
    using Elements;

    [TestFixture(typeof(MainForm))]
    [TestFixture(typeof(TestTreeView))]
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
            if (prop.GetValue(View, new object[0]) == null)
                Assert.Fail("{0} was not initialized", prop.Name);
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
