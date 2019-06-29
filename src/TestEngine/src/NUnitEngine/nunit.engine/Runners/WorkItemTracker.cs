using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;

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
        public List<XmlNode> ItemsInProcess { get; } = new List<XmlNode>();

        public ManualResetEvent AllItemsComplete = new ManualResetEvent(false);
        
        public void Clear()
        {
            ItemsInProcess.Clear();
            AllItemsComplete.Reset();
        }

        void ITestEventListener.OnTestEvent(string report)
        {
            XmlNode xmlNode = XmlHelper.CreateXmlNode(report);

            switch (xmlNode.Name)
            {
                case "start-suite":
                    ItemsInProcess.Add(xmlNode);
                    break;

                case "test-suite":
                    string id = xmlNode.GetAttribute("id");
                    RemoveItem(id);

                    if (ItemsInProcess.Count == 0)
                        AllItemsComplete.Set();
                    break;
            }
        }

        private void RemoveItem(string id)
        {
            foreach (XmlNode item in ItemsInProcess)
            {
                if (item.GetAttribute("id") == id)
                {
                    ItemsInProcess.Remove(item);
                    return;
                }
            }
        }
    }
}
