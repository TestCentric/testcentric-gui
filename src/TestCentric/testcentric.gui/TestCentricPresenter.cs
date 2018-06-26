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
using System.IO;
using System.Text;
using System.Windows.Forms;
using NUnit.Engine;

namespace TestCentric.Gui
{
    using Controls;
    using Model;
    using Model.Settings;

    /// <summary>
    /// NUnitPresenter does all file opening and closing that
    /// involves interacting with the user.
    /// 
    /// NOTE: This class originated as the static class
    /// TestLoaderUI and is slowly being converted to a
    /// true presenter. Current limitations include:
    /// 
    /// 1. At this time, the presenter is created by
    /// the form and interacts with it directly, rather
    /// than through an interface. 
    /// 
    /// 2. Many functions, which should properly be in
    /// the presenter, remain in the form.
    /// 
    /// 3. The presenter creates dialogs itself, which
    /// limits testability.
    /// </summary>
    public class TestCentricPresenter
    {
        #region Instance Variables

        // Our nunit project watcher
        //private FileWatcher projectWatcher;

        #endregion

        #region Constructor

        // TODO: Use an interface for view and model
        public TestCentricPresenter(TestCentricMainForm form, ITestModel model, CommandLineOptions options)
        {
            Form = form;
            Model = model;
            Options = options;

            UserSettings = Model.Services.UserSettings;
            RecentFiles = Model.Services.RecentFiles;
        }

        #endregion

        #region Properties

        private TestCentricMainForm Form { get; }

        private ITestModel Model { get; }

        private CommandLineOptions Options { get; }

        private UserSettings UserSettings { get; }

        private IRecentFiles RecentFiles { get; }

        #endregion

        #region Public Methods

        public void OnStartup()
        {
            InitializeControls(Form);

            // Load test specified on command line or
            // the most recent one if options call for it
            if (Options.InputFiles.Count != 0)
                LoadTests(Options.InputFiles);
            else if (UserSettings.Gui.LoadLastProject && !Options.NoLoad)
            {
                foreach (string entry in RecentFiles.Entries)
                {
                    if (entry != null && File.Exists(entry))
                    {
                        LoadTests(entry);
                        break;
                    }
                }
            }

            //if ( guiOptions.include != null || guiOptions.exclude != null)
            //{
            //    testTree.ClearSelectedCategories();
            //    bool exclude = guiOptions.include == null;
            //    string[] categories = exclude
            //        ? guiOptions.exclude.Split(',')
            //        : guiOptions.include.Split(',');
            //    if ( categories.Length > 0 )
            //        testTree.SelectCategories( categories, exclude );
            //}

            // Run loaded test automatically if called for
            if (Model.IsPackageLoaded && Options.RunAllTests)
            {
                // TODO: Temporary fix to avoid problem when /run is used 
                // with ReloadOnRun turned on. Refactor TestLoader so
                // we can just do a run without reload.
                bool reload = UserSettings.Gui.ReloadOnRun;

                try
                {
                    UserSettings.Gui.ReloadOnRun = false;
                    RunAllTests();
                }
                finally
                {
                    UserSettings.Gui.ReloadOnRun = reload;
                }
            }
        }

        #region Open Methods

        public void OpenProject()
        {
            OpenFileDialog dlg = CreateOpenFileDialog("Open Project", true, true);

            if (dlg.ShowDialog(Form) == DialogResult.OK)
                LoadTests(dlg.FileNames);
        }

        //public void WatchProject(string projectPath)
        //{
        //    this.projectWatcher = new FileWatcher(projectPath, 100);

        //    this.projectWatcher.Changed += new FileChangedHandler(OnTestProjectChanged);
        //    this.projectWatcher.Start();
        //}

        //public void RemoveWatcher()
        //{
        //    if (projectWatcher != null)
        //    {
        //        projectWatcher.Stop();
        //        projectWatcher.Dispose();
        //        projectWatcher = null;
        //    }
        //}

        private void OnTestProjectChanged(string filePath)
        {
            //string message = filePath + Environment.NewLine + Environment.NewLine +
            //    "This file has been modified outside of NUnit." + Environment.NewLine +
            //    "Do you want to reload it?";

            //if (Form.MessageDisplay.Ask(message) == DialogResult.Yes)
            //    ReloadProject();
        }
        
        // Keeping this old code from V2 for now as a reminder of how we may need to
        // handle opening of NUnit projects when a config is specified.
        // TODO: Delete when no longer relevant
        //public void OpenProject(string testFileName, string configName, string testName)
        //{
        //    _model.LoadTests(testFileName, configName);
        //    if (_model.IsPackageLoaded)
        //    {
        //        NUnitProject testProject = loader.TestProject;
        //        if (testProject.Configs.Count == 0)
        //            Form.MessageDisplay.Info("Loaded project contains no configuration data");
        //        else if (testProject.ActiveConfig == null)
        //            Form.MessageDisplay.Info("Loaded project has no active configuration");
        //        else if (testProject.ActiveConfig.Assemblies.Count == 0)
        //            Form.MessageDisplay.Info("Active configuration contains no assemblies");
        //        else
        //            loader.LoadTest(testName);
        //    }
        //}

