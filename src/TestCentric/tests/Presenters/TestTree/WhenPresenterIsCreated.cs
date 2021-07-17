// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using NUnit.Framework;
using NSubstitute;

namespace TestCentric.Gui.Presenters.TestTree
{
    public class WhenPresenterIsCreated : TreeViewPresenterTestBase
    {
        [TestCase("RunCheckedCommand", false)]
        [TestCase("DebugCheckedCommand", false)]
        public void CheckElementVisibility(string propName, bool visible)
        {
            ViewElement(propName).Received().Visible = visible;
        }

        [Test]
        public void AlternateImageSetIsSet()
        {
            string imageSet = _settings.Gui.TestTree.AlternateImageSet;
            _view.Received().AlternateImageSet = imageSet;
        }

        [Test]
        public void ShowCheckBoxesIsSet()
        {
            bool showCheckBoxes = _settings.Gui.TestTree.ShowCheckBoxes;
            _view.ShowCheckBoxes.Received().Checked = showCheckBoxes;
        }

        [Test]
        public void StrategyIsSet()
        {
            string displayFormat = _settings.Gui.TestTree.DisplayFormat.ToUpperInvariant();

            switch (displayFormat)
            {
                case "NUNIT_TREE":
                    Assert.That(_presenter.Strategy, Is.TypeOf<NUnitTreeDisplayStrategy>());
                    break;
                case "TEST_LIST":
                    Assert.That(_presenter.Strategy, Is.TypeOf<FixtureListDisplayStrategy>());
                    break;
                case "FIXTURE_LIST":
                    Assert.That(_presenter.Strategy, Is.TypeOf<TestListDisplayStrategy>());
                    break;
                default:
                    Assert.Fail($"{displayFormat} is not a valid display format");
                    break;
            }
        }
    }
}
