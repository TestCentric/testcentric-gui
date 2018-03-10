// ***********************************************************************
// Copyright (c) 2018 Charlie Poole
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

using NUnit.Framework;
using NUnit.Tests.Assemblies;
using TestCentric.Gui.Model;

namespace TestCentric.Gui.Tests
{
    using Controls;

	/// <summary>
	/// Summary description for ProgressBarTests.
	/// </summary>
	[TestFixture]
	public class ProgressBarTests
	{
		private TestProgressBar progressBar;
		//private MockTestEventSource mockEvents;
		private string testsDll = MockAssembly.AssemblyPath;
		//private TestSuite suite;
		int testCount;

		[SetUp]
		public void Setup()
		{
			progressBar = new TestProgressBar();

			//TestSuiteBuilder builder = new TestSuiteBuilder();
			//suite = builder.Build( new TestPackage( testsDll ) );

			//mockEvents = new MockTestEventSource( suite );
		}

  //      // .NET 1.0 sometimes throws:
  //      // ExternalException : A generic error occurred in GDI+.
  //      [Test, Platform(Exclude = "Net-1.0")]
  //      public void TestProgressDisplay()
		//{
		//	progressBar.Subscribe( mockEvents );
		//	mockEvents.TestFinished += new TestEventHandler( OnTestFinished );

		//	testCount = 0;
		//	//mockEvents.SimulateTestRun();
			
		//	Assert.AreEqual( 0, progressBar.Minimum );
		//	Assert.AreEqual( MockAssembly.Tests, progressBar.Maximum );
		//	Assert.AreEqual( 1, progressBar.Step );
		//	Assert.AreEqual( MockAssembly.ResultCount, progressBar.Value );
		//	Assert.AreEqual( Color.Red, progressBar.ForeColor );
		//}

		private void OnTestFinished( object sender, TestEventArgs e )
		{
			++testCount;
			// Assumes delegates are called in order of adding
			Assert.AreEqual( testCount, progressBar.Value );
		}
	}
}
