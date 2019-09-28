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
using NUnit.Engine;

namespace TestCentric.Gui.Model
{
    public interface ITestModel : IDisposable
    {
        #region General Properties

        // Work Directory
        string WorkDirectory { get; }

        // Event Dispatcher
        ITestEvents Events { get; }

        ITestServices Services { get; }

        // Project Support
        bool NUnitProjectSupport { get; }
        bool VisualStudioSupport { get; }

        // List of available runtimes, based on the engine's list
        // but filtered to meet the GUI's requirements
        IList<IRuntimeFramework> AvailableRuntimes { get; }

        // Result Format Support
        IEnumerable<string> ResultFormats { get; }

        #endregion

        #region Current State of the Model

        TestPackage TestPackage { get; }

        bool IsPackageLoaded { get; }

        List<string> TestFiles { get; }

        IDictionary<string, object> PackageOverrides { get; }

        // TestNode hierarchy representing the discovered tests
        TestNode Tests { get; }

        // See if tests are available
        bool HasTests { get; }

        IList<string> AvailableCategories { get; }

        // See if a test is running
        bool IsTestRunning { get; }

        // Do we have results from running the test?
        bool HasResults { get; }

        List<string> SelectedCategories { get; }

        bool ExcludeSelectedCategories { get; }

        TestFilter CategoryFilter { get; }

        #endregion

        #region Methods

        // Create a new empty project using a default name
        void NewProject();

        // Create a new project given a filename
        void NewProject(string filename);

        void SaveProject();

        // Load a TestPackage
        void LoadTests(IList<string> files);

        // Unload current TestPackage
        void UnloadTests();

        // Reload current TestPackage
        void ReloadTests();

        // Reload a specific package using the specified config
        void ReloadPackage(TestPackage package, string config);

        // Run all the tests
        void RunAllTests();

        // Debug all tests
        void DebugAllTests();

        // Run just the specified ITestItem
        void RunTests(ITestItem testItem);

        // Debug just the specified ITestItem
        void DebugTests(ITestItem testItem);

        // Cancel the running test
        void CancelTestRun(bool force);

        // Save the results of the last run in the specified format
        void SaveResults(string fileName, string format="nunit3");

        // Get the result for a test if available
        ResultNode GetResultForTest(string id);

        // Get the TestPackage represented by a test,if available
        TestPackage GetPackageForTest(string id);

        // Get the active config name for a TestPackage representing a project
        string GetActiveConfig(TestPackage package);

        // Get a list of config names for a TestPackage representing a project
        IList<string> GetConfigNames(TestPackage package);

        // Clear the results for all tests
        void ClearResults();

        // Broadcast event when SelectedTestItem changes
        void NotifySelectedItemChanged(ITestItem testItem);

        // Set the category filters for running and tree display
        void SelectCategories(IList<string> categories, bool exclude);

        #endregion
    }
}
