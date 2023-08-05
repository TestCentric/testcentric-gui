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
            MessageType = messageType;
            MessageData = messageData;
        }

        public string MessageType { get; }
        public string MessageData { get; }
    }
}
