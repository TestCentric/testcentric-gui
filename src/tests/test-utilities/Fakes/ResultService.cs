// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using NUnit.Engine;
using NUnit.Engine.Extensibility;

namespace TestCentric.TestUtilities.Fakes
{
    public class ResultService : IResultService
    {
        public string[] Formats
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IResultWriter GetResultWriter(string format, object[] args)
        {
            throw new NotImplementedException();
        }
    }
}
