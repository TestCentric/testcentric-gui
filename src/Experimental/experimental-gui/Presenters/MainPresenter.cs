// ***********************************************************************
// Copyright (c) 2016 Charlie Poole
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
using System.Text;
using System.Windows.Forms;
using NUnit.Engine;
using TestCentric.Gui.Model.Settings;

namespace TestCentric.Gui.Presenters
{
    using Model;
    using Views;
    using Views.AddinPages;

    public class MainPresenter : System.IDisposable
    {
        IMainView _view;
        ITestModel _model;
        UserSettings _settings;
        CommandLineOptions _options;

        private Dictionary<string, TreeNode> _nodeIndex = new Dictionary<string, TreeNode>();

        #region Construction and Initialization

        public MainPresenter(IMainView view, ITestModel model, CommandLineOptions options)
        {
            _view = view;
            _model = model;
            _settings = _model.Services.UserSettings;
            _options = options;

            InitializeMainMenu();

            WireUpEvents();
        }

        private void WireUpEvents()
        {
            // Model Events
            _model.Events.TestsLoading += NotifyTestsLoading;
            _model.Events.TestLoaded += (ea) => InitializeMainMenu();
            _model.Events.TestUnloaded += (ea) => InitializeMainMenu();
            _model.Events.TestsReloading += NotifyTestsReloading;

            _model.Events.TestReloaded += (ea) =>
            {
                InitializeMainMenu();
                _view.OnTestAssembliesLoaded();
            };

            _model.Events.TestChanged += (ea) =>
            {
                if (_model.Services.UserSettings.Engine.ReloadOnChange)
                    _model.ReloadTests();
            };

            _model.Events.RunStarting += (ea) => InitializeMainMenu();
            _model.Events.RunFinished += (ea) => InitializeMainMenu();

            // View Events
            _view.Load += MainForm_Load;
            _view.MainViewClosing += MainForm_Closing;
            _view.DragDropFiles += MainForm_DragDrop;

            _view.NewProjectCommand.Execute += ProjectSaveNotYetImplemented; // _model.NewProject;
            _view.OpenProjectCommand.Execute += OnOpenProjectCommand;
            _view.CloseCommand.Execute += _model.UnloadTests;
            _view.SaveCommand.Execute += ProjectSaveNotYetImplemented; // _model.SaveProject;
            _view.SaveAsCommand.Execute += ProjectSaveNotYetImplemented; // _model.SaveProject;
            _view.SaveResultsCommand.Execute += () => SaveResults();
            _view.ReloadTestsCommand.Execute += _model.ReloadTests;
            _view.RecentProjectsMenu.Popup += PopulateRecentProjectsMenu;
            _view.SelectedRuntime.SelectionChanged += SelectedRuntime_SelectionChanged;
            _view.ProcessModel.SelectionChanged += ProcessModel_SelectionChanged;
            _view.DomainUsage.SelectionChanged += DomainUsage_SelectionChanged;
            _view.RunAsX86.CheckedChanged += LoadAsX86_CheckedChanged;
            _view.ExitCommand.Execute += () => Application.Exit();

            _view.IncreaseFontCommand.Execute += () =>
            {
                ApplyFont(IncreaseFont(_settings.Gui.Font));
            };

            _view.DecreaseFontCommand.Execute += () =>
            {
                ApplyFont(DecreaseFont(_settings.Gui.Font));
            };

            _view.ChangeFontCommand.Execute += () =>
            {
                Font currentFont = _settings.Gui.Font;
                Font newFont = _view.DialogManager.SelectFont(currentFont);
                if (newFont != _settings.Gui.Font)
                    ApplyFont(newFont);
            };

            _view.RestoreFontCommand.Execute += () =>
            {
                ApplyFont(Form.DefaultFont);
            };

            _view.SettingsCommand.Execute += () =>
            {
                using (var dialog = new SettingsDialog((Form)_view, _settings))
                {
                    dialog.ShowDialog();
                }
            };

            _view.ExtensionsCommand.Execute += () =>
            {
                using (var extensionsDialog = new ExtensionDialog(_model.Services.ExtensionService))
                {
                    extensionsDialog.Font = _settings.Gui.Font;
                    extensionsDialog.ShowDialog();
                }
            };

            _view.NUnitHelpCommand.Execute += () =>
                { MessageBox.Show("This will show Help", "Not Yet Implemented"); };
            _view.AboutNUnitCommand.Execute += () =>
                { MessageBox.Show("This will show the About Box", "Not Yet Implemented"); };

            _view.MainViewClosing += () => _model.Dispose();
        }

