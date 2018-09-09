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

namespace TestCentric.Gui.Presenters
{
    using Controls;
    using Model;
    using Model.Settings;
    using Views;

    /// <summary>
    /// TestCentricPresenter does all file opening and closing that
    /// involves interacting with the user.
    /// 
    /// NOTE: This class originated as the static class
    /// TestLoaderUI and is slowly being converted to a
    /// true presenter. Current limitations include:
    /// 
    /// 1. Many functions, which should properly be in
    /// the presenter, remain in the form.
    /// 
    /// 2. The form has references to the model and presenter.
    /// 
    /// 3. The presenter creates dialogs itself, which
    /// limits testability.
    /// </summary>
    public class TestCentricPresenter
    {
        #region Instance Variables

        private readonly TestCentricMainView _view;

        private readonly ITestModel _model;

        private readonly CommandLineOptions _options;

        private readonly UserSettings _settings;

        private readonly IRecentFiles _recentFiles;

        // Our nunit project watcher
        //private FileWatcher projectWatcher;

        // Current Long operation display, if any, or null
        private LongRunningOperationDisplay _longOpDisplay;

        #endregion

        #region Constructor

        // TODO: Use an interface for view
        public TestCentricPresenter(TestCentricMainView view, ITestModel model, CommandLineOptions options)
        {
            _view = view;
            _model = model;
            _options = options;

            _settings = _model.Services.UserSettings;
            _recentFiles = _model.Services.RecentFiles;

            view.Presenter = this;

            EnableRunCommand(false);
            EnableStopCommand(false);

            WireUpEvents();
        }

        private void WireUpEvents()
        {
            // Model Events
            _model.Events.TestsLoading += (TestFilesLoadingEventArgs e) =>
            {
                EnableRunCommand(false);
                _view.SaveResultsCommand.Enabled = false;
                _longOpDisplay = new LongRunningOperationDisplay(_view, "Loading...");
            };

            _model.Events.TestLoaded += (TestNodeEventArgs e) =>
            {
                if (_longOpDisplay != null)
                {
                    _longOpDisplay.Dispose();
                    _longOpDisplay = null;
                }

                EnableRunCommand(true);
                _view.SaveResultsCommand.Enabled = false;
            };

            _model.Events.TestsUnloading += (TestEventArgse) =>
            {
                EnableRunCommand( false );
            };

            _model.Events.TestUnloaded += (TestEventArgs e) =>
            {
                EnableRunCommand(false);
                _view.SaveResultsCommand.Enabled = false;
            };

            _model.Events.TestsReloading += (TestEventArgs e) =>
            {
                EnableRunCommand( false );
                _longOpDisplay = new LongRunningOperationDisplay( _view, "Reloading..." );
            };

            _model.Events.TestReloaded += (TestNodeEventArgs e) =>
            {
                if (_longOpDisplay != null)
                {
                    _longOpDisplay.Dispose();
                    _longOpDisplay = null;
                }

                EnableRunCommand(true);
                _view.SaveResultsCommand.Enabled = false;
            };

            _model.Events.RunStarting += (RunStartingEventArgs e) =>
            {
                EnableRunCommand(false);
                EnableStopCommand(true);

                _view.SaveResultsCommand.Enabled = false;
            };

            _model.Events.RunFinished += (TestResultEventArgs e) =>
            {
                EnableStopCommand(false);
                EnableRunCommand(true);

                _view.SaveResultsCommand.Enabled = true;
            };

            // View Events

            _view.Startup += () => OnStartup();

            _view.FormClosing += (s, e) =>
            {
                if (_model.IsPackageLoaded)
                {
                    if (_model.IsTestRunning)
                    {
                        DialogResult dialogResult = _view.MessageDisplay.Ask(
                            "A test is running, do you want to stop the test and exit?");

                        if (dialogResult == DialogResult.No)
                        {
                            e.Cancel = true;
                            return;
                        }

                        _model.CancelTestRun();
                    }

                    if (CloseProject() == DialogResult.Cancel)
                        e.Cancel = true;
                }
            };

            _view.RunButton.Execute += () => RunSelectedTests();
            _view.StopButton.Execute += () => CancelRun();

            _view.OpenCommand.Execute += () => OpenProject();
            _view.CloseCommand.Execute += () => CloseProject();
            _view.AddTestFileCommand.Execute += () => AddTestFile();
            _view.ReloadTestsCommand.Execute += () => ReloadTests();

            _view.RunAllCommand.Execute += () => RunAllTests();
            _view.RunSelectedCommand.Execute += () => RunSelectedTests();
            _view.RunFailedCommand.Execute += () => RunFailedTests();
            _view.StopRunCommand.Execute += () => CancelRun();

            _view.ProjectEditorCommand.Execute += () =>
            {
                string editorPath = _settings.Gui.ProjectEditorPath;
                if (editorPath != null && File.Exists(editorPath))
                    System.Diagnostics.Process.Start(editorPath);
            };

            _view.SaveResultsCommand.Execute += () => SaveResults();

            _view.DisplaySettingsCommand.Execute += () =>
            {
                SettingsDialog.Display(_view, this, _model);
            };
        }

