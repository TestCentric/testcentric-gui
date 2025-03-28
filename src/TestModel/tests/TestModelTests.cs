// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NSubstitute;
using NUnit.Engine;
using NUnit.Framework;
using TestCentric.Gui.Model.Fakes;

namespace TestCentric.Gui.Model
{
    [TestFixture]
    public class TestModelTests
    {
        [Test]
        public void CreateNewEmptyProject_IsDirty_IsFalse()
        {
            // Arrange
            var engine = new MockTestEngine();
            var options = new CommandLineOptions("dummy.dll");
            var model = TestModel.CreateTestModel(engine, options);

            // Act
            model.CreateNewProject();

            // Assert
            Assert.That(model.TestCentricProject.IsDirty, Is.False);
        }

        [Test]
        public void AddTests_IsDirty_IsTrue()
        {
            // Arrange
            var engine = new MockTestEngine();
            var options = new CommandLineOptions("dummy.dll");
            var model = TestModel.CreateTestModel(engine, options);

            // Act
            model.CreateNewProject();
            model.AddTests(new[] { "Dummy.dll" });

            // Assert
            Assert.That(model.TestCentricProject.IsDirty, Is.True);
        }

        [Test]
        public void AddTests_TestCentricProjectLoadedEvent_IsTriggered()
        {
            // Arrange
            var engine = new MockTestEngine();
            var options = new CommandLineOptions("dummy.dll");
            var model = TestModel.CreateTestModel(engine, options);

            bool projectLoadedCalled = false;
            model.Events.TestCentricProjectLoaded += (t) => projectLoadedCalled = true;

            // Act
            model.CreateNewProject();
            model.AddTests(new[] { "Dummy.dll" });

            // Assert
            Assert.That(projectLoadedCalled, Is.True);
        }

        [Test]
        public void RemoveTestPackage_TestCentricProjectLoadedEvent_IsTriggered()
        {
            // Arrange
            var engine = Substitute.For<TestCentric.Engine.ITestEngine>();
            var options = new CommandLineOptions("dummy.dll");
            var model = TestModel.CreateTestModel(engine, options);

            bool projectLoadedCalled = false;
            model.Events.TestCentricProjectLoaded += (t) => projectLoadedCalled = true;

            // Act
            model.CreateNewProject(new[] { "Dummy.dll", "Dummy2.dll" });
            var subPackage = model.TestCentricProject.SubPackages[1];
            model.RemoveTestPackage(subPackage);

            // Assert
            Assert.That(projectLoadedCalled, Is.True);
        }
      
        [Test]
        public void SaveProject_RecentFiles_ContainsProjectName()
        {
            // Arrange
            var engine = new MockTestEngine();
            var options = new CommandLineOptions("dummy.dll");
            var model = TestModel.CreateTestModel(engine, options);

            // Act
            model.CreateNewProject();
            model.SaveProject("TestCentric.tcproj");

            // Assert
            Assert.That(model.RecentFiles.Latest, Is.EqualTo("TestCentric.tcproj"));
        }
    }
}
