//-----------------------------------------------------------------------
// <copyright file="BeCompletedSuccessfullyTest.cs" company="Brian Rogers">
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
    public class BeCompletedSuccessfullyTest
    {
        [TestMethod]
        public void CompletedTaskShouldPass()
        {
            Task task = TaskBuilder.Completed();

            Action act = () => task.Should().BeCompletedSuccessfully();

            act.ShouldNotThrow();
        }
    }
}
