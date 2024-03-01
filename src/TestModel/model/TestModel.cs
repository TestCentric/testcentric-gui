// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TestCentric.Engine;
using TestCentric.Engine.Services;

namespace TestCentric.Gui.Model
{
    using Services;
    using Settings;

    public class TestModel : ITestModel
    {
        static Logger log = InternalTrace.GetLogger(typeof(TestModel));

        private const string PROJECT_LOADER_EXTENSION_PATH = "/NUnit/Engine/TypeExtensions/IProjectLoader";
        private const string NUNIT_PROJECT_LOADER = "TestCentric.Engine.Services.ProjectLoaders.NUnitProjectLoader";
        private const string VISUAL_STUDIO_PROJECT_LOADER = "TestCentric.Engine.Services.ProjectLoaders.VisualStudioProjectLoader";

        // Our event dispatcher. Events are exposed through the Events
        // property. This is used when firing events from the model.
        private TestEventDispatcher _events;

        // Check if the loaded Assemblies has been changed
        private AssemblyWatcher _assemblyWatcher;

        private SettingsService _settingsService;

        private TestRunSpecification _lastTestRun;

        private bool _lastRunWasDebugRun;

        #region Constructor and Creation

        public TestModel(ITestEngine testEngine, string applicationPrefix = null)
        {
            TestEngine = testEngine;
            _settingsService = new SettingsService(true);
            _events = new TestEventDispatcher(this);
            _assemblyWatcher = new AssemblyWatcher();

            _settingsService.LoadSettings();
            Settings = new UserSettings(_settingsService, applicationPrefix);
            RecentFiles = new RecentFiles(_settingsService, applicationPrefix);

            Services = new TestServices(testEngine);

            AvailableAgents = new List<string>(
                Services.TestAgentService.GetAvailableAgents().Select((a) => a.AgentName));

            foreach (var node in Services.ExtensionService.GetExtensionNodes(PROJECT_LOADER_EXTENSION_PATH))
            {
                if (node.TypeName == NUNIT_PROJECT_LOADER)
                    NUnitProjectSupport = true;
                else if (node.TypeName == VISUAL_STUDIO_PROJECT_LOADER)
                    VisualStudioSupport = true;
            }
        }

        public static ITestModel CreateTestModel(CommandLineOptions options)
        {
            var engine = TestEngineActivator.CreateInstance();
            if (engine == null)
                throw new EngineNotFoundException();
            return CreateTestModel(engine, options);
        }

        // Public for testing
        public static ITestModel CreateTestModel(ITestEngine testEngine, CommandLineOptions options)
        {
            // Currently the InternalTraceLevel can only be set from the command-line.
            // We can't use user settings to provide a default because the settings
            // are an engine service and the engine have the internal trace level
            // set as part of its initialization.
            var traceLevel = options.InternalTraceLevel != null
                ? (InternalTraceLevel)Enum.Parse(typeof(InternalTraceLevel), options.InternalTraceLevel)
                : InternalTraceLevel.Off;

            var logFile = $"InternalTrace{Process.GetCurrentProcess().Id}.gui.log";
            if (options.WorkDirectory != null)
                logFile = Path.Combine(options.WorkDirectory, logFile);

            InternalTrace.Initialize(logFile, traceLevel);

            testEngine.InternalTraceLevel = traceLevel;
            if (options.WorkDirectory != null)
                testEngine.WorkDirectory = options.WorkDirectory;

            var model = new TestModel(testEngine, "TestCentric");

            model.PackageOverrides.Add(EnginePackageSettings.InternalTraceLevel, testEngine.InternalTraceLevel.ToString());

            if (options.WorkDirectory != null)
                model.PackageOverrides.Add(EnginePackageSettings.WorkDirectory, options.WorkDirectory);
            if (options.MaxAgents >= 0)
                model.PackageOverrides.Add(EnginePackageSettings.MaxAgents, options.MaxAgents);
            if (options.RunAsX86)
                model.PackageOverrides.Add(EnginePackageSettings.RunAsX86, true);
            if (options.DebugAgent)
                model.PackageOverrides.Add(EnginePackageSettings.DebugAgent, true);
            if (options.SimulateUnloadError)
                model.PackageOverrides.Add(EnginePackageSettings.SimulateUnloadError, true);
            if (options.SimulateUnloadTimeout)
                model.PackageOverrides.Add(EnginePackageSettings.SimulateUnloadTimeout, true);
            if (options.TestParameters.Count > 0)
            {
                string[] parms = new string[options.TestParameters.Count];
                int index = 0;
                foreach (string key in options.TestParameters.Keys)
                    parms[index++] = $"{key}={options.TestParameters[key]}";

                model.PackageOverrides.Add("TestParametersDictionary", options.TestParameters);
                model.PackageOverrides.Add("TestParameters", string.Join(";", parms));
            }

            return model;
        }

