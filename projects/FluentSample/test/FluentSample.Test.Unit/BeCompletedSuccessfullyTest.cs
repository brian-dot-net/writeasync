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
        public void PendingTaskShouldFailWithReasonFormatted()
        {
            Task task = TaskBuilder.Pending();

            Action act = () => task.Should().BeCompletedSuccessfully("I said {0}", "so");

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
        public void NullTaskShouldFailWithReason()
        {
            Task task = null;

            Action act = () => task.Should().BeCompletedSuccessfully(because: "I said so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed successfully because I said so but was <null>.");
        }

        [TestMethod]
        public void NullTaskShouldFailWithReasonFormatted()
        {
            Task task = null;

            Action act = () => task.Should().BeCompletedSuccessfully("I said {0}", "so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed successfully because I said so but was <null>.");
        }

        [TestMethod]
        public void FaultedTaskShouldFail()
        {
            Task task = TaskBuilder.Faulted();

            Action act = () => task.Should().BeCompletedSuccessfully();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed successfully but was Faulted.");
        }

        [TestMethod]
        public void CanceledTaskShouldFail()
        {
            Task task = TaskBuilder.Canceled();

            Action act = () => task.Should().BeCompletedSuccessfully();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed successfully but was Canceled.");
        }

        [TestMethod]
        public void ShouldAllowChaining()
        {
            Task task = TaskBuilder.Completed();

            Action act = () => task.Should().BeCompletedSuccessfully().BeCompletedSuccessfully();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void ShouldAllowChainingWithAnd()
        {
            Task task = TaskBuilder.Completed();

            Action act = () => task.Should().BeCompletedSuccessfully().And.BeCompletedSuccessfully();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void ShouldAllowChainingWithWhich()
        {
            Task task = TaskBuilder.Completed();

            Action act = () => task.Should().BeCompletedSuccessfully().Which.IsCompleted.Should().BeTrue();

            act.ShouldNotThrow();
        }
    }
}
