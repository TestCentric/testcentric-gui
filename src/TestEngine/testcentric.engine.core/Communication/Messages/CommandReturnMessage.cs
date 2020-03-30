// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TestCentric.Engine.Communication.Messages
{
#if !NETSTANDARD1_6
    [Serializable]
#endif
    public class CommandReturnMessage : TestEngineMessage
    {
        public CommandReturnMessage(object returnValue)
        {
            ReturnValue = returnValue;
        }

        public object ReturnValue { get; }
    }
}
