// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
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
