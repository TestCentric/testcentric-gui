// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using NUnit.Framework;
using NSubstitute;

namespace TestCentric.Gui.Presenters.Main
{
    using Model;

    public class WhenTestsAreLoading : MainPresenterTestBase
    {
        [Test]
        public void View_Receives_FileNameOfSingleAssembly()
        {
            string path = "C:\\git\\projects\\pull-request\\SomeAssembly.AcceptanceTests.dll";
            var arguments = new TestFilesLoadingEventArgs(new[] { path });

            Model.Events.TestsLoading += Raise.Event<TestFilesLoadingEventHandler>(arguments);

            View.Received().OnTestAssembliesLoading($"Loading Assembly: {path}");
        }

        [Test]
        public void View_Receives_CountOfMultipleAssemblies()
        {
            var arguments = new TestFilesLoadingEventArgs(new[]
                {
                    "C:\\git\\projects\\pull-request\\SomeAssembly.Tests.dll",
                    "C:\\git\\projects\\pull-request\\SomeAssembly.IntegrationTests.dll",
                    "C:\\git\\projects\\pull-request\\SomeAssembly.AcceptanceTests.dll"
                });
            Model.Events.TestsLoading += Raise.Event<TestFilesLoadingEventHandler>(arguments);

            View.Received().OnTestAssembliesLoading("Loading 3 Assemblies...");
        }
    }
}
