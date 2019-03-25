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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NUnit.Engine;

namespace TestCentric.Gui.Presenters
{
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
    /// 2. The presenter creates dialogs itself, which
    /// limits testability.
    /// </summary>
    public class TestCentricPresenter
    {
        #region Instance Variables

        private readonly IMainView _view;

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
        public TestCentricPresenter(IMainView view, ITestModel model, CommandLineOptions options)
        {
            _view = view;
            _model = model;
            _options = options;

            _settings = _model.Services.UserSettings;
            _recentFiles = _model.Services.RecentFiles;

            _view.Font = _settings.Gui.Font;
            _view.ResultTabs.SelectedIndex = _settings.Gui.SelectedTab;

            UpdateViewCommands();

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

                _longOpDisplay = _view.LongOperationDisplay(message);
            };

            _model.Events.TestLoaded += (TestNodeEventArgs e) =>
            {
                if (_longOpDisplay != null)
                {
                    _longOpDisplay.Dispose();
                    _longOpDisplay = null;
                }

                UpdateViewCommands();
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

                _longOpDisplay = _view.LongOperationDisplay("Reloading...");
            };

            _model.Events.TestReloaded += (TestNodeEventArgs e) =>
            {
                if (_longOpDisplay != null)
                {
                    _longOpDisplay.Dispose();
                    _longOpDisplay = null;
                }

                //SetTitleBar(TestProject.Name);

                if (_settings.Gui.ClearResultsOnReload)
                    _view.RunSummary.Text = null;

                UpdateViewCommands();
            };

            _model.Events.RunStarting += (RunStartingEventArgs e) =>
            {
                UpdateViewCommands();
            };

            _model.Events.RunFinished += (TestResultEventArgs e) =>
            {
                UpdateViewCommands();

                ResultSummary summary = ResultSummaryCreator.FromResultNode(e.Result);
                _view.RunSummary.Text = string.Format(
                    "Passed: {0}   Failed: {1}   Errors: {2}   Inconclusive: {3}   Invalid: {4}   Ignored: {5}   Skipped: {6}   Time: {7}",
                    summary.PassCount, summary.FailedCount, summary.ErrorCount, summary.InconclusiveCount, summary.InvalidCount, summary.IgnoreCount, summary.SkipCount, summary.Duration);

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

                if (e.Result.Outcome.Status == TestStatus.Failed)
                    _view.Activate();
            };

            _settings.Changed += (s, e) =>
            {
                if (e.SettingName == "Gui.Options.DisplayFormat")
                    InitializeDisplay();
            };

            #endregion

            #region View Events

