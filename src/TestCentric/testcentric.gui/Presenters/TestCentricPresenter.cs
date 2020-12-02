// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Windows.Forms;
using TestCentric.Common;

namespace TestCentric.Gui.Presenters
{
    using Model;
    using Model.Services;
    using Model.Settings;
    using Views;
    using Dialogs;
    using NUnit.Engine;

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
    /// 2. The presenter creates dialogs itself, which
    /// limits testability.
    /// </summary>
    public class TestCentricPresenter
    {
        private static readonly Logger log = InternalTrace.GetLogger(nameof(TestCentricPresenter));

        #region Instance Variables

        private readonly IMainView _view;

        private readonly ITestModel _model;

        private readonly CommandLineOptions _options;

        private readonly UserSettings _settings;

        private readonly RecentFiles _recentFiles;

        private readonly RuntimeSelectionController _runtimeSelectionController;

        // Our nunit project watcher
        //private FileWatcher projectWatcher;

        // Current Long operation display, if any, or null
        private LongRunningOperationDisplay _longOpDisplay;

        private List<string> _resultFormats = new List<string>();

        #endregion

        #region Constructor

        // TODO: Use an interface for view
        public TestCentricPresenter(IMainView view, ITestModel model, CommandLineOptions options)
        {
            _view = view;
            _model = model;
            _options = options;

            _settings = _model.Settings;
            _recentFiles = _model.RecentFiles;
            _runtimeSelectionController = new RuntimeSelectionController(_view.RuntimeMenu, _model);

            _view.Font = _settings.Gui.Font;
            _view.ResultTabs.SelectedIndex = _settings.Gui.SelectedTab;

            UpdateViewCommands();

            foreach (string format in _model.ResultFormats)
                if (format != "cases" && format != "user")
                    _resultFormats.Add(format);

            WireUpEvents();
        }

        #endregion

        #region Event Handling

