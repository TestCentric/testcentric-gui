// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Text;

namespace TestCentric.Engine.Communication.Messages
{
    public class MessageCode
    {   // All length 4
        public const string StartAgent = "STRT"; // not used
        public const string StopAgent = "EXIT";
        public const string CreateRunner = "RUNR";

        public const string LoadCommand = "LOAD";
        public const string ReloadCommand = "RELD";
        public const string UnloadCommand = "UNLD";
        public const string ExploreCommand = "XPLR";
        public const string CountCasesCommand = "CNTC";
        public const string RunCommand = "RSYN";
        public const string RunAsyncCommand = "RASY";
        public const string RequestStopCommand = "STOP";
        public const string ForcedStopCommand = "ABRT";

        public const string ProgressReport = "PROG";
        public const string CommandResult = "RSLT";
    }
}
