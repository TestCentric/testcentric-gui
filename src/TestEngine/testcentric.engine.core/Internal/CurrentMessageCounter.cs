// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Threading;

namespace TestCentric.Engine.Internal
{
    public sealed class CurrentMessageCounter
    {
        private readonly ManualResetEvent _noMessages = new ManualResetEvent(true);
        private int _currentMessageCount;

        public void OnMessageStart()
        {
            if (Interlocked.Increment(ref _currentMessageCount) == 1)
                _noMessages.Reset();
        }

        public void OnMessageEnd()
        {
            if (Interlocked.Decrement(ref _currentMessageCount) == 0)
                _noMessages.Set();
        }

        public void WaitForAllCurrentMessages()
        {
            _noMessages.WaitOne();
        }
    }
}
