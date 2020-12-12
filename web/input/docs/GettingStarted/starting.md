Title: Starting the GUI
Description: Starting the TestCentric GUI.
Order: 2
---
# Shortcut

If you created a shortcut after the install, simply click it!

# Command Line

## Chocolatey Install

Did you install the chocolatey package? In that case just type

```cmd
testcentric
```

## Directory on Path

Otherwise, if you added the install directory to the path, type

```cmd
testcentric.exe
```

## Last Resort

If you did neither of the above, use the path to which the GUI was installed:

```cmd
<path to the install directory>\testcentric.exe (*)
```

(*) Of course, you would use a forward slash on Linux.

The trick is knowing where the GUI is installed. If you used NuGet, the installation may be in various
places, depending on the environment in which you are running. Consult the NuGet documentation for more
information.
