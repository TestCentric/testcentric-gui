// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Xml;

namespace TestCentric.Gui.Model
{
    public struct AssertionResult
    {
        public AssertionResult(XmlNode assertion, string status)
            : this(assertion)
        {
            Status = status;
        }

        public AssertionResult(XmlNode assertion)
        {
            Status = assertion.GetAttribute("label") ?? assertion.GetAttribute("result");
            Message = assertion.SelectSingleNode("message")?.InnerText;
            StackTrace = assertion.SelectSingleNode("stack-trace")?.InnerText;
        }

        public string Status { get; }
        public string Message { get; }
        public string StackTrace { get; }
    }
}
