// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

#if !NETCOREAPP2_1
using System;
using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Engine.Extensibility
{
    internal class ExtensionSelectorTests
    {
        [Test]
        public void IsDuplicateOfWithSame()
        {
            var first = MockExtension("Extension1");
            var second = MockExtension("Extension1");
            Assert.Multiple(() =>
            {
                Assert.That(first.IsDuplicateOf(second), Is.True);
                Assert.That(second.IsDuplicateOf(first), Is.True);
            });
        }

        [Test]
        public void IsDuplicateOfWithDifferent()
        {
            var first = MockExtension("Extension1");
            var second = MockExtension("Extension2");
            Assert.Multiple(() =>
            {
                Assert.That(first.IsDuplicateOf(second), Is.False);
                Assert.That(second.IsDuplicateOf(first), Is.False);
            });
        }

        [Test]
        public void IsBetterVersionOfThrowsWhenNotDuplicates()
        {
            var first = MockExtension("Extension1");
            var second = MockExtension("Extension2");
            Assert.That(() => first.IsBetterVersionOf(second), Throws.InvalidOperationException);
        }

        [Test]
        public void IsBetterVersionOfChoosesHighestAssemblyVersion()
        {
            var first = MockExtension(assemblyVersion: new Version(2, 0));
            var second = MockExtension(assemblyVersion: new Version(4, 7));
            Assert.Multiple(() =>
            {
                Assert.That(first.IsBetterVersionOf(second), Is.False);
                Assert.That(second.IsBetterVersionOf(first), Is.True);
            });
        }

        [Test]
        public void IsBetterVersionOfChoosesHighestTargetFramework()
        {
            var first = MockExtension(targetFramework: new Version(2, 0));
            var second = MockExtension(targetFramework: new Version(4, 7));
            Assert.Multiple(() =>
            {
                Assert.That(first.IsBetterVersionOf(second), Is.False);
                Assert.That(second.IsBetterVersionOf(first), Is.True);
            });
        }

        [Test]
        public void IsBetterVersionOfPrioritisesAssemblyVersionOverTargetFramework()
        {
            var first = MockExtension(assemblyVersion: new Version(2, 0), targetFramework: new Version(2, 0));
            var second = MockExtension(assemblyVersion: new Version(1, 0), targetFramework: new Version(4, 7));
            Assert.Multiple(() =>
            {
                Assert.That(first.IsBetterVersionOf(second), Is.True);
                Assert.That(second.IsBetterVersionOf(first), Is.False);
            });
        }

        [Test]
        public void IsBetterVersionOfPrefersDirectlySpecifiedToWildcard()
        {
            var first = MockExtension(fromWildcard: false);
            var second = MockExtension(fromWildcard: true);
            Assert.Multiple(() =>
            {
                Assert.That(first.IsBetterVersionOf(second), Is.True);
                Assert.That(second.IsBetterVersionOf(first), Is.False);
            });
        }

        [Test]
        public void IsBetterVersionOfPrefersNoChangeIfFromWildcard()
        {
            var first = MockExtension(fromWildcard: true);
            var second = MockExtension(fromWildcard: true);
            Assert.Multiple(() =>
            {
                Assert.That(first.IsBetterVersionOf(second), Is.False);
                Assert.That(second.IsBetterVersionOf(first), Is.False);
            });
        }

        private static IExtensionAssembly MockExtension(string assemblyName = "ExtensionSelectorTestsExtension",
            Version assemblyVersion = null,
            Version targetFramework = null,
            bool fromWildcard = false)
        {
            var sub = Substitute.For<IExtensionAssembly>();
            sub.AssemblyName.Returns(assemblyName);
            sub.AssemblyVersion.Returns(assemblyVersion ?? new Version(1, 0));
            targetFramework = targetFramework ?? new Version(2, 0);
            sub.TargetFramework.Returns(new RuntimeFramework(Runtime.Net, targetFramework));
            sub.FromWildCard.Returns(fromWildcard);
            return sub;
        }
    }
}
#endif
