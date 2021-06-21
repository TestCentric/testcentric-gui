// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using NUnit.Engine;

namespace TestCentric.Engine.Services
{
    /// <summary>
    /// An ITestAgentSource is able to provide test agents of a
    /// particular type, which satisfy the criteria specified in
    /// a TestPackage.
    /// </summary>
    /// <remarks>
    /// Currently, the only supported criteria are target runtime
    /// and bitness. Additional factors may be added in the future.
    /// </remarks>
    public interface ITestAgentSource
    {
        TestAgentType AgentType { get; }

        /// <summary>
        /// AvailableAgents returns a list of information about every
        /// agent available for use. It is used for display and for 
        /// selection of the appropriate agent.
        /// </summary>
        /// <remarks>
        /// The factory creates this list by combining info from
        /// each agent source available.
        /// </remarks>
        IList<TestAgentInfo> AvailableAgents { get; }

        /// <summary>
        /// Returns true if an agent can be found, which is suitable
        /// for running the provided test package.
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        bool IsAgentAvailable(TestPackage package);

        /// <summary>
        /// Return an agent, which best matches the criteria defined
        /// in a TestPackage.
        /// </summary>
        /// <param name="package">The test package to be run</param>
        /// <returns>An ITestAgent</returns>
        /// <exception cref="ArgumentException">If no agent is available.</exception>
        ITestAgent GetAgent(TestPackage package);

        /// <summary>
        /// Select a specific agent from a list of those provided.
        /// </summary>
        /// <param name="name">The index of the agent in the list</param>
        /// <returns>An ITestAgent</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the index is out of range.
        /// </exception>
        /// <remarks>
        /// This function is intended for use by runners, which
        /// display a list of available agents and allow the user
        /// to select one of them. Its use assumes that the list
        /// of available agents remains constant while the
        /// application is running, which is currently the case.
        /// </remarks>
        ITestAgent SelectAgent(int index);

        /// <summary>
        /// Releases the test agent back to the supplier, which provided it.
        /// </summary>
        /// <param name="agent">An agent previously provided by a call to GetAgent.</param>
        /// <exception cref="InvalidOperationException">
        /// If agent was never provided by the factory or was previously released.
        /// </exception>
        /// <remarks>
        /// Disposing an agent also releases it. However, this should not
        /// normally be done by the client, but by the source that created
        /// the agent in the first place.
        /// </remarks>
        void ReleaseAgent(ITestAgent agent);
    }
}
