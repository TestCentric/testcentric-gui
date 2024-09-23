// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using NUnitLite;

namespace TestCentric.Gui.Model
{
    class Program
    {
        static int Main(string[] args)
        {
#if NETFRAMEWORK
            return new AutoRun().Execute(args);
#else
            return new TextRunner(typeof(Program).GetTypeInfo().Assembly).Execute(args);
#endif
        }
    }
}
