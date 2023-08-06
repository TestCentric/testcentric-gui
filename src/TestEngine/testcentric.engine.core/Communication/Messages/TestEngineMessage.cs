// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;

namespace TestCentric.Engine.Communication.Messages
{
    [Serializable]
    public class TestEngineMessage
    {
        public TestEngineMessage(string messageType, string messageData)
        {
            Type = messageType;
            Data = messageData;
        }

        public string Type { get; }
        public string Data { get; }

        // Alias properties for convenience
        public string CommandName => Type;
        public string Argument => Data;
    }
}
