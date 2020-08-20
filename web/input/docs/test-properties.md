Description: Displays details about an individual test and its execution.
Order: 7
---
<!-- Page-specific styles -->
<style>
  img {float:right; margin-left: 20px; margin-bottom: 20px; max-width: 400px}
</style>

The Test Properties Dialog is displayed using either the View | Properties menu item on the main
menu or the Properties item on the context menu. It shows information about the test and - if it
has been run - about the results. The dialog contains a "pin" button in the upper right corner,
which causes it to remain open as the user clicks on different tests.

![Context Menu](/testcentric-gui/assets/img/testPropertiesDialog.png)

## Header
The header at the top of the dialog shows the status of the test and its name.

If the test has not been run, the status may be one of 
* Runnable
* Not Runnable
* Explicit
* Ignored

If the test has been run, it may show any valid result, including
* Passed
* Failed
* Warning
* Skipped
* Inconclusive

**Note:** Some test statuses may include a second field, like `Skipped:Ignored`. This second
part is potentially user-extensible, so a complete list is not possible.


## Test Details
This section is present for every test.

### Test Type
The type of test, such as Test Case, Test Fixture, etc.

### Full Name
The full name of the test.

### Description
Any description provided by the definition of the test.

### Categories
A list of categories applied to the test

### Test Count
The number of test cases included in this test

### Should Run?
Yes if the test should be executed, otherwise No

### Reason
The reason given for not executing the test, if present

### Properties
A list of properties defined for this test

### Display hidden properties
Check this box to cause any hidden (internal) properties to be included in the list of properties.

## Results
This section only appears if the test was actually executed.

### Execution Time
The time in seconds that it took for the test to run.

### Assert Count
The number of assertions executed by the test.

### Message
The failure or other message issued by the test

### Stack
Stack trace, if present.
