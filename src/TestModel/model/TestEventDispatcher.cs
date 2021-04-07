// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Xml;
using NUnit.Engine;

namespace TestCentric.Gui.Model
{
    public class TestEventDispatcher : ITestEvents, ITestEventListener
    {
        private TestModel _model;

        private Dictionary<string, ProjectInfo> _projectLookup;

        private class ProjectInfo
        {
            public TestNode ProjectNode;
            public bool WasStarted;
            public int AssembliesToRun;

            public ProjectInfo(TestNode projectNode)
            {
                ProjectNode = projectNode;
                WasStarted = false;
                AssembliesToRun = 0;
            }
        }

        public TestEventDispatcher(TestModel model)
        {
            _model = model;
        }

        #region Public Methods to Fire Events

        public void FireTestsLoading(IList<string> files)
        {
            TestsLoading?.Invoke(new TestFilesLoadingEventArgs(files));
        }

        public void FireTestsReloading()
        {
            TestsReloading?.Invoke(new TestEventArgs());
        }

        public void FireTestsUnloading()
        {
            TestsUnloading?.Invoke(new TestEventArgs());
        }

        public void FireTestChanged()
        {
            TestChanged?.Invoke(new TestEventArgs());
        }

        public void FireTestLoaded(TestNode testNode)
        {
            TestLoaded?.Invoke(new TestNodeEventArgs(testNode));
        }

        public void FireTestUnloaded()
        {
            TestUnloaded?.Invoke(new TestEventArgs());
        }

        public void FireTestReloaded(TestNode testNode)
        {
            TestReloaded?.Invoke(new TestNodeEventArgs(testNode));
        }

        public void FireTestLoadFailure(Exception ex)
        {
            TestLoadFailure?.Invoke(new TestLoadFailureEventArgs(ex));
        }

        public void FireSelectedItemChanged(ITestItem testItem)
        {
            SelectedItemChanged?.Invoke(new TestItemEventArgs(testItem));
        }

        public void FireCategorySelectionChanged()
        {
            CategorySelectionChanged?.Invoke(new TestEventArgs());
        }

        #endregion

        #region ITestEvents Implementation

        // Test loading events
        public event TestFilesLoadingEventHandler TestsLoading;
        public event TestEventHandler TestsReloading;
        public event TestEventHandler TestsUnloading;
        public event TestEventHandler TestChanged;

        public event TestNodeEventHandler TestLoaded;
        public event TestNodeEventHandler TestReloaded;
        public event TestEventHandler TestUnloaded;

        public event TestLoadFailureEventHandler TestLoadFailure;

        // Test running events
        public event RunStartingEventHandler RunStarting;
        public event TestResultEventHandler RunFinished;

        public event TestNodeEventHandler SuiteStarting;
        public event TestResultEventHandler SuiteFinished;

        public event TestNodeEventHandler TestStarting;
        public event TestResultEventHandler TestFinished;

        public event TestOutputEventHandler TestOutput;

        public event UnhandledExceptionEventHandler UnhandledException;

        // Test Selection Event
        public event TestItemEventHandler SelectedItemChanged;

        public event TestEventHandler CategorySelectionChanged;

        #endregion

        #region ITestEventListener Implementation

        public void OnTestEvent(string report)
        {
            XmlNode xmlNode = XmlHelper.CreateXmlNode(report);

            switch (xmlNode.Name)
            {
                case "start-test":
                    InvokeHandler(TestStarting, new TestNodeEventArgs(new TestNode(xmlNode)));
                    break;

                case "start-suite":
                    var testNode = new TestNode(xmlNode);

                    CheckForProjectStart(testNode);

                    InvokeHandler(SuiteStarting, new TestNodeEventArgs(testNode));
                    break;

                case "start-run":
                    InitializeProjectDictionary();
                    InvokeHandler(RunStarting, new RunStartingEventArgs(xmlNode.GetAttribute("count", -1)));
                    break;

                case "test-case":
                    ResultNode resultNode = new ResultNode(xmlNode);
                    _model.Results[resultNode.Id] = resultNode;
                    InvokeHandler(TestFinished, new TestResultEventArgs(resultNode));
                    break;

                case "test-suite":
                    resultNode = new ResultNode(xmlNode);
                    _model.Results[resultNode.Id] = resultNode;

                    CheckForProjectFinish(resultNode);

                    InvokeHandler(SuiteFinished, new TestResultEventArgs(resultNode));
                    break;

                case "test-run":
                    resultNode = new ResultNode(xmlNode);
                    _model.Results[resultNode.Id] = resultNode;
                    InvokeHandler(RunFinished, new TestResultEventArgs(resultNode));
                    break;

                case "test-output":
                    string testName = xmlNode.GetAttribute("testname");
                    string stream = xmlNode.GetAttribute("stream");
                    string text = xmlNode.InnerText;
                    InvokeHandler(TestOutput, new TestOutputEventArgs(testName, stream, text));
                    break;

                case "unhandled-exception":
                    string message = xmlNode.GetAttribute("message");
                    string stackTrace = xmlNode.GetAttribute("stacktrace");

                    InvokeHandler(UnhandledException, new UnhandledExceptionEventArgs(message, stackTrace));
                    break;
            }
        }

        #endregion

        #region Helper Methods

        private void InvokeHandler(MulticastDelegate handlerList, EventArgs e)
        {
            if (handlerList == null)
                return;

            object[] args = new object[] { e };
            foreach (Delegate handler in handlerList.GetInvocationList())
            {
                object target = handler.Target;
                System.Windows.Forms.Control control
                    = target as System.Windows.Forms.Control;

                if (control != null && control.InvokeRequired)
                        control.Invoke(handler, args);
                    else
                        handler.Method.Invoke(target, args);
            }
        }

        private void InitializeProjectDictionary()
        {
            // Initialize Dictionary to look up projects to which assemblies belong
            _projectLookup = new Dictionary<string, ProjectInfo>();

            foreach (var projectNode in _model.Tests.Select(tn => tn.Type == "Project"))
            {
                var projectInfo = new ProjectInfo(projectNode);

                foreach (var child in projectNode.Select((tn) => tn.Type == "Assembly"))
                {
                    projectInfo.AssembliesToRun++;
                    _projectLookup.Add(child.GetAttribute("id"), projectInfo);
                }
            }
        }

        private void CheckForProjectStart(TestNode testNode)
        {
            if (_projectLookup.ContainsKey(testNode.Id))
            {
                var projectInfo = _projectLookup[testNode.Id];

                lock (projectInfo)
                {
                    if (!projectInfo.WasStarted)
                    {
                        InvokeHandler(SuiteStarting, new TestNodeEventArgs(projectInfo.ProjectNode));
                        projectInfo.WasStarted = true;
                    }
                }
            }
        }

        private void CheckForProjectFinish(ResultNode resultNode)
        {
            if (_projectLookup.ContainsKey(resultNode.Id))
            {
                var projectInfo = _projectLookup[resultNode.Id];

                lock (projectInfo)
                {
                    if (--projectInfo.AssembliesToRun == 0)
                    {
                        var projectResult = new ResultNode(projectInfo.ProjectNode.Xml);
                        InvokeHandler(SuiteFinished, new TestResultEventArgs(projectResult));
                    }
                }
            }
        }

        #endregion
    }
}
