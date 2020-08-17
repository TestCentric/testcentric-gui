Description: Displays the Release Notes showing changes made in each release of TestCentric.
Order: 9
---
### TestCentric Runner for NUnit 1.4.0 - April 30, 2020

This release introduces execution of .NET Core tests along with .NET Framework and 
adds several other smaller features.

#### GUI

 * The NUnit Project Editor may now be launched from the GUI. A Settings page
   has been added to allow specifying an alternate editor.

 * The Runtime menu allows overriding the version of .NET Framework or .NET 
   Core to be used, provided that the version used is compatible with all
   loaded assemblies.

 * The project configuration (e.g. Debug or Release) may be set individually
   for each loaded project, through the GUI test tree context menu.

 * Package settings are now displayed in the Test Properties Dialog for tests
   that correspond to a package.

#### Engine

 * Tests targeting .NET Core 1.1 through 3.1 may now be executed in the GUI.
   The GUI can simulataneously load .NET Framework and .NET Core test assemblies.

 * .NET Core tests communicate with the GUI using TCP. .NET Framework tests
   continue to use remoting.

#### Issues Resolved

 * 218 Support editing projects through the project editor
 * 453 Execute tests under .NET Core
 * 478 Review and update engine creation logic
 * 484 Recognize .NET Core as available and display in GUI runtime menu.
 * 508 Create agent process executable for .NET Core 2.1.
 * 512 Create transport protocol for use with .NET Core agents.
 * 565 Create agent process executable for .NET Core 3.1
 * 568 Run .NET Core 1.x tests under 2.1 or higher agent
 * 570 Support project configs
 * 572 Display Package Settings in TestPropertiesDialog

### TestCentric Runner for NUnit 1.3.3 - April 17, 2020

This release corrects a critical issue whereby engine extensions were no longer
being recognized by the GUI. Additional package testing, including use of
extensions, has been added to prevent regressions.

#### Issues Resolved

 * 551 Automate Release Process
 * 558 ReportPortal addins to TestCentric
 * 560 Support the NUnit engine API
 * 562 Expanded package testing prior to release
 
### TestCentric Runner for NUnit 1.3.2 - March 15, 2020

This release corrects a new bug found in the 1.3.0 release and makes a
number of improvements in the automation of the release process.

#### Issues Resolved

 * 428 Update assemblyinfo automatically
 * 525 Create a nuget organization for TestCentric
 * 534 Automate packaging and deployment
 * 540 Support for unattended execution of the GUI
 * 542 FIXED: Default process model for 32 bit v1.3.1

### TestCentric Runner for NUnit 1.3.1 - March 6, 2020

This release corrects a critical problem in the 1.3.0 release. None of the
1.3.0 packages included agents for use under .NET 4.0 and higher. This
release adds those agents.

#### Issues Resolved

 * 535 FIXED: Cannot load test assembly after 1.3 update
 * 536 Restructuring the build.cake script
 * 537 FIXED: Missing files in Release 1.3.0
 * 538 Add package checking tasks to build

### TestCentric Runner for NUnit 1.3.0 - February 23, 2019

#### General

 * A NuGet package is now available on nuget.org. While we continue to
   recommend use of the chocolatey.org package for most developers, because
   it provides a central installation point for use with all projects, a
   number of users requested a nuget package as well.

   NOTE: Whichever installation package you use, you should install any
   needed extensions using the same package source.

#### GUI

 * The runtime selection menu is now disabled when tests are being run in
   process, since no other runtime is available within the GUI process.

#### Engine

 * The UserSettings service is no longer part of the engine but is handled
   directly in the GUI. This is not considered a breaking change, since
   the engine is not yet released separately from the GUI.

 * A new PackageSettings service has been added to the engine. Its job
   is to analyze all packages, examine assemblies and set default
   parameters for executing them.

 * A pair of agents are provided for .NET 4.0 in addition to the existing
   .NET 2.0 agents. In addition, the engine is now built for both runtimes
   and agents use the appropriate version. The GUI itself uses the 4.0 engine.

