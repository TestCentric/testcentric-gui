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
	using Views;

	public class WhenTestRunCompletes : MainPresenterTestBase
	{
		[SetUp]
		public void SimulateTestRunFinish()
		{
			ClearAllReceivedCalls();

			Model.HasTests.Returns(true);
			Model.HasResults.Returns(true);
			Model.IsTestRunning.Returns(false);

			var resultNode = new ResultNode("<test-run/>");
			Model.Events.RunFinished += Raise.Event<TestResultEventHandler>(new TestResultEventArgs(resultNode));
		}

#if NYI // Add after implementation of project or package saving
        [TestCase("NewProjectCommand", true)]
        [TestCase("OpenProjectCommand", true)]
        [TestCase("SaveCommand", true)]
        [TestCase("SaveAsCommand", true)
#endif

        [TestCase("RunButton", true)]
        [TestCase("StopButton", false)]
        [TestCase("OpenCommand", true)]
        [TestCase("CloseCommand", true)]
        [TestCase("AddTestFileCommand", true)]
        [TestCase("ReloadTestsCommand", true)]
        [TestCase("RuntimeMenu", true)]
        [TestCase("RecentFilesMenu", true)]
        [TestCase("ExitCommand", true)]
        [TestCase("RunAllCommand", true)]
        [TestCase("RunSelectedCommand", true)]
        [TestCase("RunFailedCommand", true)]
        [TestCase("StopRunCommand", false)]
        [TestCase("SaveResultsCommand", true)]
        public void CheckCommandEnabled(string propName, bool enabled)
        {
            ViewElement(propName).Received().Enabled = enabled;
        }
    }
}
