// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace TestCentric.Tests
{
    [Category("Generics")]
    [TestFixture(typeof(List<int>))]
    [TestFixture(TypeArgs=new Type[] {typeof(ArrayList)} )]
    public class SimpleGenericFixture<TList> where TList : IList, new()
    {
        [Test]
        public void TestCollectionCount()
        {
            Console.WriteLine("TList is {0}", typeof(TList));
            IList list = new TList();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            Assert.AreEqual(3, list.Count);
        }
    }
}
