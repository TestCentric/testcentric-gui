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

using NSubstitute;
using NUnit.Framework;

namespace TestCentric.Gui.Presenters.Main
{
	using Model;

	public class WhenTestRunBegins : MainPresenterTestBase
	{
		[SetUp]
		protected void SimulateTestRunStarting()
		{
			ClearAllReceivedCalls();

			Model.HasTests.Returns(true);
			Model.IsTestRunning.Returns(true);
			Model.Events.RunStarting += Raise.Event<RunStartingEventHandler>(new RunStartingEventArgs(1234));
		}

#if NYI // Add after implementation of project or package saving
        [TestCase("NewProjectCommand", false)]
        [TestCase("OpenProjectCommand", false)]
        [TestCase("SaveCommand", false)]
        [TestCase("SaveAsCommand", false)
#endif
        
        [TestCase("RunButton", false)]
        [TestCase("StopButton", true)]
        [TestCase("OpenCommand", false)]
        [TestCase("CloseCommand", false)]
        [TestCase("AddTestFileCommand", false)]
        [TestCase("ReloadTestsCommand", false)]
        [TestCase("RuntimeMenu", false)]
        [TestCase("RecentFilesMenu", false)]
        [TestCase("ExitCommand", true)]
        [TestCase("RunAllCommand", false)]
        [TestCase("RunSelectedCommand", false)]
        [TestCase("RunFailedCommand", false)]
        [TestCase("StopRunCommand", true)]
        [TestCase("SaveResultsCommand", false)]
        public void CheckCommandEnabled(string propName, bool enabled)
        {
            ViewElement(propName).Received().Enabled = enabled;
        }
    }
}
