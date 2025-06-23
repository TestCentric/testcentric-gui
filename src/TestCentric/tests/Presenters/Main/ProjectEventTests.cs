// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using TestCentric.Gui.Elements;
using TestCentric.Gui.Model;

namespace TestCentric.Gui.Presenters.Main
{
    public class ProjectEventTests : MainPresenterTestBase
    {
        [Test]
        public void WhenProjectIsCreated_TitleBarIsSet()
        {
            var project = new TestCentricProject(_model, "dummy.dll");
            _model.TestCentricProject.Returns(project);

            FireProjectLoadedEvent();

            _view.Received().Title = "TestCentric - UNNAMED.tcproj";
        }

        [Test]
        public void WhenProjectIsClosed_TitleBarIsSet()
        {
            FireProjectUnloadedEvent();

            _view.Received().Title = "TestCentric Runner for NUnit"; 
        }


        [Test]
        public void WhenProjectIsSaved_TitleBarIsSet()
        {
            // Arrange
            var project = new TestCentricProject(_model, "dummy.dll");
            _model.TestCentricProject.Returns(project);
            _view.DialogManager.GetFileSavePath(null, null, null, null).ReturnsForAnyArgs("TestCentric.tcproj");

            // Act
            project.SaveAs("TestCentric.tcproj");
            _view.SaveProjectCommand.Execute += Raise.Event<CommandHandler>();

            // Assert
            _model.Received().SaveProject("TestCentric.tcproj");
            _view.Received().Title = "TestCentric - TestCentric.tcproj";
        }

        [Test]
        public void WhenTestAssemblyChanged_ReloadOnChangeEnabled_ReloadTests()
        {
            // Arrange
            _settings.Engine.ReloadOnChange = true;

            // Act
            FireTestAssemblyChangedEvent();

            // Assert
            _model.Received().ReloadTests();
        }

        [Test]
        public void WhenTestAssemblyChanged_ReloadOnChangeDisabled_NotReloadTests()
        {
            // Arrange
            _settings.Engine.ReloadOnChange = false;

            // Act
            FireTestAssemblyChangedEvent();

            // Assert
            _model.DidNotReceive().ReloadTests();
        }
    }
}
