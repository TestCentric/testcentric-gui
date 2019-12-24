// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETSTANDARD1_6 && !NETSTANDARD2_0
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
