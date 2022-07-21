// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCentric.Gui;

namespace TestCentric.TestUtilities
{
    public static class VisualStateFactory
    {
        public static VisualState CreateVisualState(string strategy, bool checkBoxes = false, params VisualTreeNode[] visualTreeNodes)
        {
            return CreateVisualState(strategy, null, checkBoxes, visualTreeNodes);
        }

        public static VisualState CreateVisualState(string strategy, string grouping, bool checkBoxes, params VisualTreeNode[] visualTreeNodes)
        {
            VisualState visualState;

            switch (strategy)
            {
                case "NUNIT_TREE":
                    visualState = new VisualState(strategy) { ShowCheckBoxes = checkBoxes };
                    break;
                case "FIXTURE_LIST":
                case "TEST_LIST":
                    visualState = new VisualState(strategy, grouping) { ShowCheckBoxes = checkBoxes };
                    break;
                default:
                    throw new Exception($"Unrecognized DisplayStrategy: {strategy}");
            }

            foreach (var visualTreeNode in visualTreeNodes)
                visualState.Nodes.Add(visualTreeNode);

            return visualState;
        }
    }
}
