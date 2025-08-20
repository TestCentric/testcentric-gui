// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.IO;

namespace TestCentric.Gui.Views
{
    /// <summary>
    /// TestResultItem is used by the ErrorsAndFailuresView to hold
    /// all the information about a test result for display.
    /// </summary>
    public class TestResultItem
    {
        private string _status;
        private string _testName;
        private string _message;
        private string _rawStackTrace;

        public TestResultItem(string status, string testName, string message, string stackTrace)
        {
            this._status = status;
            this._testName = testName;
            this._message = message;
            this._rawStackTrace = stackTrace;
        }

        public override string ToString()
        {
            if (_message?.Length > 64000)
                return string.Format("{0}: {1}{2}{3}", _status, _testName, Environment.NewLine, _message.Substring(0, 64000));

            return GetMessage();
        }

        public string GetMessage()
        {
            return String.Format("{0}: {1}{2}{3}", _status, _testName, Environment.NewLine, _message);
        }

        public string FilteredStackTrace
        {
            get
            {
                if (_rawStackTrace == null)
                    return null;

                StringWriter sw = new StringWriter();
                StringReader sr = new StringReader(_rawStackTrace);

                try
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (!FilterLine(line))
                            sw.WriteLine(line.Trim());
                    }
                }
                catch (Exception)
                {
                    return _rawStackTrace;
                }

                return sw.ToString();
            }
        }

        static bool FilterLine(string line)
        {
            string[] patterns = new string[]
            {
                "NUnit.Core.TestCase",
                "NUnit.Core.ExpectedExceptionTestCase",
                "NUnit.Core.TemplateTestCase",
                "NUnit.Core.TestResult",
                "NUnit.Core.TestSuite",
                "NUnit.Framework.Assertion",
                "NUnit.Framework.Assert",
                "System.Reflection.MonoMethod"
            };

            for (int i = 0; i < patterns.Length; i++)
            {
                if (line.IndexOf(patterns[i]) > 0)
                    return true;
            }

            return false;
        }
    }
}
