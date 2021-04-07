// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TestCentric.Engine.Communication.Transports
{
    public interface ITestAgencyTransport : ITransport
    {
        string ServerUrl { get; }
    }
}
