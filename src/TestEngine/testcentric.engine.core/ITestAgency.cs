// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace NUnit.Engine
{
    /// <summary>
    /// The ITestAgency interface is implemented by a TestAgency in 
    /// order to allow TestAgents to register with it.
    /// </summary>
    public interface ITestAgency
    {
        /// <summary>
        /// Registers an agent with an agency
        /// </summary>
        /// <param name="agent"></param>
        void Register(ITestAgent agent);
    }
}
