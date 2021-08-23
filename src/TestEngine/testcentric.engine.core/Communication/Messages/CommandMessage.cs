// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;

namespace TestCentric.Engine.Communication.Messages
{
    [Serializable]
    public class CommandMessage : TestEngineMessage
    {
        public CommandMessage(string commandName, params object[] arguments)
        {
            CommandName = commandName;
            Arguments = arguments;
        }

        public string CommandName { get; }

        public object[] Arguments { get; }
    }
}
