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
using System.Xml;

namespace NUnit.ProjectEditor
{
    public interface IProjectDocument
    {
        #region Events

        event ActionDelegate ProjectCreated;
        event ActionDelegate ProjectClosed;
        event ActionDelegate ProjectChanged;

        #endregion

        #region Properties

        string Name { get; }

        /// <summary>
        /// Gets or sets the path to which a doc will be saved.
        /// </summary>
        string ProjectPath { get; set; }

        bool IsEmpty { get; }
        bool IsValid { get; }

        string XmlText { get; set; }
        Exception Exception { get; }

        XmlNode RootNode { get; }
        XmlNode SettingsNode { get; }
        XmlNodeList ConfigNodes { get; }

        bool HasUnsavedChanges { get; }

        string GetSettingsAttribute(string name);
        void SetSettingsAttribute(string name, string value);
        void RemoveSettingsAttribute(string name);

        #endregion

        #region Methods

        void CreateNewProject();
        void OpenProject(string fileName);
        void CloseProject();
        void SaveProject();
        void SaveProject(string fileName);

        void LoadXml(string xmlText);

        #endregion
    }
}