        public void LoadTests(string testFileName)
        {
            LoadTests(new[] { testFileName });
        }

        public void LoadTests(IList<string> testFileNames)
        {
            Model.LoadTests(testFileNames);
        }

        //		public static void OpenResults( Form owner )
        //		{
        //			OpenFileDialog dlg = new OpenFileDialog();
        //			System.ComponentModel.ISite site = owner == null ? null : owner.Site;
        //			if ( site != null ) dlg.Site = site;
        //			dlg.Title = "Open Test Results";
        //
        //			dlg.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
        //			dlg.FilterIndex = 1;
        //			dlg.FileName = "";
        //
        //			if ( dlg.ShowDialog( owner ) == DialogResult.OK ) 
        //				OpenProject( owner, dlg.FileName );
        //		}

        #endregion

        #region Close Methods

        public DialogResult CloseProject()
        {
            //DialogResult result = SaveProjectIfDirty();

            //if (result != DialogResult.Cancel)
                Model.UnloadTests();

            //return result;
            return DialogResult.OK;
        }

        #endregion

        #region Add Methods

        //      public void AddToProject()
        //{
        //	AddToProject( null );
        //}

        // TODO: Not used?
        //public void AddToProject( string configName )
        //{
        //	ProjectConfig config = configName == null
        //		? loader.TestProject.ActiveConfig
        //		: loader.TestProject.Configs[configName];

        //	OpenFileDialog dlg = new OpenFileDialog();
        //	dlg.Title = "Add Assemblies To Project";
        //	dlg.InitialDirectory = config.BasePath;

        //	if ( VisualStudioSupport )
        //		dlg.Filter =
        //			"Projects & Assemblies(*.csproj,*.vbproj,*.vjsproj, *.vcproj,*.dll,*.exe )|*.csproj;*.vjsproj;*.vbproj;*.vcproj;*.dll;*.exe|" +
        //			"Visual Studio Projects (*.csproj,*.vjsproj,*.vbproj,*.vcproj)|*.csproj;*.vjsproj;*.vbproj;*.vcproj|" +
        //			"C# Projects (*.csproj)|*.csproj|" +
        //			"J# Projects (*.vjsproj)|*.vjsproj|" +
        //			"VB Projects (*.vbproj)|*.vbproj|" +
        //			"C++ Projects (*.vcproj)|*.vcproj|" +
        //			"Assemblies (*.dll,*.exe)|*.dll;*.exe";
        //	else
        //		dlg.Filter = "Assemblies (*.dll,*.exe)|*.dll;*.exe";

        //	dlg.FilterIndex = 1;
        //	dlg.FileName = "";

        //	if ( dlg.ShowDialog( Form ) != DialogResult.OK )
        //		return;

        //          if (PathUtils.IsAssemblyFileType(dlg.FileName))
        //          {
        //              config.Assemblies.Add(dlg.FileName);
        //              return;
        //          }
        //          else if (VSProject.IsProjectFile(dlg.FileName))
        //              try
        //              {
        //                  VSProject vsProject = new VSProject(dlg.FileName);
        //                  MessageBoxButtons buttons;
        //                  string msg;

        //                  if (configName != null && vsProject.Configs.Contains(configName))
        //                  {
        //                      msg = "The project being added may contain multiple configurations;\r\r" +
        //                          "Select\tYes to add all configurations found.\r" +
        //                          "\tNo to add only the " + configName + " configuration.\r" +
        //                          "\tCancel to exit without modifying the project.";
        //                      buttons = MessageBoxButtons.YesNoCancel;
        //                  }
        //                  else
        //                  {
        //                      msg = "The project being added may contain multiple configurations;\r\r" +
        //                          "Select\tOK to add all configurations found.\r" +
        //                          "\tCancel to exit without modifying the project.";
        //                      buttons = MessageBoxButtons.OKCancel;
        //                  }

        //                  DialogResult result = Form.MessageDisplay.Ask(msg, buttons);
        //                  if (result == DialogResult.Yes || result == DialogResult.OK)
        //                  {
        //                      loader.TestProject.Add(vsProject);
        //                      return;
        //                  }
        //                  else if (result == DialogResult.No)
        //                  {
        //                      foreach (string assembly in vsProject.Configs[configName].Assemblies)
        //                          config.Assemblies.Add(assembly);
        //                      return;
        //                  }
        //              }
        //              catch (Exception ex)
        //              {
        //                  Form.MessageDisplay.Error("Invalid VS Project", ex);
        //              }
        //      }

        public void AddTestFile()
        {
            OpenFileDialog dlg = CreateOpenFileDialog("Add Test File", true, true);

            if (dlg.ShowDialog(Form) == DialogResult.OK)
            {
                Model.TestFiles.Add(dlg.FileName);
                Model.ReloadTests();
            }
        }

