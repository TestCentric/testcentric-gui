// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace NUnit.ProjectEditor
{
    public interface IProjectModel
    {
        #region Properties

        IProjectDocument Document { get; }

        string ProjectPath { get; set; }
        string BasePath { get; set; }
        string EffectiveBasePath { get; }

        string ActiveConfigName { get; set; }

        string ProcessModel { get; set; }
        string DomainUsage { get; set; }

        ConfigList Configs { get; }
        string[] ConfigNames { get; }

        #endregion

        #region Methods

        IProjectConfig AddConfig(string name);
        void RemoveConfig(string name);
        void RemoveConfigAt(int index);

        #endregion
    }
}
