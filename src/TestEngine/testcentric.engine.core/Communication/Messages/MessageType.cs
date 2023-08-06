// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Text;

namespace TestCentric.Engine.Communication.Messages
{
    public class MessageType
    {
        public const string StartAgent = "STRTAGNT"; // not used
        public const string StopAgent = "STOPAGNT";
        public const string CreateRunner = "GETRUNNR";

        public const string LoadCommand = "LOADTEST";
        public const string ReloadCommand = "RELOAD";
        public const string UnloadCommand = "UNLOAD";
        public const string ExploreCommand = "EXPLORE";
        public const string CountCasesCommand = "CMTCASES";
        public const string RunCommand = "RUNTESTS";
        public const string RunAsyncCommand = "RUNASYNC";
        public const string RequestStopCommand = "STOP";
        public const string ForcedStopCommand = "ABORT";

        public const string ProgressReport = "PROGRESS";
        public const string CommandResult = "RESULT";
    }
}