#### Issues Resolved

 * 457 Remove UserSettings from the engine
 * 466 Update Copyright headers in all files
 * 475 Engine Tests will only run in process under GUI
 * 476 Eliminate use of Mono.Cecil from engine core assembly
 * 479 Refactor RuntimeFramework to eliminate unneeded selection options
 * 480 Determine versions of .NET Core we want to support.
 * 483 Disable Runtime menu when running in process
 * 487 Determine supported platforms for the engine.
 * 488 Integrate engine build output
 * 489 Run tests for .NET Core
 * 499 Engine Tests don't run under .NET Core 2.1
 * 502 Create new Service to manage PackageSettings
 * 505 Reorganize output to allow agents for various runtimes and both 32-bit and 64-bit execution
 * 513 Modify logic of TestAgency to initiate correct agents
 * 514 Upgrade engine to target .NET 4.0
 * 517 Net40 engine.core build
 * 518 Create agent process executable for .NET Framework 4.0 and higher
 * 522 Update nuget and chocolatey packages
 * 523 Create a nuget package for the GUI
 * 528 Chocolatey package should use TestCentric icon
 * 526 Update nuget package
 * 531 Create tasks in build script for publishing packages

### TestCentric Runner for NUnit 1.2.0 - December 17, 2019

This release is primarily marked by the elimination of our 
dependency on the NUnit Engine in preparation for moving
forward with features that require engine changes.

#### GUI

 * Inprocess test execution has been deprecated

#### Experimental GUI

 * The GUI font may be changed from the View menu

 * One or more files may be added to those open

 * Rerun on Change has been implemented

#### Engine

 * We now use the TestCentric Engine, Version 0.1, replacing the NUnit Engine. 
 The new engine is based on the NUnit 3.11 engine, with modifications.

 * Engine recognizes and runs assemblies that target .NET Framework 4.6 and up.

 * Engine now recognizes .NET Core assemblies and gives an error message
 if the user attempts to open one.

 * The RecentFilesService is no longer available.

#### Issues Resolved

 * 215 Experimental GUI: Allow setting the gui font
 * 217 Experimental GUI: Add files to those already open
 * 233 Experimental GUI: Rerun on Change
 * 426 Switch to use of separate TestCentric Engine build
 * 445 Better error message when attempting to load .NET Core assemblies
 * 447 FIXED: Engine will not recognize .NET Framework versions beyond 4.5
 * 449 FIXED: GUI window doesn't retain size between runs
 * 454 Deprecate in-process execution
 * 455 Remove RecentFilesService from engine
 * 456 Rename engine to testcentric engine
 * 461 Convert all projects to use Package References
 * 470 Remove deprecated "Single" option for ProcessModel
 * 473 Establish Initial Version for TestCentric engine

### TestCentric Runner for NUnit 1.1.0 - November 9, 2019

#### General

This release includes new features beyond the 1.0 release, together with some bug fixes.

#### Features
 * The configuration to be loaded may be specified in the GUI.
 * A new dialog allows setting parameters for the test run.
 * Test Results may be saved in any format for which an extension has been installed.
 * Additional test label options are supported.
 * The GUI uses a custom build of the latest version of the NUnit engine.
 * Includes version 0.16 of the experimental GUI.

#### Issues Resolved

 *   4 Save test results in alternative formats
 *  77 Setting up test parameters from gui
 * 193 Experimental GUI: Passing parameters from GUI to Tests
 * 216 Experimental GUI: Set project configuration to use
 * 275 Add --debug-agent option to the TestCentric GUI
 * 276 Deprecate old nunit-gui application on Chocolatey
 * 315 Reorganize Settings
 * 340 FIXED: Unable to run selected NUnit V2 tests
 * 347 Add TextOutput labelling of final test result
 * 372 Add command line option for specifying results file location
 * 394 Update engine
 * 395 Display version and path of loaded extensions
 * 397 Fix unhandled NullReferenceException thrown when test fails with Assert.Fail()
 * 399 Show the test assembly name in the main view's title bar.
 * 402 Set Project Configuration to use
 * 406 FIXED: Appveyor rebuild causes error
 * 412 Update link to Chocolatey package
 * 414 FIXED: Testcentric.exe needs icon
 * 415 Better installation documentation
 * 424 Use common dialog for extensions
 * 427 Ensure that agent exes are copied as part of building the project
 * 430 FIXED: Index out of range exception when adding a test file
 * 432 FIXED: TestParametersDialog enables Remove and Edit on startup

### TestCentric Runner for NUnit 1.0.1 - August 15, 2019

#### General

This release fixes a critical error discovered after the release of 1.0.0.

#### Features

 * Includes version 0.15.1 of the experimental GUI

#### Issues Resolved

 * 325 Deploy development builds automatically
 * 380 Automate testing of packages
 * 388 FIXED: NUnit process with multiple assemblies will not load

### TestCentric Runner for NUnit 1.0.0 - July 27, 2019

#### General

This is the final release of version 1.0 of the TestCentric Runner for NUnit, which is
primarily based on the layout and feature set of the original NUnit V2 GUI runner with
internals modified so as to run NUnit3 tests. Certain changes were made because of
differences in how NUnit3 works and are described below under Features.

