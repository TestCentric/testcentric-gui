// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace TestCentric.Gui.Model.Filter
{
    /// <summary>
    /// Filters the TestNodes by matching a text (for example: Namespace, Class name or test method name - filter is case insensitive)
    /// </summary>
    internal class TextFilter : ITestFilter
    {
        private string _condition = string.Empty;

        public string FilterId => "TextFilter";

        public IEnumerable<string> Condition
        {
            get { return new List<string>() { _condition }; }
            set { _condition = value.FirstOrDefault(); }
        }

        public bool IsMatching(TestNode testNode)
        {
            if (string.IsNullOrEmpty(_condition))
            {
                return true;
            }

            return testNode.FullName.IndexOf(_condition, StringComparison.InvariantCultureIgnoreCase) > -1;
        }

        public void Reset()
        {
            _condition = string.Empty;
        }

        public void Init()
        {
            _condition = string.Empty;
        }
    }
}
