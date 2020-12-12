Title: Installing the GUI
Description: How to install the TestCentric GUI.
Order: 1
---
# Prerequisites

The GUI itself requires runs under the .NET Framework version 4.5 or later, which must be installed
on the machine. If you want tests to run under other runtimes, those runtimes must also be installed.
Currently supported runtimes are .NET Framework 2.0 through 4.8 and .NET Core 2.1 through 3.1. Tests
targeting .NET core 1.0, 1.1 or 2.0 run under the 2.1 test agent.

# Choosing a Package

The GUI is released in three package formats, **chocolatey**, **nuget** and **zip**, all
available from the [GitHub project](https://github.com/TestCentric/testcentric-gui/releases)
site. The **nuget package** is also available from [nunit.org](https://nunit.org) and the
**chocolatey** package from [chocolatey.org](https://chocolatey.org).

For each release, all three packages contain the same binaries. The difference is in the user
experience provided by each packaging ecosystem.

## Chocolatey

The **chocolatey** package is recommended for most users. It provides the best user experience and
is the preferred way to install the **TestCentric** GUI on Windows developer machines. Unfortunately,
it is only available under Windows 7+ or Windows Server 2003+ and not at all under Linux. On GitHub,
the chocolatey download is named `testcentric-gui.x.y.z.nupkg`, where `x.y.z` is the version.

### Advantages

1. TestCentric is available centrally on the machine, ready for use with any project.
2. A simple `choco install` command installs TestCentric.
3. Engine Extensions may be installed at the same time or later using the same command.
4. A shim is created, allowing the `testcentric` command to work from the command-line.
5. Upgrading is done easily through `choco upgrade`.

### Drawbacks

1. Installation requires running as administrator.
2. Installation of chocolatey is slightly more complicated than one might like.

## Zip

The **zip** package is the easiest approach for individual developers without administrative
access to their machines. It is also the best way to try out a new version of the GUI
without replacing an older version installed through Chocolatey. It is also currently the
preferred approach for developers working on Linux. On GitHub, the zip download is named
`testcentric-gui-x.y.z.zip`, where `x.y.z` is the version.

### Advantages

1. Simple copy deployment with no administrative access needed.
2. May use a single central location on the machine.

### Drawbacks

1. There is currently no simple way to add Engine Extensions to the zip installation. (I'm working on this!)

## NuGet

Originally, there was no NuGet package. My feeling was that installing an executable tool, particularly a
GUI, is a bad use of NuGet. However, some users expected it to be available, so now it is.
Please consider the drawbacks before using it in your team.  On GitHub, the zip download is named
`TestCentric.GuiRunner.x.y.z.nupkg`, where `x.y.z` is the version.

### Advantages

1. You can install without leaving Visual Studio.

### Drawbacks

1. The install must be repeated for each project with which you want to use the GUI.
2. The package is restored whenever the project is opened, even in a headless CI environment.
3. The GUI and its dependencies may conflict with other installed runners, which use the NUnit Engine API.

# How To Install

## Chocolatey

1. Install chocolatey, if not already installed. See https://chocolatey.org/install.
2. Open an command prompt using `Run as Administrator`.
3. Issue command `choco install testcentric-gui`.
4. Optionally, create a desktop shortcut.

## Zip

1. Download `testcentric.gui-x.y.z.zip` from GitHub, where `x.y.z` is the version.
2. Unzip the contents into a convenient directory.
3. Optionally, create a shortcut to `testcentric.exe`.
4. Optionally, add the install directory to your path.

## NuGet

1. In Visual Studio, navigate to the project in which you want to install the GUI. Note that it doesn't
matter which project you choose but VS requires you to choose one.
2. Right click on either the project or its `References` and select `Manage NuGet`
3. Select `nuget.org` as the source and Browse for "TestCentric".
4. Select the `TestCentric.GuiRunner`
5. On the right, select "Latest Stable" version or choose a specific version. Click "Install".
