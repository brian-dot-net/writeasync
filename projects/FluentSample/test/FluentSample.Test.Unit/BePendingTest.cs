//-----------------------------------------------------------------------
// <copyright file="BePendingTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace FluentSample.Test.Unit
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using FluentSample;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BePendingTest
    {
        [TestMethod]
        public void PendingTaskShouldPass()
        {
            Task task = TaskBuilder.Pending();

            Action act = () => task.Should().BePending();

            act.ShouldNotThrow();
        }
    }
}
