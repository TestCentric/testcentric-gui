# What are Elements?

In developing the GUI under `System.Windows.Forms` with an MVP architecture, I wanted to make it easier to convert from Windows Forms to another platform in the future. UI Elements are designed to do that by providing an extra level of isolation between SWF and the presenters.

Each UI Element wraps one or more Windows items. Elements are created by the View, which contains properties that expose an interface for use by the Presenter.  In general, the Presenter only uses these interfaces.

For example, various means of running tests are provided in the GUI. Each of these is represented in a View and seen by a Presenter as a property of Type `ICommand`. Some of these commands are provided by SWF `Button` controls, some by `ToolStripItems`, some by context menus, etc. As a result, we are able to change the visual representation in various ways without affecting Presenter logic.

In the current implementation, there are two separate hierarchies of UI Elements: one for those that wrap SWF `Controls`, and one for `ToolStripItems`. Originally, we had a third hierarchy to represent SWF MenuItems, but that was eliminated after we converted to the use of `ToolStripMenuItems`.

**NOTE: Some items below are indicated as being Obsolete. They will be removed from this page once the code itself is removed.

## Interfaces

### IViewElement

`IViewElement` interface is implemented by all UI elements, either directly or indirecly. It provides the properties `Enabled`, `Visible` and `Text` and the method `InvokeIfRequired()`, which allows the Presenter to more easily access and modify properties created on the UI thread.

### IChecked

`IChecked` extends `IViewElement` to add a `Checked` property and a `CheckedChanged` event.

### ICommand

`ICommand` extends `IViewElement` to add an `Execute` event. 

### IControlElement

`IControlElement` is a bit of an anomaly. Unlike other interfaces, it may only be used by elements that are based on Windows Controls. It extends `IViewElement` to add properties `Location`, `Size`, `ClientSize`, `ContextMenu` and `ContextMenuStrip`.

**NOTE:** The `ContextMenu` property returns a Windows `Menu` instance and `ContextMenuStrip` returns a `ToolStripDropDownMenu`, both of these exposingt he underlying Windows implementation. This interface and its use will be reviewed in the future.

### IListBox

`IListBox` extends `IControlElement` to provide the properties `Items` and `SelectedItems`, event `DoubleClick` and methods `Add` and `Remove`. It exposed a great deal of the underlying Windows implementation and is no longer used. It is now marked as Obsolete.

### IMultiSelection

`IMultiSelection` extends IViewElement to provide a `SelectedItems` property and `SelectionChanged` event.

### IPopup

`IPopup` extends `IToolStripMenu` to add the `Popup` event.

### ISelection

`ISelection` extends `IViewElement` to add the properties `SelectedIndex` and `SelectedItem`, the `SelectionChanged` event and the `Refresh() method.

### IToolStripMenu

`IToolStripMenu` extends `IViewElement` to add a `MenuItems` property returning a colllection of Windows `ToolStripItems`.

**NOTE:** The exposure of the underlying implementation by the `MenuItems` property has proven necessary in order to allow the Presenter create some menus dynamically. We may review this in the future.

### IToolTip

`IToolTip` is an optional interface, which may be implemented by any element able to get and set its own tooltip text. It provides a single property, `ToolTipText`. It is implemented by our `ToolTipElement` class, making it available to all our tooltip elements.

## Control Elements

### ControlElement

`ControlElement` may be used directly to wrap any Windows `Control`. It is also used as a base class for control elements with added capabilities. It implements `IControlElement`.

### ButtonElement

A `ButtonElement` wraps a Windows `Button` control and implements `ICommand`. The class is no longer in use and is marked as Obsolete.

### CheckBoxElement

A `CheckBoxElement` wraps  a Windows `CheckBox` control and implements `IChecked`. Although Windows still uses the term "checked," the visual display may or may not use a check mark.

### ListBoxElement

A `ListBoxElement` wraps a Windows `ListBox` control and implements `IListBox`. The class is no longer in use and is marked as Obsolete.

### TabSelector

A `TabSelector` element wraps a Windows `TabControl` and implements `ISelection`.

## ToolStripItem Elements

### ToolStripElement

A `ToolStripElement` wraps any Windows `ToolStripItem` and implements `IViewElement` and `IToolTip`. It may be used directly in a View and also serves as the base class for ToolStrip elements with additional capabilities.

### CheckedMenuElement

`CheckedMenuELement` extends `ToolStripMenuElement` to implement `IChecked`.

### CheckedToolStripMenuGroup

`CheckedToolStripMenuGroup` wraps a set of Windows `ToolStripMenuItems` and implements `ISelection`. All wrapped items must be on the same `ToolStrip`.

### CommandMenuElement

`CommandMenuElement` extends `ToolStripMenuElement` to implement `ICommand`. This element should only be used for a menu item with no descendants, which causes an action.

### MultiCheckedToolStripButtonGroup

`MultiCheckedToolStripButtonGroup` wraps a set of Windows `ToolStripButtons` and implements `IMultiSelection`.

### PopupMenuElement

`PopupMenuElement` extends `ToolStripMenuElement` to implement `IPopup`.

### SplitButtonElement

`SplitButtonElement` wraps a Windows `ToolStripSplitButton` and implements `ICommand`. It is no longer used and is marked as Obsolete.

### ToolStripButtonElement

`ToolStripButtonElement` wraps a Windows `ToolStripButton` and implements both `IChecked` and `IChanged`.

### ToolStripMenuElement

`ToolStripMenuElement` wraps a Windows `ToolStripMenuItem`, extending the base class to provide a collection of subordinate Windows menu items.

**NOTE:** This direct exposure of the underlying Windows implementation is contrary to the general intent of UI elements, but is done as a matter of expediency. We will review this in the future.