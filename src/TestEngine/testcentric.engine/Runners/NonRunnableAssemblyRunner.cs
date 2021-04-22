// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using NUnit.Engine;
using TestCentric.Engine.Helpers;
using TestCentric.Engine.Internal;

namespace TestCentric.Engine.Runners
{
    public abstract class NonRunnableAssemblyRunner : AbstractTestRunner
    {
        public NonRunnableAssemblyRunner(TestPackage package) : base(new ServiceContext(), package) { }

        protected abstract NotRunnableAssemblyResult Result { get; }

        protected override TestEngineResult LoadPackage()
        {
            return new TestEngineResult(Result.LoadResult);
        }

        public override int CountTestCases(TestFilter filter)
        {
            return 0;
        }

        protected override TestEngineResult RunTests(ITestEventListener listener, TestFilter filter)
        {
            return new TestEngineResult(Result.RunResult);
        }

        public override TestEngineResult Explore(TestFilter filter)
        {
            return new TestEngineResult(Result.LoadResult);
        }

        public override void StopRun(bool force)
        {
        }
    }

    public class InvalidAssemblyRunner : NonRunnableAssemblyRunner
    {
        private string _assemblyPath;
        private string _message;

        public InvalidAssemblyRunner(TestPackage package, string message)
            : base(package)
        {
            _assemblyPath = package.FullName;
            _message = message;
        }

        protected override NotRunnableAssemblyResult Result =>
            new InvalidAssemblyResult(_assemblyPath, _message)
            {
                TestID = TestPackage.ID
            };
    }

    public class SkippedAssemblyRunner : NonRunnableAssemblyRunner
    {
        private string _assemblyPath;

        public SkippedAssemblyRunner(TestPackage package)
            :base(package)
        {
            _assemblyPath = package.FullName;
        }

        protected override NotRunnableAssemblyResult Result =>
            new SkippedAssemblyResult(_assemblyPath)
            {
                TestID = TestPackage.ID
            };
    }
}
