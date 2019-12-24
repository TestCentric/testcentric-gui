// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace TestCentric.Gui.Model
{
    /// <summary>
    /// Argument used for all test events
    /// </summary>
    public class TestEventArgs : EventArgs
    {
    }

    public class TestNodeEventArgs : TestEventArgs
    {
        public TestNodeEventArgs(TestNode test)
        {
            if (test == null)
                throw new ArgumentNullException("test");

            Test = test;
        }

        public TestNode Test { get; }
    }

    public class RunStartingEventArgs : TestEventArgs
    {
        public RunStartingEventArgs(int testCount)
        {
            TestCount = testCount;
        }

        public int TestCount { get; set; }
    }

    public class TestResultEventArgs : TestEventArgs
    {
        public TestResultEventArgs(ResultNode result)
        {
            if (result == null)
                throw new ArgumentNullException("result");

            Result = result;
        }

        public ResultNode Result { get; set; }
    }

    public class TestItemEventArgs : EventArgs
    {
        public TestItemEventArgs(ITestItem testItem)
        {
            TestItem = testItem;
        }

        public ITestItem TestItem { get; private set; }
    }

    public class TestOutputEventArgs : EventArgs
    {
        public TestOutputEventArgs(string testName, string stream, string text)
        {
            TestName = testName;
            Stream = stream;
            Text = text;
        }

        public string TestName { get; }
        public string Stream { get; }
        public string Text { get; }
    }

    public class UnhandledExceptionEventArgs : EventArgs
    {
        public UnhandledExceptionEventArgs(string message, string stackTrace)
        {
            Message = message;
            StackTrace = stackTrace;
        }

        public string Message;
        public string StackTrace;
    }

    public class TestFilesLoadingEventArgs : EventArgs
    {
        public TestFilesLoadingEventArgs(IList<string> testFilesLoading)
        {
            TestFilesLoading = testFilesLoading;
        }

        public IList<string> TestFilesLoading { get; }
    }
}
