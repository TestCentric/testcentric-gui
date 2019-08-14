# TestCentric Installation

**TestCentric** is released in two package formats, **zip** and **chocolatey**, both available from the [GitHub project](https://github.com/TestCentric/testcentric-gui/releases) site. The **chocolatey** package is also available at [chocolatey.org](https://chocolatey.org/packages/testcentric-gui).

## Chocolatey Installation

This is the recommended way to install TestCentric on a developer machine. It provides the best user experience and creates shim commands for both the standard and the experimental GUIs so they may be invoked from any directory.

**NOTE:** As an executable tool, TestCentric is not designed to be installed in individual projects but is intended to exist in a single version, centrally available on the developer's machine. If special cases call for a different approach, you may download the **zip** package.

### Common Steps

If you have not used chocolatey before, you should first install it on your machine. See https://chocolatey.org/install for the latest instructions.

All chocolatey install and uninstall commands should be run as administrator. Start a command prompt, running as administrator. You will not be able to successfully complete the installation from a normal user prompt.

If an earlier version of the GUI is already installed, you may either uninstall it or add the `--force` option to any of the install commands. Alternatively, if upgrading, you may use the `choco upgrade` command in lieu of `choco install`.

### Uninstalling an Earlier Release

Run `choco uninstall testcentric-gui`

### Installing a Normal Release

Run `choco install testcentric-gui`

### Installing a Pre-Release

Run `choco install testcentric-gui --pre`

### Installing a Development Release

We currently create development releases each time a change is merged to master. This is a special type of pre-release using the `dev` suffix and located on our `MyGet` feed.

Run `choco install testcentric-gui --pre -s http://www.myget.org/F/testcentric`

## Zip Installation

Download the zip file from GitHub and unzip it into any convenient directory. To use it from the command line, you may add the directory to your path. You may also create a desktop shortcut to `testcentric.exe` and / or `tc-next.exe`.
