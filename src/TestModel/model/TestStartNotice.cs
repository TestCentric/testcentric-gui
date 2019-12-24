// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Xml;

namespace TestCentric.Gui.Model
{
    public class TestStartNotice
    {
        public TestStartNotice(XmlNode xmlNode)
        {
            Id = xmlNode.GetAttribute("id");
            Name = xmlNode.GetAttribute("name");
            FullName = xmlNode.GetAttribute("fullname");
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
        public string FullName { get; private set; }
    }
}
