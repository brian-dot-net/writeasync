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

        [TestMethod]
        public void PendingTaskShouldFailWithReason()
        {
            Task task = TaskBuilder.Pending();

            Action act = () => task.Should().BeFaulted(because: "I said so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be faulted because I said so but was WaitingForActivation.");
        }

        [TestMethod]
        public void PendingTaskShouldFailWithReasonFormatted()
        {
            Task task = TaskBuilder.Pending();

            Action act = () => task.Should().BeFaulted("I said {0}", "so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be faulted because I said so but was WaitingForActivation.");
        }

        [TestMethod]
        public void NullTaskShouldFail()
        {
            Task task = null;

            Action act = () => task.Should().BeFaulted();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be faulted but was <null>.");
        }

        [TestMethod]
        public void NullTaskShouldFailWithReason()
        {
            Task task = null;

            Action act = () => task.Should().BeFaulted(because: "I said so");

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be faulted because I said so but was <null>.");
        }

        [TestMethod]
        public void CompletedTaskShouldFail()
        {
            Task task = TaskBuilder.Completed();

            Action act = () => task.Should().BeFaulted();

            act.ShouldThrow<AssertFailedException>().WithMessage("Expected task to be faulted but was RanToCompletion.");
        }

        [TestMethod]
        public void ShouldAllowChaining()
        {
            Task task = TaskBuilder.Faulted();

            Action act = () => task.Should().BeFaulted().BeFaulted();

            act.ShouldNotThrow();
        }

        [TestMethod]
        public void ShouldAllowChainingWithAnd()
        {
            Task task = TaskBuilder.Faulted();

            Action act = () => task.Should().BeFaulted().And.BeFaulted();

            act.ShouldNotThrow();
        }
    }
}
