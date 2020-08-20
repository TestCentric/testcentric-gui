Title: TestCentric GUI Runner
---

NUnit V2 came with a GUI runner in addition to the console runner. Beginning with NUnit 3,
no GUI runner was provided. The **TestCentric Gui Runner** fills that need. It is able to run
tests written for either the NUnit 3 framework or (using a standard extension) the NUnit V2
framework.

# Prerequisites

The **TestCentric GUI** requires .NET 4.5 or later in order to function. Your own projects,
whose tests will run under it should already have some version of the NUnit framework installed.
Tests will run under the most appropriate environment available using agent processes. The test
machine must have installed the  runtime, which is targeted by your tests.

# Installation

The GUI is released in three package formats, **chocolatey**, **nuget** and **zip**, all
available from the [GitHub project](https://github.com/TestCentric/testcentric-gui/releases)
site. The **nunit package** is also available from [nunit.org](https://nunit.org) and the
**chocolatey** package from [chocolatey.org](https://chocolatey.org).

The **chocolatey** package provides the best user experience and is the recommended way to install the **TestCentric** GUI on developer machines. Extensions installed via **chocolatey**
will be recognized.

The **nuget** package is useful if you want to ensure that the GUI is available automatically to
every developer who works on a project. Extensions installed through **nuget** will be recognized.

To use the **zip** distribution, you should simply unzip the contents into a convenient directory and create your own shortcut to `testcentric.exe`. To use it from the command-line, place the install directory on your path.

# Versioning

The **TestCentric GUI** uses semantic versioning, with specific exceptions and clarification
regarding what is considered a breaking change. See the VERSIONING.md file in the root of the
distribution for more information. The current version is 1.4.0.

# License

The GUI is Open Source software, released under the MIT / X11 license. See LICENSE.txt in the root of the distribution or view it [here](/testcentric-gui/docs/license.html).

The software bundles a modified copy of the NUnit test engine, which is also licensed under the
MIT / X11 license. In addition, the first version of the GUI is based in part on the NUnit 2.x
GUI runner, released under the NUnit license. See NOTICES.txt in the root of the distribution
for more information.
