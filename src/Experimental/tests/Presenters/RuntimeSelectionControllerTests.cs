// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NSubstitute;
using NUnit.Engine;
using NUnit.Framework;
using TestCentric.Common;
using TestCentric.Gui.Elements;
using TestCentric.Gui.Model;
using TestCentric.TestUtilities.Fakes;

namespace TestCentric.Gui.Presenters
{
    public class RuntimeSelectionControllerTests
    {
        private IToolStripMenu _menu;
        private ITestModel _model;
        private TestPackage _package;
        private RuntimeSelectionController _controller;

        [SetUp]
        public void Initialize()
        {
            _menu = new ToolStripMenuElement("RunTimes");
            _model = Substitute.For<ITestModel>();
            _model.AvailableRuntimes.Returns(AvailableRuntimes);
            _package = new TestPackage("dummy.dll");
            _controller = new RuntimeSelectionController(_menu, _model);
        }

        [Test]
        public void RuntimeSelection_WhenTestPackageIsNull_NotAllowed()
        {
            Assert.False(_controller.AllowRuntimeSelection());
        }

        [Test]
        public void RuntimeSelection_WithoutImageTargetFrameworName_NotAllowed()
        {
            _model.TestPackage.Returns(_package);
            Assert.False(_controller.AllowRuntimeSelection());
        }

        [Test]
        public void PopulateMenu_WhenTestPackageIsNull_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => _controller.PopulateMenu());
        }

        [Test]
        public void PopulateMenu_WithoutImageTargetFrameworkName_ThrowsInvalidOperationException()
        {
            _model.TestPackage.Returns(_package);

            Assert.Throws<InvalidOperationException>(() => _controller.PopulateMenu());
        }

        [TestCase(".NETFramework,Version=4.0", "DEFAULT", "net-2.0", "net-3.5", "net-4.0", "net-4.5", "net-4.6")]
        [TestCase(".NETCoreApp,Version=2.1", "DEFAULT", "netcore-1.1", "netcore-2.0", "netcore-2.1", "netcore-3.0", "netcore-3.1")]
        public void PopulateMenu__ListsCorrectRuntimes(string imageTarget, params string[] expectedTags)
        {
            _package.AddSetting(EnginePackageSettings.ImageTargetFrameworkName, imageTarget);
            _model.TestPackage.Returns(_package);

            _controller.PopulateMenu();

            Assert.That(Tags, Is.EqualTo(expectedTags));

            // NOTE: This requires the test cases to be set up so the image target is at index 3
            bool[] shouldBeEnabled = new[] { true, false, false, true, true, true };

            for (int index = 0; index < _menu.MenuItems.Count; index++)
            {
                var item = _menu.MenuItems[index] as ToolStripMenuItem;
                Assert.That(item.Enabled, Is.EqualTo(shouldBeEnabled[index]), $"Incorrect value for item.Enabled at index {index}");
                Assert.That(item.Checked, Is.EqualTo(index == 0), $"Incorrect value for item.Checked at index {index}");
            }
        }

        private List<string> Tags
        {
            get
            {
                var result = new List<string>();

                foreach (ToolStripMenuItem item in _menu.MenuItems)
                    result.Add((string)item.Tag);

                return result;
            }
        }

        private static readonly IRuntimeFramework[] AvailableRuntimes = new IRuntimeFramework[]
        {
            new RuntimeFramework("net-2.0", new Version(2,0)),
            new RuntimeFramework("net-3.5", new Version(3,5)),
            new RuntimeFramework("net-4.0", new Version(4,0)),
            new RuntimeFramework("net-4.5", new Version(4,5)),
            new RuntimeFramework("net-4.6", new Version(4,6)),
            new RuntimeFramework("netcore-1.1", new Version(1,1)),
            new RuntimeFramework("netcore-2.0", new Version(2,0)),
            new RuntimeFramework("netcore-2.1", new Version(2,1)),
            new RuntimeFramework("netcore-3.0", new Version(3,0)),
            new RuntimeFramework("netcore-3.1", new Version(3,1)),
        };
    }
}
