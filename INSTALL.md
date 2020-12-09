# TestCentric Installation

**TestCentric** is released in two package formats, **zip** and **chocolatey**, both available from the [GitHub project](https://github.com/TestCentric/testcentric-gui/releases) site. The **chocolatey** package is also available at [chocolatey.org](https://chocolatey.org/packages/testcentric-gui).

## Chocolatey Installation

This is the recommended way to install TestCentric on a developer machine. It provides the best user experience and creates shim commands for both the standard and the experimental GUIs so they may be invoked from any directory. In addition, it provides a simple way to install extensions to the NUnit engine.

> _**NOTE:** As an executable tool, TestCentric is not designed to be installed in individual projects but is intended to exist in a single version, centrally available on the developer's machine. If special cases call for a different approach, you may download the **zip** package and use it as a base for your own installation._

### Common Steps

If you have not used chocolatey before, you should first install it on your machine. See https://chocolatey.org/install for the latest instructions.

All chocolatey install and uninstall commands should be run as administrator. Start a command prompt, running as administrator. You will not be able to successfully complete the installation from a normal user prompt.

If an earlier version of the GUI is already installed, you may either uninstall it or add the `--force` option to any of the install commands. Alternatively, if upgrading, you may use the `choco upgrade` command in lieu of `choco install`.

### Uninstalling an Earlier Release

```
choco uninstall testcentric-gui
```

### Installing the latest production Release

```
choco install testcentric-gui
```

### Installing the latest Pre-Release

```
choco install testcentric-gui --pre
```

### Installing a Development Release

We currently create development releases each time a change is merged to main. This is a special type of pre-release using the `dev` suffix and located on our `MyGet` feed.
.
```
choco install testcentric-gui --pre -s http://www.myget.org/F/testcentric
```

### Installing Engine Extensions

 If you wish to install any NUnit engine extensions for use with the GUI, simply install them using `choco.exe` in the same way as the GUI. They will be found and loaded automatically by the **chocolatey** installation of the GUI. Extensions intended to be used with **chocolatey** have ids starting with "nunit-extension-". See current extensions at [chocolatey.org](https://chocolatey.org/packages?q=nunit-extension-).

## Zip Installation

Download the zip file from GitHub and unzip it into any convenient directory. To use it from the command line, you may add the directory to your path. You may also create a desktop shortcut to `testcentric.exe` and / or `tc-next.exe`.

> _**NOTE:** The executables `testcentric.exe` and `tc-next.exe` are the only ones intended to be used directly. The `testcentric-agent.exe` and `testcentric-agent-x86.exe` executables are used internally by the NUnit engine._

### Installing Engine Extensions

Unfortunately, installing extensions using the zip distribution is currently a "do it yourself" project. Consider using the **chocolatey** package instead. The following steps are one way of doing it if you need to:

1. Decide where you will keep the extensions themselves. For this example, I am creating a subdirectory `addins` in the unzip folder.

2. Download `zip` packages of each extension you need. Copy the contents into the `addins` directory.

3. Create a file of type `.addins` in the folder where you unzipped **TestCentric**. The name of the file is not significant, only the extension. Enter the relative path to each extension assembly, one line per assembly, in this file. Do not enter the name of support assemblies that don't contain actual extensions. You may need to consult the documentation for each extension or the source code in some cases.

Your addins file will look something like this...

```
addins/FirstExtension.dll
addins/SecondExtension.dll
```


