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

using System;

namespace TestCentric.Gui.Views
{
    /// <summary>
    /// TestResultItem is used by the ErrorsAndFailuresView to hold
    /// all the information about a test result for display.
    /// </summary>
    public class TestResultItem
    {
        private string testName;
        private string message;
        private string stackTrace;

        public TestResultItem(string testName, string message, string stackTrace)
        {
            this.testName = testName;
            this.message = message;
            this.stackTrace = stackTrace;
        }

        public override string ToString()
        {
            if (message?.Length > 64000)
                return string.Format("{0}:{1}{2}", testName, Environment.NewLine, message.Substring(0, 64000));

            return GetMessage();
        }

        public string GetMessage()
        {
            return String.Format("{0}:{1}{2}", testName, Environment.NewLine, message);
        }

        public string GetToolTipMessage()   //NRG 05/28/03 - Substitute spaces for tab characters
        {
            return (ReplaceTabs(GetMessage(), 8)); // Change each tab to 8 space characters
        }

        public string ReplaceTabs(string strOriginal, int nSpaces)  //NRG 05/28/03
        {
            string strSpaces = string.Empty;
            strSpaces = strSpaces.PadRight(nSpaces, ' ');
            return (strOriginal.Replace("\t", strSpaces));
        }

        public string StackTrace
        {
            get
            {
                return stackTrace == null ? null : StackTraceFilter.Filter(stackTrace);

                //string trace = "No stack trace is available";
                //if(stackTrace != null)
                //    trace = StackTraceFilter.Filter(stackTrace);

                //return trace;
            }
        }
    }
}