        private void NotifyTestsLoading(TestFilesLoadingEventArgs args)
        {
            var message = args.TestFilesLoading.Count == 1 ?
                $"Loading Assembly: {args.TestFilesLoading[0]}" :
                $"Loading {args.TestFilesLoading.Count} Assemblies...";
            _view.OnTestAssembliesLoading(message);
        }

        private void NotifyTestsReloading(TestEventArgs args)
        {
            _view.OnTestAssembliesLoading("Reloading Tests...");
        }

        private void MainForm_DragDrop(string[] files)
        {
            _model.LoadTests(files);
        }

        private void ProjectSaveNotYetImplemented()
        {
            _view.MessageDisplay.Error(TestModel.PROJECT_SAVE_MESSAGE);
        }

        #endregion

        #region Handlers for Model Events

        private void InitializeMainMenu()
        {
            _view.OnTestAssembliesLoaded();

            bool isTestRunning = _model.IsTestRunning;
            bool canCloseOrSave = _model.HasTests && !isTestRunning;

            // File Menu
            _view.NewProjectCommand.Enabled = !isTestRunning;
            _view.OpenProjectCommand.Enabled = !isTestRunning;
            _view.CloseCommand.Enabled = canCloseOrSave;
            _view.SaveCommand.Enabled = canCloseOrSave;
            _view.SaveAsCommand.Enabled = canCloseOrSave;
            _view.SaveResultsCommand.Enabled = canCloseOrSave && _model.HasResults;
            _view.ReloadTestsCommand.Enabled = canCloseOrSave;
            _view.RecentProjectsMenu.Enabled = !isTestRunning;
            _view.ExitCommand.Enabled = true;

            PopulateRecentProjectsMenu();

            PopulateSelectedRuntimeMenu();

            // Project Menu
            _view.ProjectMenu.Enabled = _view.ProjectMenu.Visible = _model.HasTests;
        }

        #endregion

        #region Handlers for View Events

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            var location = _settings.Gui.MainForm.Location;
            var size = _settings.Gui.MainForm.Size;
            if (size == Size.Empty)
                size = _view.Size;

            if (size.Width < 160) size.Width = 160;
            if (size.Height < 32) size.Height = 32;

            if (!IsVisiblePosition(location, size))
                location = new Point(0, 0);

            _view.Location = location;
            _view.Size = size;

            // Set to maximized if required
            if (_settings.Gui.MainForm.Maximized)
                _view.IsMaximized = true;

            // Set the font to use
            _view.Font = _settings.Gui.Font;

            var settings = _model.PackageOverrides;
            if (_options.InternalTraceLevel != null)
                settings.Add(EnginePackageSettings.InternalTraceLevel, _options.InternalTraceLevel);

            if (_options.ProcessModel != null)
                _view.ProcessModel.SelectedItem = _options.ProcessModel;
            if (_options.DomainUsage != null)
                _view.DomainUsage.SelectedItem = _options.DomainUsage;
            if (_options.MaxAgents >= 0)
                _model.Services.UserSettings.Engine.Agents = _options.MaxAgents;
            _view.RunAsX86.Checked = _options.RunAsX86;

            if (_options.InputFiles.Count > 0)
            {
                _model.LoadTests(_options.InputFiles);
            }
            else if (!_options.NoLoad && _model.Services.RecentFiles.Entries.Count > 0)
            {
                var entry = _model.Services.RecentFiles.Entries[0];
                if (!string.IsNullOrEmpty(entry) && System.IO.File.Exists(entry))
                    _model.LoadTests(new[] { entry });
            }

            if (_options.RunAllTests && _model.IsPackageLoaded)
                _model.RunAllTests();
        }