        #endregion

        #region ITestModel Implementation

        #region General Properties

        // Work Directory
        public string WorkDirectory { get { return TestEngine.WorkDirectory; } }

        // Event Dispatcher
        public ITestEvents Events { get { return _events; } }

        // Services provided either by the model itself or by the engine
        public ITestServices Services { get; }

        public UserSettings Settings { get; }

        public IList<string> AvailableAgents { get; }

        public RecentFiles RecentFiles { get; }

        // Project Support
        public bool NUnitProjectSupport { get; }
        public bool VisualStudioSupport { get; }

        // Runtime Support
        private List<IRuntimeFramework> _runtimes;
        public IList<IRuntimeFramework> AvailableRuntimes
        {
            get
            {
                if (_runtimes == null)
                    _runtimes = GetAvailableRuntimes();

                return _runtimes;
            }
        }

        // Result Format Support
        private List<string> _resultFormats;
        public IEnumerable<string> ResultFormats
        {
            get
            {
                if (_resultFormats == null)
                {
                    _resultFormats = new List<string>();
                    foreach (string format in Services.ResultService.Formats)
                        _resultFormats.Add(format);
                }

                return _resultFormats;
            }
        }

        #endregion

        #region Current State of the Model

        // The current TestPackage loaded by the model
        public TestPackage TestPackage { get; private set; }

        public bool IsPackageLoaded { get { return TestPackage != null; } }

        // The list of files passed to the model to load.
        public List<string> TestFiles { get; } = new List<string>();

        public IDictionary<string, object> PackageOverrides { get; } = new Dictionary<string, object>();

        public TestNode LoadedTests { get; private set; }
        public bool HasTests { get { return LoadedTests != null; } }

        public IList<string> AvailableCategories { get; private set; }

        public bool IsTestRunning => Runner != null && Runner.IsTestRunning;

        public IDictionary<string, ResultNode> Results { get; } = new Dictionary<string, ResultNode>();
        public ResultSummary ResultSummary { get; internal set; }
        public bool HasResults => ResultSummary != null;

        /// <summary>
        /// Gets or sets the active test item. This is the item
        /// for which details are displayed in the various views.
        /// </summary>
        public ITestItem ActiveTestItem
        {
            get { return _activeItem; }
            set { _activeItem = value; _events.FireActiveItemChanged(_activeItem); }
        }
        private ITestItem _activeItem;

        /// <summary>
        ///  Gets or sets the list of selected tests.
        /// </summary>
        public TestSelection SelectedTests { get; set; }

        public List<string> SelectedCategories { get; private set; }

        public bool ExcludeSelectedCategories { get; private set; }

        public TestFilter CategoryFilter { get; private set; } = TestFilter.Empty;

        #endregion

        #region Specifications passed as arguments to methods

        private class TestRunSpecification
        {
            // The selected tests to run (ITestItem may be a TestSelection or a TestNode
            public TestSelection SelectedTests { get; }

            // A possibly empty filter to be applied to the selected tests.
            // NOTE: Currently, filter is always empty
            public TestFilter CategoryFilter { get; }

            public TestRunSpecification(TestSelection selectedTests, TestFilter filter)
            {
                SelectedTests = selectedTests;
                CategoryFilter = filter;
            }

            public TestRunSpecification(TestNode testNode, TestFilter filter)
            {
                SelectedTests = new TestSelection();
                SelectedTests.Add(testNode);
                CategoryFilter = filter;
            }
        }

        #endregion

        #region Methods

        public void NewProject()
        {
            NewProject("Dummy");
        }

        public const string PROJECT_SAVE_MESSAGE =
            "It's not yet decided if we will implement saving of projects. The alternative is to require use of the project editor, which already supports this.";

        public void NewProject(string filename)
        {
            throw new NotImplementedException(PROJECT_SAVE_MESSAGE);
        }

        public void SaveProject()
        {
            throw new NotImplementedException(PROJECT_SAVE_MESSAGE);
        }

