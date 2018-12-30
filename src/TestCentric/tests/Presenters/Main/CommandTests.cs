// ***********************************************************************
// Copyright (c) 2016 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System.Collections.Generic;
using System.IO;
using System.Linq;
using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters.Main
{
    using Elements;
    using Views;

    public class CommandTests : MainPresenterTestBase
    {
        // TODO: Because the presenter opens dialogs for these commands,
        // they can't be tested directly. This could be fixed if the
        // presenter asked the view to open dialogs.

        //[Test]
        //public void NewProjectCommand_CallsNewProject()
        //{
        //    View.NewProjectCommand.Execute += Raise.Event<CommandHandler>();
        //    // This is NYI, change when we implement it
        //    Model.DidNotReceive().NewProject();
        //}

        [Test]
        public void OpenCommand_GetsFilesAndLoadsTests()
        {
            var files = new string[] { Path.GetFullPath("/path/to/test.dll") };
            _view.DialogManager.GetFilesToOpen(null).ReturnsForAnyArgs(files);

            _view.OpenCommand.Execute += Raise.Event<CommandHandler>();

            _view.DialogManager.Received().GetFilesToOpen("Open Project");
            _model.Received().LoadTests(files);
        }

        [Test]
        public void OpenCommand_WhenNothingIsSelected_DoesNotLoadTests()
        {
            _view.DialogManager.GetFilesToOpen(null).ReturnsForAnyArgs((string[])null);

            _view.OpenCommand.Execute += Raise.Event<CommandHandler>();

            _model.DidNotReceive().LoadTests(Arg.Any<IList<string>>());
        }

        [Test]
        public void AddTestFilesCommand_LoadsTests()
        {
            var testFiles = new List<string>();
            testFiles.Add("FILE1");
            testFiles.Add("FILE2");
            _model.TestFiles.Returns(testFiles);

            var filesToAdd = new string[] { Path.GetFullPath("/path/to/test.dll") };
            _view.DialogManager.GetFilesToOpen(null).ReturnsForAnyArgs(filesToAdd);

            var allFiles = new List<string>(testFiles);
            allFiles.AddRange(filesToAdd);

            _view.AddTestFilesCommand.Execute += Raise.Event<CommandHandler>();

            _view.DialogManager.Received().GetFilesToOpen("Add Test Files");
            _model.Received().LoadTests(Arg.Is<List<string>>(l => l.SequenceEqual(allFiles)));
        }

        [Test]
        public void AddTestFilesCommand_WhenNothingIsSelected_DoesNotLoadTests()
        {
            _view.DialogManager.GetFilesToOpen(null).ReturnsForAnyArgs((string[])null);

            _view.AddTestFilesCommand.Execute += Raise.Event<CommandHandler>();

            _model.DidNotReceive().LoadTests(Arg.Any<IList<string>>());
        }

        [Test]
        public void CloseCommand_CallsUnloadTest()
        {
            _view.CloseCommand.Execute += Raise.Event<CommandHandler>();
            _model.Received().UnloadTests();
        }

        //[Test]
        //public void SaveCommand_CallsSaveProject()
        //{
        //    View.SaveCommand.Execute += Raise.Event<CommandHandler>();
        //    // This is NYI, change when we implement it
        //    Model.DidNotReceive().SaveProject();
        //}

        //[Test]
        //public void SaveAsCommand_CallsSaveProject()
        //{
        //    View.SaveAsCommand.Execute += Raise.Event<CommandHandler>();
        //    // This is NYI, change when we implement it
        //    Model.DidNotReceive().SaveProject();
        //}

        public void SaveResultsCommand_CallsSaveResults()
        {
        }

        [Test]
        public void ReloadTestsCommand_CallsReloadTests()
        {
            _view.ReloadTestsCommand.Execute += Raise.Event<CommandHandler>();
            _model.Received().ReloadTests();
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
