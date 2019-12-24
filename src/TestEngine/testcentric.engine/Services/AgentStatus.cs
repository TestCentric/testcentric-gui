// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETSTANDARD1_6 && !NETSTANDARD2_0
namespace TestCentric.Engine.Services
{
    /// <summary>
    /// Enumeration used to report AgentStatus
    /// </summary>
    public enum AgentStatus
    {
        Starting,
        Ready,
        Terminated
    }
}
#endif
