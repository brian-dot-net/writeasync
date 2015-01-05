//-----------------------------------------------------------------------
// <copyright file="BePendingResultTest.cs" company="Brian Rogers">
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
    public class BePendingResultTest
    {
        [TestMethod]
        public void PendingTaskShouldPass()
        {
            Task<bool> task = TaskResultBuilder.Pending();

            Action act = () => task.Should().BePending();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void CompletedTaskShouldFail()
        {
            Task<bool> task = TaskResultBuilder.Completed();

            Action act = () => task.Should().BePending();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be pending but was RanToCompletion.");
        }

        [TestMethod]
        public void FaultedTaskShouldFail()
        {
            Task<bool> task = TaskResultBuilder.Faulted();

            Action act = () => task.Should().BePending();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be pending but was Faulted.");
        }

        [TestMethod]
        public void FaultedTaskShouldFailWithReason()
        {
            Task<bool> task = TaskResultBuilder.Faulted();

            Action act = () => task.Should().BePending(because: "I said so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be pending because I said so but was Faulted.");
        }

        [TestMethod]
        public void FaultedTaskShouldFailWithReasonFormatted()
        {
            Task<bool> task = TaskResultBuilder.Faulted();

            Action act = () => task.Should().BePending("I said {0}", "so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be pending because I said so but was Faulted.");
        }

        [TestMethod]
        public void NullTaskShouldFail()
        {
            Task<bool> task = null;

            Action act = () => task.Should().BePending();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be pending but was <null>.");
        }

        [TestMethod]
        public void CanceledTaskShouldFail()
        {
            Task<bool> task = TaskResultBuilder.Canceled();

            Action act = () => task.Should().BePending();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be pending but was Canceled.");
        }

        [TestMethod]
        public void NullTaskShouldFailWithReason()
        {
            Task<bool> task = null;

            Action act = () => task.Should().BePending(because: "I said so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be pending because I said so but was <null>.");
        }

        [TestMethod]
        public void NullTaskShouldFailWithReasonFormatted()
        {
            Task<bool> task = null;

            Action act = () => task.Should().BePending("I said {0}", "so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be pending because I said so but was <null>.");
        }

        [TestMethod]
        public void ShouldAllowChaining()
        {
            Task<bool> task = TaskResultBuilder.Pending();

            Action act = () => task.Should().BePending().BePending();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void ShouldAllowChainingWithAnd()
        {
            Task<bool> task = TaskResultBuilder.Pending();

            Action act = () => task.Should().BePending().And.BePending();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void ShouldAllowChainingWithWhich()
        {
            Task<bool> task = TaskResultBuilder.Pending();

            Action act = () => task.Should().BePending().Which.IsCompleted.Should().BeFalse();

            act.ShouldNotThrow();
        }
    }
}
