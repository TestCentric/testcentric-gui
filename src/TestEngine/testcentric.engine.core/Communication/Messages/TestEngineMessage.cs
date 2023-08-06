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
        public TestEngineMessage(string code, string data)
        {
            Code = code;
            Data = data;
        }

        public string Code { get; }
        public string Data { get; }

        // Alias properties for convenience
        public string CommandName => Code;
        public string Argument => Data;
    }
}
