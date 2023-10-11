// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Xml;

namespace TestCentric.Engine.Internal
{
    public static class TestFilterExtensions
    {
        public static bool Excludes(this TestFilter filter, TestPackage package)
        {
            var doc = new XmlDocument();
            doc.LoadXml(filter.Text);
            var filterNode = doc.FirstChild;

            return FilterExcludesPackageID(filterNode, package.ID);
        }

        private static bool FilterExcludesPackageID(XmlNode filterNode, string packageID)
        {
            var prefix = $"{packageID}-";

            switch (filterNode.Name)
            {
                case "filter":
                    return filterNode.ChildNodes.Count == 1
                        ? FilterExcludesPackageID(filterNode.FirstChild, packageID)
                        : false;

                case "id":
                    return !filterNode.InnerText.StartsWith(prefix);

                case "or":
                    foreach (XmlNode child in filterNode.ChildNodes)
                        if (!FilterExcludesPackageID(child, packageID))
                            return false;
                    return true;

                default:
                    return false;
            }
        }
    }
}
