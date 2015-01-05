//-----------------------------------------------------------------------
// <copyright file="BeCanceledTest.cs" company="Brian Rogers">
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
    public class BeCanceledTest
    {
        [TestMethod]
        public void CanceledTaskShouldPass()
        {
            Task task = TaskBuilder.Canceled();

            Action act = () => task.Should().BeCanceled();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void CompletedTaskShouldFail()
        {
            Task task = TaskBuilder.Completed();

            Action act = () => task.Should().BeCanceled();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be canceled but was RanToCompletion.");
        }

        [TestMethod]
        public void PendingTaskShouldFail()
        {
            Task task = TaskBuilder.Pending();

            Action act = () => task.Should().BeCanceled();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be canceled but was WaitingForActivation.");
        }

        [TestMethod]
        public void FaultedTaskShouldFail()
        {
            Task task = TaskBuilder.Faulted();

            Action act = () => task.Should().BeCanceled();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be canceled but was Faulted.");
        }

        [TestMethod]
        public void FaultedTaskShouldFailWithReason()
        {
            Task task = TaskBuilder.Faulted();

            Action act = () => task.Should().BeCanceled(because: "I said so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be canceled because I said so but was Faulted.");
        }

        [TestMethod]
        public void NullTaskShouldFail()
        {
            Task task = null;

            Action act = () => task.Should().BeCanceled();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be canceled but was <null>.");
        }

        [TestMethod]
        public void NullTaskShouldFailWithReason()
        {
            Task task = null;

            Action act = () => task.Should().BeCanceled(because: "I said so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be canceled because I said so but was <null>.");
        }

        [TestMethod]
        public void NullTaskShouldFailWithReasonFormatted()
        {
            Task task = null;

            Action act = () => task.Should().BeCanceled("I said {0}", "so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be canceled because I said so but was <null>.");
        }
    }
}
