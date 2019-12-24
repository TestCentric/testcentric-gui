// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Reflection;

namespace NUnit.ProjectEditor.Tests.Views
{
    public class PropertyViewTests
    {
        [Test]
        public void AllViewElementsAreInitialized()
        {
            PropertyView view = new PropertyView();

            foreach (PropertyInfo prop in typeof(PropertyView).GetProperties())
            {
                if (typeof(IViewElement).IsAssignableFrom(prop.PropertyType))
                {
                    if (prop.GetValue(view, new object[0]) == null)
                        Assert.Fail("{0} was not initialized", prop.Name);
                }
            }
        }
    }
}
