// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.ComponentModel;
using NUnit.Engine;

namespace TestCentric.Engine.Runners
{
    public abstract class TestEngineRunner : AbstractTestRunner
    {
        public TestEngineRunner(IServiceLocator services, TestPackage package)
            : base(package)
        {
            Services = services;
        }
    
        protected IServiceLocator Services { get; }
    }
}
