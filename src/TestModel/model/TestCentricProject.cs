// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCentric.Engine;
using TestCentric.Gui.Model.Settings;

namespace TestCentric.Gui.Model
{
    public class TestCentricProject : TestPackage
    {
        private ITestModel _model;

        public string FileName => Path.GetFileName(ProjectPath);
        public string ProjectPath { get; }

        public IList<String> TestFiles { get; private set; }

        public TestCentricProject(ITestModel model, string filename)
            : this(model, new string[] { filename }) { }

        public TestCentricProject(ITestModel model, IList<string> filenames)
            :base(filenames)
        {
            _model = model;
            TestFiles = filenames;

            var engineSettings = _model.Settings.Engine;

            // We use AddSetting rather than just setting the value because
            // it propagates the setting to all subprojects.

            if (engineSettings.Agents > 0)
                AddSetting(EnginePackageSettings.MaxAgents, engineSettings.Agents);

            if (engineSettings.SetPrincipalPolicy)
                AddSetting(EnginePackageSettings.PrincipalPolicy, engineSettings.PrincipalPolicy);

            //if (Options.InternalTraceLevel != null)
            //    package.AddSetting(EnginePackageSettings.InternalTraceLevel, Options.InternalTraceLevel);

            AddSetting(EnginePackageSettings.ShadowCopyFiles, engineSettings.ShadowCopyFiles);

            foreach (var subpackage in SubPackages)
                if (Path.GetExtension(subpackage.Name) == ".sln")
                    subpackage.AddSetting(EnginePackageSettings.SkipNonTestAssemblies, true);

            foreach (var entry in _model.PackageOverrides)
                AddSetting(entry.Key, entry.Value);
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