This new GUI runner was created by the TestCentric team: Charlie Poole, Mikkel Bundgaard,
Manfred Lange, Stefano Nuzzo and Robert Snyder. Community members who contributed code to
this release include Jeff Kryzer, zBrianW, Joseph Musser and Mattias Karlsson. Countless
other folks contributed by filing issues, asking questions and making suggestions.

#### Features

* TestCentric relies on the NUnit engine to load and run tests. Features that require
an engine extension are only available if the appropriate extension is installed. In
particular, this includes the ability to open both NUnit and Visual Studio projects,
run NUNit V2 tests and save results in V2 format.

* All direct access to NUnit project contents, such as editing, saving and adding
configurations, has been removed. The user should use a separate editor such as
the NUnit Project Editor to perform those tasks.

* Because the GUI runs under .NET 4.5 or higher, any tests that you cause to execute
in-process will run under that version of the framework. In some cases, this may give
different results. By default, tests are run in a separate process and use the target
framework for which they were built. We recommend you stick with the default.

* The File menu no longer has entries for Load Fixture or Clear Fixture. New menu
items to select Process and AppDomain options for loading have been added and the
corresponding Settings pages removed.

* The Project top level menu has been removed since it was mostly used to contain
entries related to NUnit projects. The "Add Projects" entry has been replaced by
"Add Files" on the File menu.

* The Tools menu no longer has entries for Exception Details or Test Assemblies.
The Open Log Directory entry has been replaced with Open Work Directory.

* The Result Tabs (Errors and Failures, Tests Not Run, and Text Output) may no
longer be hidden. The Text Output tab only displays Console output and the ability
to tailor it's contents has been removed with the exception of the option to label
tests by name in various ways.

* Version 0.15 of the Experimental GUI is included.

#### Known Problems

* TestCentric uses the test id to select individual tests to be run. This is not
currently supported by the V2 framework driver, which throws an exception when
an id filter is encountered. The result for the GUI is that it is not possible
to select individual tests for execution if any V2 framework tests are loaded.
For that reason, we recommend that you only use this release for NUnit 3 tests.
This problem will be fixed when a new version of the V2 driver is available,
which includes support for an id filter.

#### Issues Resolved

* 7 Credit for all contributors
* 8 Icon and Logo for TestCentric
* 224 FIXED: Appveyor build doesn't use generated identifier to tag packages 
* 375 Clean up GUI About box
* 378 Ensure that error message displays when exception is thrown in engine

### TestCentric Runner for NUnit 1.0.0-Beta4 - July 17, 2019

#### General

This is the fourth beta release of the TestCentric Runner for NUnit. While it is still beta,
it is feature-complete. Since the actual execution of tests is done by the NUnit engine
rather than the GUI itself, it's probably reasonable for developers to use this on the
desktop, provided their CI is based on a stable production-level NUnit runner, such as
the NUnit 3 Console Runner or the NUnit 3 VS adapter.

#### Features

* Version 0.14 of the Experimental GUI is included.

#### Issues Resolved

* 368 FIXED: Arguments to --process option for GUI don't match NUnit console.
* 369 Merge Options classes for two GUIs and move to common assembly.
* 370 FIXED: Beta3: unhandled .net exception

### TestCentric Runner for NUnit 1.0.0-Beta3 - July 14, 2019

#### General

This is the third beta release of the TestCentric Runner for NUnit. While it is still beta,
it is feature-complete. Since the actual execution of tests is done by the NUnit engine
rather than the GUI itself, it's probably reasonable for developers to use this on the
desktop, provided their CI is based on a stable production-level NUnit runner, such as
the NUnit 3 Console Runner or the NUnit 3 VS adapter.

#### Features

 * Version 0.13 of the Experimental GUI is included.
 * Added command-line options --process, --domain, --inprocess, --x86 and --agents.

#### Issues Resolved

 * 139 FIXED: Stop Button to interrupt infinite loop
 * 287 Assembly or project full name not shown
 * 337 FIXED: Stopping a long running test from TestCentric gui does not stop right away
 * 350 FIXED: Installed Extensions fields not cleared
 * 352 Incorporate NUnit Engine as a subtree
 * 357 Review whether File menu settings should be saved
 * 358 FIXED: Assemblies Running in Parallel (Regardless of Settings)
 * 361 FIXED: Process Model setting includes Single rather than Separate
 * 364 FIXED: Experimental GUI: Proper icons are not displayed for warnings and failures

### TestCentric Runner for NUnit 1.0.0-Beta2 - May 30, 2019

#### General

