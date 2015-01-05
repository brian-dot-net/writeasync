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

        [TestMethod]
        public void ShouldAllowChainingWithTypedException()
        {
            Task<bool> task = TaskResultBuilder.Faulted();

            Action act = () => task.Should().BeCompleted().WithException<InvalidCastException>().WithMessage("Expected failure.");

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void WithExceptionShouldPassIfAtLeastOneExceptionMatches()
        {
            Task<bool> task = TaskResultBuilder.Faulted(new InvalidCastException("Expected failure."), new InvalidProgramException("Other expected failure."));

            Action act = () => task.Should().BeCompleted().WithException<InvalidCastException>().WithMessage("Expected failure.");

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void WithExceptionShouldFailIfNoExceptionMatches()
        {
            Task<bool> task = TaskResultBuilder.Faulted(new InvalidOperationException("Unexpected failure."));

            Action act = () => task.Should().BeCompleted().WithException<InvalidCastException>();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected a <System.InvalidCastException> to be thrown, but found a*System.InvalidOperationException with message \"Unexpected failure.\"*");
        }

        [TestMethod]
        public void WithExceptionShouldFailOnNullException()
        {
            Task<bool> task = TaskResultBuilder.Completed();

            Action act = () => task.Should().BeCompleted().WithException<InvalidCastException>();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected a <System.InvalidCastException> to be thrown, but no exception was thrown.");
        }

        [TestMethod]
        public void WithExceptionShouldFailOnNullExceptionWithReason()
        {
            Task<bool> task = TaskResultBuilder.Completed();

            Action act = () => task.Should().BeCompleted().WithException<InvalidCastException>(because: "I said so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected a <System.InvalidCastException> to be thrown because I said so, but no exception was thrown.");
        }
    }
}
