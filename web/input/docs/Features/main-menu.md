Description: Explains commands available in the main menu of the runner.
Order: 2
---
<!-- Page-specific styles -->
<style>
    h1 {clear: both}
    img {float: right; margin-left: 20px; margin-bottom: 20px}
</style>

# File Menu

![File Menu](/testcentric-gui/assets/img/filemenu.png)

<!---#### New Project...
Closes any open project, prompting the user to save it if it has been changed and then opens a
FileSave dialog to allow selecting the name and location of the new project.

## Open Project...
Closes any open project, prompting the user to save it if it has been changed and then opens a
FileOpen dialog to allow selecting the name and location of an assembly or a test project.--->

## Open...

Closes any open files and then displays a FileOpen dialog to allow selecting the name and location
of an assembly or supported project file.

## Close

Closes any open files.
<!---Closes any open project, prompting the user to save it if it has been changed.--->

## Add Test File...

Displays a FileOpen dialog to allow selecting the name and location of an assembly or
supported project file, which is then added to the currently open file set.

<!---## Save
Saves the currently open project. Opens the Save As dialog if this is the first time the project
is being saved.

## Save As...
Opens a FileSave dialog to allow specifying the name and location to which the project
should be saved.

## Reload Project
Completely reloads the current project by closing and re-opening it.--->

## Reload Tests

Reloads the tests, merging any changes into the tree.

## Select Runtime

Displays a list of runtime versions you may select in order to reload
the tests using that runtime. This submenu is only present if you have
more than one runtime version available.

## Process Model
Allows the user to select how Processes are used loading and running the tests. Note that any selection other than **Default** will override process settings specified in an NUnit or other project file.

* **Default**
The tests use the NUnit engine default, which is **Single** for one assembly, **Multiple** for more than one.

* **In Process**
Tests are run in the same process as the GUI. This is useful for some types of debugging.

* **Single**
Tests are all run in the same process, separate from the process running the GUI itself.

* **Multiple**
Each test assembly is run in its own process, in parallel with other assemblies.

* **Run as X86**
Force the tests to run in a 32-bit process or processes. This setting may be selected in combination with any of the other settings with the exception of **In Process**.

## Domain Usage
Allows the user to select how AppDomains are used in loading and running the tests. Note that any selection other than **Default** will override domain settings specified in an NUnit or other project file.

* **Default**
The tests use the NUnit engine default, which is **Single** for one assembly, **Multiple** for more than one.

* **Single**
Tests are all run in the same AppDomain. When running in process, that AppDomain is separate from the domain in which the GUI itself is running.

* **Multiple**
Each test assembly is run in its own AppDomain. This setting is not permitted when running each assembly in a separate process, since there is only one test assembly per process.

## Recent Files...
Displays a list of recently opened projects and assemblies from which the user is able to select one for opening.

## Exit
Closes and exits the application. If a test is running, the user is given the opportunity to
cancel it and or to allow it to continue.
<!---If the open project has any pending changes, the user
is given the opportunity to save it--->

# View Menu

![View Menu](/testcentric-gui/assets/img/viewmenu.png)

## Full Gui
Displays the complete gui - as in prior versions of NUnit. This includes the
   errors and failures and other tabs and the progress bar.
   
## Mini Gui
Switches the display to the mini-gui, which consists of the tree display only.

## GUI Font
Displays a submenu that allows changing the general font used by the GUI.

 * **Increase**

   Increases the size of the font.

 * **Decrease**

   Decreases the size of the font.

 * **Change...**

   Displays the Font Change dialog.

 * **Restore**

   Restores the default font.

## Fixed Font
Displays a submenu that allows changing the fixed font used to display
console output from the tests.

 * **Increase**

   Increases the size of the fixed font.</p>

 * **Decrease**

   Decreases the size of the fixed font.

 * **Restore**

   Restores the default fixed font.

<!---<h4>Assembly Details...</h4>
<p>Displays information about loaded test assemblies.</p>--->

## Status Bar
Displays or hides the status bar.

# Tests Menu

![Tests Menu](/testcentric-gui/assets/img/testsmenu.png)

## Run All

Runs all the tests.

## Run Selected

Runs the test or tests that are selected in the tree. If checkboxes are visible,
any checked tests are run by preference. This is the same function provided by
the Run button.

## Run Failed

Runs only the tests that failed on the previous run.

## Stop Run

Stops the test run. This is the same function provided by the Stop button. See
the [Main Window](/testcentric-gui/docs/main-window.html) page for a detailed description.

# Tools Menu

![Tools Menu](/testcentric-gui/assets/img/toolsmenu.png)

<!---<h4>Test Assemblies...</h4>
<p>Displays information about loaded test assemblies.</p>

<h4>Save Results as XML...</h4>--->

## Save Test Results...
Opens a FileSave Dialog for saving the test results as an nunit3-formatted XML file.

<!---<h4>Exception Details...</h4>
<p>Displays detailed information about the last exception.</p>

<h4>Open Log Directory...</h4>
<p>Opens the directory containing logs.--->

## Extensions...

Displays the [Extensions Dialog](/testcentric-gui/docs/extensions-dialog.html).

## Settings...

Displays the [Settings Dialog](/testcentric-gui/docs/settings-dialog.html).

# Help Menu

![Help Menu](/testcentric-gui/assets/img/helpmenu.png)

## TestCentric Help

Displays the TestCentric documentation.

## NUnit Help

Displays the NUnit documentation.

## About TestCentric...

Displays info about your version of TestCentric.
