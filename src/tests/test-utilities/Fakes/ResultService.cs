// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TestCentric.Engine;
using TestCentric.Engine.Extensibility;

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
