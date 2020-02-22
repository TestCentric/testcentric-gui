// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Collections.Generic;
using TestCentric.Engine.Extensibility;
using TestCentric.Engine.Internal;

namespace TestCentric.Engine.Drivers
{
    public abstract class NotRunnableFrameworkDriver : IFrameworkDriver
    {
        protected abstract NotRunnableAssemblyResult Result { get; }

        public string ID { get; set; }

        protected string TestID => string.IsNullOrEmpty(ID) ? "1" : ID + "-1";

        public string Load(string assemblyPath, IDictionary<string, object> settings)
        {
            return Result.LoadResult;
        }

        public int CountTestCases(string filter)
        {
            return 0;
        }

        public string Run(ITestEventListener listener, string filter)
        {
            return Result.RunResult;
        }

        public string Explore(string filter)
        {
            return Result.LoadResult;
        }

        public void StopRun(bool force)
        {
        }
    }

    public class InvalidAssemblyFrameworkDriver : NotRunnableFrameworkDriver
    {
        private string _assemblyPath;
        private string _message;

        public InvalidAssemblyFrameworkDriver(string assemblyPath, string message)
        {
            _assemblyPath = assemblyPath;
            _message = message;
        }

        protected override NotRunnableAssemblyResult Result => 
            new InvalidAssemblyResult(_assemblyPath, _message)
            {
                TestID = TestID
            };
    }

    public class SkippedAssemblyFrameworkDriver : NotRunnableFrameworkDriver
    {
        private string _assemblyPath;

        public SkippedAssemblyFrameworkDriver(string assemblyPath)
        {
            _assemblyPath = assemblyPath;
        }

        protected override NotRunnableAssemblyResult Result => 
            new SkippedAssemblyResult(_assemblyPath)
            {
                TestID = TestID
            };
    }
}
