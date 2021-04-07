// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
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
