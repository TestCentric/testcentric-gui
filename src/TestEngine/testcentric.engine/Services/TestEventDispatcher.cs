// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using NUnit.Engine;
using TestCentric.Engine.Services;

namespace TestCentric.Engine.Services
{
    /// <summary>
    /// TestEventDispatcher is used to send test events to a number of listeners
    /// </summary>
    public class TestEventDispatcher : MarshalByRefObject, ITestEventListener, IService
    {
        private object _eventLock = new object();
        private ExtensionService _extensionService;
        private List<ITestEventListener> _listenerExtensions = new List<ITestEventListener>();
        private WorkItemTracker _workItemTracker = new WorkItemTracker();

        public TestEventDispatcher()
        {
            Listeners = new List<ITestEventListener>();
        }

        public IList<ITestEventListener> Listeners { get; private set; }

        public void ClearListeners()
        {
            _workItemTracker.Clear();
            Listeners = new List<ITestEventListener>(_listenerExtensions);
            Listeners.Add(_workItemTracker);
        }

        public bool WaitForCompletion(int millisecondsTimeout)
        {
            return _workItemTracker.WaitForCompletion(millisecondsTimeout);
        }

        public void IssuePendingNotifications()
        {
            _workItemTracker.IssuePendingNotifications(this);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        #region ITestEventListener Implementation

        public void OnTestEvent(string report)
        {
            lock (_eventLock)
            {
                foreach (var listener in Listeners)
                    listener.OnTestEvent(report);
            }
        }

        #endregion

        #region IService Implementation

        public IServiceLocator ServiceContext { get; set; }

        public ServiceStatus Status { get; private set; }

        void IService.StartService()
        {
            _extensionService = ServiceContext.GetService<ExtensionService>();
            foreach (var extension in _extensionService.GetExtensions<ITestEventListener>())
                _listenerExtensions.Add(extension);

            Status = _extensionService == null
                ? ServiceStatus.Error
                : ServiceStatus.Started;
        }

        void IService.StopService()
        {
            Status = ServiceStatus.Stopped;
        }

        #endregion
    }
}
