// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using TestCentric.Gui.Model;
using TestCentric.Gui.Presenters;

namespace TestCentric.Gui.Views
{
    public interface ITestResultSubView
    {
        /// <summary>
        /// Clear all content in the view
        /// </summary>
        void Clear();

        /// <summary>
        /// Load test outcome images 'Passed', 'Failed'...
        /// </summary>
        void LoadImages(OutcomeImageSet imageSet);

        /// <summary>
        /// View is separated in two section: Caption and detail section
        /// Caption section contain the overall outcome, duration and test + assertion count
        /// </summary>
        void UpdateCaption(TestResultCounts testCounts, ResultNode result);

        /// <summary>
        /// View is separated in two section: Caption and detail section
        /// Detail section contain the number of passed, failed tests
        /// </summary>
        void UpdateDetailSection(TestResultCounts summary);

        /// <summary>
        /// Show/hide the detail section
        /// </summary>
        void UpdateDetailSectionVisibility(bool visible);

        /// <summary>
        /// Shrink the view to show only the caption section
        /// </summary>
        void ShrinkToCaption();
    }
}
