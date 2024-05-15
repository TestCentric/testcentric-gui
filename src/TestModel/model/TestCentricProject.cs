// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TestCentric.Engine;
using TestCentric.Gui.Model.Settings;

namespace TestCentric.Gui.Model
{
    public class TestCentricProject : TestPackage
    {
        private ITestModel _model;

        public string FileName => Path.GetFileName(ProjectPath);
        public string ProjectPath { get; private set; }

        public IList<String> TestFiles { get; private set; }

        public TestCentricProject(ITestModel model, string filename)
            : this(model, new string[] { filename }) { }

        public TestCentricProject(ITestModel model, IList<string> filenames)
            :base(filenames)
        {
            _model = model;
            TestFiles = filenames;

            var engineSettings = _model.Settings.Engine;
            var options = model.Options;

            if (engineSettings.Agents > 0)
                AddSetting(EnginePackageSettings.MaxAgents, engineSettings.Agents);
            if (engineSettings.SetPrincipalPolicy)
                AddSetting(EnginePackageSettings.PrincipalPolicy, engineSettings.PrincipalPolicy);
            AddSetting(EnginePackageSettings.ShadowCopyFiles, engineSettings.ShadowCopyFiles);

            if (options != null) // Happens when we test
            {
                AddSetting(EnginePackageSettings.InternalTraceLevel, options.InternalTraceLevel ?? "Off");
                if (options.WorkDirectory != null)
                    AddSetting(EnginePackageSettings.WorkDirectory, options.WorkDirectory);
                if (options.MaxAgents >= 0)
                    Settings[EnginePackageSettings.MaxAgents] = options.MaxAgents;
                if (options.RunAsX86)
                    AddSetting(EnginePackageSettings.RunAsX86, true);
                if (options.DebugAgent)
                    AddSetting(EnginePackageSettings.DebugAgent, true);
                if (options.SimulateUnloadError)
                    AddSetting(EnginePackageSettings.SimulateUnloadError, true);
                if (options.SimulateUnloadTimeout)
                    AddSetting(EnginePackageSettings.SimulateUnloadTimeout, true);
                if (options.TestParameters.Count > 0)
                {
                    string[] parms = new string[options.TestParameters.Count];
                    int index = 0;
                    foreach (string key in options.TestParameters.Keys)
                        parms[index++] = $"{key}={options.TestParameters[key]}";

                    AddSetting("TestParametersDictionary", options.TestParameters);
                    AddSetting("TestParameters", string.Join(";", parms));
                }
            }

            foreach (var subpackage in SubPackages)
                if (Path.GetExtension(subpackage.Name) == ".sln")
                    subpackage.AddSetting(EnginePackageSettings.SkipNonTestAssemblies, true);
        }

        public void SaveAs(string projectPath)
        {
            ProjectPath = projectPath;
            Save();
        }

        public void Save()
        {
            using (StreamWriter writer = new StreamWriter(ProjectPath))
                Save(writer);
        }

        public void Save(TextWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TestPackage));

            try
            {
                serializer.Serialize(writer, this);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception(
                    "Unable to serialize TestProject.", ex);
            }
        }

        public void LoadTests()
        {
            _model.LoadTests(TestFiles);
        }

        public void UnloadTests()
        {

        }
    }
}
