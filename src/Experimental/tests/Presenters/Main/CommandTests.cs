// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using NSubstitute;

namespace TestCentric.Gui.Presenters.Main
{
    using Views;
    using Elements;
    using Model;

    public class CommandTests : MainPresenterTestBase
    {
        private static string[] NO_FILES_SELECTED = new string[0];

        [Test]
        public void NewProjectCommand_CallsNewProject()
        {
            View.NewProjectCommand.Execute += Raise.Event<CommandHandler>();
            // This is NYI, change when we implement it
            Model.DidNotReceive().NewProject();
        }

        [TestCase(false, false, "Assemblies (*.dll,*.exe)|*.dll;*.exe|All Files (*.*)|*.*")]
        [TestCase(true, false, "Projects & Assemblies (*.nunit,*.dll,*.exe)|*.nunit;*.dll;*.exe|NUnit Projects (*.nunit)|*.nunit|Assemblies (*.dll,*.exe)|*.dll;*.exe|All Files (*.*)|*.*")]
        [TestCase(false, true, "Projects & Assemblies (*.csproj,*.fsproj,*.vbproj,*.vjsproj,*.vcproj,*.sln,*.dll,*.exe)|*.csproj;*.fsproj;*.vbproj;*.vjsproj;*.vcproj;*.sln;*.dll;*.exe|Visual Studio Projects (*.csproj,*.fsproj,*.vbproj,*.vjsproj,*.vcproj,*.sln)|*.csproj;*.fsproj;*.vbproj;*.vjsproj;*.vcproj;*.sln|Assemblies (*.dll,*.exe)|*.dll;*.exe|All Files (*.*)|*.*")]
        [TestCase(true, true, "Projects & Assemblies (*.nunit,*.csproj,*.fsproj,*.vbproj,*.vjsproj,*.vcproj,*.sln,*.dll,*.exe)|*.nunit;*.csproj;*.fsproj;*.vbproj;*.vjsproj;*.vcproj;*.sln;*.dll;*.exe|NUnit Projects (*.nunit)|*.nunit|Visual Studio Projects (*.csproj,*.fsproj,*.vbproj,*.vjsproj,*.vcproj,*.sln)|*.csproj;*.fsproj;*.vbproj;*.vjsproj;*.vcproj;*.sln|Assemblies (*.dll,*.exe)|*.dll;*.exe|All Files (*.*)|*.*")]
        public void OpenProjectCommand_DisplaysDialogCorrectly(bool nunitSupport, bool vsSupport, string filter)
        {
            // Return no files so model is not called
            View.DialogManager.SelectMultipleFiles(null, null).ReturnsForAnyArgs(NO_FILES_SELECTED);
            Model.NUnitProjectSupport.Returns(nunitSupport);
            Model.VisualStudioSupport.Returns(vsSupport);

            View.OpenProjectCommand.Execute += Raise.Event<CommandHandler>();

            View.DialogManager.Received().SelectMultipleFiles("Open Project", filter);
        }

        [Test]
        public void OpenProjectCommand_FileSelected_LoadsTests()
        {
            var files = new string[] { Path.GetFullPath("/path/to/test.dll") };
            View.DialogManager.SelectMultipleFiles(null, null).ReturnsForAnyArgs(files);

            View.OpenProjectCommand.Execute += Raise.Event<CommandHandler>();

            Model.Received().LoadTests(files);
        }

        [Test]
        public void OpenProjectCommand_NoFileSelected_DoesNotLoadTests()
        {
            View.DialogManager.SelectMultipleFiles(null, null).ReturnsForAnyArgs(new string[0]);

            View.OpenProjectCommand.Execute += Raise.Event<CommandHandler>();

            Model.DidNotReceiveWithAnyArgs().LoadTests(null);
        }

        [Test]
        public void CloseCommand_CallsUnloadTest()
        {
            View.CloseCommand.Execute += Raise.Event<CommandHandler>();
            Model.Received().UnloadTests();
        }

        [Test]
        public void SaveCommand_CallsSaveProject()
        {
            View.SaveCommand.Execute += Raise.Event<CommandHandler>();
            // This is NYI, change when we implement it
            Model.DidNotReceive().SaveProject();
        }

        [Test]
        public void SaveAsCommand_CallsSaveProject()
        {
            View.SaveAsCommand.Execute += Raise.Event<CommandHandler>();
            // This is NYI, change when we implement it
            Model.DidNotReceive().SaveProject();
        }

        public void SaveResultsCommand_CallsSaveResults()
        {
        }

        [Test]
        public void ReloadTestsCommand_CallsReloadTests()
        {
            View.ReloadTestsCommand.Execute += Raise.Event<CommandHandler>();
            Model.Received().ReloadTests();
        }

        [Test]
        public void WhenTestIsChanged_ReloadSettingsIsEnabled()
        {
            Model.Settings.Engine.ReloadOnChange = true;
            Model.Events.TestChanged += Raise.Event<TestEventHandler>(new TestEventArgs());
            Model.Received().ReloadTests();
        }

        [Test]
        public void WhenTestIsChanged_ReloadSettingsIsDisabled()
        {
            Model.Settings.Engine.ReloadOnChange = false;
            Model.Events.TestChanged += Raise.Event<TestEventHandler>(new TestEventArgs());
            Model.DidNotReceive().ReloadTests();
        }

        public void SelectRuntimeCommand_PopsUpMenu()
        {
        }

        public void RecentProjectsMenu_PopsUpMenu()
        {
        }

        public void ExitCommand_CallsExit()
        {
        }
    }
}
