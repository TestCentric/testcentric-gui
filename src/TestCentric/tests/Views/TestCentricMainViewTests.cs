// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Windows.Forms;
using NUnit.Framework;

namespace TestCentric.Gui.Views
{
    [TestFixture]
    public class TestCentricMainViewTests : ControlTester
    {
        [OneTimeSetUp]
        public void CreateForm()
        {
            this.Control = new TestCentricMainView();
        }

        [OneTimeTearDown]
        public void CloseForm()
        {
            this.Control.Dispose();
        }

        [Test]
        public void DisplayFormat_Toolstrip_ExistsWithSubItems()
        {
            var displayFormatbutton = GetToolStripItem<ToolStripDropDownButton>("displayFormatButton");
            Assert.That(displayFormatbutton, Is.Not.Null);

            var menuItem = GetDropDownItem<ToolStripMenuItem>(displayFormatbutton, "nunitTreeMenuItem");
            Assert.That(menuItem, Is.Not.Null);

            menuItem = GetDropDownItem<ToolStripMenuItem>(displayFormatbutton, "fixtureListMenuItem");
            Assert.That(menuItem, Is.Not.Null);

            menuItem = GetDropDownItem<ToolStripMenuItem>(displayFormatbutton, "testListMenuItem");
            Assert.That(menuItem, Is.Not.Null);
        }

        [Test]
        public void NUnitDisplayFormat_Checked_AllSubmenuItems_Exists()
        {
            var displayFormatbutton = GetToolStripItem<ToolStripDropDownButton>("displayFormatButton");
            Assert.That(displayFormatbutton, Is.Not.Null);

            var menuItem = GetDropDownItem<ToolStripMenuItem>(displayFormatbutton, "nunitTreeMenuItem");
            Assert.That(menuItem, Is.Not.Null);

            menuItem.Checked = true;
            var subMenuItem = GetDropDownItem<ToolStripMenuItem>(menuItem, "nunitTreeShowNamespaceMenuItem");
            Assert.That(subMenuItem, Is.Not.Null);
        }

        [Test]
        public void NUnitDisplayFormat_NotChecked_Submenu_IsEmpty()
        {
            var displayFormatbutton = GetToolStripItem<ToolStripDropDownButton>("displayFormatButton");
            Assert.That(displayFormatbutton, Is.Not.Null);

            var menuItem = GetDropDownItem<ToolStripMenuItem>(displayFormatbutton, "nunitTreeMenuItem");
            Assert.That(menuItem, Is.Not.Null);

            menuItem.Checked = false;
            Assert.That(menuItem.DropDownItems.Count, Is.EqualTo(0));
        }

        [Test]
        public void FixtureListFormat_Checked_AllSubmenuItems_Exists()
        {
            var displayFormatbutton = GetToolStripItem<ToolStripDropDownButton>("displayFormatButton");
            Assert.That(displayFormatbutton, Is.Not.Null);

            var menuItem = GetDropDownItem<ToolStripMenuItem>(displayFormatbutton, "fixtureListMenuItem");
            Assert.That(menuItem, Is.Not.Null);

            menuItem.Checked = true;
            var subMenuItem = GetDropDownItem<ToolStripMenuItem>(menuItem, "byDurationMenuItem");
            Assert.That(subMenuItem, Is.Not.Null);

            subMenuItem = GetDropDownItem<ToolStripMenuItem>(menuItem, "byCategoryMenuItem");
            Assert.That(subMenuItem, Is.Not.Null);

            subMenuItem = GetDropDownItem<ToolStripMenuItem>(menuItem, "byOutcomeMenuItem");
            Assert.That(subMenuItem, Is.Not.Null);
        }

        [Test]
        public void FixtureListFormat_NotChecked_Submenu_IsEmpty()
        {
            var displayFormatbutton = GetToolStripItem<ToolStripDropDownButton>("displayFormatButton");
            Assert.That(displayFormatbutton, Is.Not.Null);

            var menuItem = GetDropDownItem<ToolStripMenuItem>(displayFormatbutton, "fixtureListMenuItem");
            Assert.That(menuItem, Is.Not.Null);

            menuItem.Checked = false;
            Assert.That(menuItem.DropDownItems.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestListFormat_Checked_AllSubmenuItems_Exists()
        {
            var displayFormatbutton = GetToolStripItem<ToolStripDropDownButton>("displayFormatButton");
            Assert.That(displayFormatbutton, Is.Not.Null);

            var menuItem = GetDropDownItem<ToolStripMenuItem>(displayFormatbutton, "testListMenuItem");
            Assert.That(menuItem, Is.Not.Null);

            menuItem.Checked = true;
            var subMenuItem = GetDropDownItem<ToolStripMenuItem>(menuItem, "byDurationMenuItem");
            Assert.That(subMenuItem, Is.Not.Null);

            subMenuItem = GetDropDownItem<ToolStripMenuItem>(menuItem, "byCategoryMenuItem");
            Assert.That(subMenuItem, Is.Not.Null);

            subMenuItem = GetDropDownItem<ToolStripMenuItem>(menuItem, "byOutcomeMenuItem");
            Assert.That(subMenuItem, Is.Not.Null);

            subMenuItem = GetDropDownItem<ToolStripMenuItem>(menuItem, "byAssemblyMenuItem");
            Assert.That(subMenuItem, Is.Not.Null);

            subMenuItem = GetDropDownItem<ToolStripMenuItem>(menuItem, "byFixtureMenuItem");
            Assert.That(subMenuItem, Is.Not.Null);
        }

        [Test]
        public void TestListFormat_NotChecked_Submenu_IsEmpty()
        {
            var displayFormatbutton = GetToolStripItem<ToolStripDropDownButton>("displayFormatButton");
            Assert.That(displayFormatbutton, Is.Not.Null);

            var menuItem = GetDropDownItem<ToolStripMenuItem>(displayFormatbutton, "testListMenuItem");
            Assert.That(menuItem, Is.Not.Null);

            menuItem.Checked = false;
            Assert.That(menuItem.DropDownItems.Count, Is.EqualTo(0));
        }

        T GetDropDownItem<T>(ToolStripMenuItem menuItem, string name) where T : ToolStripItem
        {
            foreach (ToolStripItem ctl in menuItem.DropDownItems)
            {
                if (ctl.Name == name)
                    return ctl as T;
            }

            return null;
        }

        T GetDropDownItem<T>(ToolStripDropDownButton dropDownButton, string name) where T : ToolStripItem
        {
            foreach (ToolStripItem ctl in dropDownButton.DropDownItems)
            {
                if (ctl.Name == name)
                    return ctl as T;
            }

            return null;
        }

        T GetToolStripItem<T>(string name) where T : ToolStripItem
        {
            ToolStrip toolStrip = GetToolStrip();
            foreach (ToolStripItem ctl in toolStrip.Items)
            {
                if (ctl.Name == name)
                    return ctl as T;
            }

            return null;
        }

        ToolStrip GetToolStrip()
        {
            foreach (Control ctl in _control.Controls)
            {
                if (ctl.GetType() == typeof(ToolStrip))
                    return ctl as ToolStrip;
            }

            return null;
        }
    }
}
