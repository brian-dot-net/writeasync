//-----------------------------------------------------------------------
// <copyright file="BeCanceledResultTest.cs" company="Brian Rogers">
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
    public class BeCanceledResultTest
    {
        [TestMethod]
        public void CanceledTaskShouldPass()
        {
            Task<bool> task = TaskResultBuilder.Canceled();

            Action act = () => task.Should().BeCanceled();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void CompletedTaskShouldFail()
        {
            Task<bool> task = TaskResultBuilder.Completed();

            Action act = () => task.Should().BeCanceled();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be canceled but was RanToCompletion.");
        }

        [TestMethod]
        public void PendingTaskShouldFail()
        {
            Task<bool> task = TaskResultBuilder.Pending();

            Action act = () => task.Should().BeCanceled();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be canceled but was WaitingForActivation.");
        }

        [TestMethod]
        public void FaultedTaskShouldFail()
        {
            Task<bool> task = TaskResultBuilder.Faulted();

            Action act = () => task.Should().BeCanceled();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be canceled but was Faulted.");
        }

        [TestMethod]
        public void FaultedTaskShouldFailWithReason()
        {
            Task<bool> task = TaskResultBuilder.Faulted();

            Action act = () => task.Should().BeCanceled(because: "I said so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be canceled because I said so but was Faulted.");
        }

        [TestMethod]
        public void FaultedTaskShouldFailWithReasonFormatted()
        {
            Task<bool> task = TaskResultBuilder.Faulted();

            Action act = () => task.Should().BeCanceled("I said {0}", "so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be canceled because I said so but was Faulted.");
        }

        [TestMethod]
        public void NullTaskShouldFail()
        {
            Task<bool> task = null;

            Action act = () => task.Should().BeCanceled();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be canceled but was <null>.");
        }

        [TestMethod]
        public void NullTaskShouldFailWithReason()
        {
            Task<bool> task = null;

            Action act = () => task.Should().BeCanceled(because: "I said so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be canceled because I said so but was <null>.");
        }

        [TestMethod]
        public void NullTaskShouldFailWithReasonFormatted()
        {
            Task<bool> task = null;

            Action act = () => task.Should().BeCanceled("I said {0}", "so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be canceled because I said so but was <null>.");
        }

        [TestMethod]
        public void ShouldAllowChaining()
        {
            Task<bool> task = TaskResultBuilder.Canceled();

            Action act = () => task.Should().BeCanceled().BeCanceled();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void ShouldAllowChainingWithAnd()
        {
            Task<bool> task = TaskResultBuilder.Canceled();

            Action act = () => task.Should().BeCanceled().And.BeCanceled();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void ShouldAllowChainingWithWhich()
        {
            Task<bool> task = TaskResultBuilder.Canceled();

            Action act = () => task.Should().BeCanceled().Which.IsCanceled.Should().BeTrue();

            act.ShouldNotThrow();
        }
    }
}
