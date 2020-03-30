// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric Engine contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TestCentric.Engine.Communication.Messages
{
#if !NETSTANDARD1_6
    [Serializable]
#endif
    public class CommandMessage : TestEngineMessage
    {
        public CommandMessage(string commandName, params object[] arguments)
        {
            CommandName = commandName;
            Arguments = arguments;
        }

        public string CommandName { get; }

        public object[] Arguments { get; }
    }

//#if !NETSTANDARD1_6
//    [Serializable]
//#endif
//    public class CreateRunnerCommand : CommandMessage
//    {
//        public readonly TestPackage Package;

//        public CreateRunnerCommand(TestPackage package)
//            : base("CreateRunner")
//        {
//            Package = package;
//        }
//    }

//#if !NETSTANDARD1_6
//    [Serializable]
//#endif
//    public class StopCommand : CommandMessage
//    {
//        public StopCommand() : base("Stop") { }
//    }

//#if !NETSTANDARD1_6
//    [Serializable]
//#endif
//    public class LoadCommand : CommandMessage
//    {
//        public LoadCommand() : base("Load") { }
//    }

//#if !NETSTANDARD1_6
//    [Serializable]
//#endif
//    public class ReloadCommand : CommandMessage
//    {
//        public ReloadCommand() : base("Reload") { }
//    }

//#if !NETSTANDARD1_6
//    [Serializable]
//#endif
//    public class UnloadCommand : CommandMessage
//    {
//        public UnloadCommand() : base("Unload") { }
//    }

//#if !NETSTANDARD1_6
//    [Serializable]
//#endif
//    public class ExploreCommand : CommandMessage
//    {
//        public readonly TestFilter Filter;

//        public ExploreCommand(TestFilter filter)
//            : base("Explore")
//        {
//            Filter = filter;
//        }
//    }

//#if !NETSTANDARD1_6
//    [Serializable]
//#endif
//    public class CountTestCasesCommand : CommandMessage
//    {
//        public TestFilter TestFilter;

//        public CountTestCasesCommand(TestFilter filter)
//            : base("CountTestCases")
//        {
//            TestFilter = filter;
//        }
//    }

//#if !NETSTANDARD1_6
//    [Serializable]
//#endif
//    public class RunCommand : CommandMessage
//    {
//        public readonly TestFilter TestFilter;

//        public RunCommand(TestFilter filter)
//            : base("Run")
//        {
//            TestFilter = filter;
//        }
//    }

//#if !NETSTANDARD1_6
//    [Serializable]
//#endif
//    public class RunAsyncCommand : CommandMessage
//    {
//        public TestFilter TestFilter;

//        public RunAsyncCommand(TestFilter filter)
//            : base("RunAsync")
//        {
//            TestFilter = filter;
//        }
//    }

//#if !NETSTANDARD1_6
//    [Serializable]
//#endif
//    public class StopRunCommand : CommandMessage
//    {
//        public bool ForcedStop;

//        public StopRunCommand(bool force)
//            : base("StopRun")
//        {
//            ForcedStop = force;
//        }
//    }
}
