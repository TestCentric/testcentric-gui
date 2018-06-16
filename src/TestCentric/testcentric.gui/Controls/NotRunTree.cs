// ***********************************************************************
// Copyright (c) 2018 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Windows.Forms;

namespace TestCentric.Gui.Controls
{
    using Model;
    using Model.Settings;

    /// <summary>
    /// Summary description for NotRunTree.
    /// </summary>
    public class NotRunTree : TreeView, IViewControl
    {
        const string CHILD_IGNORED_MESSAGE = "One or more child tests were ignored";

        #region IViewControl Implementation

        public void InitializeView(ITestModel model, TestCentricPresenter presenter)
        {
            model.Events.TestLoaded += (TestNodeEventArgs e) =>
            {
                Nodes.Clear();
            };

            model.Events.TestUnloaded += (TestEventArgs e) =>
            {
                Nodes.Clear();
            };

            model.Events.TestReloaded += (TestNodeEventArgs e) =>
            {
                if (model.Services.UserSettings.Gui.ClearResultsOnReload)
                    Nodes.Clear();
            };

            model.Events.RunStarting += (RunStartingEventArgs e) =>
            {
                Nodes.Clear();
            };

            model.Events.TestFinished += (TestResultEventArgs e) =>
            {
                if (e.Result.Status == TestStatus.Skipped &&
                    e.Result.Site != FailureSite.Parent)
                {
                    this.AddNode(e.Result);
                }
            };

            model.Events.SuiteFinished += (TestResultEventArgs e) =>
            {
                // NOTE: Adhoc message test is needed due to an error in
                // the framework, whereby suites are not getting the
                // FailureSite set correctly.
                if (e.Result.Status == TestStatus.Skipped &&
                    e.Result.Site != FailureSite.Parent &&
                    e.Result.Site != FailureSite.Child &&
                    e.Result.Message != CHILD_IGNORED_MESSAGE)
                {
                    this.AddNode(e.Result);
                }
            };
        }

        #endregion

        #region Helper Methods

        private void AddNode(ResultNode result)
        {
            TreeNode node = new TreeNode(result.Name);
            TreeNode reasonNode = new TreeNode("Reason: " + result.Message);
            node.Nodes.Add(reasonNode);

            Nodes.Add(node);
        }

        #endregion
    }
}
