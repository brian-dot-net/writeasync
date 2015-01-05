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

        [TestMethod]
        public void PendingTaskShouldFail()
        {
            Task task = TaskBuilder.Pending();

            Action act = () => task.Should().BeCompletedSuccessfully();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed successfully but was WaitingForActivation.");
        }

        [TestMethod]
        public void PendingTaskShouldFailWithReason()
        {
            Task task = TaskBuilder.Pending();

            Action act = () => task.Should().BeCompletedSuccessfully(because: "I said so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed successfully because I said so but was WaitingForActivation.");
        }

        [TestMethod]
        public void NullTaskShouldFail()
        {
            Task task = null;

            Action act = () => task.Should().BeCompletedSuccessfully();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed successfully but was <null>.");
        }

        [TestMethod]
        public void FaultedTaskShouldFail()
        {
            Task task = TaskBuilder.Faulted();

            Action act = () => task.Should().BeCompletedSuccessfully();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed successfully but was Faulted.");
        }
    }
}
