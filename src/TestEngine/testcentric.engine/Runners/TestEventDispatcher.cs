// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace TestCentric.Engine.Runners
{
    /// <summary>
    /// TestEventDispatcher is used to send test events to a number of listeners
    /// </summary>
    public class TestEventDispatcher :
#if !NETSTANDARD1_6
        MarshalByRefObject, 
#endif
        ITestEventListener
    {
        private object _eventLock = new object();

        public TestEventDispatcher()
        {
            Listeners = new List<ITestEventListener>();
        }

        public IList<ITestEventListener>Listeners { get; private set; }

        public void OnTestEvent(string report)
        {
            lock (_eventLock)
            {
                foreach (var listener in Listeners)
                    listener.OnTestEvent(report);
            }
        }

#if !NETSTANDARD1_6
        public override object InitializeLifetimeService()
        {
            return null;
        }
#endif
    }
}
