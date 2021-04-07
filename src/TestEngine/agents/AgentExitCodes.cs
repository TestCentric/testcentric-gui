// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TestCentric.Common
{
    internal static class AgentExitCodes
    {
        public const int OK = 0;
        public const int PARENT_PROCESS_TERMINATED = -1;
        public const int FAILED_TO_START_REMOTE_AGENT = -2;
        public const int DEBUGGER_SECURITY_VIOLATION = -3;
        public const int DEBUGGER_NOT_IMPLEMENTED = -4;
        public const int UNABLE_TO_LOCATE_AGENCY = -5;
        public const int UNEXPECTED_EXCEPTION = -100;
    }
}
