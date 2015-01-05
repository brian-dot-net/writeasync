//-----------------------------------------------------------------------
// <copyright file="BeFaultedTest.cs" company="Brian Rogers">
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
    public class BeFaultedTest
    {
        [TestMethod]
        public void FaultedTaskShouldPass()
        {
            Task task = TaskBuilder.Faulted();

            Action act = () => task.Should().BeFaulted();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void PendingTaskShouldFail()
        {
            Task task = TaskBuilder.Pending();

            Action act = () => task.Should().BeFaulted();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be faulted but was WaitingForActivation.");
        }
    }
}
