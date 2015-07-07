//-----------------------------------------------------------------------
// <copyright file="UnitTest1.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimerSample.Test.Unit
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test1()
        {
            Class1 c = new Class1();

            Assert.IsNotNull(c);
        }
    }
}