        public void LoadTests(IList<string> files)
        {
            log.Info($"Loading test files '{string.Join("', '", files.ToArray())}'");
            if (IsPackageLoaded)
                UnloadTests();

            _events.FireTestsLoading(files);

            TestFiles.Clear();
            TestFiles.AddRange(files);

            TestPackage = MakeTestPackage(files);
            log.Debug("Created TestPackage");
            _lastTestRun = null;
            _lastRunWasDebugRun = false;

            Runner = TestEngine.GetRunner(TestPackage);
            log.Debug($"Got {Runner.GetType().Name} for package");

            try
            {
                log.Debug("Loading tests");
                LoadedTests = new TestNode(Runner.Explore(Engine.TestFilter.Empty));
                log.Debug($"Loaded {LoadedTests.Xml.GetAttribute("TestCaseCount")} tests");
            }
            catch(Exception ex)
            {
                _events.FireTestLoadFailure(ex);
                log.Error("Failed to load tests", ex);
                return;
            }

            BuildTestIndex();
            MapTestsToPackages();
            AvailableCategories = GetAvailableCategories();

            ClearResults();

            _assemblyWatcher.Setup(1000, files as IList);
            _assemblyWatcher.AssemblyChanged += (path) => _events.FireTestChanged();
            _assemblyWatcher.Start();

            _events.FireTestLoaded(LoadedTests);

            foreach (var subPackage in TestPackage.SubPackages)
                RecentFiles.Latest = subPackage.FullName;
        }

        private Dictionary<string, TestNode> _testsById = new Dictionary<string, TestNode>();

        private void BuildTestIndex()
        {
            _testsById.Clear();
            BuildTestIndex(LoadedTests);
        }

        private void BuildTestIndex(TestNode node)
        {
            _testsById[node.Id] = node;

            foreach (TestNode child in node.Children)
                BuildTestIndex(child);
        }

        private Dictionary<string, TestPackage> _packageMap = new Dictionary<string, TestPackage>();

        private void MapTestsToPackages()
        {
            _packageMap.Clear();
            MapTestToPackage(LoadedTests, TestPackage);
        }

        private void MapTestToPackage(TestNode test, TestPackage package)
        {
            _packageMap[test.Id] = package;
            
            for (int index = 0; index < package.SubPackages.Count && index < test.Children.Count; index++)
                MapTestToPackage(test.Children[index], package.SubPackages[index]);
        }

        public IList<string> GetAgentsForPackage(TestPackage package = null)
        {
            if (package == null) package = TestPackage;

            return new List<string>(
                Services.TestAgentService.GetAgentsForPackage(package).Select(a => a.AgentName));
        }

        public void UnloadTests()
        {
            _events.FireTestsUnloading();

            UnloadTestsIgnoringErrors();
            Runner.Dispose();
            LoadedTests = null;
            AvailableCategories = null;
            TestPackage = null;
            TestFiles.Clear();
            ClearResults();
            _assemblyWatcher.Stop();

            _events.FireTestUnloaded();
        }

        private void UnloadTestsIgnoringErrors()
        {
            try
            {
                Runner.Unload();
            }
            catch (EngineUnloadException)
            {

            }
        }

        public void ReloadTests()
        {
            _events.FireTestsReloading();

#if true
            Runner.Reload();
#else
            // NOTE: The `ITestRunner.Reload` method supported by the engine
            // has some problems, so we simulate Unload+Load. See issue #328.

                // Replace Runner in case settings changed
            UnloadTestsIgnoringErrors();
            Runner.Dispose();
            Runner = TestEngine.GetRunner(TestPackage);

            // Discover tests
            LoadedTests = new TestNode(Runner.Explore(Engine.TestFilter.Empty));
            AvailableCategories = GetAvailableCategories();

            ClearResults();
#endif

            _events.FireTestReloaded(LoadedTests);
        }

        public void ReloadPackage(TestPackage package, string config)
        {
            //var originalSubPackages = new List<TestPackage>(package.SubPackages);
            //package.SubPackages.Clear();
            package.Settings[EnginePackageSettings.ActiveConfig] = config;

            //foreach (var subPackage in package.SubPackages)
            //    foreach (var original in originalSubPackages)
            //        if (subPackage.Name == original.Name)
            //            subPackage.SetID(original.ID);

            ReloadTests();
        }