        #endregion

        #region Save Methods

        public void SaveResults()
        {
            //TODO: Save all results
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "Save Test Results as XML";
            dlg.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
            dlg.FileName = "TestResult.xml";
            dlg.InitialDirectory = Path.GetDirectoryName(Model.TestFiles[0]);
            dlg.DefaultExt = "xml";
            dlg.ValidateNames = true;
            dlg.OverwritePrompt = true;

            if (dlg.ShowDialog(Form) == DialogResult.OK)
            {
                try
                {
                    string fileName = dlg.FileName;

                    Model.SaveResults(fileName);

                    Form.MessageDisplay.Info(String.Format($"Results saved in nunit3 format as {fileName}"));
                }
                catch (Exception exception)
                {
                    Form.MessageDisplay.Error("Unable to Save Results", exception);
                }
            }
        }

        #endregion

        #region Run Methods

        public void RunAllTests()
        {
            Model.RunAllTests();
        }

        public void RunSelectedTests()
        {
            RunTests(Form.SelectedTests);
        }

        public void RunFailedTests()
        {
            RunTests(Form.FailedTests);
        }

        public void RunTests(TestNode test)
        {
            EnableRunCommand(false);
            Model.RunTests(test);
        }

        public void RunTests(TestNode[] tests)
        {
            EnableRunCommand(false);
            if (tests != null && tests.Length > 0)
                Model.RunTests(new TestNodeCollection(tests));
        }

        delegate void VoidMethodTakingBooleanArg(bool b);

        public void EnableRunCommand( bool enabled )
        {
            if (Form.InvokeRequired)
                Form.Invoke( new VoidMethodTakingBooleanArg(EnableRunCommand), new object[] { enabled } );
            else
                Form.EnableRunCommand(enabled);
        }

        public void EnableStopCommand( bool enabled )
        {
            if (Form.InvokeRequired)
                Form.Invoke( new VoidMethodTakingBooleanArg(EnableStopCommand), new object[] { enabled } );
            else
                Form.EnableStopCommand(enabled);
        }

        #endregion

        #endregion

        #region Helper Methods

        private void InitializeControls(Control owner)
        {
            foreach (Control control in owner.Controls)
            {
                var view = control as IViewControl;
                if (view != null)
                    view.InitializeView(Model, this);
                
                InitializeControls(control);
            }
        }

        private OpenFileDialog CreateOpenFileDialog(string title, bool includeProjects, bool includeAssemblies)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            System.ComponentModel.ISite site = Form == null ? null : Form.Site;
            if (site != null) dlg.Site = site;
            dlg.Title = title;
            dlg.Filter = DialogFilter(includeProjects, includeAssemblies);
            dlg.FilterIndex = 1;
            dlg.FileName = "";
            dlg.Multiselect = true;
            return dlg;
        }

        private string DialogFilter(bool includeProjects, bool includeAssemblies)
        {
            StringBuilder sb = new StringBuilder();
            bool nunit = includeProjects && Model.NUnitProjectSupport;
            bool vs = includeProjects && Model.VisualStudioSupport;

            if (includeProjects && includeAssemblies)
            {
                if (nunit && vs)
                    sb.Append("Projects & Assemblies(*.nunit,*.csproj,*.fsproj,*.vbproj,*.vjsproj,*.vcproj,*.sln,*.dll,*.exe )|*.nunit;*.csproj;*.fsproj;*.vjsproj;*.vbproj;*.vcproj;*.sln;*.dll;*.exe|");
                else if (nunit)
                    sb.Append("Projects & Assemblies (*.nunit,*.dll,*.exe)|*.nunit;*.dll;*.exe|");
                else if (vs)
                    sb.Append("Projects & Assemblies(*.csproj,*.fsproj,*.vbproj,*.vjsproj,*.vcproj,*.sln,*.dll,*.exe )|*.csproj;*.fsproj;*.vjsproj;*.vbproj;*.vcproj;*.sln;*.dll;*.exe|");
            }

            if (nunit)
                sb.Append("NUnit Projects (*.nunit)|*.nunit|");

            if (vs)
                sb.Append("Visual Studio Projects (*.csproj,*.fsproj,*.vbproj,*.vjsproj,*.vcproj,*.sln)|*.csproj;*.fsproj;*.vjsproj;*.vbproj;*.vcproj;*.sln|");

            if (includeAssemblies)
                sb.Append("Assemblies (*.dll,*.exe)|*.dll;*.exe|");

            sb.Append("All Files (*.*)|*.*");

            return sb.ToString();
        }

        private static bool CanWriteProjectFile(string path)
        {
            return !File.Exists(path) ||
                (File.GetAttributes(path) & FileAttributes.ReadOnly) == 0;
        }

        private static string Quoted(string s)
        {
            return "\"" + s + "\"";
        }

        #endregion
    }
}
