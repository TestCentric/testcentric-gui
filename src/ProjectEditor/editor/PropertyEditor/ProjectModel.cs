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

using System.IO;
using System.Xml;

namespace NUnit.ProjectEditor
{
    public class ProjectModel : IProjectModel
    {
        private IProjectDocument doc;

        public ProjectModel(IProjectDocument doc)
        {
            this.doc = doc;
        }

        #region IProjectModel Members

        public IProjectDocument Document
        {
            get { return doc; }
        }

        public string ProjectPath
        {
            get { return doc.ProjectPath; }
            set { doc.ProjectPath = value; }
        }

        /// <summary>
        /// BasePath is the base as it appears in the document
        /// and may be null if there is no setting.
        /// </summary>
        public string BasePath
        {
            get { return doc.GetSettingsAttribute("appbase"); }
            set { doc.SetSettingsAttribute("appbase", value); }
        }

        /// <summary>
        /// EffectiveBasePath uses the BasePath if present and otherwise
        /// defaults to the directory part of the ProjectPath.
        /// </summary>
        public string EffectiveBasePath
        {
            get
            {
                return this.BasePath == null
                    ? Path.GetDirectoryName(this.ProjectPath)
                    : Path.Combine(
                        Path.GetDirectoryName(this.ProjectPath),
                        this.BasePath);
            }
        }

        public string ActiveConfigName
        {
            get { return doc.GetSettingsAttribute("activeconfig"); }
            set { doc.SetSettingsAttribute("activeconfig", value); }
        }

        public string ProcessModel
        {
            get { return doc.GetSettingsAttribute("processModel") ?? "Default"; }
            set { doc.SetSettingsAttribute("processModel", value.ToString()); }
        }

        public string DomainUsage
        {
            get { return doc.GetSettingsAttribute("domainUsage") ?? "Default"; }
            set { doc.SetSettingsAttribute("domainUsage", value.ToString()); }
        }

        public ConfigList Configs
        {
            get { return new ConfigList(this); }
        }

        public string[] ConfigNames
        {
            get
            {
                string[] configList = new string[Configs.Count];
                for (int i = 0; i < Configs.Count; i++)
                    configList[i] = Configs[i].Name;

                return configList;
            }
        }

        public IProjectConfig AddConfig(string name)
        {
            XmlNode configNode = XmlHelper.AddElement(doc.RootNode, "Config");
            XmlHelper.AddAttribute(configNode, "name", name);

            return new ProjectConfig(this, configNode);
        }

        public void RemoveConfigAt(int index)
        {
            bool itWasActive = ActiveConfigName == Configs[index].Name;

            doc.RootNode.RemoveChild(doc.ConfigNodes[index]);

            if (itWasActive)
                doc.RemoveSettingsAttribute("activeconfig");
        }

        public void RemoveConfig(string name)
        {
            int index = IndexOf(name);
            if (index >= 0)
            {
                RemoveConfigAt(index);
            }
        }

        #endregion

        #region Helper Properties and Methods

        private int IndexOf(string name)
        {
            for (int index = 0; index < doc.ConfigNodes.Count; index++)
            {
                if (XmlHelper.GetAttribute(doc.ConfigNodes[index], "name") == name)
                    return index;
            }

            return -1;
        }

        #endregion
    }
}