        public void RunTests(TestNode testNode)
        {
            if (testNode == null)
                throw new ArgumentNullException(nameof(testNode));

            log.Info($"Running test: {testNode.GetAttribute("name")}");
            RunTests(new TestRunSpecification(testNode, CategoryFilter));
        }

        public void RunTests(TestSelection tests)
        {
            if (tests == null)
                throw new ArgumentNullException(nameof(tests));

            log.Info($"Running test: {string.Join(", ", tests.Select(node => node.GetAttribute("name").ToArray()))}");
            RunTests(new TestRunSpecification(tests, CategoryFilter));
        }

        public void RepeatLastRun()
        {
            if (_lastTestRun == null)
                throw new InvalidOperationException("RepeatLastRun called before any tests were run");

            log.Info($"Running test: {string.Join(", ", _lastTestRun.SelectedTests.Select(node => node.GetAttribute("name").ToArray()))}");
            RunTests(_lastTestRun);
        }

        public void DebugTests(TestNode testNode)
        {
            if (testNode == null)
                throw new ArgumentNullException(nameof(testNode));

            DebugTests(testNode.GetTestFilter());
        }

        public void StopTestRun(bool force)
        {
            Runner.StopRun(force);
        }

        public void SaveResults(string filePath, string format = "nunit3")
        {
            log.Debug($"Saving test results to {filePath} in {format} format");

            try
            {
                var resultWriter = Services.ResultService.GetResultWriter(format, new object[0]);
                var results = GetResultForTest(LoadedTests.Id);
                log.Debug(results.Xml.OuterXml);
                resultWriter.WriteResultFile(results.Xml, filePath);
            }
            catch(Exception ex)
            {
                log.Error("Failed to save results", ex);
            }
        }

        public TestNode GetTestById(string id)
        {
            return _testsById.TryGetValue(id, out var node) ? node : null;
        }

        public ResultNode GetResultForTest(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                ResultNode result;
                if (Results.TryGetValue(id, out result))
                    return result;
            }

            return null;
        }

        public IDictionary<string, object> GetPackageSettingsForTest(string id)
        {
            return GetPackageForTest(id)?.Settings;
        }
        public TestPackage GetPackageForTest(string id)
        {
            return _packageMap.ContainsKey(id) 
                ? _packageMap[id] 
                : null;
        }

        public void ClearResults()
        {
            Results.Clear();
            ResultSummary = null;
        }

        public void SelectCategories(IList<string> categories, bool exclude)
        {
            SelectedCategories = new List<string>(categories);
            ExcludeSelectedCategories = exclude;

            UpdateCategoryFilter();

            _events.FireCategorySelectionChanged();
        }

