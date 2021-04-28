// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using NUnit.Engine;
using NUnit.Engine.Extensibility;
using TestCentric.Engine.Internal;

namespace TestCentric.Engine.Drivers
{
    public class InvalidAssemblyFrameworkDriver : IFrameworkDriver
    {
        private string _assemblyPath;
        private string _message;

        public InvalidAssemblyFrameworkDriver(string assemblyPath, string message)
        {
            _assemblyPath = assemblyPath;
            _message = message;
        }

        public string ID { get; set; }

        protected string TestID => string.IsNullOrEmpty(ID) ? "1" : ID + "-1";

        public NotRunnableAssemblyResult Result => 
            new InvalidAssemblyResult(_assemblyPath, _message)
            {
                TestID = TestID
            };

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
}
