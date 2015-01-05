//-----------------------------------------------------------------------
// <copyright file="BeCompletedResultTest.cs" company="Brian Rogers">
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
    public class BeCompletedResultTest
    {
        [TestMethod]
        public void CompletedTaskShouldPass()
        {
            Task<bool> task = TaskResultBuilder.Completed();

            Action act = () => task.Should().BeCompleted();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void PendingTaskShouldFail()
        {
            Task<bool> task = TaskResultBuilder.Pending();

            Action act = () => task.Should().BeCompleted();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed but was WaitingForActivation.");
        }

        [TestMethod]
        public void PendingTaskShouldFailWithReason()
        {
            Task<bool> task = TaskResultBuilder.Pending();

            Action act = () => task.Should().BeCompleted(because: "I said so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed because I said so but was WaitingForActivation.");
        }

        [TestMethod]
        public void PendingTaskShouldFailWithReasonFormatted()
        {
            Task<bool> task = TaskResultBuilder.Pending();

            Action act = () => task.Should().BeCompleted("I said {0}", "so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed because I said so but was WaitingForActivation.");
        }

        [TestMethod]
        public void NullTaskShouldFail()
        {
            Task<bool> task = null;

            Action act = () => task.Should().BeCompleted();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed but was <null>.");
        }

        [TestMethod]
        public void NullTaskShouldFailWithReason()
        {
            Task<bool> task = null;

            Action act = () => task.Should().BeCompleted(because: "I said so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed because I said so but was <null>.");
        }

        [TestMethod]
        public void NullTaskShouldFailWithReasonFormatted()
        {
            Task<bool> task = null;

            Action act = () => task.Should().BeCompleted("I said {0}", "so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be completed because I said so but was <null>.");
        }

        [TestMethod]
        public void FaultedTaskShouldPass()
        {
            Task<bool> task = TaskResultBuilder.Faulted();

            Action act = () => task.Should().BeCompleted();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void CanceledTaskShouldPass()
        {
            Task<bool> task = TaskResultBuilder.Canceled();

            Action act = () => task.Should().BeCompleted();

            act.ShouldNotThrow();
        }
        
        [TestMethod]
        public void ShouldAllowChaining()
        {
            Task<bool> task = TaskResultBuilder.Faulted();

            Action act = () => task.Should().BeCompleted().BeCompleted();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void ShouldAllowChainingWithAnd()
        {
            Task<bool> task = TaskResultBuilder.Faulted();

            Action act = () => task.Should().BeCompleted().And.BeCompleted();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void ShouldAllowChainingWithWhich()
        {
            Task<bool> task = TaskResultBuilder.Faulted();

            Action act = () => task.Should().BeCompleted().Which.IsCompleted.Should().BeTrue();

            act.ShouldNotThrow();
        }
    }
}
