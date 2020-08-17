Description: Shows to run TestCentric from the command line and lists available options.
Order: 8
---
**TestCentric** is invoked from the command line as follows:

```
TESTCENTRIC [inputfiles] [options]
```

For example,

```
TESTCENTRIC mytests.dll
TESTCENTRIC onetest.dll anothertest.dll --run
TESTCENTRIC myproject.nunit
```

If **TestCentric** is started without any files specified, it automatically loads the most recently loaded test file. Alternatively, you may specify one or more assemblies or supported project types. 

**NOTE:** The types of projects supported depends on the project loader extensions that are installed. Without any extensions installed, only assemblies (`.dll` or `.exe`) are permitted.

### Options Supported

Option            | Action
------------------|--------
--config=CONFIG   | Specify the CONFIG to use for any project files loaded
--noload          | Suppress loading of the most recent test file.
--run             | Automatically run the loaded tests.
--unattended      | In conjunction with --run, causes the GUI to exit immediately after running
--process=PROCESS | PROCESS isolation for test assemblies. Values: `Single`, `Separate`, `Multiple`. If not specified, defaults to `Separate` for a single assembly or `Multiple` for more than one.
--inprocess       | Synonym for `--process:Single`
--domain=DOMAIN   | DOMAIN isolation for test assemblies. Values: `None`, `Single`, `Multiple`. If not specified, defaults to `Single` for a single assembly or `Multiple` for more than one.
--x86             | Run tests in an X86 process on 64-bit systems.
--agents=NUMBER   | Specify the maximum NUMBER of test assembly agents to run at one time. If not specified, there is no limit.
--work=PATH       | PATH to directory used for any output files by default.
--trace=LEVEL     | Set internal trace level. Valid values are `Off`, `Error`, `Warning`, `Info` or `Debug`. `Verbose` is a synonym for `Debug`.
--help            | Display the help message and exit.
