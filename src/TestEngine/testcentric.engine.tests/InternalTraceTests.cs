// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TestCentric.Engine
{
    public class InternalTraceTests
    {
        TestEngine _engine;
        
        InternalTraceLevel _savedInternalTraceLevel;
        string _savedInternalTraceLogPath;

        InternalTraceLevel _testLevel;

        [SetUp]
        public void CreateTestEngine()
        {
            _engine = new TestEngine();
            
            _savedInternalTraceLevel = InternalTrace.DefaultTraceLevel;
            _savedInternalTraceLogPath = InternalTrace.TraceWriter.LogPath;

            // Ensure _testLevel is different from saved level
            _testLevel = _savedInternalTraceLevel == InternalTraceLevel.Warning ? InternalTraceLevel.Error : InternalTraceLevel.Warning;
        }

        [TearDown]
        public void RestoreSavedTraceInfo()
        {
            InternalTrace.Initialize(_savedInternalTraceLogPath, _savedInternalTraceLevel);
        }

        [Test]
        public void CanSetTraceLevelBeforeInitialization()
        {
            _engine.InternalTraceLevel = _testLevel;
            Assert.That(InternalTrace.DefaultTraceLevel, Is.EqualTo(_testLevel), "Trace level not set");
            Assert.That(InternalTrace.TraceWriter.LogPath, Is.EqualTo(_savedInternalTraceLogPath), "Log path should not change");

            _engine.Initialize();
            Assert.That(InternalTrace.DefaultTraceLevel, Is.EqualTo(_testLevel), "Trace level changed after Initialization");
            Assert.That(InternalTrace.TraceWriter.LogPath, Is.EqualTo(_savedInternalTraceLogPath), "Log path changed after initialization");
        }

        [Test]
        public void CanSetTraceLevelAfterInitialization()
        {
            _engine.Initialize();
            _engine.InternalTraceLevel = _testLevel;
            Assert.That(InternalTrace.DefaultTraceLevel, Is.EqualTo(_testLevel), "Trace level not set");
            Assert.That(InternalTrace.TraceWriter.LogPath, Is.EqualTo(_savedInternalTraceLogPath), "Log path should not change");
        }
    }
}
