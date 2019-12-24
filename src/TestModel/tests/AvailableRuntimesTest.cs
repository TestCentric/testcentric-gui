// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using TestCentric.Engine;
using NUnit.Framework;
using TestCentric.TestUtilities.Fakes;
using RuntimeFramework = TestCentric.TestUtilities.Fakes.RuntimeFramework;

namespace TestCentric.Gui.Model
{
    public class AvailableRuntimesTest
    {
        [Test]
        public void RuntimesSupportedByEngineAreAvailable()
        {
            var mockEngine = new MockTestEngine().WithRuntimes(
                new RuntimeFramework("net-4.5", new Version(4, 5)),
                new RuntimeFramework("net-4.0", new Version(4, 0)));

            var model = new TestModel(mockEngine);

            Assert.That(model.AvailableRuntimes.Count, Is.EqualTo(2));
            Assert.That(model.AvailableRuntimes, Has.One.Property("Id").EqualTo("net-4.5"));
            Assert.That(model.AvailableRuntimes, Has.One.Property("Id").EqualTo("net-4.0"));
        }
    }
}