        private void MainForm_Closing()
        {
            var isMaximized = _settings.Gui.MainForm.Maximized = _view.IsMaximized;

            if (!isMaximized)
            {
                _settings.Gui.MainForm.Location = _view.Location;
                _settings.Gui.MainForm.Size = _view.Size;
            }
        }

        private void SelectedRuntime_SelectionChanged()
        {
            OverridePackageSetting(EnginePackageSettings.RuntimeFramework, _view.SelectedRuntime.SelectedItem);
        }

        private void ProcessModel_SelectionChanged()
        {
            OverridePackageSetting(EnginePackageSettings.ProcessModel, _view.ProcessModel.SelectedItem);
        }

        private void DomainUsage_SelectionChanged()
        {
            OverridePackageSetting(EnginePackageSettings.DomainUsage, _view.DomainUsage.SelectedItem);
        }

        private void LoadAsX86_CheckedChanged()
        {
            var key = EnginePackageSettings.RunAsX86;
            if (_view.RunAsX86.Checked)
                OverridePackageSetting(key, true);
            else
                OverridePackageSetting(key, null);
        }

        private void OverridePackageSetting(string key, object setting)
        {
            if (setting == null || setting as string == "DEFAULT")
                _model.PackageOverrides.Remove(key);
            else
                _model.PackageOverrides[key] = setting;

            string message = string.Format("New {0} setting will not take effect until you reload.\r\n\r\n\t\tReload Now?", key);

            if (_view.MessageDisplay.Ask(message) == DialogResult.Yes)
                _model.ReloadTests();
        }

        private void ApplyFont(Font font)
        {
            _settings.Gui.Font = _view.Font = font;
        }

        private static Font IncreaseFont(Font font)
        {
            return new Font(font.FontFamily, font.SizeInPoints * 1.2f, font.Style);
        }

        private static Font DecreaseFont(Font font)
        {
            return new Font(font.FontFamily, font.SizeInPoints / 1.2f, font.Style);
        }

        #region Command Handlers

        private void OnOpenProjectCommand()
        {
            var files = _view.DialogManager.SelectMultipleFiles("Open Project", CreateOpenFileFilter());
            if (files.Count > 0)
                _model.LoadTests(files);
        }

        public void SaveResults()
        {
            string savePath = _view.DialogManager.GetFileSavePath("Save Results as XML", "XML Files (*.xml)|*.xml|All Files (*.*)|*.*", _model.WorkDirectory, "TestResult.xml");

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

        #region Menu Popup Handlers

        private void PopulateSelectedRuntimeMenu()
        {
            if (_view.SelectRuntimeMenu.MenuItems != null && _view.SelectRuntimeMenu.MenuItems.Count == 1)
            {
                foreach (var runtime in _model.AvailableRuntimes)
                {
                    var text = runtime.DisplayName;
                    // Don't use Full suffix, but keep Client if present
                    if (text.EndsWith(" - Full"))
                        text = text.Substring(0, text.Length - 7);
                    var menuItem = new ToolStripMenuItem(text) { Tag = runtime.ToString() };
                    _view.SelectRuntimeMenu.MenuItems.Add(menuItem);
                }

                _view.SelectedRuntime.Refresh();
            }
        }

        private void PopulateRecentProjectsMenu()
        {
            if (_view.RecentProjectsMenu.MenuItems != null) // Null when mocked
            {
                _view.RecentProjectsMenu.MenuItems.Clear();

                int num = 0;
                foreach (string entry in _model.Services.RecentFiles.Entries)
                {
                    var menuText = string.Format("{0} {1}", ++num, entry);
                    var menuItem = new ToolStripMenuItem(menuText);
                    menuItem.Click += (s, e) => _model.LoadTests(new[] { entry });
                    _view.RecentProjectsMenu.MenuItems.Add(menuItem);
                }
            }
        }

        #endregion

        #endregion


        // Ensure that the proposed window position intersects
        // at least one screen area.
        private static bool IsVisiblePosition(Point location, Size size)
        {
            Rectangle myArea = new Rectangle(location, size);
            bool intersect = false;
            foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens)
            {
                intersect |= myArea.IntersectsWith(screen.WorkingArea);
            }
            return intersect;
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

        #region IDispose Implementation

        public void Dispose()
        {
        }

        #endregion
    }
}
