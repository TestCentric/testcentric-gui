// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace TestCentric.Engine.Services.TestRunnerFactoryTests
{
    internal class RunnerResultComparer : IEqualityComparer<RunnerResult>
    {
        public static readonly RunnerResultComparer Instance = new RunnerResultComparer();

        public bool Equals(RunnerResult x, RunnerResult y)
        {
            x = x ?? throw new ArgumentNullException(nameof(x));
            y = y ?? throw new ArgumentNullException(nameof(y));

            return x.TestRunner == y.TestRunner &&
                   x.SubRunners.SequenceEqual(y.SubRunners, Instance);
        }

        public int GetHashCode(RunnerResult obj)
        {
            throw new NotImplementedException();
        }
    }
}
