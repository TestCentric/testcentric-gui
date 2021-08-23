// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;

namespace TestCentric.Engine.Communication.Messages
{
    [Serializable]
    public class CommandReturnMessage : TestEngineMessage
    {
        public CommandReturnMessage(object returnValue)
        {
            ReturnValue = returnValue;
        }

        public object ReturnValue { get; }
    }
}
