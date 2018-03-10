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
using System.Collections.Generic;
using System.Xml;

namespace NUnit.ProjectEditor
{
	/// <summary>
	/// Represents a list of assemblies. It stores paths 
	/// that are added and fires an event whenevever it
	/// changes. All paths should be added as absolute paths.
	/// </summary>
	public class AssemblyList
	{
        private XmlNode configNode;

        public AssemblyList(XmlNode configNode)
        {
            this.configNode = configNode;
        }

        #region Properties

        public string this[int index]
        {
            get { return XmlHelper.GetAttribute(AssemblyNodes[index], "path"); }
            set { XmlHelper.SetAttribute(AssemblyNodes[index], "path", value); }
        }

        public int Count
        {
            get { return AssemblyNodes.Count; }
        }

        #endregion

        #region Methods

        public void Add(string assemblyPath)
        {
            XmlHelper.AddAttribute(
                XmlHelper.AddElement(configNode, "assembly"),
                "path",
                assemblyPath);
        }

        public void Insert(int index, string assemblyPath)
        {
            XmlHelper.AddAttribute(
                XmlHelper.InsertElement(configNode, "assembly", index),
                "path",
                assemblyPath);
        }

        public void Remove(string assemblyPath)
        {
            foreach (XmlNode node in configNode.SelectNodes("assembly"))
            {
                string path = XmlHelper.GetAttribute(node, "path");
                if (path == assemblyPath)
                {
                    configNode.RemoveChild(node);
                    break;
                }
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            foreach (XmlNode node in AssemblyNodes)
                yield return XmlHelper.GetAttribute(node, "path");
        }

        #endregion

        #region private Properties

        private XmlNodeList AssemblyNodes
        {
            get { return configNode.SelectNodes("assembly"); }
        }

        private XmlNode GetAssemblyNodes(int index)
        {
            return AssemblyNodes[index];
        }

        #endregion
    }
}
