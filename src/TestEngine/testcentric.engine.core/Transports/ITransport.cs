// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TestCentric.Engine.Transports
{
    public interface ITransport
    {
        bool Start();
        void Stop();
    }
}