This is the second beta release of the TestCentric Runner for NUnit. While it is still beta,
it is feature-complete. Since the actual execution of tests is done by the NUnit engine
rather than the GUI itself, it's probably reasonable for developers to use this on the
desktop, provided their CI is based on a stable production-level NUnit runner, such as
the NUnit 3 Console Runner or the NUnit 3 VS adapter.

#### Features

 * Version 0.12 of the Experimental GUI is included with support for user-defined icons
   and immediate text output display.

#### Issues Resolved
 *   5 Add online docs for the GUI
 * 229 Experimental GUI: Multiple and user-defined tree icons
 * 236 Experimental GUI: missing test output
 * 326 Update CONTRIBUTING.md
 * 328 FIXED: Reloading loads the assembly multiple times
 * 327 Document how TestCentric GUI is versioned
 * 333 FIXED: InvalidOperationException is thrown from RunStarting event handler
 * 336 FIXED: In TestCentric gui, Selected Categories are cleared on Reload
 * 345 Show names of settings in Text Output settings dialog

### TestCentric Runner for NUnit 1.0.0-Beta - April 13, 2019

#### General

This is the beta release of the TestCentric Runner for NUnit. While it is still beta,
it is feature-complete. Since the actual execution of tests is done by the NUnit engine
rather than the GUI itself, it's probably reasonable for developers to use this on the
desktop, provided their CI is based on a stable production-level NUnit runner, such as
the NUnit 3 Console Runner or the NUnit 3 VS adapter.

#### Features

 * Version 0.11 of the Experimental GUI is included with support for saving test results,
   reload on change and reload on run.

#### Issues Resolved

 *  22 Create new signing key for TestCentric
 * 212 Experimental GUI: Need SaveResults menu item
 * 213 Experimental GUI: Reload on change
 * 226 Display time in a more natural format
 * 232 Experimental GUI: Reload on Run
 * 306 FIXED: Save TestResult Dialog not initialized correctly
 * 318 Disable ReRun on Change setting

### TestCentric Runner for NUnit 1.0.0-Alpha4 - April 5, 2019

#### General

This is the fourth alpha release of the TestCentric Runner for NUnit. It is not yet recommended
for use in production work.

#### Features

 * Entries now appear in the tree display for any projects loaded
 * The GUI now uses a custom built version of the engine, based on the NUnit 3.10 engine.

#### Issues Resolved

 * 164 Update NSubstitute to version 4.0
 * 241 Terminate nunit-agent.exe / nunit-agent-x86.exe once they are no longer in use
 * 273 FIXED: No project elements are present in the test XML received from the engine
 * 285 FIXED: NUnit Agent locks dll
 * 288 FIXED: Property dialog not showing assembly process id or appdomain name
 * 290 Need a way to locate and examine log files
 * 292 Remove project editor integration
 * 294 Document differences from the NUnit V2 GUI
 * 299 FIXED: Choosing "Run as X86" from menu doesn't actually load the X86 agent.
 * 301 FIXED Tree icons of previous run do not clear when starting second run
 * 304 FIXED Use new 3.10 release of NUnit engine

### TestCentric Runner for NUnit 1.0.0-Alpha3 - February 27, 2019

#### General

This is the third alpha release of the TestCentric Runner for NUnit. It is not yet recommended
for use in production work.

#### Features

 * The Experimental GUI, which was previously a separate project, is now included along with the standard TestCentric GUI.
 * Assemblies and projects are now loadable using all process and domain settings.
 * Process and domain settings are now part of the File menu.
 * A custom engine build is included while waiting for the next NUnit engine release.
 * The zip and chocolatey packagegs now include pdb files.

#### Issues Resolved

 *  18 FIXED: Reload on Change not working
 * 138 Double click test to start it
 * 153 Changing font should affect status bar
 * 154 FIXED: When opening solution file non-test assemblies cause an error message
 * 155 FIXED: Redundant menu items under View | Tree
 * 156 FIXED: Changing checkbox setting should not change default
 * 159 FIXED: Project name not shown in tree
 * 161 Create separate GUI Components assembly
 * 163 Create Linux build.
 * 165 Review and merge build script with experimental GUI script
 * 170 Review and merge components assembly
 * 175 FIXED: Start and stop button are misaligned
 * 178 Status Bar height should adjust to font
 * 182 Integrate ToolStripItem and ToolStripMenuItem Elements
 * 185 FIXED: Not building Mono 5.10 or 4.6 under Travis
 * 189 Merge experimental GUI into standard GUI project
 * 238 FIXED: Categories on test fixtures not considered in test list design (Experimental)
 * 251 Show file names when loading
 * 253 FIXED: Add file to open files is broken
 * 256 Use DialogManager to display open and save dialogs
 * 257 FIXED: Error on reload when running in process
 * 261 Run tests under 3.9 console runner
 * 263 FIXED: Cannot load nunit test solution in process
 * 264 Use custom build of NUnit engine
 * 269 FIXED: No option to run using default process model or domain usage
 * 274 Eliminate Default Setting Page for process model and domain usage
 * 278 Include pdb files in packages
 * 279 Remove engine version hack in build script

