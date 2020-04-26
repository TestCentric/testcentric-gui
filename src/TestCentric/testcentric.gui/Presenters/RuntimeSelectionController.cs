using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Windows.Forms;
using TestCentric.Common;
using TestCentric.Gui.Elements;
using TestCentric.Gui.Model;

namespace TestCentric.Gui.Presenters
{
    public class RuntimeSelectionController
    {
        private IMenu _runtimeMenu;
        private ITestModel _model;

        public RuntimeSelectionController(IMenu runtimeMenu, ITestModel model)
        {
            _runtimeMenu = runtimeMenu;
            _model = model;
        }

        public bool AllowRuntimeSelection()
        {
            var package = _model.TestPackage;
            return package != null
                && package.GetProcessModel() != "InProcess"
                && package.HasImageTargetFrameworkName();
        }

        public void PopulateMenu()
        {
            var package = _model.TestPackage;
            Guard.OperationValid(package != null,
                "PopulateMenu called without a loaded TestPackage");
            Guard.OperationValid(package.HasImageTargetFrameworkName(),
                "PopulateMenu called without an ImageTargetFrameworkName setting");

            string imageTargetSetting = package.GetImageTargetFrameworkName();
            var imageTargetRuntime = new FrameworkName(imageTargetSetting);

            // TODO: Temp fix due to different naming in FrameworkName and RuntimeFramework
            // Assumes only two runtime types for now.
            var validRuntimePrefix = imageTargetRuntime.Identifier == ".NETFramework"
                ? "net-"
                : "netcore-";

            var menuItems = _runtimeMenu.MenuItems;
            menuItems.Clear();

            var defaultMenuItem = new MenuItem("Default")
            {
                Name = "defaultMenuItem",
                Tag = "DEFAULT"
            };

            menuItems.Add(defaultMenuItem);

            string requestedRuntimeSetting = package.GetRequestedRuntimeFramework();

            // Add all runtimes, which are valid for the package
            foreach (var runtime in _model.AvailableRuntimes)
            {
                if (runtime.Id.StartsWith(validRuntimePrefix))
                    menuItems.Add(new MenuItem(runtime.DisplayName)
                    {
                        Tag = runtime.Id,
                        Enabled = runtime.FrameworkVersion >= imageTargetRuntime.Version
                    });
            }

            // Go through all menu items and check one
            bool isItemChecked = false;
            foreach (MenuItem item in menuItems)
            {
                if ((string)item.Tag == requestedRuntimeSetting)
                    item.Checked = isItemChecked = true;
                else
                    item.Click += (s, e) =>
                    {
                        item.Checked = true;
                        if (item.Tag == null || item.Tag as string == "DEFAULT")
                            _model.PackageOverrides.Remove(EnginePackageSettings.RequestedRuntimeFramework);
                        else
                            _model.PackageOverrides[EnginePackageSettings.RequestedRuntimeFramework] = item.Tag;

                        // Even though the _model has a Reload method, we cannot use it because Reload
                        // does not re-create the Engine.  Since we just changed a setting, we must
                        // re-create the Engine by unloading/reloading the tests. We make a copy of
                        // __model.TestFiles because the method does an unload before it loads.
                        _model.LoadTests(new List<string>(_model.TestFiles));
                    };
            }

            if (!isItemChecked)
            {
                defaultMenuItem.Checked = true;
                _model.TestPackage.Settings.Remove(EnginePackageSettings.RequestedRuntimeFramework);
                _model.PackageOverrides.Remove(EnginePackageSettings.RequestedRuntimeFramework);
            }
        }
    }
}