        private void WireUpEvents()
        {
            #region Model Events

            _model.Events.TestsLoading += (TestFilesLoadingEventArgs e) =>
            {
                UpdateViewCommands(testLoading: true);

                var message = e.TestFilesLoading.Count == 1 ?
                    $"Loading Assembly: {e.TestFilesLoading[0]}" :
                    $"Loading {e.TestFilesLoading.Count} Assemblies...";

                ShowLongRunningOperation(message);
            };

            _model.Events.TestLoaded += (TestNodeEventArgs e) =>
            {
                HideLongRunningOperation();

                UpdateViewCommands();

                var files = _model.TestFiles;
                if (files.Count == 1)
                    _view.SetTitleBar(files.First());
            };

            _model.Events.TestsUnloading += (TestEventArgse) =>
            {
                UpdateViewCommands();
            };

            _model.Events.TestUnloaded += (TestEventArgs e) =>
            {
                _view.RunSummary.Text = null;

                UpdateViewCommands();
            };

            _model.Events.TestsReloading += (TestEventArgs e) =>
            {
                UpdateViewCommands();

                ShowLongRunningOperation("Reloading...");
            };

            _model.Events.TestReloaded += (TestNodeEventArgs e) =>
            {
                HideLongRunningOperation();

                //SetTitleBar(TestProject.Name);

                if (_settings.Gui.ClearResultsOnReload)
                    _view.RunSummary.Text = null;

                UpdateViewCommands();
            };

            _model.Events.TestLoadFailure += (TestLoadFailureEventArgs e) =>
            {
                HideLongRunningOperation();

                _view.MessageDisplay.Error(e.Exception.Message);
            };

            _model.Events.RunStarting += (RunStartingEventArgs e) =>
            {
                UpdateViewCommands();

                _view.RunSummary.Text = null;
            };

            _model.Events.RunFinished += (TestResultEventArgs e) =>
            {
                HideLongRunningOperation();

                UpdateViewCommands();

                ResultSummary summary = ResultSummaryCreator.FromResultNode(e.Result);
                _view.RunSummary.Text = string.Format(
                    "Passed: {0}   Failed: {1}   Errors: {2}   Inconclusive: {3}   Invalid: {4}   Ignored: {5}   Skipped: {6}   Time: {7}",
                    summary.PassCount, summary.FailedCount, summary.ErrorCount, summary.InconclusiveCount, summary.InvalidCount, summary.IgnoreCount, summary.SkipCount, summary.Duration);

                if (summary.RunCount == 0)
                {

                }

                //string resultPath = Path.Combine(TestProject.BasePath, "TestResult.xml");
                // TODO: Use Work Directory
                string resultPath = "TestResult.xml";
                _model.SaveResults(resultPath);
                //try
                //{
                //    _model.SaveResults(resultPath);
                //    //log.Debug("Saved result to {0}", resultPath);
                //}
                //catch (Exception ex)
                //{
                //    //log.Warning("Unable to save result to {0}\n{1}", resultPath, ex.ToString());
                //}

                //if (e.Result.Outcome.Status == TestStatus.Failed)
                //    _view.Activate();
                
                // If we were running unattended, it's time to close
                if (_options.Unattended)
                    _view.Close();
            };

            _settings.Changed += (s, e) =>
            {
                if (e.SettingName == "Gui.Options.DisplayFormat")
                    InitializeDisplay();
            };

            _model.Events.UnhandledException += (UnhandledExceptionEventArgs e) =>
            {
                var display = new MessageDisplay("TestCentric - Internal Error");
                display.Error($"{e.Message}\n\n{e.StackTrace}");
            };

            #endregion

            #region View Events

            _view.Load += (s, e) =>
            {
                InitializeDisplay(_settings.Gui.DisplayFormat);

                var settings = _model.PackageOverrides;
                if (_options.ProcessModel != null)
                    _view.ProcessModel.SelectedItem = _options.ProcessModel;
                if (_options.DomainUsage != null)
                    _view.DomainUsage.SelectedItem = _options.DomainUsage;
                if (_options.MaxAgents >= 0)
                    _model.Settings.Engine.Agents = _options.MaxAgents;
                _view.RunAsX86.Checked = _options.RunAsX86;
            };

            _view.Shown += (s, e) =>
            {
                Application.DoEvents();

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
                    RunAllTests();
                // Currently, --unattended without --run does nothing except exit.
                else if (_options.Unattended)
                    _view.Close();
            };

            _view.SplitterPositionChanged += (s, e) =>
            {
                _settings.Gui.MainForm.SplitPosition = _view.SplitterPosition;
            };

            _view.FormClosing += (s, e) =>
            {
                if (_model.IsPackageLoaded)
                {
                    if (_model.IsTestRunning)
                    {
                        DialogResult dialogResult = _view.MessageDisplay.Ask(
                            "A test is running, do you want to forcibly stop the test and exit?");

                        if (dialogResult == DialogResult.No)
                        {
                            e.Cancel = true;
                            return;
                        }

                        _model.StopTestRun(true);
                    }

                    if (CloseProject() == DialogResult.Cancel)
                        e.Cancel = true;
                }

                if (!e.Cancel)
                {
                    if (_settings.Gui.DisplayFormat == "Mini")
                    {
                        _settings.Gui.MiniForm.Location = _view.Location;
                        _settings.Gui.MiniForm.Size = _view.Size;
                    }
                    else
                    {
                        _settings.Gui.MainForm.Location = _view.Location;
                        _settings.Gui.MainForm.Size = _view.Size;
                    }
                }
            };

            _view.RunButton.Execute += () => RunSelectedTests();
            _view.StopButton.Execute += () => StopTests();

            _view.FileMenu.Popup += () =>
            {
                bool isPackageLoaded = _model.IsPackageLoaded;
                bool isTestRunning = _model.IsTestRunning;

                _view.OpenCommand.Enabled = !isTestRunning;
                _view.CloseCommand.Enabled = isPackageLoaded && !isTestRunning;

                _view.ReloadTestsCommand.Enabled = isPackageLoaded && !isTestRunning;

                _view.RuntimeMenu.Visible = _model.AvailableRuntimes.Count > 1;

                _view.RecentFilesMenu.Enabled = !isTestRunning;

                //if (!isTestRunning)
                //{
                //    _recentProjectsMenuHandler.Load();
                //}
            };

            _view.RuntimeMenu.Popup += () => _runtimeSelectionController.PopulateMenu();

            _view.OpenCommand.Execute += () => OpenProject();
            _view.CloseCommand.Execute += () => CloseProject();
            _view.AddTestFilesCommand.Execute += () => AddTestFiles();
            _view.ReloadTestsCommand.Execute += () => ReloadTests();

            _view.ProcessModel.SelectionChanged += () =>
            {
                ChangePackageSettingAndReload(EnginePackageSettings.ProcessModel, _view.ProcessModel.SelectedItem);
            };

            _view.DomainUsage.SelectionChanged += () =>
            {
                ChangePackageSettingAndReload(EnginePackageSettings.DomainUsage, _view.DomainUsage.SelectedItem);
            };

            _view.RunAsX86.CheckedChanged += () =>
            {
                var key = EnginePackageSettings.RunAsX86;
                if (_view.RunAsX86.Checked)
                    ChangePackageSettingAndReload(key, true);
                else
                    ChangePackageSettingAndReload(key, null);
            };

            _view.RecentFilesMenu.Popup += () =>
            {
                var menuItems = _view.RecentFilesMenu.MenuItems;
                // Test for null, in case we are running tests with a mock
                if (menuItems == null)
                    return;

                menuItems.Clear();
                int num = 0;
                foreach (string entry in _model.RecentFiles.Entries)
                {
                    var menuText = string.Format("{0} {1}", ++num, entry);
                    var menuItem = new MenuItem(menuText);
                    menuItem.Click += (sender, ea) =>
                    {
                        // HACK: We are loading new files, cancel any runtime override
                        _model.PackageOverrides.Remove(EnginePackageSettings.RequestedRuntimeFramework);
                        string path = ((MenuItem)sender).Text.Substring(2);
                        _model.LoadTests(new[] { path });
                    };
                    menuItems.Add(menuItem);
                    if (num >= _settings.Gui.RecentProjects.MaxFiles) break;
                }
            };

            _view.ExitCommand.Execute += () => _view.Close();

            _view.DisplayFormat.SelectionChanged += () =>
            {
                _settings.Gui.DisplayFormat = _view.DisplayFormat.SelectedItem;
                InitializeDisplay(_view.DisplayFormat.SelectedItem);
            };

            _view.IncreaseFontCommand.Execute += () =>
            {
                applyFont(IncreaseFont(_settings.Gui.Font));
            };

            _view.DecreaseFontCommand.Execute += () =>
            {
                applyFont(DecreaseFont(_settings.Gui.Font));
            };

            _view.ChangeFontCommand.Execute += () =>
            {
                Font currentFont = _settings.Gui.Font;
                Font newFont = _view.DialogManager.SelectFont(currentFont);
                if (newFont != _settings.Gui.Font)
                    applyFont(newFont);
            };

            _view.DialogManager.ApplyFont += (font) => applyFont(font);

            _view.RestoreFontCommand.Execute += () =>
            {
                applyFont(Form.DefaultFont);
            };

            _view.IncreaseFixedFontCommand.Execute += () =>
            {
                _settings.Gui.FixedFont = IncreaseFont(_settings.Gui.FixedFont);
            };

            _view.DecreaseFixedFontCommand.Execute += () =>
            {
                _settings.Gui.FixedFont = DecreaseFont(_settings.Gui.FixedFont);
            };

            _view.RestoreFixedFontCommand.Execute += () =>
            {
                _settings.Gui.FixedFont = new Font(FontFamily.GenericMonospace, 8.0f);
            };

            _view.StatusBarCommand.CheckedChanged += () =>
            {
                _view.StatusBarView.Visible = _view.StatusBarCommand.Checked;
            };

            _view.RunAllCommand.Execute += () => RunAllTests();
            _view.RunSelectedCommand.Execute += () => RunSelectedTests();
            _view.RunFailedCommand.Execute += () => RunFailedTests();

            _view.StopRunCommand.Execute += () => StopTests();

            _view.TestParametersCommand.Execute += () =>
            {
                using (var dlg = new TestParametersDialog())
                {
                    dlg.Font = _settings.Gui.Font;
                    dlg.StartPosition = FormStartPosition.CenterParent;

                    if (_model.PackageOverrides.ContainsKey("TestParametersDictionary"))
                    {
                        var testParms = _model.PackageOverrides["TestParametersDictionary"] as IDictionary<string, string>;
                        foreach (string key in testParms.Keys)
                            dlg.Parameters.Add(key, testParms[key]);
                    }

                    if (dlg.ShowDialog(_view as IWin32Window) == DialogResult.OK)
                    {
                        ChangePackageSettingAndReload("TestParametersDictionary", dlg.Parameters);
                    }
                }
            };

            _view.ToolsMenu.Popup += () =>
            {
                _view.SaveResultsAsMenu.MenuItems.Clear();

                foreach (string format in _resultFormats)
                {
                    MenuItem formatItem = new MenuItem(format);
                    formatItem.Click += (s, e) => SaveResults(format);
                    _view.SaveResultsAsMenu.MenuItems.Add(formatItem);
                }
            };

            _view.SaveResultsCommand.Execute += () => SaveResults();

            _view.OpenWorkDirectoryCommand.Execute += () => System.Diagnostics.Process.Start(_model.WorkDirectory);

            _view.ExtensionsCommand.Execute += () =>
            {
                using (var extensionsDialog = new ExtensionDialog(_model.Services.ExtensionService))
                {
                    extensionsDialog.Font = _settings.Gui.Font;
                    extensionsDialog.ShowDialog();
                }
            };

            _view.SettingsCommand.Execute += () =>
            {
                SettingsDialog.Display(this, _model);
            };

            _view.TestCentricHelpCommand.Execute += () =>
            {
                _view.MessageDisplay.Error("Not Yet Implemented");

            };

            _view.NUnitHelpCommand.Execute += () =>
            {
                System.Diagnostics.Process.Start("https://github.com/nunit/docs/wiki/NUnit-Documentation");
            };

            _view.AboutCommand.Execute += () =>
            {
                using (AboutBox aboutBox = new AboutBox())
                {
                    aboutBox.ShowDialog();
                }
            };

            _view.ResultTabs.SelectionChanged += () =>
            {
                _settings.Gui.SelectedTab = _view.ResultTabs.SelectedIndex;
            };

            #endregion
        }

