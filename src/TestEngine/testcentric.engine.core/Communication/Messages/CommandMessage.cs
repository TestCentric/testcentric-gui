// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;

namespace TestCentric.Engine.Communication.Messages
{
    [Serializable]
    public class CommandMessage : TestEngineMessage
    {
        public CommandMessage(string commandName, object argument)
        {
            CommandName = commandName;
            Argument = argument;
        }

        public string CommandName { get; }

        public object Argument { get; }
    }
}
