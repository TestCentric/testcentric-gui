// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

#if !NETSTANDARD
using System;
using System.Web.UI;

namespace TestCentric.Engine
{
    public class CallbackHandler : MarshalByRefObject, ICallbackEventHandler
    {
        public string Result { get; private set; }

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
            Result = eventArgument;
        }
    }
}
#endif
