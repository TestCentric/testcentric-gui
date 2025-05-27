// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NSubstitute;
using NUnit.Engine;
using NUnit.Framework;
using TestCentric.Gui.Model;
using TestCentric.Gui.Model.Settings;
using TestCentric.Gui.Views;

namespace TestCentric.Gui.Presenters
{
    public class ImageSetManagerTests
    {
        private ImageSetManager _manager;
        private ITestModel _model;
        private IMainView _mainView;
        private Fakes.UserSettings _settings;

        [SetUp]
        public void CreateManager()
        {
            _model = Substitute.For<ITestModel>();
            _mainView = Substitute.For<IMainView>();
            _settings = new Fakes.UserSettings();
            _model.Settings.Returns(_settings);

            Assert.That(_mainView.TreeView, Is.Not.Null);
            Assert.That(_mainView.TestResultSubView, Is.Not.Null);
            Assert.That(_mainView.StatusBarView, Is.Not.Null);

            _manager = new ImageSetManager(_model, _mainView);
        }

        [TestCase("Circles")]
        [TestCase("Default")]
        [TestCase("Visual Studio")]
        public void AllImageSetsAreFound(string name)
        {
            Assert.That(_manager.ImageSets.ContainsKey(name));
        }

        [Test]
        public void DefaultImageSetIsDefault()
        {
            Assert.That(_manager.CurrentImageSet.Name, Is.EqualTo("Default"));
        }

        [Test]
        public void CanChangeCurrentImageSet()
        {
            OutcomeImageSet imgSet = _manager.LoadImageSet("Visual Studio");
            Assert.That(imgSet.Name, Is.EqualTo("Visual Studio"));
            Assert.That(_manager.CurrentImageSet, Is.SameAs(imgSet));
        }

        [Test]
        public void SettingCurrentImageSetToInvalidValueThrows()
        {
            Assert.That(() => _manager.LoadImageSet("NonExistent"), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void WhenManagerIsCreated_TreeViewImagesAreSet()
        {
            _mainView.TreeView.Received().OutcomeImages = Arg.Is<OutcomeImageSet>((set) => set.Name == "Default");
        }

        [Test]
        public void WhenManagerIsCreated_TestResultImagesAreSet()
        {
            _mainView.TestResultSubView.Received().LoadImages(Arg.Is<OutcomeImageSet>((set) => set.Name == "Default"));
        }

        [Test]
        public void WhenManagerIsCreated_StatusBarImagesAreSet()
        {
            _mainView.StatusBarView.Received().LoadImages(Arg.Is<OutcomeImageSet>((set) => set.Name == "Default"));
        }

        [Test]
        public void WhenImageSetChanges_TreeView_ImageSetIsSet()
        {
            _mainView.TreeView.ClearReceivedCalls();
            string newImageSet = "Visual Studio";
            _settings.Gui.TestTree.AlternateImageSet = newImageSet;

            Raise.Event<Model.Settings.SettingsEventHandler>(this, "Gui.TestTree.AlternateImageSet");
            Assert.That(_manager.CurrentImageSet.Name, Is.EqualTo(newImageSet));

            _mainView.TreeView.Received().OutcomeImages = Arg.Is<OutcomeImageSet>((set) => set.Name == newImageSet);
        }

        [Test]
        public void WhenImageSetChanges_TestResult_ImageSetIsSet()
        {
            _mainView.TestResultSubView.ClearReceivedCalls();
            string newImageSet = "Visual Studio";
            _settings.Gui.TestTree.AlternateImageSet = newImageSet;

            Raise.Event<Model.Settings.SettingsEventHandler>(this, "Gui.TestTree.AlternateImageSet");
            Assert.That(_manager.CurrentImageSet.Name, Is.EqualTo(newImageSet));

            _mainView.TestResultSubView.Received().LoadImages(Arg.Is<OutcomeImageSet>((set) => set.Name == newImageSet));
        }

        [Test]
        public void WhenImageSetChanges_StatusBarImagesAreSet()
        {
            _mainView.StatusBarView.ClearReceivedCalls();
            string newImageSet = "Visual Studio";
            _settings.Gui.TestTree.AlternateImageSet = newImageSet;

            Raise.Event<Model.Settings.SettingsEventHandler>(this, "Gui.TestTree.AlternateImageSet");
            Assert.That(_manager.CurrentImageSet.Name, Is.EqualTo(newImageSet));

            _mainView.StatusBarView.Received().LoadImages(Arg.Is<OutcomeImageSet>((set) => set.Name == newImageSet));
        }
    }
}
