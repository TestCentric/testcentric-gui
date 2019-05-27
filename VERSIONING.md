# TestCentric Runner Versioning

**TestCentric** packages use a version number in the form MAJOR.MINOR.PATCH. We change the

- MAJOR version when we have made incompatible API changes
- MINOR version when adding functionality in a backwards-compatible manner
- PATCH version when you making backwards-compatible bug fixes

What remains is to define the API or APIs we are protecting from incompatible changes and the degree of change that is needed before something is considered a breaking change.

## TestCentric APIs

First, let's note that the **TestCentric GUI** is not designed to be used as a library, so there is no ABI or library-like API to worry about. So what other APIs do we need to support? 

There seem to be two of them at this point:

1. The GUI interface itself, as presented to the user.
2. The command-line interface to the GUI.

### User Interface

The GUI presents the user with information (output) in a graphical format and allows the user to specify actions like running tests by interacting with it. In general, we are concerned about preserving the capabilities, which the GUI provides to the user, much more than we are about the specific appearance of the GUI elements. However, those elements may also be important in certain cases.

#### Examples of MAJOR UI Changes

- Removal of a capability. For example, if we were to remove the abiity, now provided, to automatically reload tests before running.
- Changing an existing UI element so it does something completely different.

#### Examples of MINOR UI Changes

- Provision of a new capability. F1or example, adding the ability to save test results in multiple formats.
- Changes to how a capability is accessed. For example, replacing a simple menu item with a dialog.
- Addition of a new UI element.

#### Examples of PATCH UI Changes
- Fixing a bug in how a command functions.
- Fixing alignment, font, appearance, etc. of elements in the UI.
- Restoring behavior of some capability to that originally designed.
- Changing the wording of a message.

### Command-Line

The command-line API is defined by the documentation for the GUI, which includes the help message the GUI itself displays and the command-line section of the published documentation on the TestCentric wiki.

#### Examples of MAJOR Command-Line Changes

- Complete removal of an option.
- Changing the behavior of an option so that it no longer does what it was defined to do.

#### Examples of MINOR Command-Line Changes

- Addition of a new option
- Modifying the behavior of an option so it does the same thing better or more effectively or has added suboptions.

#### Examples of PATCH Command-Line changes
- Fixing a bug in the behavior of an option.
- Restoring an option to its original defined behavior.
- Correcting spelling errors.


## Additional Considerations

Even in the case of non-breaking changes, as defined above, every effort will be made to avoid negative impact on users.

The included **Experimental GUI** is not subject to the above considerations. It is currently versioned separately from the standard GUI, with a MAJOR component of 0. The **Experimental GUI** may gain and lose features at any time until it becomes (as we eventually intend) the second major version of the GUI.