// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using NUnit.Framework;

namespace TestCentric.Gui.TextDisplay
{
    [TestFixture]
    public abstract class TextDisplayTests
    {
        [Test]
        public void DisplayTest([Range(1, 5)] int num)
        {
            TestContext.Progress.WriteLine($"Immediate output from test {num}");
            Console.WriteLine($"Result output from test {num}");
            TestContext.Progress.WriteLine($"More immediate output from test {num}");
        }
    }

    public class SequentialTextDisplay : TextDisplayTests
    { }

    [Parallelizable(ParallelScope.Children)]
    public class ParallelTextDisplay : TextDisplayTests
    { }
}
