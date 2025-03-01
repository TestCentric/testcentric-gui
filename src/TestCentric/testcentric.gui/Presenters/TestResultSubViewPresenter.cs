// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using TestCentric.Gui.Model;
using TestCentric.Gui.Model.Settings;
using TestCentric.Gui.Views;

namespace TestCentric.Gui.Presenters
{
    public interface ITestResultSubViewPresenter
    {
        /// <summary>
        /// Clear test result sub view
        /// </summary>
        void Clear();

        /// <summary>
        /// Update test result sub view to display result of testnode
        /// </summary>
        void Update(TestNode testNode);
    }

    public class TestResultSubViewPresenter : ITestResultSubViewPresenter
    {
        private ITestResultSubView _view;
        private ITestModel _model;

        public TestResultSubViewPresenter(ITestResultSubView view, ITestModel model)
        {
            _view = view;
            _model = model;

            string imageSet = _model.Settings.Gui.TestTree.AlternateImageSet;
            _view.LoadImages(imageSet);
        }

        /// <summary>
        /// Clear test result sub view
        /// </summary>
        public void Clear()
        {
            _view.Clear();
        }

        /// <summary>
        /// Update test result sub view to display result of testnode
        /// </summary>
        public void Update(TestNode testNode)
        {
            TestResultCounts summary = TestResultCounts.GetResultCounts(_model, testNode);
            ResultNode result = _model.GetResultForTest(testNode.Id);
            _view.UpdateCaption(summary, result);

            bool detailSectionVisible = testNode.IsAssembly || testNode.IsProject || testNode.IsSuite;
            _view.UpdateDetailSectionVisibility(detailSectionVisible);
            if (!detailSectionVisible)
            {
                _view.ShrinkToCaption();
                return;
            }

            _view.UpdateDetailSection(summary);
        }

        private void WireUpEvents()
        {
            _model.Settings.Changed += (object sender, SettingsEventArgs e) =>
            {
                if (e.SettingName == "TestCentric.Gui.TestTree.AlternateImageSet")
                    _view.LoadImages(_model.Settings.Gui.TestTree.AlternateImageSet);
            };
        }
    }
}
