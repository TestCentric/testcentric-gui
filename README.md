# TestCentric Runner for NUnit - Version 0.1

[![Build status](https://ci.appveyor.com/api/projects/status/i7ymql47e8bo2rel/branch/master?svg=true)](https://ci.appveyor.com/project/CharliePoole/testcentric-gui/branch/master)

The **TestCentric Runner for NUnit** (aka **TestCentric**) is a GUI runner aimed at eventually supporting a range of .NET testing frameworks. As we work toward the version 1.0 release, we are concentrating on support of `NUnit` tests.

## Features

The initial release of this project is based on the layout and feature set of the of the original NUnit GUI, with the internals modified so as to run NUnit 3 tests. For convenience, we note exceptions to the original GUI feature set here:

1. **TestCentric** relies on the NUnit engine to load and run tests. Features that require an engine extension to be installed are only available if the appropriate extension is present. In particular, this includes the ability to open both NUnit and Visual Studio projects, run NUNit V2 tests and save results in V2 format.

2. Related to point 1, all direct access to NUnit project contents, such as editing, saving and adding configurations, has been removed. A tools menu entry has been added for convenience in opening the **Project Editor**, which can perform those tasks.

3. Because **TestCentric** runs under .NET 4.5 or higher, any tests that you cause to execute in-process will run under that version of the frameowrk. In some cases, this may give variable results. By default, tests are run out-of-process.

For more details of features available and how to use them, consult the online documentation. **[Under Development]**

## Getting Started
### Prerequisites

**TestCentric** requires .NET 4.5 or later in order to function. Your projects whose tests will run under **TestCentric** should already have some version of the NUnit framework installed.

### Installation

**TestCentric** is still under development. You may build it from the source on GitHub or download one of the pre-release builds as they come out. For a zip distribution, you should simply unzip the contents into a convenient directory and create your own shortcut to `tc.exe` and (if desired) `nunit-editor.exe`. To use it from the command-line, place the install directory on your path.

A `chocolatey` package will be developed soon and these instructions will be updated to cover it.

## Licensing

**TestCentric** is Open Source software, released under the MIT / X11 license. See LICENSE.txt for a copy of the license.

**TestCentric** bundles an unchanged binary copy of the NUnit test engine, version 3.9, which is also licensed under the MIT / X11 license. In addition, the first version of the GUI is based in part on the NUnit 2.x GUI runner, released under the NUnit license. See NOTICES.txt for copyright information.
