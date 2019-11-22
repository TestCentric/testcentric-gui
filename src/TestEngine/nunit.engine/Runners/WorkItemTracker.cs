// ***********************************************************************
// Copyright (c) 2019 Charlie Poole, Rob Prouse
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
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using NUnit.Engine.Helpers;

namespace NUnit.Engine.Runners
{
    /// <summary>
    /// WorkItemTracker examines test events and keeps track of those
    /// that have started but not yet finished. It implements ITestEventListener
    /// in order to capture events and provides a property to return a list of 
    /// all item ids that have started but not yet finished and a Clear() method
    /// to clear the contents of that list.
    /// </summary>
    internal class WorkItemTracker : ITestEventListener
    {
        private List<XmlNode> _itemsInProcess = new List<XmlNode>();
        private ManualResetEvent _allItemsComplete = new ManualResetEvent(false);
        private object _trackerLock = new object();
        
        public void Clear()
        {
            lock (_trackerLock)
            {
                _itemsInProcess.Clear();
                _allItemsComplete.Reset();
            }
        }

        public bool WaitForCompletion(int millisecondsTimeout)
        {
            return _allItemsComplete.WaitOne(millisecondsTimeout);
        }

        public void IssuePendingNotifications(ITestEventListener listener)
        {
            lock (_trackerLock)
            {
                int count = _itemsInProcess.Count;

                // Signal completion of all pending suites, in reverse order
                while (count > 0)
                    listener.OnTestEvent(CreateTestSuiteElement(_itemsInProcess[--count]).OuterXml);
            }
        }

        private static XmlNode CreateTestSuiteElement(XmlNode startSuiteElement)
        {
            XmlNode testSuiteElement = XmlHelper.CreateTopLevelElement("test-suite");
            testSuiteElement.AddAttribute("type", startSuiteElement.GetAttribute("type"));
            testSuiteElement.AddAttribute("id", startSuiteElement.GetAttribute("id"));
            testSuiteElement.AddAttribute("name", startSuiteElement.GetAttribute("name"));
            testSuiteElement.AddAttribute("fullame", startSuiteElement.GetAttribute("fullname"));
            testSuiteElement.AddAttribute("result", "Failed");
            testSuiteElement.AddAttribute("label", "Cancelled");
            XmlNode failure = testSuiteElement.AddElement("failure");
            XmlNode message = failure.AddElementWithCDataSection("message", "Test run cancelled by user");
            return testSuiteElement;
        }

        void ITestEventListener.OnTestEvent(string report)
        {
            XmlNode xmlNode = XmlHelper.CreateXmlNode(report);

            lock (_trackerLock)
            {
                switch (xmlNode.Name)
                {
                    case "start-suite":
                        _itemsInProcess.Add(xmlNode);
                        break;

                    case "test-suite":
                        string id = xmlNode.GetAttribute("id");
                        RemoveItem(id);

                        if (_itemsInProcess.Count == 0)
                            _allItemsComplete.Set();
                        break;
                }
            }
        }

        private void RemoveItem(string id)
        {
            foreach (XmlNode item in _itemsInProcess)
            {
                if (item.GetAttribute("id") == id)
                {
                    _itemsInProcess.Remove(item);
                    return;
                }
            }
        }
    }
}
