// ***********************************************************************
// Copyright (c) 2018 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Collections.Generic;
using NUnit.Engine;
using NUnit.Framework;

namespace TestCentric.Gui.Model
{
    public class AvailableRuntimesTest
    {
        private IList<IRuntimeFramework> _availableRuntimes;
        private TestModel _model;

        [OneTimeSetUp]
        public void CreateTestModel()
        {
            var engine = TestEngineActivator.CreateInstance();
            Assert.NotNull(engine, "Unable to create engine instance for testing");

            _model = new TestModel(engine);
            _availableRuntimes = _model.AvailableRuntimes;
            Assert.NotNull(_availableRuntimes);
        }

        [OneTimeTearDown]
        public void DisposeTestModel()
        {
            _model.Dispose();
        }

        [Test]
        public void AtLeastOneRuntimeIsAvailable()
        {
            Assert.That(_availableRuntimes.Count, Is.GreaterThan(0));
            foreach (IRuntimeFramework runtime in _availableRuntimes)
                Console.WriteLine("Available: " + runtime.DisplayName);
        }
    }
}