### TestCentric Runner for NUnit 1.0.0-Alpha2 - November 11, 2018

#### General

This is the second alpha release of the TestCentric Runner for NUnit. It is not yet recommended
for use in production work.

#### Features

 * The GUI now uses the NUnit 3.9 engine
 * Reloading on each Run is now functional.
 * Alternate images for the tree are now implemented.
 * Saving and Restoring the visual state is now implemented.

#### Issues Resolved

 *  12 Use UpDown counter for number of recent files to display
 *  15 FIXED: Alternate Tree Images not working
 *  21 FIXED: Disabling Shadow Copy is Ignored
 *  76 FIXED: Exception "Cross-thread operation not valid: Control 'progressBar' accessed from a thread other than the thread it was created on."
 *  93 FIXED: NRE when starting the GUI
 *  95 Implement MVP for main form and sub-views
 *  99 FIXED: Scrolling not working correctly for ErrorDisplay
 * 100 Don't display "Test-Run" as root node in tree with only one assembly
 * 115 FIXED: Zero Length VisualState.xml file results in exception
 * 132 Create TestCentric.Common assembly
 * 144 Update NUnit Engine to 3.9

### TestCentric Runner for NUnit 1.0.0-Alpha1 - August 3, 2018

#### General

This is the initial alpha release of the TestCentric Runner for NUnit. It is not yet recommended
for use in production work.

#### Features

This GUI runner resembles the NUnit V2 GUI, but since it runs NUnit 3 tests the internal implementation
is entirely new. In addition, features not available or not easily supported in NUnit 3 have been
removed. Some key differences from the V2 GUI are...

 * The GUI itself targets .NET 4.5, rather than 2.0.
 * We use chocolatey as the primary distribution method for the GUI. This makes extensions that are
 also installed through chocolatey available when running under the GUI. A zip file is also provided.
 * Displaying tests as a flat list of fixtures is not supported.
 * Merging tests in the same namespace across assemblies is not supported.
 * The GUI no longer understands the layout of NUnit project files, which is taken care of by the 
 NUnit test engine. Consequently, menu items relating to creating, editiong and saving such files 
 are no longer present.
 * The ability to open project files, including NUnit and VS projects, is dependent on the presence
 of the approprate engine extensions, which are not bundled with the GUI.

#### Issues Resolved

 *   1 Select Target Framework for the GUI
 *   2 Review of Initial Upload
 *   9 Update the About box
 *  10 Correct name for program in --help option
 *  13 FIXED: Number of Recent Files has no effect
 *  14 FIXED: Tree always cleared of results on reload
 *  16 FIXED: Flat list of TestFixtures not working
 *  17 FIXED: Reload on Run not working
 *  19 FIXED: Principal Policy not working
 *  20 FIXED: InternalTrace level not honored
 *  24 FIXED: Setting number of agents has no effect
 *  26 FIXED: Clear Results Setting is on wrong page
 *  27 Create CI Build Script
 *  28 FIXED: "Clear All" and "Check Failed" has no effect
 *  29 FIXED: "Run" context menu is never enabled
 *  32 Renaming test-centric to testcentric
 *  33 Restore Tree-based Setting Dialog
 *  39 FIXED: Cannot run the tests in TestCentric.Gui.Model.Tests.dll more than once
 *  42 FIXED: Not possible to run explicit tests using the GUI
 *  50 Review and reorganize Settings for NUnit 3
 *  52 FIXED: Show CheckBoxes throws NRE
 *  61 Disable or remove non-functional elements on Settings pages
 *  62 FIXED: Switching between full and mini gui display doesn't happen immediately
 *  63 FIXED: Check files exist setting has no effect
 *  64 Better error message when an assembly is not found
 *  66 FIXED: Project editor menu item not working.
 *  67 FIXED: Show Checkboxes menu and settings elements are inconsistent
 *  68 Save results menu should be disabled when no test has been run
 *  71 FIXED: Multiple files on command-line or in file open dialog not opened
 *  73 Multiple loaded files have no root in tree
 *  80 Standardize naming of TestCentric packages and executables
 *  83 Update tests to use NUnit 3.10.1
