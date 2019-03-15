# NUnit GUI Differences

The initial release of the **TestCentric GUI** is based on the layout and feature set of the of the original **NUnit GUI**, with the internals modified so as to run NUnit 3 tests. This document describes key differences between the NUnit GUI feature set and that of TestCentric.

1. **TestCentric** relies on the NUnit engine to load and run tests. Features that require an engine extension to be installed are only available if the appropriate extension is present. In particular, this includes the ability to open both NUnit and Visual Studio projects, run NUNit V2 tests and save results in V2 format.

2. Related to point 1, all direct access to NUnit project contents, such as editing, saving and adding configurations, has been removed. The user may use a separate install of the **NUnit Project Editor** or any other XML-capable editor to perform those tasks.

3. Because **TestCentric** runs under .NET 4.5 or higher, any tests that you cause to execute in-process will run under that version of the frameowrk. In some cases, this may give variable results. By default, tests are run in a separate process and use the target framework for which they were built.

4. The ** File Menu** no longer has entries for **Load Fixture** or **Clear Fixture**. New menu items to selct Process and AppDomain options for loading have been added and the corresponding Settings pages removed.

5. The **Project** top level menu has been removed since it was mostly used to contain entries related to NUnit projects. The "Add Projects" entry has been replaced by "Add Files" on the File menu.

6. The **Tools** menu no longer has entries for **Exception Details** or **Test Assemblies**. The **Open Log Directory** entry has been replaced with **Open Work Directory**.

7. The **Result Tabs** (Errors, Not Run, and Test Output) may no longer be hidden. The TestOutput tab only displays output to the
console and the ability to tailor it's contents has been removed with the exception of the option to label tests by name in various ways.