            _view.Load += (s, e) =>
            {
                InitializeDisplay(_settings.Gui.DisplayFormat);
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
                {
                    // TODO: Temporary fix to avoid problem when /run is used 
                    // with ReloadOnRun turned on. Refactor TestModel so
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
            };

            _view.Move += (s, e) =>
            {
                if (!_view.Maximized)
                {
                    var location = _view.Location;

                    switch (_view.DisplayFormat.SelectedItem)
                    {
                        case "Full":
                        default:
                            _settings.Gui.MainForm.Left = location.X;
                            _settings.Gui.MainForm.Top = location.Y;
                            _settings.Gui.MainForm.Maximized = false;
                            break;
                        case "Mini":
                            _settings.Gui.MiniForm.Left = location.X;
                            _settings.Gui.MiniForm.Top = location.Y;
                            _settings.Gui.MiniForm.Maximized = false;
                            break;
                    }
                }
            };

            _view.Resize += (s, e) =>
            {
                if (!_view.Maximized)
                {
                    var size = _view.Size;

                    if (_view.DisplayFormat.SelectedItem == "Full")
                    {
                        _settings.Gui.MainForm.Width = size.Width;
                        _settings.Gui.MainForm.Height = size.Height;
                    }
                    else
                    {
                        _settings.Gui.MiniForm.Width = size.Width;
                        _settings.Gui.MiniForm.Height = size.Height;
                    }
                }
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

            _view.FileMenu.Popup += () =>
            {
                bool isPackageLoaded = _model.IsPackageLoaded;
                bool isTestRunning = _model.IsTestRunning;

                _view.OpenCommand.Enabled = !isTestRunning;
                _view.CloseCommand.Enabled = isPackageLoaded && !isTestRunning;

                _view.ReloadTestsCommand.Enabled = isPackageLoaded && !isTestRunning;

                var frameworks = _model.AvailableRuntimes;
                var runtimeMenu = _view.RuntimeMenu;

                runtimeMenu.Visible = frameworks.Count > 1;

                if (runtimeMenu.Visible && runtimeMenu.Enabled && runtimeMenu.MenuItems.Count == 0)
                {
                    var defaultMenuItem = new MenuItem("Default");
                    defaultMenuItem.Name = "defaultMenuItem";
                    defaultMenuItem.Tag = "DEFAULT";
                    defaultMenuItem.Checked = true;

                    runtimeMenu.MenuItems.Add(defaultMenuItem);

                    // TODO: Disable selections that are not supported for the target?
                    foreach (IRuntimeFramework framework in frameworks)
                    {
                        MenuItem item = new MenuItem(framework.DisplayName);
                        item.Tag = framework.Id;
                        runtimeMenu.MenuItems.Add(item);
                    }

                    _view.SelectedRuntime.Refresh();
                }

                _view.RecentFilesMenu.Enabled = !isTestRunning;

                //if (!isTestRunning)
                //{
                //    _recentProjectsMenuHandler.Load();
                //}
            };

            _view.OpenCommand.Execute += () => OpenProject();
            _view.CloseCommand.Execute += () => CloseProject();
            _view.AddTestFilesCommand.Execute += () => AddTestFiles();
            _view.ReloadTestsCommand.Execute += () => ReloadTests();

            _view.SelectedRuntime.SelectionChanged += () =>
            {
                ChangePackageSetting(EnginePackageSettings.RuntimeFramework, _view.SelectedRuntime.SelectedItem);
            };

            _view.ProcessModel.SelectionChanged += () =>
            {
                ChangePackageSetting(EnginePackageSettings.ProcessModel, _view.ProcessModel.SelectedItem);
            };

            _view.DomainUsage.SelectionChanged += () =>
            {
                ChangePackageSetting(EnginePackageSettings.DomainUsage, _view.DomainUsage.SelectedItem);
            };

            _view.RunAsX86.CheckedChanged += () =>
            {
                var key = EnginePackageSettings.RunAsX86;
                if (_view.RunAsX86.Checked)
                    ChangePackageSetting(key, true);
                else
                    ChangePackageSetting(key, null);
            };

            _view.RecentFilesMenu.Popup += () =>
            {
                var menuItems = _view.RecentFilesMenu.MenuItems;
                // Test for null, in case we are running tests with a mock
                if (menuItems == null)
                    return;

                menuItems.Clear();
                int num = 0;
                foreach (string entry in _model.Services.RecentFiles.Entries)
                {
                    var menuText = string.Format("{0} {1}", ++num, entry);
                    var menuItem = new MenuItem(menuText);
                    menuItem.Click += (sender, ea) =>
                    {
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
            _view.StopRunCommand.Execute += () => CancelRun();

            _view.SaveResultsCommand.Execute += () => SaveResults();

            _view.OpenWorkDirectoryCommand.Execute += () => System.Diagnostics.Process.Start(".");

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
                LoadTests(files);
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
                var files = new List<string>();
                if (_model.TestFiles != null)
                    files.AddRange(_model.TestFiles);
                files.AddRange(filesToAdd);

                _model.LoadTests(files);
            }
        }

        #endregion

        #region Save Methods

        public void SaveResults()
        {
            string savePath = _view.DialogManager.GetFileSavePath("Save Results as XML", "XML Files (*.xml)|*.xml|All Files (*.*)|*.*");
            if (savePath != null)
            {
                try
                {
                    _model.SaveResults(savePath);

                    _view.MessageDisplay.Info(String.Format($"Results saved in nunit3 format as {savePath}"));
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
            if (_settings.Gui.ReloadOnRun)
                _model.ClearResults();

            if (tests != null && tests.Length > 0)
                _model.RunTests(new TestSelection(tests));
        }

        internal void CancelRun()
        {
            if (_model.IsTestRunning)
            {
                DialogResult dialogResult = _view.MessageDisplay.Ask(
                    "Do you want to cancel the running test?");

                if (dialogResult == DialogResult.Yes)
                    _model.CancelTestRun();
            }
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

            _view.StopButton.Enabled = testRunning;
            _view.StopRunCommand.Enabled = testRunning;

            _view.OpenCommand.Enabled = !testRunning && !testLoading;
            _view.CloseCommand.Enabled = testLoaded && !testRunning;
            _view.AddTestFilesCommand.Enabled = testLoaded && !testRunning;
            _view.ReloadTestsCommand.Enabled = testLoaded && !testRunning;
            _view.RuntimeMenu.Enabled = !testRunning && !testLoading;
            _view.RecentFilesMenu.Enabled = !testRunning && !testLoading;
            _view.ExitCommand.Enabled = !testLoading;
            _view.SaveResultsCommand.Enabled = !testRunning && !testLoading && _model.HasResults;
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

        private void ChangePackageSetting(string key, object setting)
        {
            if (setting == null || setting as string == "DEFAULT")
                _model.PackageSettings.Remove(key);
            else
                _model.PackageSettings[key] = setting;

            string message = string.Format("New {0} setting will not take effect until you reload.\r\n\r\n\t\tReload Now?", key);

            if (_view.MessageDisplay.Ask(message) == DialogResult.Yes)
            {
                // Even though the _model has a Reload method, we cannot use it because Reload
                // does not re-create the Engine.  Since we just changed a setting, we must
                // re-create the Engine by unloading/reloading the tests.
                List<string> listCopy = _model.TestFiles.ToList();
                LoadTests(listCopy);    // This also does an Unload first.
            }
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

            int x = 0, y = 0, width = 0, height = 0;
            bool isMaximized = false;
            bool useFullGui = displayFormat != "Mini";

            _view.Configure(useFullGui);

            if (useFullGui)
            {
                x = _settings.Gui.MainForm.Left;
                y = _settings.Gui.MainForm.Top;
                width = _settings.Gui.MainForm.Width;
                height = _settings.Gui.MainForm.Height;
                isMaximized = _settings.Gui.MainForm.Maximized;
            }
            else
            {
                x = _settings.Gui.MiniForm.Left;
                y = _settings.Gui.MiniForm.Top;
                width = _settings.Gui.MiniForm.Width;
                height = _settings.Gui.MiniForm.Height;
                isMaximized = _settings.Gui.MiniForm.Maximized;
            }

            Point location = new Point(x, y);
            Size size = new Size(Math.Max(width, 160), Math.Max(height, 32));

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

        #endregion
    }
}
