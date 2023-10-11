// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.IO;

namespace TestCentric.Engine.Internal
{
    public delegate bool TestPackageSelectorDelegate(TestPackage p);

    /// <summary>
    /// Extension methods for use with TestPackages
    /// </summary>
    public static class TestPackageExtensions
    {
        public static bool IsAssemblyPackage(this TestPackage package)
        {
            if (package.FullName == null)
                return false;

            var ext = Path.GetExtension(package.FullName).ToLowerInvariant();
            return ext == ".dll" || ext== ".exe";
        }

        public static bool HasSubPackages(this TestPackage package)
        {
            Guard.OperationValid(package.SubPackages != null, "Package has null subpackage");
            return package.SubPackages.Count > 0;
        }

        public static IList<TestPackage> Select(this TestPackage package, TestPackageSelectorDelegate selector)
        {
            var selection = new List<TestPackage>();

            AccumulatePackages(package, selection, selector);

            return selection;
        }

        private static void AccumulatePackages(TestPackage package, IList<TestPackage> selection, TestPackageSelectorDelegate selector)
        {
            if (selector(package))
                selection.Add(package);

            foreach (var subPackage in package.SubPackages)
                AccumulatePackages(subPackage, selection, selector);
        }

        public static string ToXml(this TestPackage package)
        {
            var stringWriter = new StringWriter();
            //var settings = new XmlWriterSettings { OmitXmlDeclaration = true };
            //var xmlWriter = XmlWriter.Create(stringWriter, settings);

            new TestPackageSerializer().Serialize(stringWriter, package);

            //xmlWriter.Flush();
            //xmlWriter.Close();

            return stringWriter.ToString();
        }
    }
}