        #endregion

        #region Public Methods

        #region Open Methods

        private void OpenProject()
        {
            var files = _view.DialogManager.SelectMultipleFiles("Open Project", CreateOpenFileFilter());
            if (files.Count > 0)
            {
                // HACK: We are loading new files, cancel any runtime override
                _model.PackageOverrides.Remove(EnginePackageSettings.RequestedRuntimeFramework);
                LoadTests(files);
            }
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

        public void AddTestFiles()
        {
            var filesToAdd = _view.DialogManager.SelectMultipleFiles("Add Test Files", CreateOpenFileFilter());

            if (filesToAdd.Count > 0)
            {
                var files = new List<string>(_model.TestFiles);
                files.AddRange(filesToAdd);

                _model.LoadTests(files);
            }
        }

        #endregion

        #region Save Methods

        public void SaveResults(string format = "nunit3")
        {
            string savePath = _view.DialogManager.GetFileSavePath($"Save Results in {format} format", "XML Files (*.xml)|*.xml|All Files (*.*)|*.*", _model.WorkDirectory, "TestResult.xml");

            if (savePath != null)
            {
                try
                {
                    _model.SaveResults(savePath, format);

                    _view.MessageDisplay.Info(String.Format($"Results saved in {format} format as {savePath}"));
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
            _model.ClearResults();
            _model.RunAllTests();
        }

        public void RunSelectedTests()
        {
            RunTests(_view.TreeView.SelectedTests);
        }

        public void RunFailedTests()
        {
            RunTests(_view.TreeView.FailedTests);
        }

        public void RunTests(TestNode test)
        {
            RunTests(new TestNode[] { test });
        }

        public void RunTests(TestNode[] tests)
        {
            if (_settings.Engine.ReloadOnRun)
            {
                _model.ClearResults();
                _model.ReloadTests();
            }

            if (tests != null && tests.Length > 0)
                _model.RunTests(new TestSelection(tests));
        }

        internal void StopTests()
        {
            const int INITIAL_WAIT_TIME = 15000;
            const int MESSAGE_WAIT_TIME = 30000;

            var runComplete = new System.Threading.AutoResetEvent(false);
            _model.Events.RunFinished += (e) => runComplete.Set();
            bool forced = false;

            ShowLongRunningOperation("Waiting for all running tests to complete.");
            _model.StopTestRun(false);
            runComplete.WaitOne(INITIAL_WAIT_TIME);

            while (_model.IsTestRunning)
            {
                DialogResult dialogResult = _view.MessageDisplay.Ask(
                    "One or more tests are still running. Do you want to force cancellation? Enter 'Yes' to forcibly cancel the run, 'No' to keep waiting.");

                if (dialogResult == DialogResult.Yes)
                    break;
                else
                    runComplete.WaitOne(MESSAGE_WAIT_TIME);
            }

            if (_model.IsTestRunning)
                _model.StopTestRun(true);
        }

        #endregion

        #endregion

        #region Helper Methods

        private void UpdateViewCommands(bool testLoading = false)
        {
            bool testLoaded = _model.HasTests;
            bool testRunning = _model.IsTestRunning;

            _view.RunButton.Enabled = testLoaded && !testRunning;
            _view.RunAllCommand.Enabled = testLoaded && !testRunning;
            _view.RunSelectedCommand.Enabled = testLoaded && !testRunning;
            _view.RunFailedCommand.Enabled = testLoaded && !testRunning && _model.HasResults;
            _view.TestParametersCommand.Enabled = testLoaded && !testRunning;

            _view.StopButton.Enabled = testRunning;
            _view.StopRunCommand.Enabled = testRunning;

            _view.OpenCommand.Enabled = !testRunning && !testLoading;
            _view.CloseCommand.Enabled = testLoaded && !testRunning;
            _view.AddTestFilesCommand.Enabled = testLoaded && !testRunning;
            _view.ReloadTestsCommand.Enabled = testLoaded && !testRunning;
            _view.RuntimeMenu.Enabled = testLoaded && !testRunning && !testLoading && _runtimeSelectionController.AllowRuntimeSelection();
            _view.RecentFilesMenu.Enabled = !testRunning && !testLoading;
            _view.ExitCommand.Enabled = !testLoading;
            _view.SaveResultsCommand.Enabled = _view.SaveResultsAsMenu.Enabled = !testRunning && !testLoading && _model.HasResults;
        }

        private string CreateOpenFileFilter()
        {
            StringBuilder sb = new StringBuilder();
            bool nunit = _model.NUnitProjectSupport;
            bool vs = _model.VisualStudioSupport;

            if (nunit && vs)
                sb.Append("Projects & Assemblies (*.nunit,*.csproj,*.fsproj,*.vbproj,*.vjsproj,*.vcproj,*.sln,*.dll,*.exe)|*.nunit;*.csproj;*.fsproj;*.vbproj;*.vjsproj;*.vcproj;*.sln;*.dll;*.exe|");
            else if (nunit)
                sb.Append("Projects & Assemblies (*.nunit,*.dll,*.exe)|*.nunit;*.dll;*.exe|");
            else if (vs)
                sb.Append("Projects & Assemblies (*.csproj,*.fsproj,*.vbproj,*.vjsproj,*.vcproj,*.sln,*.dll,*.exe)|*.csproj;*.fsproj;*.vbproj;*.vjsproj;*.vcproj;*.sln;*.dll;*.exe|");

            if (nunit)
                sb.Append("NUnit Projects (*.nunit)|*.nunit|");

            if (vs)
                sb.Append("Visual Studio Projects (*.csproj,*.fsproj,*.vbproj,*.vjsproj,*.vcproj,*.sln)|*.csproj;*.fsproj;*.vbproj;*.vjsproj;*.vcproj;*.sln|");

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

        private void ChangePackageSettingAndReload(string key, object setting)
        {
            if (setting == null || setting as string == "DEFAULT")
                _model.PackageOverrides.Remove(key);
            else
                _model.PackageOverrides[key] = setting;

            // Even though the _model has a Reload method, we cannot use it because Reload
            // does not re-create the Engine.  Since we just changed a setting, we must
            // re-create the Engine by unloading/reloading the tests. We make a copy of
            // __model.TestFiles because the method does an unload before it loads.
            LoadTests(new List<string>(_model.TestFiles));
        }

        private void applyFont(Font font)
        {
            _settings.Gui.Font = _view.Font = font;
        }

        private void InitializeDisplay()
        {
            InitializeDisplay(_settings.Gui.DisplayFormat);
        }

        private void InitializeDisplay(string displayFormat)
        {
            _view.DisplayFormat.SelectedItem = displayFormat;

            Point location;
            Size size;
            bool isMaximized = false;
            bool useFullGui = displayFormat != "Mini";

            _view.Configure(useFullGui);

            if (useFullGui)
            {
                location = _settings.Gui.MainForm.Location;
                size = _settings.Gui.MainForm.Size;
                isMaximized = _settings.Gui.MainForm.Maximized;
            }
            else
            {
                location = _settings.Gui.MiniForm.Location;
                size = _settings.Gui.MiniForm.Size;
                isMaximized = _settings.Gui.MiniForm.Maximized;
            }

            if (!IsValidLocation(location, size))
                location = new Point(10, 10);

            _view.Location = location;
            _view.Size = size;
            _view.Maximized = isMaximized;

            if (useFullGui)
                _view.SplitterPosition = _settings.Gui.MainForm.SplitPosition;
        }

        private static bool IsValidLocation(Point location, Size size)
        {
            Rectangle myArea = new Rectangle(location, size);
            bool intersect = false;
            foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens)
            {
                intersect |= myArea.IntersectsWith(screen.WorkingArea);
            }
            return intersect;
        }

        private static Font IncreaseFont(Font font)
        {
            return new Font(font.FontFamily, font.SizeInPoints * 1.2f, font.Style);
        }

        private static Font DecreaseFont(Font font)
        {
            return new Font(font.FontFamily, font.SizeInPoints / 1.2f, font.Style);
        }

        private void ShowLongRunningOperation(string msg)
        {
            _longOpDisplay = _view.LongOperationDisplay(msg);
        }

        private void HideLongRunningOperation()
        {
            _longOpDisplay?.Dispose();
            _longOpDisplay = null;
        }

        #endregion
    }
}
