// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;

namespace TestCentric.Gui.Model.Filter
{
    /// <summary>
    /// Filters the TestNodes by outcome (for example: 'Passed', 'Failed' or 'Not run')
    /// </summary>
    public class OutcomeFilter : ITestFilter
    {
        public const string AllOutcome = "All";
        public const string NotRunOutcome = "Not Run";

        private List<string> _condition = new List<string>() { AllOutcome };

        internal OutcomeFilter(ITestModel model)
        {
            TestModel = model;
        }

        public string FilterId => "OutcomeFilter";

        private ITestModel TestModel { get; }

        public IEnumerable<string> Condition
        {
            get { return _condition; }
            set { _condition = value.ToList(); }
        }

        public bool IsMatching(TestNode testNode)
        {
            // All kind of outcomes should be displayed (no outcome filtering)
            if (_condition.Contains(AllOutcome))
                return true;

            string outcome = NotRunOutcome;

            var result = TestModel.GetResultForTest(testNode.Id);
            if (result != null)
            {
                switch (result.Outcome.Status)
                {
                    case TestStatus.Failed:
                    case TestStatus.Passed:
                    case TestStatus.Inconclusive:
                        outcome = result.Outcome.Status.ToString();
                        break;
                    case TestStatus.Skipped:
                        outcome = result.Outcome.Label == "Ignored" ? "Ignored" : "Skipped";
                        break;
                }
            }

            return _condition.Contains(outcome);
        }

        public void Reset()
        {
            _condition = new List<string>() { AllOutcome };
        }

        public void Init()
        {
            _condition = new List<string>() { AllOutcome };
        }
    }
}
