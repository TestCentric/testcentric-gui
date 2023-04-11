// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using NUnit.Engine;

namespace TestCentric.Gui.Model.Fakes
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
