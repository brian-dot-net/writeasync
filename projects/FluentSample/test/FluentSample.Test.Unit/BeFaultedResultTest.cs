//-----------------------------------------------------------------------
// <copyright file="BeFaultedResultTest.cs" company="Brian Rogers">
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
    public class BeFaultedResultTest
    {
        [TestMethod]
        public void FaultedTaskShouldPass()
        {
            Task<bool> task = TaskResultBuilder.Faulted();

            Action act = () => task.Should().BeFaulted();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void PendingTaskShouldFail()
        {
            Task<bool> task = TaskResultBuilder.Pending();

            Action act = () => task.Should().BeFaulted();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be faulted but was WaitingForActivation.");
        }

        [TestMethod]
        public void PendingTaskShouldFailWithReason()
        {
            Task<bool> task = TaskResultBuilder.Pending();

            Action act = () => task.Should().BeFaulted(because: "I said so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be faulted because I said so but was WaitingForActivation.");
        }

        [TestMethod]
        public void PendingTaskShouldFailWithReasonFormatted()
        {
            Task<bool> task = TaskResultBuilder.Pending();

            Action act = () => task.Should().BeFaulted("I said {0}", "so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be faulted because I said so but was WaitingForActivation.");
        }

        [TestMethod]
        public void NullTaskShouldFail()
        {
            Task<bool> task = null;

            Action act = () => task.Should().BeFaulted();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be faulted but was <null>.");
        }

        [TestMethod]
        public void NullTaskShouldFailWithReason()
        {
            Task<bool> task = null;

            Action act = () => task.Should().BeFaulted(because: "I said so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be faulted because I said so but was <null>.");
        }

        [TestMethod]
        public void CompletedTaskShouldFail()
        {
            Task<bool> task = TaskResultBuilder.Completed();

            Action act = () => task.Should().BeFaulted();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be faulted but was RanToCompletion.");
        }

        [TestMethod]
        public void CanceledTaskShouldFail()
        {
            Task<bool> task = TaskResultBuilder.Canceled();

            Action act = () => task.Should().BeFaulted();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be faulted but was Canceled.");
        }

        [TestMethod]
        public void ShouldAllowChaining()
        {
            Task<bool> task = TaskResultBuilder.Faulted();

            Action act = () => task.Should().BeFaulted().BeFaulted();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void ShouldAllowChainingWithAnd()
        {
            Task<bool> task = TaskResultBuilder.Faulted();

            Action act = () => task.Should().BeFaulted().And.BeFaulted();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void ShouldAllowChainingWithWhich()
        {
            Task<bool> task = TaskResultBuilder.Faulted();

            Action act = () => task.Should().BeFaulted().Which.IsFaulted.Should().BeTrue();

            act.ShouldNotThrow();
        }
    }
}
