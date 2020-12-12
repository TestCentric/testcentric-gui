Title: Stopping a Test Run
Description: How to stop a test run without waiting for it to complete.
Order: 4
---
# Normal Stop

Once a test run begins, the Stop button is enabled. Clicking it initiates a normal stop.
We often call this a "cooperative stop" because it requires cooperation from the test
framework.

When a Stop is initiated, a command is set to the framework. The intent is that it should
allow all executing tests and all teardowns to complete. No further tests should be started.
This usually terminates the run quickly, unless there is a problem like an infinite loop
in the test code.

# Forced Stop

As soon as the Stop is initiated, the Stop button text changes to "Forced Stop." The user
is able to observe the completion of each test in the GUI and may decide that one of the 
tests is hung and needs to be stopped forcibly. Clicking "Forced Stop" initiates that process.
The framework is expected to terminate all threads, which are running tests and return.

# Last Resort

Some frameworks may not support Forced Stop or it may not work due to a bug. After waiting
a period of time (currently 5 seconds) for the run to terminate, the GUI takes the extreme
action of unloading all test AppDomains and Processes.