        private void UpdateCategoryFilter()
        {
            var catFilter = TestFilter.Empty;

            if (SelectedCategories != null && SelectedCategories.Count > 0)
            {
                catFilter = TestFilter.MakeCategoryFilter(SelectedCategories);

                if (ExcludeSelectedCategories)
                    catFilter = TestFilter.MakeNotFilter(catFilter);
            }

            CategoryFilter = catFilter;
        }

#endregion

#endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            try
            {
                if (IsPackageLoaded)
                    UnloadTests();

                if (Runner != null)
                    Runner.Dispose();

                if (TestEngine != null)
                    TestEngine.Dispose();

                if (_assemblyWatcher != null)
                    _assemblyWatcher.Dispose();

                if (_settingsService != null)
                    _settingsService.SaveSettings();
            }
            catch (EngineUnloadException)
            {
                // TODO: Figure out what to do about this
            }
        }

        #endregion

        #region Private and Internal Properties

        private ITestEngine TestEngine { get; }

        private ITestRunner Runner { get; set; }

        #endregion

        #region Helper Methods

        // Public for testing only
        public TestPackage MakeTestPackage(IList<string> testFiles)
        {
            var package = new TestPackage(testFiles);
            var engineSettings = Settings.Engine;

            // We use AddSetting rather than just setting the value because
            // it propagates the setting to all subprojects.

            if (engineSettings.Agents > 0)
                package.AddSetting(EnginePackageSettings.MaxAgents, engineSettings.Agents);

            if (engineSettings.SetPrincipalPolicy)
                package.AddSetting(EnginePackageSettings.PrincipalPolicy, engineSettings.PrincipalPolicy);

            //if (Options.InternalTraceLevel != null)
            //    package.AddSetting(EnginePackageSettings.InternalTraceLevel, Options.InternalTraceLevel);

            package.AddSetting(EnginePackageSettings.ShadowCopyFiles, engineSettings.ShadowCopyFiles);

            foreach (var subpackage in package.SubPackages)
                if (Path.GetExtension(subpackage.Name) == ".sln")
                    subpackage.AddSetting(EnginePackageSettings.SkipNonTestAssemblies, true);

            foreach (var entry in PackageOverrides)
                package.AddSetting(entry.Key, entry.Value);

            return package;
        }

        // The engine returns more values than we really want.
        // For example, we don't currently distinguish between
        // client and full profiles when executing tests. We
        // drop unwanted entries here. Even if some of these items
        // are removed in a later version of the engine, we may
        // have to retain this code to work with older engines.
        private List<IRuntimeFramework> GetAvailableRuntimes()
        {
            var runtimes = new List<IRuntimeFramework>();

            foreach (var runtime in Services.GetService<IAvailableRuntimes>().AvailableRuntimes)
            {
                // We don't support anything below .NET Framework 2.0
                if (runtime.Id.StartsWith("net-") && runtime.FrameworkVersion.Major < 2)
                    continue;

                runtimes.Add(runtime);
            }

            runtimes.Sort((x, y) => x.DisplayName.CompareTo(y.DisplayName));

            // Now eliminate client entries where full entry follows
            for (int i = runtimes.Count - 2; i >= 0; i--)
            {
                var rt1 = runtimes[i];
                var rt2 = runtimes[i + 1];

                if (rt1.Id != rt2.Id)
                    continue;

                if (rt1.Profile == "Client" && rt2.Profile == "Full")
                    runtimes.RemoveAt(i);
            }

            return runtimes;
        }

        // All Test running except for DebugTests eventually comes down to this method
        private void RunTests(TestRunSpecification runSpec)
        {
            // Create a test filter incorporating both the
            // selected tests and the category filter.
            var filter = runSpec.SelectedTests.GetTestFilter();
            if (!runSpec.CategoryFilter.IsEmpty)
                filter = TestFilter.MakeAndFilter(filter, runSpec.CategoryFilter);

            SetTestDebuggingFlag(false);
            _lastTestRun = runSpec;

            // TODO: Does this belong here? Maybe need to do before creating the run specification.
            if (Settings.Engine.ReloadOnRun)
            {
                // TODO: reinstate when engine Reload works. Currently
                // we simulate it with Unload + Load, so the results
                // are always cleared.
                //if (Settings.Gui.ClearResultsOnReload)
                //    ClearResults();
                ReloadTests();
            }

            Runner.RunAsync(_events, filter.AsNUnitFilter());
        }

        private void DebugTests(TestFilter filter)
        {
            SetTestDebuggingFlag(true);

            // We bypass use RunTests(RunSpecification) here because
            // we don't want to either save this as the last test run
            // or reload the tests.
            Runner.RunAsync(_events, filter.AsNUnitFilter());
        }

        private void SetTestDebuggingFlag(bool debuggingRequested)
        {
            // We need to re-create the test runner because settings such
            // as debugging have already been passed to the test runner.
            // For performance reasons, we only do this if we did run
            // in a different mode than last time.
            if (_lastRunWasDebugRun != debuggingRequested)
            {
                foreach (var subPackage in TestPackage.SubPackages)
                {
                    subPackage.Settings["DebugTests"] = debuggingRequested;
                }

                Runner = TestEngine.GetRunner(TestPackage);
                Runner.Load();

                // It is not strictly necessary to load the tests
                // because the runner will do that automatically, however,
                // the initial test count will be incorrect causing UI crashes.

                _lastRunWasDebugRun = debuggingRequested;
            }
        }

        public IList<string> GetAvailableCategories()
        {
            var categories = new Dictionary<string, string>();
            CollectCategories(LoadedTests, categories);

            var list = new List<string>(categories.Values);
            list.Sort();
            return list;
        }

        private void CollectCategories(TestNode test, Dictionary<string, string> categories)

        {
            foreach (string name in test.GetPropertyList("Category").Split(new[] { ',' }))
                if (IsValidCategoryName(name))
                    categories[name] = name;

            if (test.IsSuite)
                foreach (TestNode child in test.Children)
                    CollectCategories(child, categories);
        }

        public static bool IsValidCategoryName(string name)
        {
            return name.Length > 0 && name.IndexOfAny(new char[] { ',', '!', '+', '-' }) < 0;
        }

        #endregion
    }
}
