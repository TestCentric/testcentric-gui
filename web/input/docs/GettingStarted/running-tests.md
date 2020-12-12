Title: Running Tests
Description: How to load and run a set of tests under.
Order: 3
---
# Loading Tests

Normally, when you start the GUI, the most recently used test file will be loaded.

If this is the first time you have used the GUI or if you want to load a different file,
use the `File | Open` menu item, select a test assembly and open it. If you want to
re-open a file that has been used before, you may use the `File | Recent Files` menu item.

You may also select the file to open from the command-line, by putting it's path after
the name of the program when you execute it.

When a test assembly has been loaded, the tests it contains are shown in the tree display.

# Using the Run Button

The simplest way to run tests is to select the test or tests you want to run in the tree
display and then click the Run button.

Tests may be selected in two main ways.

1. Clicking on any test in the tree display highlights it, which means it has been selected.
While only one tree node may be selected in this way, all the tests under that node are also
considered to have been selected for execution.

2. If you right click in the tree display, the context menu allows you to enable the display
of checkboxes. When checkboxes are enabled, you may use them to select a single test or even
multiple tests.

# Using the Context Menu

You may run a test - and any tests under it - by right-clicking on the node in the tree display
and selecting `Run` in the menu that appears.

# Using the Tests Menu

The `Tests` menu includes three menu items for running tests, each one with a shortcut key defined:

* Run All (F5) - Runs all the tests.
* Run Selected (F6) - Runs the currently selected tests, just like the Run button.
* Run Failed (F7) - Runs the tests that failed in the previous run.