        #endregion

        #region Public Methods

        public void OnStartup()
        {
            InitializeControls(_view);

            // Load test specified on command line or
            // the most recent one if options call for it
            if (_options.InputFiles.Count != 0)
                LoadTests(_options.InputFiles);
            else if (_settings.Gui.LoadLastProject && !_options.NoLoad)
            {
                foreach (string entry in _recentFiles.Entries)
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
            if (_model.IsPackageLoaded && _options.RunAllTests)
            {
                // TODO: Temporary fix to avoid problem when /run is used 
                // with ReloadOnRun turned on. Refactor TestLoader so
                // we can just do a run without reload.
                bool reload = _settings.Gui.ReloadOnRun;

                try
                {
                    _settings.Gui.ReloadOnRun = false;
                    RunAllTests();
                }
                finally
                {
                    _settings.Gui.ReloadOnRun = reload;
                }
            }
        }

        #region Open Methods

        private void OpenProject()
        {
            OpenFileDialog dlg = CreateOpenFileDialog("Open Project", true, true);

            if (dlg.ShowDialog() == DialogResult.OK)
                LoadTests(dlg.FileNames);
        }

        public void LoadTests(string testFileName)
        {
            LoadTests(new[] { testFileName });
        }

        private void LoadTests(IList<string> testFileNames)
        {
            _model.LoadTests(testFileNames);
        }

        #endregion

        #region Close Methods

        public DialogResult CloseProject()
        {
            //DialogResult result = SaveProjectIfDirty();

            //if (result != DialogResult.Cancel)
                _model.UnloadTests();

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

        //	if ( dlg.ShowDialog() != DialogResult.OK )
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

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _model.TestFiles.Add(dlg.FileName);
                _model.ReloadTests();
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
            dlg.InitialDirectory = Path.GetDirectoryName(_model.TestFiles[0]);
            dlg.DefaultExt = "xml";
            dlg.ValidateNames = true;
            dlg.OverwritePrompt = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileName = dlg.FileName;

                    _model.SaveResults(fileName);

                    _view.MessageDisplay.Info(String.Format($"Results saved in nunit3 format as {fileName}"));
                }
                catch (Exception exception)
                {
                    _view.MessageDisplay.Error("Unable to Save Results", exception);
                }
            }
        }

        #endregion

        #region Reload Methods

        public void ReloadTests()
        {
            _model.ReloadTests();
            //NUnitProject project = loader.TestProject;

            //bool wrapper = project.IsAssemblyWrapper;
            //string projectPath = project.ProjectPath;
            //string activeConfigName = project.ActiveConfigName;

            //// Unload first to avoid message asking about saving
            //loader.UnloadProject();

            //if (wrapper)
            //    OpenProject(projectPath);
            //else
            //    OpenProject(projectPath, activeConfigName, null);
        }

        #endregion

        #region Run Methods

        public void RunAllTests()
        {
            EnableRunCommand(false);
            _model.ClearResults();
            _model.RunAllTests();
        }

        public void RunSelectedTests()
        {
            RunTests(_view.SelectedTests);
        }

        public void RunFailedTests()
        {
            RunTests(_view.FailedTests);
        }

        public void RunTests(TestNode test)
        {
            RunTests(new TestNode[] { test });
        }

        public void RunTests(TestNode[] tests)
        {
            EnableRunCommand(false);
            
            if (_settings.Gui.ReloadOnRun)
                _model.ClearResults();

            if (tests != null && tests.Length > 0)
                _model.RunTests(new TestSelection(tests));
        }

        public void CancelRun()
        {
            EnableStopCommand(false);

            if (_model.IsTestRunning)
            {
                DialogResult dialogResult = _view.MessageDisplay.Ask(
                    "Do you want to cancel the running test?");

                if (dialogResult == DialogResult.No)
                    EnableStopCommand(true);
                else
                    _model.CancelTestRun();
            }
        }

        public void EnableRunCommand( bool enabled )
        {
            _view.RunButton.Enabled = enabled;
            _view.RunAllCommand.Enabled = enabled;
            _view.RunSelectedCommand.Enabled = enabled;
            _view.RunFailedCommand.Enabled = enabled && _model.HasResults && _view.FailedTests != null;
        }

        public void EnableStopCommand(bool enabled)
        {
            _view.StopButton.Enabled = enabled;
            _view.StopRunCommand.Enabled = enabled;
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
                    view.InitializeView(_model, this);
                
                InitializeControls(control);
            }
        }

        private OpenFileDialog CreateOpenFileDialog(string title, bool includeProjects, bool includeAssemblies)
        {
            OpenFileDialog dlg = new OpenFileDialog();
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
            bool nunit = includeProjects && _model.NUnitProjectSupport;
            bool vs = includeProjects && _model.VisualStudioSupport;

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
