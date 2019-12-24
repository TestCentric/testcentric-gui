// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Collections;

namespace NUnit.ProjectEditor
{
    /// <summary>
    /// A simple collection to hold VSProjectConfigs. Originally,
    /// we used the (NUnit) ProjectConfigCollection, but the
    /// classes have since diverged.
    /// </summary>
    public class VSProjectConfigCollection : CollectionBase
    {
        public VSProjectConfig this[int index]
        {
            get { return List[index] as VSProjectConfig; }
        }

        public VSProjectConfig this[string name]
        {
            get
            {
                foreach (VSProjectConfig config in InnerList)
                    if (config.Name == name) return config;

                return null;
            }
        }

        public void Add(VSProjectConfig config)
        {
            List.Add(config);
        }

        public bool Contains(string name)
        {
            foreach (VSProjectConfig config in InnerList)
                if (config.Name == name) return true;

            return false;
        }
    }
}
