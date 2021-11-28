// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Xml;
using NUnit.Engine;

namespace TestCentric.Engine.TestBed
{
    using Internal;

    class TestEventHandler : ITestEventListener
    {
        public void OnTestEvent(string report)
        {
            var doc = new XmlDocument();
            doc.LoadXml(report);

            var testEvent = doc.FirstChild;
            switch (testEvent.Name)
            {
                case "unhandled-exception":
                    Console.WriteLine(testEvent.GetAttribute("message"));
                    break;
            }
        }
    }
}
