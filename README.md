# TestCentric Runner for NUnit - Version 1.0

[![Build status](https://ci.appveyor.com/api/projects/status/i7ymql47e8bo2rel/branch/main?svg=true)](https://ci.appveyor.com/project/CharliePoole/testcentric-gui/branch/main)

The **TestCentric Runner for NUnit** (aka **TestCentric**) is a GUI runner aimed at eventually supporting a range of .NET testing frameworks. In the version 1.0 release, we are concentrating on support of `NUnit` tests.

## Features

The initial release is based on the layout and feature set of the of the original NUnit GUI, with the internals modified so as to run NUnit 3 tests. See [CHANGES](./CHANGES.txt) for a list of the most important changes. See [ROADMAP](./ROADMAP.md) for info about future plans.

## Getting Started

### Prerequisites

**TestCentric** requires .NET 4.5 or later in order to function. Your projects whose tests will run under **TestCentric** should already have some version of the NUnit framework installed.

### Installation

**TestCentric** is released in two package formats, **zip** and **chocolatey**, both available from the [GitHub project](https://github.com/TestCentric/testcentric-gui/releases) site. The **chocolatey** package is also available at [chocolatey.org](https://chocolatey.org/packages/testcentric-gui).

The **chocolatey** package provides the best user experience and is the recommended way to install **TestCentric**. Follow the directions at [chocolatey.org](https://chocolatey.org/packages/testcentric-gui) or see the [INSTALL](./INSTALL.md) document. If you wish to install any NUnit engine extensions for use with the GUI, simply install them using `choco.exe` in the same way as the GUI.

To use the **zip** distribution, you should simply unzip the contents into a convenient directory and create your own shortcut to `testcentric.exe`. To use it from the command-line, place the install directory on your path. See the [INSTALL](./INSTALL.md) document for details, including installation of pre-release versions of **TestCentric**.

## Versioning

**TestCentric** uses semantic versioning for its version numbers, with specific exceptions and clarification regarding what is considered a breaking change. See the [VERSIONING](./VERSIONING.md) file for more information.

## Licensing

**TestCentric** is Open Source software, released under the MIT / X11 license. See LICENSE.txt for a copy of the license.

**TestCentric** bundles a modified copy of the NUnit test engine, which is also licensed under the MIT / X11 license. In addition, the first version of the GUI is based in part on the NUnit 2.x GUI runner, released under the NUnit license. See NOTICES.txt for copyright information.
