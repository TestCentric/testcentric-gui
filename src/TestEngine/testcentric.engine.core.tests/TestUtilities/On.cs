// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Threading;

namespace NUnit.Engine.TestUtilities
{
    public static class On
    {
        public static IDisposable Dispose(Action action) => new OnDisposeAction(action);

        private sealed class OnDisposeAction : IDisposable
        {
            private Action action;

            public OnDisposeAction(Action action)
            {
                this.action = action;
            }

            public void Dispose() => Interlocked.Exchange(ref action, null)?.Invoke();
        }
    }
}
