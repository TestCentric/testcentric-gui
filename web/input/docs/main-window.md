Title: Main Window
Description: Describes the Main Window of the runner, where a user spends most of their time.
Order: 1
---

<!-- Page-specific styles -->
<style>
    h1 {clear: both}
    .table {width: 300px; float: right; margin-left: 30px}
</style>

The **TestCentric** Runner shows the tests in an tree display on the left of the main window and provides a visual indication of the success or failure of the tests. It allows you to selectively run single tests or suites and reloads automatically as you modify and re-compile your code. The following is a screenshot of the runner after running the tests in a sample assembly.

![TestCentric GUI Runner](/testcentric-gui/assets/img/testcentric.png)

In this example, there were a total of 31 test cases, of which 21 were run.

# Tree Display

Symbol | Test Status
-------|------------
![Success](/testcentric-gui/assets/img/Success.png) | Passed
![Failure](/testcentric-gui/assets/img/Failure.png) | Error or Failure
![Ignored](/testcentric-gui/assets/img/Ignored.png) | Warning or Ignored
![Skipped](/testcentric-gui/assets/img/Skipped.png) | Skipped
![Inconclusive](/testcentric-gui/assets/img/Inconclusive.png) | Inconclusive

The test tree uses both colors and symbols to indicate the test status.

**Note:** Tests marked with the `IgnoreAttribute` are shown in yellow immediately upon loading. Similarly, non-runnable tests (e.g.: wrong argument type) are shown in red immediately, without waiting for the user to press Run. Other statuses are not shown until after the test is run.

# Progress Bar

The progress bar shows the progress of the test. It is colored according to the "worst" result obtained: red if there were any failures, yellow if some tests were ignored and green for success.

# Result Summary
At the end of the test run, a summary of the results is displayed immediately below the progress bar. If the result information does not fit in the space available, hovering over it shows the full information.

# Result Tabs

Three tabs along the bottom of the display show the results of running a test.

## Errors, Failures and Warnings

The **Errors, Failures and Warnings** tab displays the error message and stack trace for both unexpected exceptions and assertion failures. Either the raw stacktrace or actual source code for each stack location can be displayed in this tab, provided that the program was compiled with debug information.

## Tests Not Run

The **Tests Not Run** tab provides a list of all tests that were selected for running but were not run, together with the reason.

## Text Output

The **Text Output** tab displays text output from the tests in a format similar to that shown by the console runner, including labels for the tests if specified by the user in the settings dialog.