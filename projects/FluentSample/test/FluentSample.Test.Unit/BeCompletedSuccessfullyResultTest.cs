//-----------------------------------------------------------------------
// <copyright file="BeCompletedSuccessfullyResultTest.cs" company="Brian Rogers">
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
    public class BeCompletedSuccessfullyResultTest
    {
        [TestMethod]
        public void CompletedTaskShouldPass()
        {
            Task<bool> task = TaskResultBuilder.Completed();

            Action act = () => task.Should().BeCompletedSuccessfully();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void PendingTaskShouldFail()
        {
            Task<bool> task = TaskResultBuilder.Pending();

            Action act = () => task.Should().BeCompletedSuccessfully();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed successfully but was WaitingForActivation.");
        }

        [TestMethod]
        public void PendingTaskShouldFailWithReason()
        {
            Task<bool> task = TaskResultBuilder.Pending();

            Action act = () => task.Should().BeCompletedSuccessfully(because: "I said so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed successfully because I said so but was WaitingForActivation.");
        }

        [TestMethod]
        public void PendingTaskShouldFailWithReasonFormatted()
        {
            Task<bool> task = TaskResultBuilder.Pending();

            Action act = () => task.Should().BeCompletedSuccessfully("I said {0}", "so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed successfully because I said so but was WaitingForActivation.");
        }

        [TestMethod]
        public void NullTaskShouldFail()
        {
            Task<bool> task = null;

            Action act = () => task.Should().BeCompletedSuccessfully();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed successfully but was <null>.");
        }

        [TestMethod]
        public void NullTaskShouldFailWithReason()
        {
            Task<bool> task = null;

            Action act = () => task.Should().BeCompletedSuccessfully(because: "I said so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed successfully because I said so but was <null>.");
        }

        [TestMethod]
        public void NullTaskShouldFailWithReasonFormatted()
        {
            Task<bool> task = null;

            Action act = () => task.Should().BeCompletedSuccessfully("I said {0}", "so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed successfully because I said so but was <null>.");
        }

        [TestMethod]
        public void FaultedTaskShouldFail()
        {
            Task<bool> task = TaskResultBuilder.Faulted();

            Action act = () => task.Should().BeCompletedSuccessfully();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed successfully but was Faulted.");
        }

        [TestMethod]
        public void CanceledTaskShouldFail()
        {
            Task<bool> task = TaskResultBuilder.Canceled();

            Action act = () => task.Should().BeCompletedSuccessfully();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed successfully but was Canceled.");
        }

        [TestMethod]
        public void ShouldAllowChaining()
        {
            Task<bool> task = TaskResultBuilder.Completed();

            Action act = () => task.Should().BeCompletedSuccessfully().BeCompletedSuccessfully();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void ShouldAllowChainingWithAnd()
        {
            Task<bool> task = TaskResultBuilder.Completed();

            Action act = () => task.Should().BeCompletedSuccessfully().And.BeCompletedSuccessfully();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void ShouldAllowChainingWithWhich()
        {
            Task<bool> task = TaskResultBuilder.Completed();

            Action act = () => task.Should().BeCompletedSuccessfully().Which.IsCompleted.Should().BeTrue();

            act.ShouldNotThrow();
        }
    }
}
