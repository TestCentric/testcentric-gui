// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using TestCentric.Gui.Model;

namespace TestCentric.Gui.Presenters
{
    public interface ITreeDisplayStrategy
    {
        /// <summary>
        /// Called when a test is loaded: build of all tree nodes and apply VisualState
        /// (for example: expand/collapse nodes)
        /// </summary>
        void OnTestLoaded(TestNode testNode, VisualState visualState);

        /// <summary>
        /// Called when a test is unloaded: clear all tree nodes
        /// </summary>
        void OnTestUnloaded();

        /// <summary>
        /// Reload tree: clear all tree nodes first and rebuild all nodes afterwards
        /// </summary>
        void Reload();

        /// <summary>
        /// Save the visual state of the tree display strategy into a file
        /// </summary>
        void SaveVisualState();

        /// <summary>
        /// Called when one test is finished: update tree node according to test result
        /// </summary>
        void OnTestFinished(ResultNode result);

        /// <summary>
        /// Called when a test run is starting
        /// </summary>
        void OnTestRunStarting();

        /// <summary>
        /// Collapse all tree nodes beneath the fixture nodes
        /// </summary>
        void CollapseToFixtures();
    }
}
