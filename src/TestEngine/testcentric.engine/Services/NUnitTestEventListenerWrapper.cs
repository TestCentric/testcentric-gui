// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TestCentric.Engine.Services
{
    /// <summary>
    /// Nested wrapper class for listeners based on the NUnit API
    /// </summary>
    public class NUnitTestEventListenerWrapper : ITestEventListener
    {
        private NUnit.Engine.ITestEventListener _listener;

        public NUnitTestEventListenerWrapper(NUnit.Engine.ITestEventListener listener)
        {
            _listener = listener;
        }

        public void OnTestEvent(string report)
        {
            _listener.OnTestEvent(report);
        }
    }
}
