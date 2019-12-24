// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Collections.Generic;
using TestCentric.Engine;

namespace TestCentric.TestUtilities.Fakes
{
    public class AvailableRuntimesService : IAvailableRuntimes
    {
        private List<IRuntimeFramework> _availableRuntimes = new List<IRuntimeFramework>();

        public void AddRuntimes(params IRuntimeFramework[] runtimes)
        {
            _availableRuntimes.AddRange(runtimes);
        }

        public IList<IRuntimeFramework> AvailableRuntimes
        {
            get { return _availableRuntimes; }
        }
    }
}
