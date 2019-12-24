// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NET35
using System.Reflection;
using NUnitLite;

namespace TestCentric.Engine
{
    class Program
    {
        static int Main(string[] args)
        {
            return new TextRunner(typeof(Program).GetTypeInfo().Assembly).Execute(args);
        }
    }
}
#endif
