// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETSTANDARD1_6 && !NETSTANDARD2_0
using System;
using System.Diagnostics;
using System.Web.UI;

namespace TestCentric.Engine
{
    public class RunTestsCallbackHandler : MarshalByRefObject, ICallbackEventHandler
    {
        private ITestEventListener _listener;

        public string Result { get; private set; }

        public RunTestsCallbackHandler(ITestEventListener listener)
        {
            _listener = listener ?? new NullListener();
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public string GetCallbackResult()
        {
            throw new NotImplementedException();
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            if (IsFinalResult(eventArgument))
                Result = eventArgument;
            else
                ReportProgress(eventArgument);
        }

        private void ReportProgress(string state)
        {
            _listener.OnTestEvent(state);
        }

        private bool IsFinalResult(string eventArgument)
        {
            // TODO: If we add a prefix to the final result in the next framework
            // release, then we can immediately recognize the final result but we
            // would need to continue to examine the non-final result in case the
            // the framework in use were an older version. Building in more knowledge
            // of framework versions is probably not a good idea, since it would be
            // potentially fragile as changes were made.

            // Eliminate all events except for test-suite
            if (!eventArgument.StartsWith("<test-suite", StringComparison.Ordinal))
                return false;
            // Return for all test cases, for example, 90% of events

            // Eliminate suites that do not represent an assembly
            if (!eventArgument.Contains("type=\"Assembly\""))
                return false; 
            // Return for all except the assembly result. Remaining code is only
            // executed twice per assembly.

            // Heuristic: only final results may have an environment element
            if (eventArgument.Contains("<environment"))
                return true;
            // For the actual final result, the current framework version returns
            // true at this point. Only older versions need further checking.

            // Heuristic: only final results may have a settings element
            if (eventArgument.Contains("<settings"))
                return true;

            // Heuristic: only final results have nested suites
            return eventArgument.IndexOf("<test-suite", 12, StringComparison.Ordinal) > 0;
        }

        class NullListener : ITestEventListener
        {
            public void OnTestEvent(string report)
            {
            }
        }
    }
}
#endif
