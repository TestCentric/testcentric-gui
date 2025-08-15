# TestCentric Runner  - Version 2.0

[![Build status](https://ci.appveyor.com/api/projects/status/i7ymql47e8bo2rel/branch/main?svg=true)](https://ci.appveyor.com/project/CharliePoole/testcentric-gui/branch/main)

The **TestCentric Runner** (aka **TestCentric**) is a GUI runner aimed that allows you to run `NUnit` test, but also aimed at supporting additional .NET testing frameworks in the future.

The **TestCentric Test Engine** is included with the GUI and is also available as an independent package.

## Features

## Getting Started

### Prerequisites

**TestCentric** requires .NET 4.6.2 or later in order to function. Your projects whose tests will run under **TestCentric** should already have some version of the NUnit framework installed.

### Installation

**TestCentric** is released in two package formats, **nuget** and **chocolatey**, both available from the [GitHub project](https://github.com/TestCentric/testcentric-gui/releases) site. The **nuget** package is also available at [nuget.org](https://www.nuget.org/packages/TestCentric.GuiRunner) and the **chocolatey** package is available at [chocolatey.org](https://chocolatey.org/packages/testcentric-gui).

The **chocolatey** package provides the best user experience and is the recommended way to install **TestCentric**. Follow the directions at [chocolatey.org](https://chocolatey.org/packages/testcentric-gui) or see the [INSTALL](./INSTALL.md) document. If you wish to install any NUnit engine extensions for use with the GUI, simply install them using `choco.exe` in the same way as the GUI.

## Versioning

**TestCentric** uses semantic versioning for its version numbers, with specific exceptions and clarification regarding what is considered a breaking change. See the [VERSIONING](./VERSIONING.md) file for more information.

## Licensing

**TestCentric** is an Open Source software, released under the MIT / X11 license. See [LICENSE](./LICENSE.txt) for a copy of the license.

The **TestCentric Engine** is based on the NUnit test engine, which is also licensed under the MIT / X11 license. See [NOTICES](./NOTICES.txt) for copyright information.
