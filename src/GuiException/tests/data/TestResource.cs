// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric GUI contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************


namespace NUnit.UiException.Tests.data
{
    public class TestResource : TestCentric.TestUtilities.TempResourceFile
    {
        public TestResource(string name)
            : base(typeof(TestResource), name)
        {
        }
    }
}
