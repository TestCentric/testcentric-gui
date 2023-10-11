// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Framework;

namespace TestCentric.Engine.Internal
{
    public class TestFilterExtensionTests
    {
        static TestPackage ASSEMBLY1_PACKAGE = new TestPackage("assembly1.dll");
        static TestPackage ASSEMBLY2_PACKAGE = new TestPackage("assembly2.dll");
        static TestPackage PROJECT_PACKAGE = new TestPackage("MyProject.nunit");

        static string ASSEMBLY1_ID = ASSEMBLY1_PACKAGE.ID;
        static string ASSEMBLY2_ID = ASSEMBLY2_PACKAGE.ID;

        [Test]
        public void PackageIdsAreDistinct()
        {
            Assert.That(ASSEMBLY1_ID != ASSEMBLY2_ID, $"Error in test setup: Assembly1 and Assembly2 packages have the same ID: {ASSEMBLY1_ID}");
        }

        [Test]
        public void EmptyFilterIsNeverExcluded()
        {
            var filter = new TestFilter("<filter/>");
            Assert.Multiple(() =>
            {
                Assert.False(filter.Excludes(ASSEMBLY1_PACKAGE), "Assembly1 should be included");
                Assert.False(filter.Excludes(ASSEMBLY2_PACKAGE), "Assembly2 should be included");
                Assert.False(filter.Excludes(PROJECT_PACKAGE), "MyProject.nunit should be included");
            });
        }

        [Test]
        public void SingleIdFilter_SameAssembly_IsIncluded()
        {
            TestFilter filter = new TestFilter($"<filter><id>{ASSEMBLY1_ID}-123</id></filter>");
            Assert.False(filter.Excludes(ASSEMBLY1_PACKAGE));
        }

        [Test]
        public void SingleIdFilter_DifferentAssembly_IsExcluded()
        {
            TestFilter filter = new TestFilter($"<filter><id>{ASSEMBLY1_ID}-123</id></filter>");
            Assert.That(filter.Excludes(ASSEMBLY2_PACKAGE));
        }

        [Test]
        public void MultipleIdFilters_SameAssembly_IsIncluded()
        {
            TestFilter filter = new TestFilter($"<filter><or><id>{ASSEMBLY1_ID}-123</id><id>{ASSEMBLY1_ID}-456</id><id>{ASSEMBLY1_ID}-789</id></or></filter>");
            Assert.False(filter.Excludes(ASSEMBLY1_PACKAGE));
        }

        [Test]
        public void MultipleIdFilters_DifferentAssembly_IsExcluded()
        {
            TestFilter filter = new TestFilter($"<filter><or><id>{ASSEMBLY1_ID}-123</id><id>{ASSEMBLY1_ID}-456</id><id>{ASSEMBLY1_ID}-789</id></or></filter>");
            Assert.That(filter.Excludes(ASSEMBLY2_PACKAGE));
        }

        [Test]
        public void MultipleIdFilters_MixedAssemblies_AreIncluded()
        {
            TestFilter filter = new TestFilter($"<filter><or><id>{ASSEMBLY1_ID}-123</id><id>{ASSEMBLY2_ID}-456</id><id>{ASSEMBLY1_ID}-789</id></or></filter>");

            Assert.Multiple(() =>
            {
                Assert.False(filter.Excludes(ASSEMBLY1_PACKAGE), "Assembly1 should be included");
                Assert.False(filter.Excludes(ASSEMBLY2_PACKAGE), "Assembly2 should be included");
            });
        }

        [Test]
        public void CategoryFilter_IsIncluded()
        {
            var filter = new TestFilter("<filter><cat>SomeCategory</cat></filter>");
            Assert.False(filter.Excludes(ASSEMBLY1_PACKAGE));
        }

        [Test]
        public void MixedFilterTypes_AreIncluded()
        {
            var filter = new TestFilter($"<filter><or><id>{ASSEMBLY1_ID}-123</id><cat>SomeCategory</cat></or></filter>");

            Assert.Multiple(() =>
            {
                Assert.False(filter.Excludes(ASSEMBLY1_PACKAGE), "Assembly1 should be included");
                Assert.False(filter.Excludes(ASSEMBLY2_PACKAGE), "Assembly2 should be included");
            });

        }
    }
}
